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
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.editDatabaseSettingsControl = new Junte.UI.WinForms.NHibernate.EditDatabaseSettingsUserControl();
            this.tcpCheckBox = new System.Windows.Forms.CheckBox();
            this.tcpGroupBox = new System.Windows.Forms.GroupBox();
            this.tcpPortUpDown = new System.Windows.Forms.NumericUpDown();
            this.tcpHostTextBox = new System.Windows.Forms.TextBox();
            this.httpCheckBox = new System.Windows.Forms.CheckBox();
            this.httpGroupBox = new System.Windows.Forms.GroupBox();
            this.httpPortUpDown = new System.Windows.Forms.NumericUpDown();
            this.httpHostTextBox = new System.Windows.Forms.TextBox();
            this.languageLabel = new System.Windows.Forms.Label();
            this.languageControl = new Queue.UI.WinForms.EnumItemControl();
            this.saveButton = new System.Windows.Forms.Button();
            this.serviceGroupBox = new System.Windows.Forms.GroupBox();
            this.startServiceButton = new System.Windows.Forms.Button();
            this.serviceStatePicture = new System.Windows.Forms.PictureBox();
            this.installServiseButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.serviceStateTimer = new System.Windows.Forms.Timer(this.components);
            this.settingsTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.registerKeyLabel = new System.Windows.Forms.Label();
            this.serialKeyLabel = new System.Windows.Forms.Label();
            this.registerKeyTextBox = new System.Windows.Forms.MaskedTextBox();
            this.serialKeyTextBox = new System.Windows.Forms.MaskedTextBox();
            this.licenseTypeControl = new Queue.UI.WinForms.EnumItemControl();
            this.licenseTypeLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.tcpGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcpPortUpDown)).BeginInit();
            this.httpGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.httpPortUpDown)).BeginInit();
            this.serviceGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).BeginInit();
            this.settingsTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(10, 450);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(190, 30);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Запустить сервер";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(415, 95);
            this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.logoPictureBox.TabIndex = 13;
            this.logoPictureBox.TabStop = false;
            // 
            // editDatabaseSettingsControl
            // 
            this.editDatabaseSettingsControl.Location = new System.Drawing.Point(0, 0);
            this.editDatabaseSettingsControl.Name = "editDatabaseSettingsControl";
            this.editDatabaseSettingsControl.Size = new System.Drawing.Size(377, 186);
            this.editDatabaseSettingsControl.TabIndex = 0;
            // 
            // tcpCheckBox
            // 
            this.tcpCheckBox.AutoSize = true;
            this.tcpCheckBox.Location = new System.Drawing.Point(10, 15);
            this.tcpCheckBox.Name = "tcpCheckBox";
            this.tcpCheckBox.Size = new System.Drawing.Size(86, 17);
            this.tcpCheckBox.TabIndex = 3;
            this.tcpCheckBox.Text = "TCP-сервис";
            this.tcpCheckBox.UseVisualStyleBackColor = true;
            this.tcpCheckBox.CheckedChanged += new System.EventHandler(this.tcpCheckBox_CheckedChanged);
            this.tcpCheckBox.Leave += new System.EventHandler(this.tcpCheckBox_Leave);
            // 
            // tcpGroupBox
            // 
            this.tcpGroupBox.Controls.Add(this.tcpPortUpDown);
            this.tcpGroupBox.Controls.Add(this.tcpHostTextBox);
            this.tcpGroupBox.Enabled = false;
            this.tcpGroupBox.Location = new System.Drawing.Point(5, 30);
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
            // httpCheckBox
            // 
            this.httpCheckBox.AutoSize = true;
            this.httpCheckBox.Location = new System.Drawing.Point(215, 15);
            this.httpCheckBox.Name = "httpCheckBox";
            this.httpCheckBox.Size = new System.Drawing.Size(94, 17);
            this.httpCheckBox.TabIndex = 5;
            this.httpCheckBox.Text = "HTTP-сервис";
            this.httpCheckBox.UseVisualStyleBackColor = true;
            this.httpCheckBox.CheckedChanged += new System.EventHandler(this.httpCheckBox_CheckedChanged);
            this.httpCheckBox.Leave += new System.EventHandler(this.httpCheckBox_Leave);
            // 
            // httpGroupBox
            // 
            this.httpGroupBox.Controls.Add(this.httpPortUpDown);
            this.httpGroupBox.Controls.Add(this.httpHostTextBox);
            this.httpGroupBox.Enabled = false;
            this.httpGroupBox.Location = new System.Drawing.Point(210, 30);
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
            // languageLabel
            // 
            this.languageLabel.Location = new System.Drawing.Point(170, 310);
            this.languageLabel.Name = "languageLabel";
            this.languageLabel.Size = new System.Drawing.Size(80, 25);
            this.languageLabel.TabIndex = 11;
            this.languageLabel.Text = "Язык";
            this.languageLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // languageControl
            // 
            this.languageControl.Location = new System.Drawing.Point(250, 315);
            this.languageControl.Name = "languageControl";
            this.languageControl.Size = new System.Drawing.Size(160, 21);
            this.languageControl.TabIndex = 10;
            this.languageControl.SelectedChanged += new System.EventHandler<System.EventArgs>(this.languageControl_SelectedChanged);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(5, 340);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(405, 35);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // serviceGroupBox
            // 
            this.serviceGroupBox.Controls.Add(this.startServiceButton);
            this.serviceGroupBox.Controls.Add(this.serviceStatePicture);
            this.serviceGroupBox.Controls.Add(this.installServiseButton);
            this.serviceGroupBox.Location = new System.Drawing.Point(5, 380);
            this.serviceGroupBox.Name = "serviceGroupBox";
            this.serviceGroupBox.Size = new System.Drawing.Size(405, 65);
            this.serviceGroupBox.TabIndex = 2;
            this.serviceGroupBox.TabStop = false;
            this.serviceGroupBox.Text = "Служба";
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
            // serviceStatePicture
            // 
            this.serviceStatePicture.Location = new System.Drawing.Point(190, 25);
            this.serviceStatePicture.Name = "serviceStatePicture";
            this.serviceStatePicture.Size = new System.Drawing.Size(24, 24);
            this.serviceStatePicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.serviceStatePicture.TabIndex = 2;
            this.serviceStatePicture.TabStop = false;
            // 
            // installServiseButton
            // 
            this.installServiseButton.Location = new System.Drawing.Point(5, 20);
            this.installServiseButton.Name = "installServiseButton";
            this.installServiseButton.Size = new System.Drawing.Size(180, 35);
            this.installServiseButton.TabIndex = 0;
            this.installServiseButton.Text = "Установить службу";
            this.installServiseButton.UseVisualStyleBackColor = true;
            this.installServiseButton.Click += new System.EventHandler(this.installServiseButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(215, 450);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(190, 30);
            this.stopButton.TabIndex = 12;
            this.stopButton.Text = "Остановить сервер";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // serviceStateTimer
            // 
            this.serviceStateTimer.Interval = 1000;
            this.serviceStateTimer.Tick += new System.EventHandler(this.serviceStateTimer_Tick);
            // 
            // settingsTabControl
            // 
            this.settingsTabControl.Controls.Add(this.tabPage1);
            this.settingsTabControl.Controls.Add(this.tabPage2);
            this.settingsTabControl.Controls.Add(this.tabPage3);
            this.settingsTabControl.Location = new System.Drawing.Point(5, 100);
            this.settingsTabControl.Name = "settingsTabControl";
            this.settingsTabControl.Padding = new System.Drawing.Point(5, 5);
            this.settingsTabControl.SelectedIndex = 0;
            this.settingsTabControl.Size = new System.Drawing.Size(405, 210);
            this.settingsTabControl.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.editDatabaseSettingsControl);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(397, 180);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "База данных";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tcpGroupBox);
            this.tabPage2.Controls.Add(this.httpCheckBox);
            this.tabPage2.Controls.Add(this.tcpCheckBox);
            this.tabPage2.Controls.Add(this.httpGroupBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(397, 180);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Сервисы";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.registerKeyLabel);
            this.tabPage3.Controls.Add(this.serialKeyLabel);
            this.tabPage3.Controls.Add(this.registerKeyTextBox);
            this.tabPage3.Controls.Add(this.serialKeyTextBox);
            this.tabPage3.Controls.Add(this.licenseTypeControl);
            this.tabPage3.Controls.Add(this.licenseTypeLabel);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(397, 180);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Лицензия";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // registerKeyLabel
            // 
            this.registerKeyLabel.Location = new System.Drawing.Point(5, 65);
            this.registerKeyLabel.Name = "registerKeyLabel";
            this.registerKeyLabel.Size = new System.Drawing.Size(115, 25);
            this.registerKeyLabel.TabIndex = 20;
            this.registerKeyLabel.Text = "Ключ регистрации";
            this.registerKeyLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serialKeyLabel
            // 
            this.serialKeyLabel.Location = new System.Drawing.Point(5, 35);
            this.serialKeyLabel.Name = "serialKeyLabel";
            this.serialKeyLabel.Size = new System.Drawing.Size(115, 25);
            this.serialKeyLabel.TabIndex = 19;
            this.serialKeyLabel.Text = "Серийный ключ";
            this.serialKeyLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // registerKeyTextBox
            // 
            this.registerKeyTextBox.Location = new System.Drawing.Point(120, 70);
            this.registerKeyTextBox.Mask = "0000-0000-0000-0000-0000";
            this.registerKeyTextBox.Name = "registerKeyTextBox";
            this.registerKeyTextBox.Size = new System.Drawing.Size(140, 20);
            this.registerKeyTextBox.TabIndex = 18;
            this.registerKeyTextBox.Leave += new System.EventHandler(this.registerKeyTextBox_Leave);
            // 
            // serialKeyTextBox
            // 
            this.serialKeyTextBox.Location = new System.Drawing.Point(120, 40);
            this.serialKeyTextBox.Mask = "0000-0000-0000-0000-0000";
            this.serialKeyTextBox.Name = "serialKeyTextBox";
            this.serialKeyTextBox.Size = new System.Drawing.Size(140, 20);
            this.serialKeyTextBox.TabIndex = 17;
            this.serialKeyTextBox.Leave += new System.EventHandler(this.serialKeyTextBox_Leave);
            // 
            // licenseTypeControl
            // 
            this.licenseTypeControl.Location = new System.Drawing.Point(120, 10);
            this.licenseTypeControl.Name = "licenseTypeControl";
            this.licenseTypeControl.Size = new System.Drawing.Size(145, 21);
            this.licenseTypeControl.TabIndex = 15;
            this.licenseTypeControl.Leave += new System.EventHandler(this.licenseTypeControl_Leave);
            // 
            // licenseTypeLabel
            // 
            this.licenseTypeLabel.Location = new System.Drawing.Point(5, 5);
            this.licenseTypeLabel.Name = "licenseTypeLabel";
            this.licenseTypeLabel.Size = new System.Drawing.Size(115, 25);
            this.licenseTypeLabel.TabIndex = 16;
            this.licenseTypeLabel.Text = "Тип лицензии";
            this.licenseTypeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(414, 487);
            this.Controls.Add(this.settingsTabControl);
            this.Controls.Add(this.logoPictureBox);
            this.Controls.Add(this.languageControl);
            this.Controls.Add(this.languageLabel);
            this.Controls.Add(this.serviceGroupBox);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Сервер очереди";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.tcpGroupBox.ResumeLayout(false);
            this.tcpGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcpPortUpDown)).EndInit();
            this.httpGroupBox.ResumeLayout(false);
            this.httpGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.httpPortUpDown)).EndInit();
            this.serviceGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serviceStatePicture)).EndInit();
            this.settingsTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.ResumeLayout(false);

        }

       

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.CheckBox tcpCheckBox;
        private System.Windows.Forms.GroupBox tcpGroupBox;
        private System.Windows.Forms.NumericUpDown tcpPortUpDown;
        private System.Windows.Forms.TextBox tcpHostTextBox;
        private System.Windows.Forms.CheckBox httpCheckBox;
        private System.Windows.Forms.GroupBox httpGroupBox;
        private System.Windows.Forms.NumericUpDown httpPortUpDown;
        private System.Windows.Forms.TextBox httpHostTextBox;
        private Junte.UI.WinForms.NHibernate.EditDatabaseSettingsUserControl editDatabaseSettingsControl;
        private System.Windows.Forms.Button startServiceButton;
        private System.Windows.Forms.Button installServiseButton;
        private System.Windows.Forms.GroupBox serviceGroupBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Timer serviceStateTimer;
        private System.Windows.Forms.PictureBox serviceStatePicture;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Label languageLabel;
        private UI.WinForms.EnumItemControl languageControl;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.TabControl settingsTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private UI.WinForms.EnumItemControl licenseTypeControl;
        private System.Windows.Forms.Label licenseTypeLabel;
        private System.Windows.Forms.MaskedTextBox registerKeyTextBox;
        private System.Windows.Forms.MaskedTextBox serialKeyTextBox;
        private System.Windows.Forms.Label registerKeyLabel;
        private System.Windows.Forms.Label serialKeyLabel;
    }
}

