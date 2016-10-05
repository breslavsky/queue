namespace Queue.Administrator
{
    partial class EditAdministratorForm
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
            this.surnameLabel = new System.Windows.Forms.Label();
            this.surnameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.patronymicLabel = new System.Windows.Forms.Label();
            this.patronymicTextBox = new System.Windows.Forms.TextBox();
            this.emailLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.mobileLabel = new System.Windows.Forms.Label();
            this.mobileTextBox = new System.Windows.Forms.MaskedTextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.passwordButton = new System.Windows.Forms.Button();
            this.permissionsLabel = new System.Windows.Forms.Label();
            this.permissionsFlagsControl = new Queue.UI.WinForms.EnumFlagsControl();
            this.isActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.isMultisessionCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // surnameLabel
            // 
            this.surnameLabel.Location = new System.Drawing.Point(10, 10);
            this.surnameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.surnameLabel.Name = "surnameLabel";
            this.surnameLabel.Size = new System.Drawing.Size(220, 35);
            this.surnameLabel.TabIndex = 0;
            this.surnameLabel.Text = "Фамилия";
            this.surnameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // surnameTextBox
            // 
            this.surnameTextBox.Location = new System.Drawing.Point(240, 10);
            this.surnameTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.surnameTextBox.Name = "surnameTextBox";
            this.surnameTextBox.Size = new System.Drawing.Size(276, 31);
            this.surnameTextBox.TabIndex = 0;
            this.surnameTextBox.Leave += new System.EventHandler(this.surnameTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(10, 58);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(220, 35);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Имя";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(240, 58);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(176, 31);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // patronymicLabel
            // 
            this.patronymicLabel.Location = new System.Drawing.Point(10, 106);
            this.patronymicLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.patronymicLabel.Name = "patronymicLabel";
            this.patronymicLabel.Size = new System.Drawing.Size(220, 35);
            this.patronymicLabel.TabIndex = 0;
            this.patronymicLabel.Text = "Отчество";
            this.patronymicLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // patronymicTextBox
            // 
            this.patronymicTextBox.Location = new System.Drawing.Point(240, 106);
            this.patronymicTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.patronymicTextBox.Name = "patronymicTextBox";
            this.patronymicTextBox.Size = new System.Drawing.Size(176, 31);
            this.patronymicTextBox.TabIndex = 2;
            this.patronymicTextBox.Leave += new System.EventHandler(this.patronymicTextBox_Leave);
            // 
            // emailLabel
            // 
            this.emailLabel.Location = new System.Drawing.Point(10, 154);
            this.emailLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(220, 38);
            this.emailLabel.TabIndex = 0;
            this.emailLabel.Text = "Электронный адрес";
            this.emailLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(240, 154);
            this.emailTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(266, 31);
            this.emailTextBox.TabIndex = 3;
            this.emailTextBox.Leave += new System.EventHandler(this.emailTextBox_Leave);
            // 
            // mobileLabel
            // 
            this.mobileLabel.Location = new System.Drawing.Point(10, 202);
            this.mobileLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.mobileLabel.Name = "mobileLabel";
            this.mobileLabel.Size = new System.Drawing.Size(220, 35);
            this.mobileLabel.TabIndex = 0;
            this.mobileLabel.Text = "Мобильный";
            this.mobileLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // mobileTextBox
            // 
            this.mobileTextBox.Location = new System.Drawing.Point(240, 202);
            this.mobileTextBox.Margin = new System.Windows.Forms.Padding(6);
            this.mobileTextBox.Mask = "8(999)-000-0000";
            this.mobileTextBox.Name = "mobileTextBox";
            this.mobileTextBox.Size = new System.Drawing.Size(176, 31);
            this.mobileTextBox.TabIndex = 4;
            this.mobileTextBox.Leave += new System.EventHandler(this.mobileTextBox_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(370, 702);
            this.saveButton.Margin = new System.Windows.Forms.Padding(6);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(150, 48);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Записать";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // passwordButton
            // 
            this.passwordButton.Location = new System.Drawing.Point(20, 702);
            this.passwordButton.Margin = new System.Windows.Forms.Padding(6);
            this.passwordButton.Name = "passwordButton";
            this.passwordButton.Size = new System.Drawing.Size(150, 48);
            this.passwordButton.TabIndex = 6;
            this.passwordButton.Text = "Пароль";
            this.passwordButton.UseVisualStyleBackColor = true;
            this.passwordButton.Click += new System.EventHandler(this.passwordButton_Click);
            // 
            // permissionsLabel
            // 
            this.permissionsLabel.Location = new System.Drawing.Point(20, 295);
            this.permissionsLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.permissionsLabel.Name = "permissionsLabel";
            this.permissionsLabel.Size = new System.Drawing.Size(500, 35);
            this.permissionsLabel.TabIndex = 0;
            this.permissionsLabel.Text = "Права доступа";
            this.permissionsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // permissionsFlagsControl
            // 
            this.permissionsFlagsControl.Location = new System.Drawing.Point(20, 335);
            this.permissionsFlagsControl.Margin = new System.Windows.Forms.Padding(12);
            this.permissionsFlagsControl.Name = "permissionsFlagsControl";
            this.permissionsFlagsControl.Size = new System.Drawing.Size(500, 358);
            this.permissionsFlagsControl.TabIndex = 5;
            this.permissionsFlagsControl.Leave += new System.EventHandler(this.permissionsFlagsControl_Leave);
            // 
            // isActiveCheckBox
            // 
            this.isActiveCheckBox.AutoSize = true;
            this.isActiveCheckBox.Location = new System.Drawing.Point(15, 255);
            this.isActiveCheckBox.Margin = new System.Windows.Forms.Padding(6);
            this.isActiveCheckBox.Name = "isActiveCheckBox";
            this.isActiveCheckBox.Size = new System.Drawing.Size(126, 29);
            this.isActiveCheckBox.TabIndex = 8;
            this.isActiveCheckBox.Text = "Активен";
            this.isActiveCheckBox.UseVisualStyleBackColor = true;
            this.isActiveCheckBox.Leave += new System.EventHandler(this.isActiveCheckBox_Leave);
            // 
            // isMultisessionCheckBox
            // 
            this.isMultisessionCheckBox.AutoSize = true;
            this.isMultisessionCheckBox.Location = new System.Drawing.Point(160, 255);
            this.isMultisessionCheckBox.Margin = new System.Windows.Forms.Padding(6);
            this.isMultisessionCheckBox.Name = "isMultisessionCheckBox";
            this.isMultisessionCheckBox.Size = new System.Drawing.Size(186, 29);
            this.isMultisessionCheckBox.TabIndex = 9;
            this.isMultisessionCheckBox.Text = "Мультисессия";
            this.isMultisessionCheckBox.UseVisualStyleBackColor = true;
            this.isMultisessionCheckBox.Leave += new System.EventHandler(this.isMultisessionCheckBox_Leave);
            // 
            // EditAdministratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 764);
            this.Controls.Add(this.isMultisessionCheckBox);
            this.Controls.Add(this.isActiveCheckBox);
            this.Controls.Add(this.surnameLabel);
            this.Controls.Add(this.surnameTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.patronymicLabel);
            this.Controls.Add(this.patronymicTextBox);
            this.Controls.Add(this.emailLabel);
            this.Controls.Add(this.emailTextBox);
            this.Controls.Add(this.mobileLabel);
            this.Controls.Add(this.mobileTextBox);
            this.Controls.Add(this.permissionsLabel);
            this.Controls.Add(this.permissionsFlagsControl);
            this.Controls.Add(this.passwordButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "EditAdministratorForm";
            this.Text = "Редактирование администратора";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditAdministratorForm_FormClosing);
            this.Load += new System.EventHandler(this.EditAdministratorForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label surnameLabel;
        private System.Windows.Forms.TextBox surnameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label patronymicLabel;
        private System.Windows.Forms.TextBox patronymicTextBox;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label mobileLabel;
        private System.Windows.Forms.MaskedTextBox mobileTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button passwordButton;
        private System.Windows.Forms.Label permissionsLabel;
        private UI.WinForms.EnumFlagsControl permissionsFlagsControl;
        private System.Windows.Forms.CheckBox isActiveCheckBox;
        private System.Windows.Forms.CheckBox isMultisessionCheckBox;
    }
}