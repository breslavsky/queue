namespace Queue.UI.WinForms
{
    partial class LoginSettingsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginSettingsControl));
            this.connectionGroupBox = new System.Windows.Forms.GroupBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.serverLabel = new System.Windows.Forms.Label();
            this.endpointTextBox = new System.Windows.Forms.TextBox();
            this.serverConnectionSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.loginGroupBox = new System.Windows.Forms.GroupBox();
            this.selectUserControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.loginLabel = new System.Windows.Forms.Label();
            this.connectionGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverConnectionSettingsBindingSource)).BeginInit();
            this.loginGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectionGroupBox
            // 
            this.connectionGroupBox.Controls.Add(this.connectButton);
            this.connectionGroupBox.Controls.Add(this.serverLabel);
            this.connectionGroupBox.Controls.Add(this.endpointTextBox);
            this.connectionGroupBox.Location = new System.Drawing.Point(3, 3);
            this.connectionGroupBox.Name = "connectionGroupBox";
            this.connectionGroupBox.Size = new System.Drawing.Size(330, 70);
            this.connectionGroupBox.TabIndex = 1;
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
            this.endpointTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serverConnectionSettingsBindingSource, "Endpoint", true));
            this.endpointTextBox.Location = new System.Drawing.Point(100, 30);
            this.endpointTextBox.Name = "endpointTextBox";
            this.endpointTextBox.Size = new System.Drawing.Size(185, 20);
            this.endpointTextBox.TabIndex = 0;
            this.endpointTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.endpointTextBox_KeyDown);
            // 
            // serverConnectionSettingsBindingSource
            // 
            this.serverConnectionSettingsBindingSource.DataSource = typeof(Queue.Common.LoginSettings);
            // 
            // loginGroupBox
            // 
            this.loginGroupBox.Controls.Add(this.selectUserControl);
            this.loginGroupBox.Controls.Add(this.passwordLabel);
            this.loginGroupBox.Controls.Add(this.passwordTextBox);
            this.loginGroupBox.Controls.Add(this.loginLabel);
            this.loginGroupBox.Enabled = false;
            this.loginGroupBox.Location = new System.Drawing.Point(3, 79);
            this.loginGroupBox.Name = "loginGroupBox";
            this.loginGroupBox.Size = new System.Drawing.Size(330, 92);
            this.loginGroupBox.TabIndex = 2;
            this.loginGroupBox.TabStop = false;
            this.loginGroupBox.Text = "Данные пользователя";
            // 
            // selectUserControl
            // 
            this.selectUserControl.Location = new System.Drawing.Point(100, 30);
            this.selectUserControl.Name = "selectUserControl";
            this.selectUserControl.Size = new System.Drawing.Size(220, 21);
            this.selectUserControl.TabIndex = 1;
            this.selectUserControl.UseResetButton = true;
            this.selectUserControl.SelectedChanged += new System.EventHandler<System.EventArgs>(this.selectUserControl_SelectedChanged);
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
            // passwordTextBox
            // 
            this.passwordTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.serverConnectionSettingsBindingSource, "Password", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
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
            // LoginSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.loginGroupBox);
            this.Controls.Add(this.connectionGroupBox);
            this.Name = "LoginSettingsControl";
            this.Size = new System.Drawing.Size(340, 176);
            this.connectionGroupBox.ResumeLayout(false);
            this.connectionGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serverConnectionSettingsBindingSource)).EndInit();
            this.loginGroupBox.ResumeLayout(false);
            this.loginGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox connectionGroupBox;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Label serverLabel;
        private System.Windows.Forms.TextBox endpointTextBox;
        private System.Windows.Forms.GroupBox loginGroupBox;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label loginLabel;
        private System.Windows.Forms.BindingSource serverConnectionSettingsBindingSource;
        private IdentifiedEntityControl selectUserControl;
    }
}
