namespace Queue.Administrator
{
    partial class OfficeLoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Queue.Common.LoginSettings loginSettings1 = new Queue.Common.LoginSettings();
            this.loginSettingsControl = new Queue.UI.WinForms.LoginSettingsControl();
            this.loginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loginSettingsControl
            // 
            this.loginSettingsControl.Location = new System.Drawing.Point(5, 5);
            this.loginSettingsControl.Name = "loginSettingsControl";
            loginSettings1.Endpoint = "net.tcp://queue:4505";
            loginSettings1.LockItem = false;
            loginSettings1.Password = "";
            loginSettings1.User = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.loginSettingsControl.Settings = loginSettings1;
            this.loginSettingsControl.Size = new System.Drawing.Size(340, 176);
            this.loginSettingsControl.TabIndex = 0;
            this.loginSettingsControl.UserRole = Queue.Model.Common.UserRole.Administrator;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(270, 190);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 25);
            this.loginButton.TabIndex = 1;
            this.loginButton.Text = "Войти";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // OfficeLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 222);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.loginSettingsControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OfficeLoginForm";
            this.Text = "Вход в удаленный офис";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OfficeLoginForm_FormClosing);
            this.Load += new System.EventHandler(this.OfficeLoginForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.WinForms.LoginSettingsControl loginSettingsControl;
        private System.Windows.Forms.Button loginButton;
    }
}