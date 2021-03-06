﻿namespace Queue.Administrator
{
    partial class EditOperatorForm
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
            this.workplaceControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.workplaceLabel = new System.Windows.Forms.Label();
            this.isActiveCheckBox = new System.Windows.Forms.CheckBox();
            this.identityTextBox = new System.Windows.Forms.TextBox();
            this.indentityLabel = new System.Windows.Forms.Label();
            this.isMultisessionCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // surnameLabel
            // 
            this.surnameLabel.Location = new System.Drawing.Point(10, 10);
            this.surnameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.surnameLabel.Name = "surnameLabel";
            this.surnameLabel.Size = new System.Drawing.Size(220, 38);
            this.surnameLabel.TabIndex = 0;
            this.surnameLabel.Text = "Фамилия";
            this.surnameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // surnameTextBox
            // 
            this.surnameTextBox.Location = new System.Drawing.Point(240, 10);
            this.surnameTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.surnameTextBox.Name = "surnameTextBox";
            this.surnameTextBox.Size = new System.Drawing.Size(266, 31);
            this.surnameTextBox.TabIndex = 0;
            this.surnameTextBox.Leave += new System.EventHandler(this.surnameTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(10, 58);
            this.nameLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(220, 38);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Имя";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(240, 58);
            this.nameTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
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
            this.patronymicLabel.Size = new System.Drawing.Size(220, 38);
            this.patronymicLabel.TabIndex = 0;
            this.patronymicLabel.Text = "Отчество";
            this.patronymicLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // patronymicTextBox
            // 
            this.patronymicTextBox.Location = new System.Drawing.Point(240, 106);
            this.patronymicTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
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
            this.emailTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
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
            this.mobileTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.mobileTextBox.Mask = "8(999)-000-0000";
            this.mobileTextBox.Name = "mobileTextBox";
            this.mobileTextBox.Size = new System.Drawing.Size(176, 31);
            this.mobileTextBox.TabIndex = 4;
            this.mobileTextBox.Leave += new System.EventHandler(this.mobileTextBox_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(365, 400);
            this.saveButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(150, 48);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Записать";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // passwordButton
            // 
            this.passwordButton.Location = new System.Drawing.Point(5, 400);
            this.passwordButton.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.passwordButton.Name = "passwordButton";
            this.passwordButton.Size = new System.Drawing.Size(150, 48);
            this.passwordButton.TabIndex = 8;
            this.passwordButton.Text = "Пароль";
            this.passwordButton.UseVisualStyleBackColor = true;
            this.passwordButton.Click += new System.EventHandler(this.passwordButton_Click);
            // 
            // workplaceControl
            // 
            this.workplaceControl.Location = new System.Drawing.Point(240, 250);
            this.workplaceControl.Margin = new System.Windows.Forms.Padding(12, 12, 12, 12);
            this.workplaceControl.Name = "workplaceControl";
            this.workplaceControl.Size = new System.Drawing.Size(280, 40);
            this.workplaceControl.TabIndex = 5;
            this.workplaceControl.UseResetButton = false;
            this.workplaceControl.Leave += new System.EventHandler(this.workplaceControl_Leave);
            // 
            // workplaceLabel
            // 
            this.workplaceLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.workplaceLabel.Location = new System.Drawing.Point(10, 250);
            this.workplaceLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.workplaceLabel.Name = "workplaceLabel";
            this.workplaceLabel.Size = new System.Drawing.Size(220, 38);
            this.workplaceLabel.TabIndex = 0;
            this.workplaceLabel.Text = "Рабочее место";
            this.workplaceLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // isActiveCheckBox
            // 
            this.isActiveCheckBox.AutoSize = true;
            this.isActiveCheckBox.Location = new System.Drawing.Point(15, 355);
            this.isActiveCheckBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.isActiveCheckBox.Name = "isActiveCheckBox";
            this.isActiveCheckBox.Size = new System.Drawing.Size(126, 29);
            this.isActiveCheckBox.TabIndex = 7;
            this.isActiveCheckBox.Text = "Активен";
            this.isActiveCheckBox.UseVisualStyleBackColor = true;
            this.isActiveCheckBox.Leave += new System.EventHandler(this.isActiveCheckBox_Leave);
            // 
            // identityTextBox
            // 
            this.identityTextBox.Location = new System.Drawing.Point(240, 298);
            this.identityTextBox.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.identityTextBox.Name = "identityTextBox";
            this.identityTextBox.Size = new System.Drawing.Size(276, 31);
            this.identityTextBox.TabIndex = 6;
            this.identityTextBox.Leave += new System.EventHandler(this.identityTextBox_Leave);
            // 
            // indentityLabel
            // 
            this.indentityLabel.Location = new System.Drawing.Point(10, 298);
            this.indentityLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.indentityLabel.Name = "indentityLabel";
            this.indentityLabel.Size = new System.Drawing.Size(220, 38);
            this.indentityLabel.TabIndex = 13;
            this.indentityLabel.Text = "Идентификация";
            this.indentityLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // isMultisessionCheckBox
            // 
            this.isMultisessionCheckBox.AutoSize = true;
            this.isMultisessionCheckBox.Location = new System.Drawing.Point(160, 355);
            this.isMultisessionCheckBox.Margin = new System.Windows.Forms.Padding(6);
            this.isMultisessionCheckBox.Name = "isMultisessionCheckBox";
            this.isMultisessionCheckBox.Size = new System.Drawing.Size(186, 29);
            this.isMultisessionCheckBox.TabIndex = 14;
            this.isMultisessionCheckBox.Text = "Мультисессия";
            this.isMultisessionCheckBox.UseVisualStyleBackColor = true;
            this.isMultisessionCheckBox.Leave += new System.EventHandler(this.isMultisessionCheckBox_Leave);
            // 
            // EditOperatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(539, 469);
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
            this.Controls.Add(this.workplaceLabel);
            this.Controls.Add(this.workplaceControl);
            this.Controls.Add(this.indentityLabel);
            this.Controls.Add(this.identityTextBox);
            this.Controls.Add(this.passwordButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "EditOperatorForm";
            this.Text = "Редактирование администратора";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditOperatorForm_FormClosing);
            this.Load += new System.EventHandler(this.EditOperatorForm_Load);
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
        private UI.WinForms.IdentifiedEntityControl workplaceControl;
        private System.Windows.Forms.Label workplaceLabel;
        private System.Windows.Forms.CheckBox isActiveCheckBox;
        private System.Windows.Forms.TextBox identityTextBox;
        private System.Windows.Forms.Label indentityLabel;
        private System.Windows.Forms.CheckBox isMultisessionCheckBox;
    }
}