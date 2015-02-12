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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.rememberCheckBox = new System.Windows.Forms.CheckBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.languageControl = new Queue.UI.WinForms.EnumItemControl();
            this.logoPictureBox = new System.Windows.Forms.PictureBox();
            this.productNameLabel = new System.Windows.Forms.Label();
            this.loginSettingsControl = new Queue.UI.WinForms.LoginSettingsControl();
            this.loginFormSettingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loginFormSettingsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // rememberCheckBox
            // 
            this.rememberCheckBox.AutoSize = true;
            this.rememberCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.loginFormSettingsBindingSource, "IsRemember", true));
            this.rememberCheckBox.Location = new System.Drawing.Point(21, 315);
            this.rememberCheckBox.Name = "rememberCheckBox";
            this.rememberCheckBox.Size = new System.Drawing.Size(82, 17);
            this.rememberCheckBox.TabIndex = 0;
            this.rememberCheckBox.Text = "Запомнить";
            this.rememberCheckBox.UseVisualStyleBackColor = true;
            // 
            // loginButton
            // 
            this.loginButton.Image = ((System.Drawing.Image)(resources.GetObject("loginButton.Image")));
            this.loginButton.Location = new System.Drawing.Point(260, 310);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(80, 25);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Вход";
            this.loginButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
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
            // loginSettingsControl
            // 
            this.loginSettingsControl.Location = new System.Drawing.Point(10, 136);
            this.loginSettingsControl.Name = "loginSettingsControl";
            this.loginSettingsControl.Size = new System.Drawing.Size(330, 168);
            this.loginSettingsControl.TabIndex = 3;
            // 
            // loginFormSettingsBindingSource
            // 
            this.loginFormSettingsBindingSource.DataSource = typeof(Queue.UI.WinForms.LoginFormSettings);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 342);
            this.Controls.Add(this.loginSettingsControl);
            this.Controls.Add(this.languageControl);
            this.Controls.Add(this.rememberCheckBox);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.productNameLabel);
            this.Controls.Add(this.logoPictureBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(365, 360);
            this.Name = "LoginForm";
            this.Text = "Вход в систему";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logoPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loginFormSettingsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.CheckBox rememberCheckBox;
        private System.Windows.Forms.PictureBox logoPictureBox;
        private System.Windows.Forms.Label productNameLabel;
        private EnumItemControl languageControl;
        private LoginSettingsControl loginSettingsControl;
        private System.Windows.Forms.BindingSource loginFormSettingsBindingSource;
    }
}

