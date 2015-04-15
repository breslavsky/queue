namespace Queue.Hosts.Metric.WinForms
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
            this.stopButton = new System.Windows.Forms.Button();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.databaseGroupBox = new System.Windows.Forms.GroupBox();
            this.editDatabaseSettingsControl = new Junte.UI.WinForms.NHibernate.EditDatabaseSettingsUserControl();
            this.startButton = new System.Windows.Forms.Button();
            this.serviceStateTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.serviceGroupBox = new System.Windows.Forms.GroupBox();
            this.serviceStatePicture = new System.Windows.Forms.PictureBox();
            this.runServiceButton = new System.Windows.Forms.Button();
            this.installServiseButton = new System.Windows.Forms.Button();
            this.settingsGroupBox.SuspendLayout();
            this.databaseGroupBox.SuspendLayout();
            this.layoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.serviceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).BeginInit();
            this.SuspendLayout();
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(200, 5);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(190, 30);
            this.stopButton.TabIndex = 16;
            this.stopButton.Text = "Остановить сбор метрик";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.saveButton);
            this.settingsGroupBox.Controls.Add(this.databaseGroupBox);
            this.settingsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsGroupBox.Location = new System.Drawing.Point(3, 103);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(398, 268);
            this.settingsGroupBox.TabIndex = 15;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Настройки";
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(5, 231);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(385, 30);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // databaseGroupBox
            // 
            this.databaseGroupBox.Controls.Add(this.editDatabaseSettingsControl);
            this.databaseGroupBox.Location = new System.Drawing.Point(5, 19);
            this.databaseGroupBox.Name = "databaseGroupBox";
            this.databaseGroupBox.Size = new System.Drawing.Size(385, 206);
            this.databaseGroupBox.TabIndex = 7;
            this.databaseGroupBox.TabStop = false;
            this.databaseGroupBox.Text = "База данных";
            // 
            // editDatabaseSettingsControl
            // 
            this.editDatabaseSettingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editDatabaseSettingsControl.Location = new System.Drawing.Point(3, 16);
            this.editDatabaseSettingsControl.Name = "editDatabaseSettingsControl";
            this.editDatabaseSettingsControl.Size = new System.Drawing.Size(379, 187);
            this.editDatabaseSettingsControl.TabIndex = 1;
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(5, 5);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(190, 30);
            this.startButton.TabIndex = 13;
            this.startButton.Text = "Запустить сбор метрик";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // serviceStateTimer
            // 
            this.serviceStateTimer.Interval = 1000;
            this.serviceStateTimer.Tick += new System.EventHandler(this.serviceStateTimer_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "Junte Queue Metric";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // layoutPanel
            // 
            this.layoutPanel.ColumnCount = 1;
            this.layoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.Controls.Add(this.pictureBox1, 0, 0);
            this.layoutPanel.Controls.Add(this.panel1, 0, 3);
            this.layoutPanel.Controls.Add(this.settingsGroupBox, 0, 1);
            this.layoutPanel.Controls.Add(this.serviceGroupBox, 0, 2);
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanel.Location = new System.Drawing.Point(0, 0);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.RowCount = 4;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.layoutPanel.Size = new System.Drawing.Size(404, 489);
            this.layoutPanel.TabIndex = 17;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(398, 94);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 17;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.startButton);
            this.panel1.Controls.Add(this.stopButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 442);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(398, 44);
            this.panel1.TabIndex = 0;
            // 
            // serviceGroupBox
            // 
            this.serviceGroupBox.Controls.Add(this.serviceStatePicture);
            this.serviceGroupBox.Controls.Add(this.runServiceButton);
            this.serviceGroupBox.Controls.Add(this.installServiseButton);
            this.serviceGroupBox.Location = new System.Drawing.Point(3, 377);
            this.serviceGroupBox.Name = "serviceGroupBox";
            this.serviceGroupBox.Size = new System.Drawing.Size(395, 59);
            this.serviceGroupBox.TabIndex = 14;
            this.serviceGroupBox.TabStop = false;
            this.serviceGroupBox.Text = "Служба";
            // 
            // serviceStatePicture
            // 
            this.serviceStatePicture.Location = new System.Drawing.Point(185, 25);
            this.serviceStatePicture.Name = "serviceStatePicture";
            this.serviceStatePicture.Size = new System.Drawing.Size(24, 24);
            this.serviceStatePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.serviceStatePicture.TabIndex = 2;
            this.serviceStatePicture.TabStop = false;
            // 
            // runServiceButton
            // 
            this.runServiceButton.Location = new System.Drawing.Point(225, 20);
            this.runServiceButton.Name = "runServiceButton";
            this.runServiceButton.Size = new System.Drawing.Size(165, 35);
            this.runServiceButton.TabIndex = 1;
            this.runServiceButton.Text = "Запустить службу";
            this.runServiceButton.UseVisualStyleBackColor = true;
            this.runServiceButton.Click += new System.EventHandler(this.runServiceButton_Click);
            // 
            // installServiseButton
            // 
            this.installServiseButton.Location = new System.Drawing.Point(5, 20);
            this.installServiseButton.Name = "installServiseButton";
            this.installServiseButton.Size = new System.Drawing.Size(165, 35);
            this.installServiseButton.TabIndex = 0;
            this.installServiseButton.Text = "Установить службу";
            this.installServiseButton.UseVisualStyleBackColor = true;
            this.installServiseButton.Click += new System.EventHandler(this.installServiseButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 489);
            this.Controls.Add(this.layoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(420, 527);
            this.MinimumSize = new System.Drawing.Size(420, 527);
            this.Name = "MainForm";
            this.Text = "Метрики";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.settingsGroupBox.ResumeLayout(false);
            this.databaseGroupBox.ResumeLayout(false);
            this.layoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.serviceGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox databaseGroupBox;
        private System.Windows.Forms.Button startButton;
        private Junte.UI.WinForms.NHibernate.EditDatabaseSettingsUserControl editDatabaseSettingsControl;
        private System.Windows.Forms.Timer serviceStateTimer;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox serviceGroupBox;
        private System.Windows.Forms.PictureBox serviceStatePicture;
        private System.Windows.Forms.Button runServiceButton;
        private System.Windows.Forms.Button installServiseButton;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

