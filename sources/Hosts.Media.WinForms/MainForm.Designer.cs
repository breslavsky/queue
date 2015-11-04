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
            this.mediaSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.stopButton = new System.Windows.Forms.Button();
            this.serviceGroupBox = new System.Windows.Forms.GroupBox();
            this.installServiceButton = new System.Windows.Forms.Button();
            this.serviceStatePicture = new System.Windows.Forms.PictureBox();
            this.startServiceButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.serviceStateTimer = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.folderGroupBox = new System.Windows.Forms.GroupBox();
            this.mediaFolderTextBox = new System.Windows.Forms.TextBox();
            this.selectMediaFolderButton = new System.Windows.Forms.Button();
            this.httpServiceGroupBox = new System.Windows.Forms.GroupBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.portUpDown = new System.Windows.Forms.NumericUpDown();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.mediaServiceSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.mediaSettingsBindingSource)).BeginInit();
            this.serviceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.folderGroupBox.SuspendLayout();
            this.httpServiceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).BeginInit();
            this.settingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mediaServiceSettingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // mediaSettingsBindingSource
            // 
            this.mediaSettingsBindingSource.DataSource = typeof(Queue.Media.MediaSettings);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(215, 285);
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
            this.serviceGroupBox.Location = new System.Drawing.Point(5, 220);
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
            this.startButton.Location = new System.Drawing.Point(10, 285);
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
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(5, 70);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(395, 35);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // folderGroupBox
            // 
            this.folderGroupBox.Controls.Add(this.mediaFolderTextBox);
            this.folderGroupBox.Controls.Add(this.selectMediaFolderButton);
            this.folderGroupBox.Location = new System.Drawing.Point(205, 20);
            this.folderGroupBox.Name = "folderGroupBox";
            this.folderGroupBox.Size = new System.Drawing.Size(195, 45);
            this.folderGroupBox.TabIndex = 14;
            this.folderGroupBox.TabStop = false;
            this.folderGroupBox.Text = "Папка контента";
            // 
            // mediaFolderTextBox
            // 
            this.mediaFolderTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mediaServiceSettingsBindingSource, "MediaFolder", true));
            this.mediaFolderTextBox.Location = new System.Drawing.Point(6, 18);
            this.mediaFolderTextBox.Name = "mediaFolderTextBox";
            this.mediaFolderTextBox.ReadOnly = true;
            this.mediaFolderTextBox.Size = new System.Drawing.Size(144, 20);
            this.mediaFolderTextBox.TabIndex = 12;
            // 
            // selectMediaFolderButton
            // 
            this.selectMediaFolderButton.Location = new System.Drawing.Point(155, 15);
            this.selectMediaFolderButton.Name = "selectMediaFolderButton";
            this.selectMediaFolderButton.Size = new System.Drawing.Size(35, 25);
            this.selectMediaFolderButton.TabIndex = 13;
            this.selectMediaFolderButton.Text = "...";
            this.selectMediaFolderButton.Click += new System.EventHandler(this.selectMediaFolderButton_Click);
            // 
            // httpServiceGroupBox
            // 
            this.httpServiceGroupBox.Controls.Add(this.portLabel);
            this.httpServiceGroupBox.Controls.Add(this.portUpDown);
            this.httpServiceGroupBox.Location = new System.Drawing.Point(5, 20);
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
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.httpServiceGroupBox);
            this.settingsGroupBox.Controls.Add(this.folderGroupBox);
            this.settingsGroupBox.Controls.Add(this.saveButton);
            this.settingsGroupBox.Location = new System.Drawing.Point(5, 100);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(405, 115);
            this.settingsGroupBox.TabIndex = 20;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Настройки";
            // 
            // mediaServiceSettingsBindingSource
            // 
            this.mediaServiceSettingsBindingSource.DataSource = typeof(Queue.Services.Media.Settings.MediaServiceSettings);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 322);
            this.Controls.Add(this.logoPictureBox);
            this.Controls.Add(this.settingsGroupBox);
            this.Controls.Add(this.serviceGroupBox);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.stopButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(430, 360);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Медиа-служба";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.mediaSettingsBindingSource)).EndInit();
            this.serviceGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.folderGroupBox.ResumeLayout(false);
            this.folderGroupBox.PerformLayout();
            this.httpServiceGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).EndInit();
            this.settingsGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mediaServiceSettingsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox serviceGroupBox;
        private System.Windows.Forms.PictureBox serviceStatePicture;
        private System.Windows.Forms.Button startServiceButton;
        private System.Windows.Forms.Button installServiceButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Timer serviceStateTimer;
        private System.Windows.Forms.BindingSource mediaSettingsBindingSource;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox folderGroupBox;
        private System.Windows.Forms.TextBox mediaFolderTextBox;
        private System.Windows.Forms.Button selectMediaFolderButton;
        private System.Windows.Forms.GroupBox httpServiceGroupBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.NumericUpDown portUpDown;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.BindingSource mediaServiceSettingsBindingSource;
    }
}

