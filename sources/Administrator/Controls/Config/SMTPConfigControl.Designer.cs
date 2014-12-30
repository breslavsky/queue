namespace Queue.Administrator
{
    partial class SMTPConfigControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.serverLabel = new System.Windows.Forms.Label();
            this.serverTextBox = new System.Windows.Forms.TextBox();
            this.userLabel = new System.Windows.Forms.Label();
            this.userTextBox = new System.Windows.Forms.TextBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.fromLabel = new System.Windows.Forms.Label();
            this.fromTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.enableSslCheckBox = new System.Windows.Forms.CheckBox();
            this.portUpDown = new System.Windows.Forms.NumericUpDown();
            this.portLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(7, 10);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(44, 13);
            this.serverLabel.TabIndex = 1;
            this.serverLabel.Text = "Сервер";
            // 
            // serverTextBox
            // 
            this.serverTextBox.Location = new System.Drawing.Point(95, 5);
            this.serverTextBox.Name = "serverTextBox";
            this.serverTextBox.Size = new System.Drawing.Size(130, 20);
            this.serverTextBox.TabIndex = 2;
            this.serverTextBox.Leave += new System.EventHandler(this.serverTextBox_Leave);
            // 
            // userLabel
            // 
            this.userLabel.AutoSize = true;
            this.userLabel.Location = new System.Drawing.Point(7, 80);
            this.userLabel.Name = "userLabel";
            this.userLabel.Size = new System.Drawing.Size(80, 13);
            this.userLabel.TabIndex = 3;
            this.userLabel.Text = "Пользователь";
            // 
            // userTextBox
            // 
            this.userTextBox.Location = new System.Drawing.Point(95, 75);
            this.userTextBox.Name = "userTextBox";
            this.userTextBox.Size = new System.Drawing.Size(130, 20);
            this.userTextBox.TabIndex = 4;
            this.userTextBox.Leave += new System.EventHandler(this.userTextBox_Leave);
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(7, 105);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(45, 13);
            this.passwordLabel.TabIndex = 5;
            this.passwordLabel.Text = "Пароль";
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(95, 100);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(130, 20);
            this.passwordTextBox.TabIndex = 6;
            this.passwordTextBox.UseSystemPasswordChar = true;
            this.passwordTextBox.Leave += new System.EventHandler(this.passwordTextBox_Leave);
            // 
            // fromLabel
            // 
            this.fromLabel.AutoSize = true;
            this.fromLabel.Location = new System.Drawing.Point(7, 130);
            this.fromLabel.Name = "fromLabel";
            this.fromLabel.Size = new System.Drawing.Size(73, 13);
            this.fromLabel.TabIndex = 7;
            this.fromLabel.Text = "Отправитель";
            // 
            // fromTextBox
            // 
            this.fromTextBox.Location = new System.Drawing.Point(95, 125);
            this.fromTextBox.Name = "fromTextBox";
            this.fromTextBox.Size = new System.Drawing.Size(130, 20);
            this.fromTextBox.TabIndex = 8;
            this.fromTextBox.Leave += new System.EventHandler(this.fromTextBox_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(150, 150);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Записать";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // enableSslCheckBox
            // 
            this.enableSslCheckBox.AutoSize = true;
            this.enableSslCheckBox.Location = new System.Drawing.Point(95, 55);
            this.enableSslCheckBox.Name = "enableSslCheckBox";
            this.enableSslCheckBox.Size = new System.Drawing.Size(122, 17);
            this.enableSslCheckBox.TabIndex = 10;
            this.enableSslCheckBox.Text = "Использовать SSL";
            this.enableSslCheckBox.UseVisualStyleBackColor = true;
            this.enableSslCheckBox.Leave += new System.EventHandler(this.enableSslCheckBox_Leave);
            // 
            // portUpDown
            // 
            this.portUpDown.Location = new System.Drawing.Point(95, 30);
            this.portUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.portUpDown.Name = "portUpDown";
            this.portUpDown.Size = new System.Drawing.Size(60, 20);
            this.portUpDown.TabIndex = 11;
            this.portUpDown.Leave += new System.EventHandler(this.portUpDown_Leave);
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(10, 35);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(32, 13);
            this.portLabel.TabIndex = 12;
            this.portLabel.Text = "Порт";
            // 
            // SMTPConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.portLabel);
            this.Controls.Add(this.portUpDown);
            this.Controls.Add(this.enableSslCheckBox);
            this.Controls.Add(this.serverLabel);
            this.Controls.Add(this.serverTextBox);
            this.Controls.Add(this.userLabel);
            this.Controls.Add(this.userTextBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.fromLabel);
            this.Controls.Add(this.fromTextBox);
            this.Controls.Add(this.saveButton);
            this.Name = "SMTPConfigControl";
            this.Size = new System.Drawing.Size(230, 180);
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox serverTextBox;
        private System.Windows.Forms.Label userLabel;
        private System.Windows.Forms.TextBox userTextBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label fromLabel;
        private System.Windows.Forms.TextBox fromTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox enableSslCheckBox;
        private System.Windows.Forms.NumericUpDown portUpDown;
        private System.Windows.Forms.Label portLabel;
    }
}
