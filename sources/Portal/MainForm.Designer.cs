namespace Queue.Portal
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.portUpDown = new System.Windows.Forms.NumericUpDown();
            this.portLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.serverGroupBox = new System.Windows.Forms.GroupBox();
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.UACButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openButton = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutButton = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).BeginInit();
            this.serverGroupBox.SuspendLayout();
            this.topMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // portUpDown
            // 
            this.portUpDown.Location = new System.Drawing.Point(95, 25);
            this.portUpDown.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.portUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.portUpDown.Name = "portUpDown";
            this.portUpDown.Size = new System.Drawing.Size(55, 20);
            this.portUpDown.TabIndex = 0;
            this.portUpDown.Value = new decimal(new int[] {
            9090,
            0,
            0,
            0});
            // 
            // portLabel
            // 
            this.portLabel.Location = new System.Drawing.Point(10, 30);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(70, 15);
            this.portLabel.TabIndex = 0;
            this.portLabel.Text = "HTTP порт";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(155, 20);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 25);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Запуск";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // serverGroupBox
            // 
            this.serverGroupBox.Controls.Add(this.portLabel);
            this.serverGroupBox.Controls.Add(this.portUpDown);
            this.serverGroupBox.Controls.Add(this.startButton);
            this.serverGroupBox.Location = new System.Drawing.Point(10, 30);
            this.serverGroupBox.Name = "serverGroupBox";
            this.serverGroupBox.Size = new System.Drawing.Size(240, 60);
            this.serverGroupBox.TabIndex = 0;
            this.serverGroupBox.TabStop = false;
            this.serverGroupBox.Text = "Параметры сервера";
            // 
            // topMenu
            // 
            this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UACButton,
            this.openButton,
            this.logoutButton});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(259, 24);
            this.topMenu.TabIndex = 0;
            this.topMenu.Text = "menuStrip1";
            // 
            // UACButton
            // 
            this.UACButton.Image = ((System.Drawing.Image)(resources.GetObject("UACButton.Image")));
            this.UACButton.Name = "UACButton";
            this.UACButton.Size = new System.Drawing.Size(59, 20);
            this.UACButton.Text = "UAC";
            this.UACButton.Click += new System.EventHandler(this.UACButton_Click);
            // 
            // openButton
            // 
            this.openButton.Enabled = false;
            this.openButton.Image = ((System.Drawing.Image)(resources.GetObject("openButton.Image")));
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(82, 20);
            this.openButton.Text = "Открыть";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // logoutButton
            // 
            this.logoutButton.Image = ((System.Drawing.Image)(resources.GetObject("logoutButton.Image")));
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(70, 20);
            this.logoutButton.Text = "Выйти";
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(259, 97);
            this.Controls.Add(this.topMenu);
            this.Controls.Add(this.serverGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(275, 135);
            this.Name = "MainForm";
            this.Text = "Портал";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).EndInit();
            this.serverGroupBox.ResumeLayout(false);
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown portUpDown;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox serverGroupBox;
        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.ToolStripMenuItem openButton;
        private System.Windows.Forms.ToolStripMenuItem logoutButton;
        private System.Windows.Forms.ToolStripMenuItem UACButton;
    }
}

