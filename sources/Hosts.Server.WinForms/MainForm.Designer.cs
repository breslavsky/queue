namespace Queue.Hosts.Server.WinForms
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.startButton = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.stopButton = new System.Windows.Forms.Button();
            this.serviceGroupBox = new System.Windows.Forms.GroupBox();
            this.serviceStatePicture = new System.Windows.Forms.PictureBox();
            this.runServiceButton = new System.Windows.Forms.Button();
            this.installServiseButton = new System.Windows.Forms.Button();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.languageControl = new Queue.UI.WinForms.EnumItemControl();
            this.httpCheckBox = new System.Windows.Forms.CheckBox();
            this.tcpCheckBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.debugCheckBox = new System.Windows.Forms.CheckBox();
            this.databaseGroupBox = new System.Windows.Forms.GroupBox();
            this.editDatabaseSettingsControl = new Junte.UI.WinForms.NHibernate.EditDatabaseSettingsUserControl();
            this.tcpGroupBox = new System.Windows.Forms.GroupBox();
            this.tcpPortUpDown = new System.Windows.Forms.NumericUpDown();
            this.tcpHostTextBox = new System.Windows.Forms.TextBox();
            this.httpGroupBox = new System.Windows.Forms.GroupBox();
            this.httpPortUpDown = new System.Windows.Forms.NumericUpDown();
            this.httpHostTextBox = new System.Windows.Forms.TextBox();
            this.serviceStateTimer = new System.Windows.Forms.Timer(this.components);
            this.layoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.serviceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).BeginInit();
            this.settingsGroupBox.SuspendLayout();
            this.databaseGroupBox.SuspendLayout();
            this.tcpGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcpPortUpDown)).BeginInit();
            this.httpGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.httpPortUpDown)).BeginInit();
            this.layoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(5, 5);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(190, 30);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Запустить сервер";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // panel
            // 
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(406, 591);
            this.panel.TabIndex = 2;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(400, 94);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(205, 5);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(190, 30);
            this.stopButton.TabIndex = 12;
            this.stopButton.Text = "Остановить сервер";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // serviceGroupBox
            // 
            this.serviceGroupBox.Controls.Add(this.serviceStatePicture);
            this.serviceGroupBox.Controls.Add(this.runServiceButton);
            this.serviceGroupBox.Controls.Add(this.installServiseButton);
            this.serviceGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceGroupBox.Location = new System.Drawing.Point(3, 479);
            this.serviceGroupBox.Name = "serviceGroupBox";
            this.serviceGroupBox.Size = new System.Drawing.Size(400, 59);
            this.serviceGroupBox.TabIndex = 2;
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
            this.runServiceButton.Location = new System.Drawing.Point(235, 20);
            this.runServiceButton.Name = "runServiceButton";
            this.runServiceButton.Size = new System.Drawing.Size(160, 35);
            this.runServiceButton.TabIndex = 1;
            this.runServiceButton.Text = "Запустить службу";
            this.runServiceButton.UseVisualStyleBackColor = true;
            this.runServiceButton.Click += new System.EventHandler(this.runServiceButton_Click);
            // 
            // installServiseButton
            // 
            this.installServiseButton.Location = new System.Drawing.Point(5, 20);
            this.installServiseButton.Name = "installServiseButton";
            this.installServiseButton.Size = new System.Drawing.Size(160, 35);
            this.installServiseButton.TabIndex = 0;
            this.installServiseButton.Text = "Установить службу";
            this.installServiseButton.UseVisualStyleBackColor = true;
            this.installServiseButton.Click += new System.EventHandler(this.installServiseButton_Click);
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.httpCheckBox);
            this.settingsGroupBox.Controls.Add(this.tcpCheckBox);
            this.settingsGroupBox.Controls.Add(this.label1);
            this.settingsGroupBox.Controls.Add(this.languageControl);
            this.settingsGroupBox.Controls.Add(this.saveButton);
            this.settingsGroupBox.Controls.Add(this.debugCheckBox);
            this.settingsGroupBox.Controls.Add(this.databaseGroupBox);
            this.settingsGroupBox.Controls.Add(this.tcpGroupBox);
            this.settingsGroupBox.Controls.Add(this.httpGroupBox);
            this.settingsGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsGroupBox.Location = new System.Drawing.Point(3, 103);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(400, 370);
            this.settingsGroupBox.TabIndex = 11;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Настройки";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 305);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Язык";
            // 
            // languageControl
            // 
            this.languageControl.Location = new System.Drawing.Point(240, 300);
            this.languageControl.Name = "languageControl";
            this.languageControl.Size = new System.Drawing.Size(150, 21);
            this.languageControl.TabIndex = 10;
            this.languageControl.SelectedChanged += new System.EventHandler<System.EventArgs>(this.languageControl_SelectedChanged);
            // 
            // httpCheckBox
            // 
            this.httpCheckBox.AutoSize = true;
            this.httpCheckBox.Location = new System.Drawing.Point(220, 235);
            this.httpCheckBox.Name = "httpCheckBox";
            this.httpCheckBox.Size = new System.Drawing.Size(94, 17);
            this.httpCheckBox.TabIndex = 5;
            this.httpCheckBox.Text = "HTTP-сервис";
            this.httpCheckBox.UseVisualStyleBackColor = true;
            this.httpCheckBox.CheckedChanged += new System.EventHandler(this.httpCheckBox_CheckedChanged);
            this.httpCheckBox.Leave += new System.EventHandler(this.httpCheckBox_Leave);
            // 
            // tcpCheckBox
            // 
            this.tcpCheckBox.AutoSize = true;
            this.tcpCheckBox.Location = new System.Drawing.Point(20, 235);
            this.tcpCheckBox.Name = "tcpCheckBox";
            this.tcpCheckBox.Size = new System.Drawing.Size(86, 17);
            this.tcpCheckBox.TabIndex = 3;
            this.tcpCheckBox.Text = "TCP-сервис";
            this.tcpCheckBox.UseVisualStyleBackColor = true;
            this.tcpCheckBox.CheckedChanged += new System.EventHandler(this.tcpCheckBox_CheckedChanged);
            this.tcpCheckBox.Leave += new System.EventHandler(this.tcpCheckBox_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(10, 330);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(380, 30);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // debugCheckBox
            // 
            this.debugCheckBox.AutoSize = true;
            this.debugCheckBox.Location = new System.Drawing.Point(15, 300);
            this.debugCheckBox.Name = "debugCheckBox";
            this.debugCheckBox.Size = new System.Drawing.Size(105, 17);
            this.debugCheckBox.TabIndex = 8;
            this.debugCheckBox.Text = "Режим отладки";
            this.debugCheckBox.UseVisualStyleBackColor = true;
            this.debugCheckBox.Leave += new System.EventHandler(this.debugCheckBox_Leave);
            // 
            // databaseGroupBox
            // 
            this.databaseGroupBox.Controls.Add(this.editDatabaseSettingsControl);
            this.databaseGroupBox.Location = new System.Drawing.Point(10, 20);
            this.databaseGroupBox.Name = "databaseGroupBox";
            this.databaseGroupBox.Size = new System.Drawing.Size(380, 205);
            this.databaseGroupBox.TabIndex = 7;
            this.databaseGroupBox.TabStop = false;
            this.databaseGroupBox.Text = "База данных";
            // 
            // editDatabaseSettingsControl
            // 
            this.editDatabaseSettingsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editDatabaseSettingsControl.Location = new System.Drawing.Point(3, 16);
            this.editDatabaseSettingsControl.Name = "editDatabaseSettingsControl";
            this.editDatabaseSettingsControl.Size = new System.Drawing.Size(374, 186);
            this.editDatabaseSettingsControl.TabIndex = 0;
            // 
            // tcpGroupBox
            // 
            this.tcpGroupBox.Controls.Add(this.tcpPortUpDown);
            this.tcpGroupBox.Controls.Add(this.tcpHostTextBox);
            this.tcpGroupBox.Enabled = false;
            this.tcpGroupBox.Location = new System.Drawing.Point(10, 235);
            this.tcpGroupBox.Name = "tcpGroupBox";
            this.tcpGroupBox.Size = new System.Drawing.Size(180, 60);
            this.tcpGroupBox.TabIndex = 2;
            this.tcpGroupBox.TabStop = false;
            // 
            // tcpPortUpDown
            // 
            this.tcpPortUpDown.Location = new System.Drawing.Point(105, 30);
            this.tcpPortUpDown.Maximum = new decimal(new int[] {
            65025,
            0,
            0,
            0});
            this.tcpPortUpDown.Name = "tcpPortUpDown";
            this.tcpPortUpDown.Size = new System.Drawing.Size(65, 20);
            this.tcpPortUpDown.TabIndex = 1;
            this.tcpPortUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.tcpPortUpDown.Leave += new System.EventHandler(this.tcpPortUpDown_Leave);
            // 
            // tcpHostTextBox
            // 
            this.tcpHostTextBox.Location = new System.Drawing.Point(5, 30);
            this.tcpHostTextBox.Name = "tcpHostTextBox";
            this.tcpHostTextBox.Size = new System.Drawing.Size(95, 20);
            this.tcpHostTextBox.TabIndex = 0;
            this.tcpHostTextBox.Leave += new System.EventHandler(this.tcpHostTextBox_Leave);
            // 
            // httpGroupBox
            // 
            this.httpGroupBox.Controls.Add(this.httpPortUpDown);
            this.httpGroupBox.Controls.Add(this.httpHostTextBox);
            this.httpGroupBox.Enabled = false;
            this.httpGroupBox.Location = new System.Drawing.Point(210, 235);
            this.httpGroupBox.Name = "httpGroupBox";
            this.httpGroupBox.Size = new System.Drawing.Size(180, 60);
            this.httpGroupBox.TabIndex = 4;
            this.httpGroupBox.TabStop = false;
            // 
            // httpPortUpDown
            // 
            this.httpPortUpDown.Location = new System.Drawing.Point(105, 30);
            this.httpPortUpDown.Maximum = new decimal(new int[] {
            65025,
            0,
            0,
            0});
            this.httpPortUpDown.Name = "httpPortUpDown";
            this.httpPortUpDown.Size = new System.Drawing.Size(65, 20);
            this.httpPortUpDown.TabIndex = 1;
            this.httpPortUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.httpPortUpDown.Leave += new System.EventHandler(this.httpPortUpDown_Leave);
            // 
            // httpHostTextBox
            // 
            this.httpHostTextBox.Location = new System.Drawing.Point(5, 30);
            this.httpHostTextBox.Name = "httpHostTextBox";
            this.httpHostTextBox.Size = new System.Drawing.Size(95, 20);
            this.httpHostTextBox.TabIndex = 0;
            this.httpHostTextBox.Leave += new System.EventHandler(this.httpHostTextBox_Leave);
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
            this.layoutPanel.Controls.Add(this.serviceGroupBox, 0, 2);
            this.layoutPanel.Controls.Add(this.settingsGroupBox, 0, 1);
            this.layoutPanel.Controls.Add(this.panel1, 0, 3);
            this.layoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPanel.Location = new System.Drawing.Point(0, 0);
            this.layoutPanel.Name = "layoutPanel";
            this.layoutPanel.RowCount = 4;
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.layoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.layoutPanel.Size = new System.Drawing.Size(406, 591);
            this.layoutPanel.TabIndex = 14;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stopButton);
            this.panel1.Controls.Add(this.startButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 544);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 44);
            this.panel1.TabIndex = 14;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(406, 591);
            this.Controls.Add(this.layoutPanel);
            this.Controls.Add(this.panel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Сервер очереди";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.serviceGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).EndInit();
            this.settingsGroupBox.ResumeLayout(false);
            this.settingsGroupBox.PerformLayout();
            this.databaseGroupBox.ResumeLayout(false);
            this.tcpGroupBox.ResumeLayout(false);
            this.tcpGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcpPortUpDown)).EndInit();
            this.httpGroupBox.ResumeLayout(false);
            this.httpGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.httpPortUpDown)).EndInit();
            this.layoutPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

       

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.CheckBox tcpCheckBox;
        private System.Windows.Forms.GroupBox tcpGroupBox;
        private System.Windows.Forms.NumericUpDown tcpPortUpDown;
        private System.Windows.Forms.TextBox tcpHostTextBox;
        private System.Windows.Forms.CheckBox httpCheckBox;
        private System.Windows.Forms.GroupBox httpGroupBox;
        private System.Windows.Forms.NumericUpDown httpPortUpDown;
        private System.Windows.Forms.TextBox httpHostTextBox;
        private System.Windows.Forms.GroupBox databaseGroupBox;
        private Junte.UI.WinForms.NHibernate.EditDatabaseSettingsUserControl editDatabaseSettingsControl;
        private System.Windows.Forms.CheckBox debugCheckBox;
        private System.Windows.Forms.Button runServiceButton;
        private System.Windows.Forms.Button installServiseButton;
        private System.Windows.Forms.GroupBox serviceGroupBox;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Timer serviceStateTimer;
        private System.Windows.Forms.PictureBox serviceStatePicture;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label label1;
        private UI.WinForms.EnumItemControl languageControl;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TableLayoutPanel layoutPanel;
        private System.Windows.Forms.Panel panel1;
    }
}

