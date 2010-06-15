namespace DHCP4IPTV
{
    partial class frmDHCP4IPTV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDHCP4IPTV));
            this.cmbNIC = new System.Windows.Forms.ComboBox();
            this.lblNIC = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMAC0 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMAC1 = new System.Windows.Forms.TextBox();
            this.txtMAC2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtMAC3 = new System.Windows.Forms.TextBox();
            this.txtMAC4 = new System.Windows.Forms.TextBox();
            this.txtMAC5 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbDecoder = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.chkSaveSettings = new System.Windows.Forms.CheckBox();
            this.chkStartWithWindows = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.chkStartMinimized = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbNIC
            // 
            this.cmbNIC.FormattingEnabled = true;
            this.cmbNIC.Location = new System.Drawing.Point(35, 12);
            this.cmbNIC.Name = "cmbNIC";
            this.cmbNIC.Size = new System.Drawing.Size(266, 21);
            this.cmbNIC.TabIndex = 0;
            this.cmbNIC.SelectedIndexChanged += new System.EventHandler(this.cmbNIC_SelectedIndexChanged);
            // 
            // lblNIC
            // 
            this.lblNIC.AutoSize = true;
            this.lblNIC.Location = new System.Drawing.Point(307, 15);
            this.lblNIC.Name = "lblNIC";
            this.lblNIC.Size = new System.Drawing.Size(122, 13);
            this.lblNIC.TabIndex = 1;
            this.lblNIC.Text = "Select network interface";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "1.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(16, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "2.";
            // 
            // txtMAC0
            // 
            this.txtMAC0.Location = new System.Drawing.Point(35, 44);
            this.txtMAC0.Name = "txtMAC0";
            this.txtMAC0.ReadOnly = true;
            this.txtMAC0.Size = new System.Drawing.Size(22, 20);
            this.txtMAC0.TabIndex = 4;
            this.txtMAC0.Text = "00";
            this.txtMAC0.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(63, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(10, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = ":";
            // 
            // txtMAC1
            // 
            this.txtMAC1.Location = new System.Drawing.Point(79, 44);
            this.txtMAC1.Name = "txtMAC1";
            this.txtMAC1.ReadOnly = true;
            this.txtMAC1.Size = new System.Drawing.Size(22, 20);
            this.txtMAC1.TabIndex = 5;
            this.txtMAC1.Text = "02";
            this.txtMAC1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMAC2
            // 
            this.txtMAC2.Location = new System.Drawing.Point(124, 44);
            this.txtMAC2.Name = "txtMAC2";
            this.txtMAC2.ReadOnly = true;
            this.txtMAC2.Size = new System.Drawing.Size(22, 20);
            this.txtMAC2.TabIndex = 6;
            this.txtMAC2.Text = "02";
            this.txtMAC2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(108, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(10, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = ":";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(152, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(10, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = ":";
            // 
            // txtMAC3
            // 
            this.txtMAC3.Location = new System.Drawing.Point(170, 44);
            this.txtMAC3.Name = "txtMAC3";
            this.txtMAC3.Size = new System.Drawing.Size(22, 20);
            this.txtMAC3.TabIndex = 7;
            this.txtMAC3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMAC4
            // 
            this.txtMAC4.Location = new System.Drawing.Point(214, 44);
            this.txtMAC4.Name = "txtMAC4";
            this.txtMAC4.Size = new System.Drawing.Size(22, 20);
            this.txtMAC4.TabIndex = 8;
            this.txtMAC4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtMAC5
            // 
            this.txtMAC5.Location = new System.Drawing.Point(259, 44);
            this.txtMAC5.Name = "txtMAC5";
            this.txtMAC5.Size = new System.Drawing.Size(22, 20);
            this.txtMAC5.TabIndex = 9;
            this.txtMAC5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(198, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(10, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = ":";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(243, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(10, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = ":";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(307, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(98, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Enter MAC address";
            // 
            // cmbDecoder
            // 
            this.cmbDecoder.FormattingEnabled = true;
            this.cmbDecoder.Items.AddRange(new object[] {
            "Amino 110",
            "Amino 130m"});
            this.cmbDecoder.Location = new System.Drawing.Point(35, 81);
            this.cmbDecoder.Name = "cmbDecoder";
            this.cmbDecoder.Size = new System.Drawing.Size(266, 21);
            this.cmbDecoder.TabIndex = 11;
            this.cmbDecoder.SelectedIndexChanged += new System.EventHandler(this.cmbDecoder_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(307, 84);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "Select decoder";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 84);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(16, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "3.";
            // 
            // chkSaveSettings
            // 
            this.chkSaveSettings.AutoSize = true;
            this.chkSaveSettings.Location = new System.Drawing.Point(35, 119);
            this.chkSaveSettings.Name = "chkSaveSettings";
            this.chkSaveSettings.Size = new System.Drawing.Size(90, 17);
            this.chkSaveSettings.TabIndex = 13;
            this.chkSaveSettings.Text = "Sa&ve settings";
            this.chkSaveSettings.UseVisualStyleBackColor = true;
            this.chkSaveSettings.CheckedChanged += new System.EventHandler(this.chkSaveSettings_CheckedChanged);
            // 
            // chkStartWithWindows
            // 
            this.chkStartWithWindows.AutoSize = true;
            this.chkStartWithWindows.Location = new System.Drawing.Point(145, 119);
            this.chkStartWithWindows.Name = "chkStartWithWindows";
            this.chkStartWithWindows.Size = new System.Drawing.Size(117, 17);
            this.chkStartWithWindows.TabIndex = 14;
            this.chkStartWithWindows.Text = "Start with &Windows";
            this.chkStartWithWindows.UseVisualStyleBackColor = true;
            this.chkStartWithWindows.CheckedChanged += new System.EventHandler(this.chkStartWithWindows_CheckedChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 120);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "4.";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(307, 120);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(82, 13);
            this.label12.TabIndex = 15;
            this.label12.Text = "Set preferences";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(35, 176);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(266, 23);
            this.btnStart.TabIndex = 16;
            this.btnStart.Text = "&Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 181);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(16, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "5.";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(307, 181);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 13);
            this.label14.TabIndex = 1;
            this.label14.Text = "Go fetch an IP...";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 232);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(441, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 9;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(369, 207);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(60, 13);
            this.linkLabel1.TabIndex = 17;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Email Muyz";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // chkStartMinimized
            // 
            this.chkStartMinimized.AutoSize = true;
            this.chkStartMinimized.Location = new System.Drawing.Point(35, 142);
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.Size = new System.Drawing.Size(96, 17);
            this.chkStartMinimized.TabIndex = 14;
            this.chkStartMinimized.Text = "Start &minimized";
            this.chkStartMinimized.UseVisualStyleBackColor = true;
            this.chkStartMinimized.CheckedChanged += new System.EventHandler(this.chkStartMinimized_CheckedChanged);
            // 
            // frmDHCP4IPTV
            // 
            this.AcceptButton = this.btnStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 254);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.chkStartMinimized);
            this.Controls.Add(this.chkStartWithWindows);
            this.Controls.Add(this.chkSaveSettings);
            this.Controls.Add(this.cmbDecoder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMAC5);
            this.Controls.Add(this.txtMAC4);
            this.Controls.Add(this.txtMAC2);
            this.Controls.Add(this.txtMAC3);
            this.Controls.Add(this.txtMAC1);
            this.Controls.Add(this.txtMAC0);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblNIC);
            this.Controls.Add(this.cmbNIC);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmDHCP4IPTV";
            this.ShowInTaskbar = false;
            this.Text = "DHCP4IPTV";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbNIC;
        private System.Windows.Forms.Label lblNIC;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMAC0;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMAC1;
        private System.Windows.Forms.TextBox txtMAC2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtMAC3;
        private System.Windows.Forms.TextBox txtMAC4;
        private System.Windows.Forms.TextBox txtMAC5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbDecoder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox chkSaveSettings;
        private System.Windows.Forms.CheckBox chkStartWithWindows;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox chkStartMinimized;
    }
}

