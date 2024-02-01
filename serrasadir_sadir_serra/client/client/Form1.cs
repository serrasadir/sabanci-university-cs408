using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace client
{
    public partial class Form1 : Form
    {
        int combo_enabled = 0;
        bool terminating = false;
        bool connected = false;
        Socket clientSocket;

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBox_ip.Text;

            int portNum;
            if(Int32.TryParse(textBox_port.Text, out portNum))
            {
                try
                {
                    clientSocket.Connect(IP, portNum);
                    textBox_ip.Enabled = false;
                    textBox_port.Enabled = false;
                    textBox_username.Enabled = false;
                    button_connect.Enabled = false;
                    buttonA.Enabled = true;
                    buttonB.Enabled = true;
                    textBox_message.Enabled = true;
                    connected = true;
                    terminating = false;
                    button_disconnect.Enabled = true;
                    logs.AppendText("Connected to the server!\n");

                    combobox_channel.SelectedItem = null;

                    Thread receiveThread = new Thread(Receive);
                    receiveThread.Start();
                    string username = textBox_username.Text;

                    if (username != "" && username.Length <= 64)
                    {
                        Byte[] buffer = Encoding.Default.GetBytes(username);
                        clientSocket.Send(buffer);
                    }

                }
                catch
                {
                    logs.AppendText("Could not connect to the server!\n");
                }
            }
            else
            {
                logs.AppendText("Check the port\n");
            }

        }

        private void Receive()
        {
            while(connected)
            {
                try
                {
                    Byte[] buffer = new Byte[64];
                    clientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    if (incomingMessage.Substring(0,3) == "->A")
                    {
                        logsA.AppendText(incomingMessage.Substring(3) + "\n");
                    }
                    else if (incomingMessage.Substring(0, 3) == "->B")
                    {
                        logsB.AppendText(incomingMessage.Substring(3) + "\n");
                    }
                    else if (incomingMessage.Substring(0, 7) == "ABleave")
                    {
                        logsA.AppendText(incomingMessage.Substring(7) + "\n");
                        logsB.AppendText(incomingMessage.Substring(7) + "\n");
                    }
                    else if (incomingMessage != "")
                    {
                        logs.AppendText("Server: " + incomingMessage + "\n");
                    }
                }
                catch
                {
                    if (!terminating)
                    {
                        logs.AppendText("The server has disconnected\n");
                        textBox_username.Enabled = true;
                        textBox_ip.Enabled = true;
                        textBox_port.Enabled = true;
                        button_connect.Enabled = true;
                        button_disconnect.Enabled = false;
                        textBox_message.Enabled = false;
                        button_send.Enabled = false;
                        buttonB.Text = "Subscribe-SPS101";
                        buttonA.Text = "Subscribe-IF100";
                        if (combobox_channel.Items.Contains("IF100"))
                        {
                            combobox_channel.Items.Remove("IF100");                           
                        }
                        if (combobox_channel.Items.Contains("SPS101"))
                        {
                            combobox_channel.Items.Remove("SPS101");
                        }
                        combobox_channel.SelectedItem = null;
                        combobox_channel.Enabled = false;
                        buttonB.Enabled = false;
                        buttonA.Enabled = false;
                    }

                    clientSocket.Close();
                    connected = false;
                }

            }
        }

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            string user = '!' + textBox_username.Text;
            Byte[] buffer = Encoding.Default.GetBytes(user);
            try
            {
                connected = false;
                terminating = true;
                if(textBox_username.Text != "") {
                    clientSocket.Send(buffer);
                }
                    
                Environment.Exit(0);
            }
            catch
            {
                Environment.Exit(0);
            }

        }

        private void button_send_Click(object sender, EventArgs e)
        {
            string message = textBox_message.Text;

            string initial = "";

            if (combobox_channel.SelectedItem != null && combobox_channel.SelectedItem.ToString() == "IF100")
            {
                initial = "->A";
            }
            if (combobox_channel.SelectedItem != null && combobox_channel.SelectedItem.ToString() == "SPS101")
            {
                initial = "->B";
            }
            textBox_message.Text = "";
            string result = initial + message;

            if (message != "" && result.Length <= 64)
            {
                Byte[] buffer = Encoding.Default.GetBytes(result);
                clientSocket.Send(buffer);
            }

        }

        private void button_disconnect_Click(object sender, EventArgs e)
        {

            try
            {
                if (buttonA.Text == "Unsubscribe-IF100" && buttonB.Text == "Unsubscribe-SPS101")
                {
                    string channelToRemove = "IF100";
                    string channelToRemove2 = "SPS101";

                    string message = "AB-both";
                    combo_enabled = 0;
                    if (message != "" && message.Length <= 64)
                    {
                        Byte[] buffer2 = Encoding.Default.GetBytes(message);
                        clientSocket.Send(buffer2);
                    }


                    combobox_channel.Items.Remove(channelToRemove);
                    combobox_channel.Items.Remove(channelToRemove2);

                    logs.AppendText("Successfully unsubscribed from channel IF100.\n");
                    logs.AppendText("Successfully unsubscribed from channel SPS101.\n");

                    combobox_channel.Enabled = false;
                    button_send.Enabled = false;

                    buttonB.Text = "Subscribe-SPS101";
                    buttonA.Text = "Subscribe-IF100";
                }
                else if (buttonA.Text == "Unsubscribe-IF100")
                {
                    string channelToRemove = "IF100";

                    if (combobox_channel.Items.Contains(channelToRemove))
                    {
                        string message = "-A";
                        combo_enabled -= 1;
                        if (message != "" && message.Length <= 64)
                        {
                            Byte[] buffer2 = Encoding.Default.GetBytes(message);
                            clientSocket.Send(buffer2);
                        }
                        combobox_channel.Items.Remove(channelToRemove);

                        logs.AppendText("Successfully unsubscribed from channel IF100.\n");

                        combobox_channel.Enabled = false;
                        button_send.Enabled = false;


                        buttonA.Text = "Subscribe-IF100";
                    }
                }
                else if (buttonB.Text == "Unsubscribe-SPS101")
                {

                    string channelToRemove2 = "SPS101";

                    if (combobox_channel.Items.Contains(channelToRemove2))
                    {
                        combo_enabled -= 1;
                        string message2 = "-B";

                        Byte[] buffer4 = Encoding.Default.GetBytes(message2);
                        clientSocket.Send(buffer4);

                        combobox_channel.Items.Remove(channelToRemove2);
                        logs.AppendText("Successfully unsubscribed from channel SPS101.\n");

                        buttonB.Text = "Subscribe-SPS101";
                    }
                }
                string user = '!' + textBox_username.Text;
                Byte[] buffer = Encoding.Default.GetBytes(user);
                clientSocket.Send(buffer);
                textBox_ip.Enabled = true;
                textBox_port.Enabled = true;
                textBox_username.Enabled = true;
                button_connect.Enabled = true;
                button_disconnect.Enabled = false;
                combobox_channel.Enabled = false;
                textBox_message.Enabled = false;
                button_send.Enabled = false;               
                buttonA.Enabled = false;
                buttonB.Enabled = false;
                terminating = true;
                connected = false;

                logs.AppendText("Successfully disconnected...\n");

            }
            catch
            {
                logs.AppendText("Server is not running!\n");

            }           
        }

        private void buttonA_Click(object sender, EventArgs e)
        {
            if (buttonA.Text == "Subscribe-IF100")
            {                
                string channelToAdd = "IF100";
              
                if (!combobox_channel.Items.Contains(channelToAdd))
                {                   
                    combobox_channel.Items.Add(channelToAdd);
                    string message = "+A";
                    combo_enabled += 1;

                    if (message != "" && message.Length <= 64)
                    {
                        Byte[] buffer = Encoding.Default.GetBytes(message);
                        clientSocket.Send(buffer);
                    }
                    combobox_channel.Enabled = true;
                    button_send.Enabled = true;

                    logs.AppendText("Successfully subscribed to channel IF100.\n");
                    if(combo_enabled == 1)
                    {
                        combobox_channel.SelectedIndex = 0;
                    }
                   
                    buttonA.Text = "Unsubscribe-IF100";
                }
                else
                {
                    
                    logs.AppendText("Channel IF100 is already in the list. Duplicate Entry\n");
                }
            }
            else if (buttonA.Text == "Unsubscribe-IF100")
            {              
                string channelToRemove = "IF100";
               
                if (combobox_channel.Items.Contains(channelToRemove))
                {
                    string message = "-A";
                    combo_enabled -= 1;
                    if (message != "" && message.Length <= 64)
                    {
                        Byte[] buffer = Encoding.Default.GetBytes(message);
                        clientSocket.Send(buffer);
                    }
                    combobox_channel.Items.Remove(channelToRemove);

                    logs.AppendText("Successfully unsubscribed from channel IF100.\n");

                    if (combo_enabled < 1)
                    {
                        combobox_channel.SelectedItem = null;
                        combobox_channel.Enabled = false;
                        button_send.Enabled = false;
                    }

                    buttonA.Text = "Subscribe-IF100";
                }
                else
                {
                    logs.AppendText("Channel IF100 is not in the list.\n");
                }
            }
        }

        private void buttonB_Click(object sender, EventArgs e)
        {

            if (buttonB.Text == "Subscribe-SPS101")
            {
               
                string channelToAdd = "SPS101";
                combo_enabled += 1;

                if (!combobox_channel.Items.Contains(channelToAdd))
                {                   
                    combobox_channel.Items.Add(channelToAdd);
                    string message = "+B";

                    if (message != "" && message.Length <= 64)
                    {
                        Byte[] buffer = Encoding.Default.GetBytes(message);
                        clientSocket.Send(buffer);
                    }
                    combobox_channel.Enabled = true;
                    button_send.Enabled = true;

                    logs.AppendText("Successfully subscribed to channel SPS101.\n");
                    if (combo_enabled == 1)
                    {
                        combobox_channel.SelectedIndex = 0;
                    }

                                       

                    buttonB.Text = "Unsubscribe-SPS101";
                }
                else
                {              
                    logs.AppendText("Channel SPS101 is already in the list. Duplicate Entry\n");
                }
            }
            else if (buttonB.Text == "Unsubscribe-SPS101")
            {                
                string channelToRemove = "SPS101";
         
                if (combobox_channel.Items.Contains(channelToRemove))
                {
                    string message = "-B";
                    combo_enabled -= 1;

                    if (message != "" && message.Length <= 64)
                    {
                        Byte[] buffer = Encoding.Default.GetBytes(message);
                        clientSocket.Send(buffer);
                    }
                    combobox_channel.Items.Remove(channelToRemove);

                    logs.AppendText("Successfully unsubscribed from channel SPS101.\n");
                    
                    if(combo_enabled < 1)
                    {
                        combobox_channel.SelectedItem = null;
                        combobox_channel.Enabled = false;
                        button_send.Enabled = false;
                    }

                    buttonB.Text = "Subscribe-SPS101";
                }
                else
                {                  
                    logs.AppendText("Channel SPS101 is not in the list.\n");
                }
            }


        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
