namespace server
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textBox_port = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button_listen = new System.Windows.Forms.Button();
            this.logs = new System.Windows.Forms.RichTextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.textBox_message = new System.Windows.Forms.TextBox();
            this.button_send = new System.Windows.Forms.Button();
            this.logsA = new System.Windows.Forms.RichTextBox();
            this.logsB = new System.Windows.Forms.RichTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.combobox_channel = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.if100_connected = new System.Windows.Forms.RichTextBox();
            this.sps101_connected = new System.Windows.Forms.RichTextBox();
            this.general_connected = new System.Windows.Forms.RichTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox_port
            // 
            this.textBox_port.Location = new System.Drawing.Point(66, 14);
            this.textBox_port.Name = "textBox_port";
            this.textBox_port.Size = new System.Drawing.Size(205, 26);
            this.textBox_port.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port:";
            // 
            // button_listen
            // 
            this.button_listen.Location = new System.Drawing.Point(294, 14);
            this.button_listen.Name = "button_listen";
            this.button_listen.Size = new System.Drawing.Size(84, 34);
            this.button_listen.TabIndex = 2;
            this.button_listen.Text = "Listen";
            this.button_listen.UseVisualStyleBackColor = true;
            this.button_listen.Click += new System.EventHandler(this.button_listen_Click);
            // 
            // logs
            // 
            this.logs.Location = new System.Drawing.Point(63, 94);
            this.logs.Name = "logs";
            this.logs.Size = new System.Drawing.Size(197, 342);
            this.logs.TabIndex = 3;
            this.logs.Text = "";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(72, 439);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(78, 20);
            this.Label2.TabIndex = 4;
            this.Label2.Text = "Message:";
            // 
            // textBox_message
            // 
            this.textBox_message.Enabled = false;
            this.textBox_message.Location = new System.Drawing.Point(434, 442);
            this.textBox_message.Name = "textBox_message";
            this.textBox_message.Size = new System.Drawing.Size(313, 26);
            this.textBox_message.TabIndex = 5;
            // 
            // button_send
            // 
            this.button_send.Enabled = false;
            this.button_send.Location = new System.Drawing.Point(659, 474);
            this.button_send.Name = "button_send";
            this.button_send.Size = new System.Drawing.Size(88, 43);
            this.button_send.TabIndex = 6;
            this.button_send.Text = "send";
            this.button_send.UseVisualStyleBackColor = true;
            this.button_send.Click += new System.EventHandler(this.button_send_Click);
            // 
            // logsA
            // 
            this.logsA.Location = new System.Drawing.Point(283, 94);
            this.logsA.Name = "logsA";
            this.logsA.Size = new System.Drawing.Size(223, 342);
            this.logsA.TabIndex = 7;
            this.logsA.Text = "";
            // 
            // logsB
            // 
            this.logsB.Location = new System.Drawing.Point(530, 94);
            this.logsB.Name = "logsB";
            this.logsB.Size = new System.Drawing.Size(217, 342);
            this.logsB.TabIndex = 8;
            this.logsB.Text = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(290, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "IF100:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(526, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "SPS101:";
            // 
            // combobox_channel
            // 
            this.combobox_channel.Enabled = false;
            this.combobox_channel.FormattingEnabled = true;
            this.combobox_channel.Items.AddRange(new object[] {
            "IF100",
            "SPS101"});
            this.combobox_channel.Location = new System.Drawing.Point(157, 439);
            this.combobox_channel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.combobox_channel.Name = "combobox_channel";
            this.combobox_channel.Size = new System.Drawing.Size(180, 28);
            this.combobox_channel.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(609, 278);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 20);
            this.label5.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(60, 71);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 20);
            this.label6.TabIndex = 13;
            this.label6.Text = "General";
            // 
            // if100_connected
            // 
            this.if100_connected.Location = new System.Drawing.Point(783, 94);
            this.if100_connected.Name = "if100_connected";
            this.if100_connected.Size = new System.Drawing.Size(213, 204);
            this.if100_connected.TabIndex = 14;
            this.if100_connected.Text = "";
            // 
            // sps101_connected
            // 
            this.sps101_connected.Location = new System.Drawing.Point(783, 327);
            this.sps101_connected.Name = "sps101_connected";
            this.sps101_connected.Size = new System.Drawing.Size(213, 190);
            this.sps101_connected.TabIndex = 15;
            this.sps101_connected.Text = "";
            // 
            // general_connected
            // 
            this.general_connected.Location = new System.Drawing.Point(1009, 94);
            this.general_connected.Name = "general_connected";
            this.general_connected.Size = new System.Drawing.Size(213, 423);
            this.general_connected.TabIndex = 16;
            this.general_connected.Text = "";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(779, 304);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(160, 20);
            this.label7.TabIndex = 17;
            this.label7.Text = "SPS101 Subscribers:";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(779, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(143, 20);
            this.label8.TabIndex = 18;
            this.label8.Text = "IF100 Subscribers:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1234, 588);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.general_connected);
            this.Controls.Add(this.sps101_connected);
            this.Controls.Add(this.if100_connected);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.combobox_channel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.logsB);
            this.Controls.Add(this.logsA);
            this.Controls.Add(this.button_send);
            this.Controls.Add(this.textBox_message);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.logs);
            this.Controls.Add(this.button_listen);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_port);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox_port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button_listen;
        private System.Windows.Forms.RichTextBox logs;
        private System.Windows.Forms.Label Label2;
        private System.Windows.Forms.TextBox textBox_message;
        private System.Windows.Forms.Button button_send;
        private System.Windows.Forms.RichTextBox logsA;
        private System.Windows.Forms.RichTextBox logsB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox combobox_channel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox if100_connected;
        private System.Windows.Forms.RichTextBox sps101_connected;
        private System.Windows.Forms.RichTextBox general_connected;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}

