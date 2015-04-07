namespace Queue.Hosts.Media.WinForms
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.folderTextBox = new System.Windows.Forms.TextBox();
            this.mediaSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.portUpDown = new System.Windows.Forms.NumericUpDown();
            this.portLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.loginSettingsControl = new Queue.UI.WinForms.LoginSettingsControl();
            this.stopButton = new System.Windows.Forms.Button();
            this.serviceGroupBox = new System.Windows.Forms.GroupBox();
            this.serviceStatePicture = new System.Windows.Forms.PictureBox();
            this.runServiceButton = new System.Windows.Forms.Button();
            this.installServiceButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.serviceStateTimer = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.settingsGroupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mediaSettingsBindingSource)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.serviceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.groupBox3);
            this.settingsGroupBox.Controls.Add(this.saveButton);
            this.settingsGroupBox.Controls.Add(this.groupBox2);
            this.settingsGroupBox.Controls.Add(this.groupBox1);
            this.settingsGroupBox.Location = new System.Drawing.Point(10, 12);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(384, 325);
            this.settingsGroupBox.TabIndex = 20;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Настройки";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.folderTextBox);
            this.groupBox3.Controls.Add(this.selectFolderButton);
            this.groupBox3.Location = new System.Drawing.Point(189, 232);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(179, 48);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Папка контента";
            // 
            // folderTextBox
            // 
            this.folderTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mediaSettingsBindingSource, "Folder", true));
            this.folderTextBox.Location = new System.Drawing.Point(6, 18);
            this.folderTextBox.Name = "folderTextBox";
            this.folderTextBox.ReadOnly = true;
            this.folderTextBox.Size = new System.Drawing.Size(126, 20);
            this.folderTextBox.TabIndex = 12;
            // 
            // mediaSettingsBindingSource
            // 
            this.mediaSettingsBindingSource.DataSource = typeof(Queue.Media.MediaSettings);
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Location = new System.Drawing.Point(138, 15);
            this.selectFolderButton.Name = "selectFolderButton";
            this.selectFolderButton.Size = new System.Drawing.Size(35, 25);
            this.selectFolderButton.TabIndex = 13;
            this.selectFolderButton.Text = "...";
            this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(293, 286);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.portUpDown);
            this.groupBox2.Controls.Add(this.portLabel);
            this.groupBox2.Location = new System.Drawing.Point(7, 232);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(176, 46);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "HTTP сервис";
            // 
            // portUpDown
            // 
            this.portUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mediaSettingsBindingSource, "Port", true));
            this.portUpDown.Location = new System.Drawing.Point(115, 16);
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
            this.portUpDown.TabIndex = 2;
            this.portUpDown.Value = new decimal(new int[] {
            9090,
            0,
            0,
            0});
            // 
            // portLabel
            // 
            this.portLabel.Location = new System.Drawing.Point(11, 18);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(70, 15);
            this.portLabel.TabIndex = 1;
            this.portLabel.Text = "Порт";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.loginSettingsControl);
            this.groupBox1.Location = new System.Drawing.Point(8, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(370, 200);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Соединение с сервером";
            // 
            // loginSettingsControl
            // 
            this.loginSettingsControl.Location = new System.Drawing.Point(14, 19);
            this.loginSettingsControl.Name = "loginSettingsControl";
            this.loginSettingsControl.Size = new System.Drawing.Size(340, 176);
            this.loginSettingsControl.TabIndex = 0;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(210, 411);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(184, 30);
            this.stopButton.TabIndex = 19;
            this.stopButton.Text = "Остановить";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // serviceGroupBox
            // 
            this.serviceGroupBox.Controls.Add(this.serviceStatePicture);
            this.serviceGroupBox.Controls.Add(this.runServiceButton);
            this.serviceGroupBox.Controls.Add(this.installServiceButton);
            this.serviceGroupBox.Location = new System.Drawing.Point(10, 343);
            this.serviceGroupBox.Name = "serviceGroupBox";
            this.serviceGroupBox.Size = new System.Drawing.Size(384, 62);
            this.serviceGroupBox.TabIndex = 18;
            this.serviceGroupBox.TabStop = false;
            this.serviceGroupBox.Text = "Служба";
            // 
            // serviceStatePicture
            // 
            this.serviceStatePicture.Location = new System.Drawing.Point(349, 19);
            this.serviceStatePicture.Name = "serviceStatePicture";
            this.serviceStatePicture.Size = new System.Drawing.Size(24, 24);
            this.serviceStatePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.serviceStatePicture.TabIndex = 2;
            this.serviceStatePicture.TabStop = false;
            // 
            // runServiceButton
            // 
            this.runServiceButton.Location = new System.Drawing.Point(154, 19);
            this.runServiceButton.Name = "runServiceButton";
            this.runServiceButton.Size = new System.Drawing.Size(127, 23);
            this.runServiceButton.TabIndex = 1;
            this.runServiceButton.Text = "Запустить службу";
            this.runServiceButton.UseVisualStyleBackColor = true;
            this.runServiceButton.Click += new System.EventHandler(this.runServiceButton_Click);
            // 
            // installServiceButton
            // 
            this.installServiceButton.Location = new System.Drawing.Point(21, 19);
            this.installServiceButton.Name = "installServiceButton";
            this.installServiceButton.Size = new System.Drawing.Size(127, 23);
            this.installServiceButton.TabIndex = 0;
            this.installServiceButton.Text = "Установить службу";
            this.installServiceButton.UseVisualStyleBackColor = true;
            this.installServiceButton.Click += new System.EventHandler(this.installServiceButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(10, 411);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(180, 30);
            this.startButton.TabIndex = 17;
            this.startButton.Text = "Запустить";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Junte Queue Media";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // serviceStateTimer
            // 
            this.serviceStateTimer.Interval = 1000;
            this.serviceStateTimer.Tick += new System.EventHandler(this.serviceStateTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 452);
            this.Controls.Add(this.settingsGroupBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.serviceGroupBox);
            this.Controls.Add(this.startButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(420, 490);
            this.MinimumSize = new System.Drawing.Size(420, 490);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Медиа";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.settingsGroupBox.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mediaSettingsBindingSource)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.serviceGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown portUpDown;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox serviceGroupBox;
        private System.Windows.Forms.PictureBox serviceStatePicture;
        private System.Windows.Forms.Button runServiceButton;
        private System.Windows.Forms.Button installServiceButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox folderTextBox;
        private System.Windows.Forms.Button selectFolderButton;
        private Queue.UI.WinForms.LoginSettingsControl loginSettingsControl;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Timer serviceStateTimer;
        private System.Windows.Forms.BindingSource mediaSettingsBindingSource;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

