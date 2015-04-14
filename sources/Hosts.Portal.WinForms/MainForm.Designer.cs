namespace Hosts.Portal.WinForms
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
            this.stopButton = new System.Windows.Forms.Button();
            this.serviceGroupBox = new System.Windows.Forms.GroupBox();
            this.serviceStatePicture = new System.Windows.Forms.PictureBox();
            this.runServiceButton = new System.Windows.Forms.Button();
            this.installServiseButton = new System.Windows.Forms.Button();
            this.startButton = new System.Windows.Forms.Button();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.serverGroupBox = new System.Windows.Forms.GroupBox();
            this.portUpDown = new System.Windows.Forms.NumericUpDown();
            this.portLabel = new System.Windows.Forms.Label();
            this.serviceStateTimer = new System.Windows.Forms.Timer();
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.loginSettingsControl = new Queue.UI.WinForms.LoginSettingsControl();
            this.portalSettingsBindingSource = new System.Windows.Forms.BindingSource();
            this.serviceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).BeginInit();
            this.settingsGroupBox.SuspendLayout();
            this.serverGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).BeginInit();
            this.layoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portalSettingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(205, 5);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(190, 30);
            this.stopButton.TabIndex = 15;
            this.stopButton.Text = "Остановить";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // serviceGroupBox
            // 
            this.serviceGroupBox.Controls.Add(this.serviceStatePicture);
            this.serviceGroupBox.Controls.Add(this.runServiceButton);
            this.serviceGroupBox.Controls.Add(this.installServiseButton);
            this.serviceGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceGroupBox.Location = new System.Drawing.Point(3, 403);
            this.serviceGroupBox.Name = "serviceGroupBox";
            this.serviceGroupBox.Size = new System.Drawing.Size(398, 59);
            this.serviceGroupBox.TabIndex = 14;
            this.serviceGroupBox.TabStop = false;
            this.serviceGroupBox.Text = "Служба";
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
            // runServiceButton
            // 
            this.runServiceButton.Location = new System.Drawing.Point(220, 20);
            this.runServiceButton.Margin = new System.Windows.Forms.Padding(5);
            this.runServiceButton.Name = "runServiceButton";
            this.runServiceButton.Size = new System.Drawing.Size(175, 35);
            this.runServiceButton.TabIndex = 1;
            this.runServiceButton.Text = "Запустить службу";
            this.runServiceButton.UseVisualStyleBackColor = true;
            this.runServiceButton.Click += new System.EventHandler(this.runServiceButton_Click);
            // 
            // installServiseButton
            // 
            this.installServiseButton.Location = new System.Drawing.Point(5, 20);
            this.installServiseButton.Margin = new System.Windows.Forms.Padding(5);
            this.installServiseButton.Name = "installServiseButton";
            this.installServiseButton.Size = new System.Drawing.Size(180, 35);
            this.installServiseButton.TabIndex = 0;
            this.installServiseButton.Text = "Установить службу";
            this.installServiseButton.UseVisualStyleBackColor = true;
            this.installServiseButton.Click += new System.EventHandler(this.installServiseButton_Click);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(5, 5);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(195, 30);
            this.startButton.TabIndex = 13;
            this.startButton.Text = "Запустить";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.loginSettingsControl);
            this.settingsGroupBox.Controls.Add(this.saveButton);
            this.settingsGroupBox.Controls.Add(this.serverGroupBox);
            this.settingsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsGroupBox.Location = new System.Drawing.Point(3, 103);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(398, 294);
            this.settingsGroupBox.TabIndex = 16;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Настройки";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(5, 260);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(390, 35);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // serverGroupBox
            // 
            this.serverGroupBox.Controls.Add(this.portUpDown);
            this.serverGroupBox.Controls.Add(this.portLabel);
            this.serverGroupBox.Location = new System.Drawing.Point(5, 210);
            this.serverGroupBox.Name = "serverGroupBox";
            this.serverGroupBox.Size = new System.Drawing.Size(176, 46);
            this.serverGroupBox.TabIndex = 3;
            this.serverGroupBox.TabStop = false;
            this.serverGroupBox.Text = "HTTP сервис";
            // 
            // portUpDown
            // 
            this.portUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.portalSettingsBindingSource, "Port", true));
            this.portUpDown.Location = new System.Drawing.Point(115, 20);
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
            this.portLabel.Location = new System.Drawing.Point(10, 15);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(105, 25);
            this.portLabel.TabIndex = 1;
            this.portLabel.Text = "Порт";
            this.portLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceStateTimer
            // 
            this.serviceStateTimer.Interval = 1000;
            this.serviceStateTimer.Tick += new System.EventHandler(this.serviceStateTimer_Tick);
            // 
            // layoutPanel
            // 
            this.layoutPanel.ColumnCount = 1;
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.Controls.Add(this.pictureBox1, 0, 0);
            this.layoutPanel.Controls.Add(this.settingsGroupBox, 0, 1);
            this.layoutPanel.Controls.Add(this.panel1, 0, 3);
            this.layoutPanel.Controls.Add(this.serviceGroupBox, 0, 2);
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanel.Location = new System.Drawing.Point(0, 0);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.RowCount = 4;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.layoutPanel.Size = new System.Drawing.Size(404, 512);
            this.layoutPanel.TabIndex = 17;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(398, 94);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 18;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.startButton);
            this.panel1.Controls.Add(this.stopButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 468);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(398, 41);
            this.panel1.TabIndex = 17;
            // 
            // loginSettingsControl
            // 
            this.loginSettingsControl.Location = new System.Drawing.Point(10, 15);
            this.loginSettingsControl.Name = "loginSettingsControl";
            this.loginSettingsControl.Size = new System.Drawing.Size(345, 190);
            this.loginSettingsControl.TabIndex = 0;
            // 
            // portalSettingsBindingSource
            // 
            this.portalSettingsBindingSource.DataSource = typeof(Queue.Portal.PortalSettings);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 512);
            this.Controls.Add(this.layoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(420, 550);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Портал";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.serviceGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).EndInit();
            this.settingsGroupBox.ResumeLayout(false);
            this.serverGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).EndInit();
            this.layoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.portalSettingsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox serviceGroupBox;
        private System.Windows.Forms.PictureBox serviceStatePicture;
        private System.Windows.Forms.Button runServiceButton;
        private System.Windows.Forms.Button installServiseButton;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.GroupBox serverGroupBox;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.NumericUpDown portUpDown;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.BindingSource portalSettingsBindingSource;
        private System.Windows.Forms.Timer serviceStateTimer;
        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Queue.UI.WinForms.LoginSettingsControl loginSettingsControl;
    }
}

