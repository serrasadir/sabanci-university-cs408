using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace server
{
    public partial class Form1 : Form
    {

        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<ClientInfo> connectedClients = new List<ClientInfo>();


        List<string> user_list = new List<string>();
        bool expectingName = true;
        bool terminating = false;
        bool listening = false;
        string username = string.Empty;


        public struct ClientInfo
        {
            public Socket ClientSocket { get; set; }
            public string Username { get; set; }
            public bool IsSubscribedToChannelA { get; set; }
            public bool IsSubscribedToChannelB { get; set; }

            public ClientInfo(Socket clientSocket, string username)
            {
                ClientSocket = clientSocket;
                Username = username;
                IsSubscribedToChannelA = false;
                IsSubscribedToChannelB = false;
            }
        }

        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void button_listen_Click(object sender, EventArgs e)
        {
            int serverPort;

            if (Int32.TryParse(textBox_port.Text, out serverPort))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint);
                serverSocket.Listen(int.MaxValue);

                combobox_channel.Enabled = true;
                listening = true;
                button_listen.Enabled = false;
                textBox_message.Enabled = true;
                button_send.Enabled = true;
                combobox_channel.SelectedItem = "IF100";

                Thread acceptThread = new Thread(Accept);
                acceptThread.Start();

                logs.AppendText("Started listening on port: " + serverPort + "\n");

            }
            else
            {
                logs.AppendText("Please check port number \n");
            }
        }

        bool IsUsernameTaken(string username, List<ClientInfo> clients)
        {
            foreach (var client in clients)
            {
                if (client.Username == username)
                {
                    return true; // Username is already taken
                }
            }
            return false; // Username is not taken
        }

        private void Accept()
        {
            while (listening)
            {
                try
                {

                    Socket newClient = serverSocket.Accept();
                    logs.AppendText("A client is connected.\n");

                    Byte[] buffer = new Byte[64];
                    newClient.Receive(buffer);



                    string username = Encoding.Default.GetString(buffer);
                    username = username.Substring(0, username.IndexOf("\0"));

                    if (IsUsernameTaken(username, connectedClients))
                    {
                        logs.AppendText("Client with already existing username could not connect.\n");
                        string result = "Username already exists, change your username.";
                        byte[] buffer2 = Encoding.Default.GetBytes(result);
                        newClient.Send(buffer2);

                        newClient.Close();
                    }
                    else
                    {
                        ClientInfo clientInfo = new ClientInfo(newClient, username);
                        connectedClients.Add(clientInfo);

                        //user_list.Add(username);

                        logs.AppendText("Username: " + username + " is connected.\n");
                        logs.AppendText("*******\n");


             
                        general_connected.Text = ("List of users already connected:\n");
                        foreach (ClientInfo item in connectedClients)
                        {
                            general_connected.AppendText("Socket: " + item.ClientSocket.RemoteEndPoint.ToString() + "\n");
                            general_connected.AppendText("Username: " + item.Username + "\n");
                            general_connected.AppendText("*******\n");
                        }


                        Thread receiveThread = new Thread(() => Receive(clientInfo)); // updated
                        receiveThread.Start();
                    }
                }
                catch
                {
                    if (terminating)
                    {
                        listening = false;
                    }
                    else
                    {
                        logs.AppendText("The socket stopped working.\n");
                    }

                }
            }
        }

        private void Receive(ClientInfo thisClient) // updated
        {
            bool connected = true;

            while (connected && !terminating)
            {
                try
                {
                    Byte[] buffer = new Byte[64];
                    thisClient.ClientSocket.Receive(buffer);

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    if (incomingMessage[0] == '!')
                    {

                        throw new Exception(username);
                    }

                    else if (incomingMessage.Substring(0, 2) == "+A")
                    {
                        thisClient.IsSubscribedToChannelA = true;

                        int last_index = connectedClients.FindIndex(clientInfo => clientInfo.ClientSocket == thisClient.ClientSocket);

                        if (last_index != -1)
                        {
                            connectedClients[last_index] = thisClient;
                            logs.AppendText(thisClient.Username + ": is subscribed to channel IF100.\n");
                            string result = "->A" + thisClient.Username + " has joined the channel!";
                            logsA.AppendText(result.Substring(3) + "\n");
                            byte[] buffer3 = Encoding.Default.GetBytes(result);


                          
                            if100_connected.Text = ("List of users connected to IF100:\n");

                            foreach (ClientInfo connectedClient in connectedClients)
                            {
                                if (connectedClient.IsSubscribedToChannelA)
                                {
                                    connectedClient.ClientSocket.Send(buffer3);
                                    if100_connected.AppendText("Socket: " + connectedClient.ClientSocket.RemoteEndPoint.ToString() + "\n");
                                    if100_connected.AppendText("Username: " + connectedClient.Username + "\n");
                                    if100_connected.AppendText("*******\n");
                                }
                            }
                        }
                    }
                    else if (incomingMessage.Substring(0, 2) == "+B")
                    {
                        thisClient.IsSubscribedToChannelB = true;

                        int last_index = connectedClients.FindIndex(clientInfo => clientInfo.ClientSocket == thisClient.ClientSocket);

                        if (last_index != -1)
                        {
                            connectedClients[last_index] = thisClient;
                            logs.AppendText(thisClient.Username + ": is subscribed to channel SPS101.\n");
                            string result = "->B" + thisClient.Username + " has joined the channel!";
                            logsB.AppendText(result.Substring(3) + "\n");
                            byte[] buffer3 = Encoding.Default.GetBytes(result);



                            sps101_connected.Text = ("List of users connected to SPS101:\n");
                            foreach (ClientInfo connectedClient in connectedClients)
                            {
                                if (connectedClient.IsSubscribedToChannelB)
                                {
                                    connectedClient.ClientSocket.Send(buffer3);
                                    sps101_connected.AppendText("Socket: " + connectedClient.ClientSocket.RemoteEndPoint.ToString() + "\n");
                                    sps101_connected.AppendText("Username: " + connectedClient.Username + "\n");
                                    sps101_connected.AppendText("*******\n");
                                }
                            }
                        }
                    }
                    else if (incomingMessage.Substring(0, 2) == "-A")
                    {


                        int last_index = connectedClients.FindIndex(clientInfo => clientInfo.ClientSocket == thisClient.ClientSocket);

                        if (last_index != -1)
                        {
                            logs.AppendText(thisClient.Username + ": is unsubscribed from channel IF100.\n");
                            string result = "->A" + thisClient.Username + " has left the channel!";
                            logsA.AppendText(result.Substring(3) + "\n");
                            byte[] buffer3 = Encoding.Default.GetBytes(result);

                            if100_connected.Text = ("List of users connected to IF100:\n");

                            foreach (ClientInfo connectedClient in connectedClients)
                            {
                                if (connectedClient.IsSubscribedToChannelA)
                                {
                                    connectedClient.ClientSocket.Send(buffer3);

                                }
                            }
                            thisClient.IsSubscribedToChannelA = false;
                            connectedClients[last_index] = thisClient;
                        }
                        foreach (ClientInfo connectedClient in connectedClients)
                        {
                            if (connectedClient.IsSubscribedToChannelA)
                            {

                                if100_connected.AppendText("Socket: " + connectedClient.ClientSocket.RemoteEndPoint.ToString() + "\n");
                                if100_connected.AppendText("Username: " + connectedClient.Username + "\n");
                                if100_connected.AppendText("*******\n");
                            }
                        }
                    }
                    else if (incomingMessage.Substring(0, 2) == "-B")
                    {

                        int last_index = connectedClients.FindIndex(clientInfo => clientInfo.ClientSocket == thisClient.ClientSocket);

                        if (last_index != -1)
                        {
                            
                            logs.AppendText(thisClient.Username + ": is unsubscribed from channel SPS101.\n");
                            string result = "->B" + thisClient.Username + " has left the channel!";
                            logsB.AppendText(result.Substring(3) + "\n");
                            byte[] buffer3 = Encoding.Default.GetBytes(result);



                            foreach (ClientInfo connectedClient in connectedClients)
                            {
                                if (connectedClient.IsSubscribedToChannelB)
                                {
                                    connectedClient.ClientSocket.Send(buffer3);

                                }
                            }
                            thisClient.IsSubscribedToChannelB = false;
                            connectedClients[last_index] = thisClient;


                            sps101_connected.Text = ("List of users connected to SPS101:\n");
                            foreach (ClientInfo connectedClient in connectedClients)
                            {
                                if (connectedClient.IsSubscribedToChannelB)
                                {
                                    sps101_connected.AppendText("Socket: " + connectedClient.ClientSocket.RemoteEndPoint.ToString() + "\n");
                                    sps101_connected.AppendText("Username: " + connectedClient.Username + "\n");
                                    sps101_connected.AppendText("*******\n");
                                }
                            }
                        }
                    }
                    else if (incomingMessage.Substring(0, 7) == "AB-both")
                    {

                        int last_index = connectedClients.FindIndex(clientInfo => clientInfo.ClientSocket == thisClient.ClientSocket);

                        if (last_index != -1)
                        {

                            logs.AppendText(thisClient.Username + ": is unsubscribed from channel IF101.\n");
                            logs.AppendText(thisClient.Username + ": is unsubscribed from channel SPS101.\n");
                            string result = "ABleave" + thisClient.Username + " has left the channel!";
                            logsA.AppendText(result.Substring(7) + "\n");
                            logsB.AppendText(result.Substring(7) + "\n");
                            byte[] buffer3 = Encoding.Default.GetBytes(result);

                            foreach (ClientInfo connectedClient in connectedClients)
                            {
                                if (connectedClient.IsSubscribedToChannelB)
                                {
                                    connectedClient.ClientSocket.Send(buffer3);
                                }
                            }
                            thisClient.IsSubscribedToChannelA = false;
                            thisClient.IsSubscribedToChannelB = false;
                            connectedClients[last_index] = thisClient;
                        }
                    }

                    else if (incomingMessage.Substring(0, 3) == "->A")
                    {
                        logsA.AppendText(thisClient.Username + ": " + incomingMessage.Substring(3) + "\n");



                        foreach (ClientInfo connectedClient in connectedClients)
                        {
                            try
                            {
                                if (connectedClient.IsSubscribedToChannelA)
                                {

                                    string result = incomingMessage.Substring(0, 3) + thisClient.Username + ": " + incomingMessage.Substring(3);
                                    byte[] buffer2 = Encoding.Default.GetBytes(result);
                                    connectedClient.ClientSocket.Send(buffer2);
                                }
                            }

                            catch
                            {
                                logs.AppendText("There is a problem! Check the connection...\n");
                                terminating = true;
                                textBox_message.Enabled = false;
                                button_send.Enabled = false;
                                textBox_port.Enabled = true;
                                button_listen.Enabled = true;
                                serverSocket.Close();
                            }

                        }

                    }
                    else if (incomingMessage.Substring(0, 3) == "->B")
                    {
                        logsB.AppendText(thisClient.Username + ": " + incomingMessage.Substring(3) + "\n");

                        foreach (ClientInfo connectedClient in connectedClients)
                        {
                            try
                            {
                                if (connectedClient.IsSubscribedToChannelB)
                                {

                                    string result = incomingMessage.Substring(0, 3) + thisClient.Username + ": " + incomingMessage.Substring(3);
                                    byte[] buffer2 = Encoding.Default.GetBytes(result);
                                    connectedClient.ClientSocket.Send(buffer2);
                                }
                            }

                            catch
                            {
                                logs.AppendText("There is a problem! Check the connection...\n");
                                terminating = true;
                                textBox_message.Enabled = false;
                                button_send.Enabled = false;
                                textBox_port.Enabled = true;
                                button_listen.Enabled = true;
                                serverSocket.Close();
                            }
                        }
                    }
                    else
                    {
                        logs.AppendText(thisClient.Username + ": " + incomingMessage + "\n");
                    }


                }
                catch (Exception)
                {
                    if (!terminating)
                    {
                        logs.AppendText(thisClient.Username + " has disconnected\n");
                    }
                    thisClient.ClientSocket.Close();
                    connectedClients.Remove(thisClient);

                    sps101_connected.Text = ("List of users connected to SPS101:\n");
                    foreach (ClientInfo connectedClient in connectedClients)
                    {
                        if (connectedClient.IsSubscribedToChannelB)
                        {
                            sps101_connected.AppendText("Socket: " + connectedClient.ClientSocket.RemoteEndPoint.ToString() + "\n");
                            sps101_connected.AppendText("Username: " + connectedClient.Username + "\n");
                            sps101_connected.AppendText("*******\n");
                        }
                    }

                    if100_connected.Text = ("List of users connected to IF100:\n");
                    foreach (ClientInfo connectedClient in connectedClients)
                    {
                        if (connectedClient.IsSubscribedToChannelA)
                        {
                            if100_connected.AppendText("Socket: " + connectedClient.ClientSocket.RemoteEndPoint.ToString() + "\n");
                            if100_connected.AppendText("Username: " + connectedClient.Username + "\n");
                            if100_connected.AppendText("*******\n");
                        }
                    }

                    general_connected.Text = ("List of users already connected:\n");
                    foreach (ClientInfo item in connectedClients)
                    {
                        general_connected.AppendText("Socket: " + item.ClientSocket.RemoteEndPoint.ToString() + "\n");
                        general_connected.AppendText("Username: " + item.Username + "\n");
                        general_connected.AppendText("*******\n");
                    }


                    connected = false;
                }
            }
        }
        
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
            terminating = true;
            Environment.Exit(0);
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            string message = "Server: " + textBox_message.Text;

            if (combobox_channel.SelectedItem.ToString().Equals("IF100", StringComparison.OrdinalIgnoreCase))
            {
                logsA.AppendText(message + "\n");
            }
            else if (combobox_channel.SelectedItem.ToString().Equals("SPS101", StringComparison.OrdinalIgnoreCase))
            {
                logsB.AppendText(message + "\n");
            }


            string initialA = "->A";
            string initialB = "->B";

            if (message != "" && message.Length <= 64)
            {
                foreach (ClientInfo connectedClient in connectedClients)
                {
                    try
                    {

                        if (combobox_channel.SelectedItem.ToString().Equals("IF100", StringComparison.OrdinalIgnoreCase))
                        {

                            if (connectedClient.IsSubscribedToChannelA)
                            {
                                string result = initialA + message;
                                byte[] buffer = Encoding.Default.GetBytes(result);
                                connectedClient.ClientSocket.Send(buffer);

                            }
                        }

                        else if (combobox_channel.SelectedItem.ToString().Equals("SPS101", StringComparison.OrdinalIgnoreCase))
                        {

                            if (connectedClient.IsSubscribedToChannelB)
                            {
                                string result = initialB + message;
                                byte[] buffer = Encoding.Default.GetBytes(result);
                                connectedClient.ClientSocket.Send(buffer);
                            }
                        }
                        else
                        {
                            logs.AppendText("Server: " + message);
                        }
                    }

                    catch
                    {
                        logs.AppendText("There is a problem! Check the connection...\n");
                        terminating = true;
                        textBox_message.Enabled = false;
                        button_send.Enabled = false;
                        textBox_port.Enabled = true;
                        button_listen.Enabled = true;
                        serverSocket.Close();
                    }

                }

            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
