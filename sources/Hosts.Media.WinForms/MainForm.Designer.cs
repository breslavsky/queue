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
            this.loginSettingsControl = new Queue.UI.WinForms.LoginSettingsControl();
            this.httpServiceGroupBox = new System.Windows.Forms.GroupBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.portUpDown = new System.Windows.Forms.NumericUpDown();
            this.mediaSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.folderGroupBox = new System.Windows.Forms.GroupBox();
            this.folderTextBox = new System.Windows.Forms.TextBox();
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.serviceGroupBox = new System.Windows.Forms.GroupBox();
            this.installServiceButton = new System.Windows.Forms.Button();
            this.serviceStatePicture = new System.Windows.Forms.PictureBox();
            this.startServiceButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.serviceStateTimer = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.settingsGroupBox.SuspendLayout();
            this.httpServiceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaSettingsBindingSource)).BeginInit();
            this.folderGroupBox.SuspendLayout();
            this.serviceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.loginSettingsControl);
            this.settingsGroupBox.Controls.Add(this.httpServiceGroupBox);
            this.settingsGroupBox.Controls.Add(this.folderGroupBox);
            this.settingsGroupBox.Controls.Add(this.saveButton);
            this.settingsGroupBox.Location = new System.Drawing.Point(5, 100);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(405, 285);
            this.settingsGroupBox.TabIndex = 20;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Настройки";
            // 
            // loginSettingsControl
            // 
            this.loginSettingsControl.Location = new System.Drawing.Point(5, 20);
            this.loginSettingsControl.Name = "loginSettingsControl";
            this.loginSettingsControl.Size = new System.Drawing.Size(330, 170);
            this.loginSettingsControl.TabIndex = 0;
            // 
            // httpServiceGroupBox
            // 
            this.httpServiceGroupBox.Controls.Add(this.portLabel);
            this.httpServiceGroupBox.Controls.Add(this.portUpDown);
            this.httpServiceGroupBox.Location = new System.Drawing.Point(5, 195);
            this.httpServiceGroupBox.Name = "httpServiceGroupBox";
            this.httpServiceGroupBox.Size = new System.Drawing.Size(195, 45);
            this.httpServiceGroupBox.TabIndex = 3;
            this.httpServiceGroupBox.TabStop = false;
            this.httpServiceGroupBox.Text = "HTTP сервис";
            // 
            // portLabel
            // 
            this.portLabel.Location = new System.Drawing.Point(5, 15);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(130, 25);
            this.portLabel.TabIndex = 1;
            this.portLabel.Text = "Порт";
            this.portLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // portUpDown
            // 
            this.portUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mediaSettingsBindingSource, "Port", true));
            this.portUpDown.Location = new System.Drawing.Point(135, 20);
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
            // mediaSettingsBindingSource
            // 
            this.mediaSettingsBindingSource.DataSource = typeof(Queue.Media.MediaSettings);
            // 
            // folderGroupBox
            // 
            this.folderGroupBox.Controls.Add(this.folderTextBox);
            this.folderGroupBox.Controls.Add(this.selectFolderButton);
            this.folderGroupBox.Location = new System.Drawing.Point(205, 195);
            this.folderGroupBox.Name = "folderGroupBox";
            this.folderGroupBox.Size = new System.Drawing.Size(195, 45);
            this.folderGroupBox.TabIndex = 14;
            this.folderGroupBox.TabStop = false;
            this.folderGroupBox.Text = "Папка контента";
            // 
            // folderTextBox
            // 
            this.folderTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mediaSettingsBindingSource, "Folder", true));
            this.folderTextBox.Location = new System.Drawing.Point(6, 18);
            this.folderTextBox.Name = "folderTextBox";
            this.folderTextBox.ReadOnly = true;
            this.folderTextBox.Size = new System.Drawing.Size(144, 20);
            this.folderTextBox.TabIndex = 12;
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Location = new System.Drawing.Point(155, 15);
            this.selectFolderButton.Name = "selectFolderButton";
            this.selectFolderButton.Size = new System.Drawing.Size(35, 25);
            this.selectFolderButton.TabIndex = 13;
            this.selectFolderButton.Text = "...";
            this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(5, 245);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(395, 35);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(215, 455);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(190, 30);
            this.stopButton.TabIndex = 19;
            this.stopButton.Text = "Остановить";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // serviceGroupBox
            // 
            this.serviceGroupBox.Controls.Add(this.installServiceButton);
            this.serviceGroupBox.Controls.Add(this.serviceStatePicture);
            this.serviceGroupBox.Controls.Add(this.startServiceButton);
            this.serviceGroupBox.Location = new System.Drawing.Point(5, 390);
            this.serviceGroupBox.Name = "serviceGroupBox";
            this.serviceGroupBox.Size = new System.Drawing.Size(405, 60);
            this.serviceGroupBox.TabIndex = 18;
            this.serviceGroupBox.TabStop = false;
            this.serviceGroupBox.Text = "Служба";
            // 
            // installServiceButton
            // 
            this.installServiceButton.Location = new System.Drawing.Point(5, 20);
            this.installServiceButton.Name = "installServiceButton";
            this.installServiceButton.Size = new System.Drawing.Size(180, 35);
            this.installServiceButton.TabIndex = 0;
            this.installServiceButton.Text = "Установить службу";
            this.installServiceButton.UseVisualStyleBackColor = true;
            this.installServiceButton.Click += new System.EventHandler(this.installServiceButton_Click);
            // 
            // serviceStatePicture
            // 
            this.serviceStatePicture.Location = new System.Drawing.Point(190, 25);
            this.serviceStatePicture.Name = "serviceStatePicture";
            this.serviceStatePicture.Size = new System.Drawing.Size(24, 24);
            this.serviceStatePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.serviceStatePicture.TabIndex = 2;
            this.serviceStatePicture.TabStop = false;
            // 
            // startServiceButton
            // 
            this.startServiceButton.Location = new System.Drawing.Point(220, 20);
            this.startServiceButton.Name = "startServiceButton";
            this.startServiceButton.Size = new System.Drawing.Size(180, 35);
            this.startServiceButton.TabIndex = 1;
            this.startServiceButton.Text = "Запустить службу";
            this.startServiceButton.UseVisualStyleBackColor = true;
            this.startServiceButton.Click += new System.EventHandler(this.startServiceButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(10, 455);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(190, 30);
            this.startButton.TabIndex = 17;
            this.startButton.Text = "Запустить";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // serviceStateTimer
            // 
            this.serviceStateTimer.Interval = 1000;
            this.serviceStateTimer.Tick += new System.EventHandler(this.serviceStateTimer_Tick);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(415, 95);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.logoPictureBox.TabIndex = 21;
            this.logoPictureBox.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 492);
            this.Controls.Add(this.logoPictureBox);
            this.Controls.Add(this.settingsGroupBox);
            this.Controls.Add(this.serviceGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(430, 530);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Медиа-служба";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.settingsGroupBox.ResumeLayout(false);
            this.httpServiceGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaSettingsBindingSource)).EndInit();
            this.folderGroupBox.ResumeLayout(false);
            this.folderGroupBox.PerformLayout();
            this.serviceGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox httpServiceGroupBox;
        private System.Windows.Forms.NumericUpDown portUpDown;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox serviceGroupBox;
        private System.Windows.Forms.PictureBox serviceStatePicture;
        private System.Windows.Forms.Button startServiceButton;
        private System.Windows.Forms.Button installServiceButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox folderGroupBox;
        private System.Windows.Forms.TextBox folderTextBox;
        private System.Windows.Forms.Button selectFolderButton;
        private Queue.UI.WinForms.LoginSettingsControl loginSettingsControl;
        private System.Windows.Forms.Timer serviceStateTimer;
        private System.Windows.Forms.BindingSource mediaSettingsBindingSource;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.PictureBox logoPictureBox;
    }
}

