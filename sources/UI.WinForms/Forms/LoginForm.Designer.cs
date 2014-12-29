namespace Queue.UI.WinForms
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.connectionGroupBox = new System.Windows.Forms.GroupBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.serverLabel = new System.Windows.Forms.Label();
            this.endpointTextBox = new System.Windows.Forms.TextBox();
            this.loginGroupBox = new System.Windows.Forms.GroupBox();
            this.usersControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.rememberCheckBox = new System.Windows.Forms.CheckBox();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.loginButton = new System.Windows.Forms.Button();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.loginLabel = new System.Windows.Forms.Label();
            this.languageControl = new Queue.UI.WinForms.EnumItemControl();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.productNameLabel = new System.Windows.Forms.Label();
            this.connectionGroupBox.SuspendLayout();
            this.loginGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // connectionGroupBox
            // 
            this.connectionGroupBox.Controls.Add(this.connectButton);
            this.connectionGroupBox.Controls.Add(this.serverLabel);
            this.connectionGroupBox.Controls.Add(this.endpointTextBox);
            this.connectionGroupBox.Location = new System.Drawing.Point(10, 135);
            this.connectionGroupBox.Name = "connectionGroupBox";
            this.connectionGroupBox.Size = new System.Drawing.Size(330, 70);
            this.connectionGroupBox.TabIndex = 0;
            this.connectionGroupBox.TabStop = false;
            this.connectionGroupBox.Text = "Параметры соединения";
            // 
            // connectButton
            // 
            this.connectButton.Image = ((System.Drawing.Image)(resources.GetObject("connectButton.Image")));
            this.connectButton.Location = new System.Drawing.Point(290, 30);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(30, 25);
            this.connectButton.TabIndex = 0;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // serverLabel
            // 
            this.serverLabel.AutoSize = true;
            this.serverLabel.Location = new System.Drawing.Point(10, 30);
            this.serverLabel.Name = "serverLabel";
            this.serverLabel.Size = new System.Drawing.Size(88, 13);
            this.serverLabel.TabIndex = 0;
            this.serverLabel.Text = "Сервер очереди";
            // 
            // endpointTextBox
            // 
            this.endpointTextBox.Location = new System.Drawing.Point(100, 30);
            this.endpointTextBox.Name = "endpointTextBox";
            this.endpointTextBox.Size = new System.Drawing.Size(185, 20);
            this.endpointTextBox.TabIndex = 0;
            this.endpointTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.serverTextBox_KeyDown);
            // 
            // loginGroupBox
            // 
            this.loginGroupBox.Controls.Add(this.usersControl);
            this.loginGroupBox.Controls.Add(this.rememberCheckBox);
            this.loginGroupBox.Controls.Add(this.passwordLabel);
            this.loginGroupBox.Controls.Add(this.loginButton);
            this.loginGroupBox.Controls.Add(this.passwordTextBox);
            this.loginGroupBox.Controls.Add(this.loginLabel);
            this.loginGroupBox.Enabled = false;
            this.loginGroupBox.Location = new System.Drawing.Point(10, 215);
            this.loginGroupBox.Name = "loginGroupBox";
            this.loginGroupBox.Size = new System.Drawing.Size(330, 120);
            this.loginGroupBox.TabIndex = 0;
            this.loginGroupBox.TabStop = false;
            this.loginGroupBox.Text = "Данные пользователя";
            // 
            // usersControl
            // 
            this.usersControl.Location = new System.Drawing.Point(100, 25);
            this.usersControl.Name = "usersControl";
            this.usersControl.Size = new System.Drawing.Size(220, 21);
            this.usersControl.TabIndex = 1;
            this.usersControl.UseResetButton = false;
            // 
            // rememberCheckBox
            // 
            this.rememberCheckBox.AutoSize = true;
            this.rememberCheckBox.Location = new System.Drawing.Point(150, 90);
            this.rememberCheckBox.Name = "rememberCheckBox";
            this.rememberCheckBox.Size = new System.Drawing.Size(82, 17);
            this.rememberCheckBox.TabIndex = 0;
            this.rememberCheckBox.Text = "Запомнить";
            this.rememberCheckBox.UseVisualStyleBackColor = true;
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(10, 60);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(45, 13);
            this.passwordLabel.TabIndex = 0;
            this.passwordLabel.Text = "Пароль";
            // 
            // loginButton
            // 
            this.loginButton.Image = ((System.Drawing.Image)(resources.GetObject("loginButton.Image")));
            this.loginButton.Location = new System.Drawing.Point(240, 85);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(80, 25);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Вход";
            this.loginButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(100, 60);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(220, 20);
            this.passwordTextBox.TabIndex = 0;
            this.passwordTextBox.UseSystemPasswordChar = true;
            this.passwordTextBox.Enter += new System.EventHandler(this.passwordTextBox_Enter);
            this.passwordTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.passwordTextBox_KeyDown);
            // 
            // loginLabel
            // 
            this.loginLabel.AutoSize = true;
            this.loginLabel.Location = new System.Drawing.Point(10, 30);
            this.loginLabel.Name = "loginLabel";
            this.loginLabel.Size = new System.Drawing.Size(80, 13);
            this.loginLabel.TabIndex = 0;
            this.loginLabel.Text = "Пользователь";
            // 
            // languageControl
            // 
            this.languageControl.Location = new System.Drawing.Point(225, 5);
            this.languageControl.Name = "languageControl";
            this.languageControl.Size = new System.Drawing.Size(115, 21);
            this.languageControl.TabIndex = 2;
            this.languageControl.SelectedChanged += new System.EventHandler<System.EventArgs>(this.languageControl_SelectedChanged);
            // 
            // logoPictureBox
            // 
            this.logoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("logoPictureBox.Image")));
            this.logoPictureBox.Location = new System.Drawing.Point(10, 30);
            this.logoPictureBox.Name = "logoPictureBox";
            this.logoPictureBox.Size = new System.Drawing.Size(330, 100);
            this.logoPictureBox.TabIndex = 1;
            this.logoPictureBox.TabStop = false;
            // 
            // productNameLabel
            // 
            this.productNameLabel.Location = new System.Drawing.Point(135, 110);
            this.productNameLabel.Name = "productNameLabel";
            this.productNameLabel.Size = new System.Drawing.Size(202, 15);
            this.productNameLabel.TabIndex = 2;
            this.productNameLabel.Text = "система электронной очереди";
            this.productNameLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 342);
            this.Controls.Add(this.languageControl);
            this.Controls.Add(this.productNameLabel);
            this.Controls.Add(this.logoPictureBox);
            this.Controls.Add(this.connectionGroupBox);
            this.Controls.Add(this.loginGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(365, 360);
            this.Name = "LoginForm";
            this.Text = "Вход в систему";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.connectionGroupBox.ResumeLayout(false);
            this.connectionGroupBox.PerformLayout();
            this.loginGroupBox.ResumeLayout(false);
            this.loginGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox connectionGroupBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox endpointTextBox;
        private System.Windows.Forms.GroupBox loginGroupBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.CheckBox rememberCheckBox;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label productNameLabel;
        private IdentifiedEntityControl usersControl;
        private EnumItemControl languageControl;
    }
}

