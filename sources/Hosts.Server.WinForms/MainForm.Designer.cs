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
            this.startButton = new System.Windows.Forms.Button();
            this.panel = new System.Windows.Forms.Panel();
            this.httpCheckBox = new System.Windows.Forms.CheckBox();
            this.httpGroupBox = new System.Windows.Forms.GroupBox();
            this.httpPortUpDown = new System.Windows.Forms.NumericUpDown();
            this.httpHostTextBox = new System.Windows.Forms.TextBox();
            this.tcpCheckBox = new System.Windows.Forms.CheckBox();
            this.tcpGroupBox = new System.Windows.Forms.GroupBox();
            this.tcpPortUpDown = new System.Windows.Forms.NumericUpDown();
            this.tcpHostTextBox = new System.Windows.Forms.TextBox();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.editDatabaseSettingsControl = new Junte.UI.WinForms.NHibernate.EditDatabaseSettingsUserControl();
            this.databaseGroupBox = new System.Windows.Forms.GroupBox();
            this.panel.SuspendLayout();
            this.httpGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.httpPortUpDown)).BeginInit();
            this.tcpGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcpPortUpDown)).BeginInit();
            this.databaseGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(10, 295);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(370, 30);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Запустить сервер";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.databaseGroupBox);
            this.panel.Controls.Add(this.httpCheckBox);
            this.panel.Controls.Add(this.httpGroupBox);
            this.panel.Controls.Add(this.tcpCheckBox);
            this.panel.Controls.Add(this.tcpGroupBox);
            this.panel.Controls.Add(this.startButton);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(389, 332);
            this.panel.TabIndex = 2;
            // 
            // httpCheckBox
            // 
            this.httpCheckBox.AutoSize = true;
            this.httpCheckBox.Location = new System.Drawing.Point(205, 230);
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
            this.httpGroupBox.Location = new System.Drawing.Point(195, 230);
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
            // tcpCheckBox
            // 
            this.tcpCheckBox.AutoSize = true;
            this.tcpCheckBox.Location = new System.Drawing.Point(20, 230);
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
            this.tcpGroupBox.Location = new System.Drawing.Point(10, 230);
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
            // notifyIcon
            // 
            this.notifyIcon.Text = "notifyIcon1";
            this.notifyIcon.Visible = true;
            // 
            // editDatabaseSettingsControl
            // 
            this.editDatabaseSettingsControl.Location = new System.Drawing.Point(10, 15);
            this.editDatabaseSettingsControl.Name = "editDatabaseSettingsControl";
            this.editDatabaseSettingsControl.Size = new System.Drawing.Size(340, 186);
            this.editDatabaseSettingsControl.TabIndex = 6;
            // 
            // databaseGroupBox
            // 
            this.databaseGroupBox.Controls.Add(this.editDatabaseSettingsControl);
            this.databaseGroupBox.Location = new System.Drawing.Point(10, 10);
            this.databaseGroupBox.Name = "databaseGroupBox";
            this.databaseGroupBox.Size = new System.Drawing.Size(365, 205);
            this.databaseGroupBox.TabIndex = 7;
            this.databaseGroupBox.TabStop = false;
            this.databaseGroupBox.Text = "База данных";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 332);
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(235, 255);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Сервер очереди";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.httpGroupBox.ResumeLayout(false);
            this.httpGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.httpPortUpDown)).EndInit();
            this.tcpGroupBox.ResumeLayout(false);
            this.tcpGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcpPortUpDown)).EndInit();
            this.databaseGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.CheckBox tcpCheckBox;
        private System.Windows.Forms.GroupBox tcpGroupBox;
        private System.Windows.Forms.NumericUpDown tcpPortUpDown;
        private System.Windows.Forms.TextBox tcpHostTextBox;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.CheckBox httpCheckBox;
        private System.Windows.Forms.GroupBox httpGroupBox;
        private System.Windows.Forms.NumericUpDown httpPortUpDown;
        private System.Windows.Forms.TextBox httpHostTextBox;
        private System.Windows.Forms.GroupBox databaseGroupBox;
        private Junte.UI.WinForms.NHibernate.EditDatabaseSettingsUserControl editDatabaseSettingsControl;
    }
}

