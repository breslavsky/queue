﻿namespace Queue.Administrator
{
    partial class EditClientForm
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
            this.passwordButton = new System.Windows.Forms.Button();
            this.emailLabel = new System.Windows.Forms.Label();
            this.emailTextBox = new System.Windows.Forms.TextBox();
            this.mobileLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.mobileTextBox = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // surnameLabel
            // 
            this.surnameLabel.Location = new System.Drawing.Point(10, 10);
            this.surnameLabel.Name = "surnameLabel";
            this.surnameLabel.Size = new System.Drawing.Size(110, 20);
            this.surnameLabel.TabIndex = 0;
            this.surnameLabel.Text = "Фамилия";
            this.surnameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // surnameTextBox
            // 
            this.surnameTextBox.Location = new System.Drawing.Point(125, 10);
            this.surnameTextBox.Name = "surnameTextBox";
            this.surnameTextBox.Size = new System.Drawing.Size(135, 20);
            this.surnameTextBox.TabIndex = 0;
            this.surnameTextBox.Leave += new System.EventHandler(this.surnameTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(10, 35);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(110, 20);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Имя";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(125, 35);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(90, 20);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // patronymicLabel
            // 
            this.patronymicLabel.Location = new System.Drawing.Point(10, 60);
            this.patronymicLabel.Name = "patronymicLabel";
            this.patronymicLabel.Size = new System.Drawing.Size(110, 18);
            this.patronymicLabel.TabIndex = 0;
            this.patronymicLabel.Text = "Отчество";
            this.patronymicLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // patronymicTextBox
            // 
            this.patronymicTextBox.Location = new System.Drawing.Point(125, 60);
            this.patronymicTextBox.Name = "patronymicTextBox";
            this.patronymicTextBox.Size = new System.Drawing.Size(90, 20);
            this.patronymicTextBox.TabIndex = 2;
            this.patronymicTextBox.Leave += new System.EventHandler(this.patronymicTextBox_Leave);
            // 
            // passwordButton
            // 
            this.passwordButton.Location = new System.Drawing.Point(110, 140);
            this.passwordButton.Name = "passwordButton";
            this.passwordButton.Size = new System.Drawing.Size(75, 25);
            this.passwordButton.TabIndex = 5;
            this.passwordButton.Text = "Пароль";
            this.passwordButton.UseVisualStyleBackColor = true;
            this.passwordButton.Click += new System.EventHandler(this.passwordButton_Click);
            // 
            // emailLabel
            // 
            this.emailLabel.Location = new System.Drawing.Point(10, 85);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(110, 18);
            this.emailLabel.TabIndex = 0;
            this.emailLabel.Text = "Электронный адрес";
            this.emailLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(125, 85);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(135, 20);
            this.emailTextBox.TabIndex = 3;
            this.emailTextBox.Leave += new System.EventHandler(this.emailTextBox_Leave);
            // 
            // mobileLabel
            // 
            this.mobileLabel.Location = new System.Drawing.Point(10, 110);
            this.mobileLabel.Name = "mobileLabel";
            this.mobileLabel.Size = new System.Drawing.Size(110, 18);
            this.mobileLabel.TabIndex = 0;
            this.mobileLabel.Text = "Мобильный";
            this.mobileLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(190, 140);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Записать";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // mobileTextBox
            // 
            this.mobileTextBox.Location = new System.Drawing.Point(125, 110);
            this.mobileTextBox.Mask = "8(999)-000-0000";
            this.mobileTextBox.Name = "mobileTextBox";
            this.mobileTextBox.Size = new System.Drawing.Size(90, 20);
            this.mobileTextBox.TabIndex = 4;
            this.mobileTextBox.Leave += new System.EventHandler(this.mobileTextBox_Leave);
            // 
            // EditClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 171);
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
            this.Controls.Add(this.passwordButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditClientForm";
            this.Text = "Клиент";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditClientRequestForm_FormClosing);
            this.Load += new System.EventHandler(this.EditClientForm_Load);
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
        private System.Windows.Forms.Button passwordButton;
        private System.Windows.Forms.Label emailLabel;
        private System.Windows.Forms.TextBox emailTextBox;
        private System.Windows.Forms.Label mobileLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.MaskedTextBox mobileTextBox;

    }
}