namespace Queue.Administrator
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
            this.interruptionStartTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.interruptionLabel = new System.Windows.Forms.Label();
            this.interruptionFinishTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.isInterruptionCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // surnameLabel
            // 
            this.surnameLabel.Location = new System.Drawing.Point(5, 5);
            this.surnameLabel.Name = "surnameLabel";
            this.surnameLabel.Size = new System.Drawing.Size(110, 20);
            this.surnameLabel.TabIndex = 0;
            this.surnameLabel.Text = "Фамилия";
            this.surnameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // surnameTextBox
            // 
            this.surnameTextBox.Location = new System.Drawing.Point(120, 5);
            this.surnameTextBox.Name = "surnameTextBox";
            this.surnameTextBox.Size = new System.Drawing.Size(135, 20);
            this.surnameTextBox.TabIndex = 0;
            this.surnameTextBox.Leave += new System.EventHandler(this.surnameTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(5, 30);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(110, 20);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Имя";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(120, 30);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(90, 20);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // patronymicLabel
            // 
            this.patronymicLabel.Location = new System.Drawing.Point(5, 55);
            this.patronymicLabel.Name = "patronymicLabel";
            this.patronymicLabel.Size = new System.Drawing.Size(110, 20);
            this.patronymicLabel.TabIndex = 0;
            this.patronymicLabel.Text = "Отчество";
            this.patronymicLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // patronymicTextBox
            // 
            this.patronymicTextBox.Location = new System.Drawing.Point(120, 55);
            this.patronymicTextBox.Name = "patronymicTextBox";
            this.patronymicTextBox.Size = new System.Drawing.Size(90, 20);
            this.patronymicTextBox.TabIndex = 2;
            this.patronymicTextBox.Leave += new System.EventHandler(this.patronymicTextBox_Leave);
            // 
            // emailLabel
            // 
            this.emailLabel.Location = new System.Drawing.Point(5, 80);
            this.emailLabel.Name = "emailLabel";
            this.emailLabel.Size = new System.Drawing.Size(110, 20);
            this.emailLabel.TabIndex = 0;
            this.emailLabel.Text = "Электронный адрес";
            this.emailLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // emailTextBox
            // 
            this.emailTextBox.Location = new System.Drawing.Point(120, 80);
            this.emailTextBox.Name = "emailTextBox";
            this.emailTextBox.Size = new System.Drawing.Size(135, 20);
            this.emailTextBox.TabIndex = 3;
            this.emailTextBox.Leave += new System.EventHandler(this.emailTextBox_Leave);
            // 
            // mobileLabel
            // 
            this.mobileLabel.Location = new System.Drawing.Point(5, 105);
            this.mobileLabel.Name = "mobileLabel";
            this.mobileLabel.Size = new System.Drawing.Size(110, 18);
            this.mobileLabel.TabIndex = 0;
            this.mobileLabel.Text = "Мобильный";
            this.mobileLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // mobileTextBox
            // 
            this.mobileTextBox.Location = new System.Drawing.Point(120, 105);
            this.mobileTextBox.Mask = "8(999)-000-0000";
            this.mobileTextBox.Name = "mobileTextBox";
            this.mobileTextBox.Size = new System.Drawing.Size(90, 20);
            this.mobileTextBox.TabIndex = 4;
            this.mobileTextBox.Leave += new System.EventHandler(this.mobileTextBox_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(185, 210);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // passwordButton
            // 
            this.passwordButton.Location = new System.Drawing.Point(105, 210);
            this.passwordButton.Name = "passwordButton";
            this.passwordButton.Size = new System.Drawing.Size(75, 25);
            this.passwordButton.TabIndex = 9;
            this.passwordButton.Text = "Пароль";
            this.passwordButton.UseVisualStyleBackColor = true;
            this.passwordButton.Click += new System.EventHandler(this.passwordButton_Click);
            // 
            // workplaceControl
            // 
            this.workplaceControl.Enabled = false;
            this.workplaceControl.Location = new System.Drawing.Point(120, 130);
            this.workplaceControl.Name = "workplaceControl";
            this.workplaceControl.Size = new System.Drawing.Size(140, 21);
            this.workplaceControl.TabIndex = 5;
            this.workplaceControl.UseResetButton = false;
            this.workplaceControl.Leave += new System.EventHandler(this.workplaceControl_Leave);
            // 
            // workplaceLabel
            // 
            this.workplaceLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.workplaceLabel.Location = new System.Drawing.Point(5, 130);
            this.workplaceLabel.Name = "workplaceLabel";
            this.workplaceLabel.Size = new System.Drawing.Size(110, 20);
            this.workplaceLabel.TabIndex = 0;
            this.workplaceLabel.Text = "Рабочее место";
            this.workplaceLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // interruptionStartTimeTextBox
            // 
            this.interruptionStartTimeTextBox.Location = new System.Drawing.Point(115, 180);
            this.interruptionStartTimeTextBox.Mask = "00:00";
            this.interruptionStartTimeTextBox.Name = "interruptionStartTimeTextBox";
            this.interruptionStartTimeTextBox.Size = new System.Drawing.Size(35, 20);
            this.interruptionStartTimeTextBox.TabIndex = 7;
            this.interruptionStartTimeTextBox.Text = "0000";
            this.interruptionStartTimeTextBox.ValidatingType = typeof(System.DateTime);
            this.interruptionStartTimeTextBox.Leave += new System.EventHandler(this.interruptionStartTimeTextBox_Leave);
            // 
            // interruptionLabel
            // 
            this.interruptionLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.interruptionLabel.Location = new System.Drawing.Point(5, 180);
            this.interruptionLabel.Name = "interruptionLabel";
            this.interruptionLabel.Size = new System.Drawing.Size(105, 20);
            this.interruptionLabel.TabIndex = 0;
            this.interruptionLabel.Text = "Время перерыва";
            this.interruptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // interruptionFinishTimeTextBox
            // 
            this.interruptionFinishTimeTextBox.Location = new System.Drawing.Point(155, 180);
            this.interruptionFinishTimeTextBox.Mask = "00:00";
            this.interruptionFinishTimeTextBox.Name = "interruptionFinishTimeTextBox";
            this.interruptionFinishTimeTextBox.Size = new System.Drawing.Size(35, 20);
            this.interruptionFinishTimeTextBox.TabIndex = 8;
            this.interruptionFinishTimeTextBox.Text = "0000";
            this.interruptionFinishTimeTextBox.ValidatingType = typeof(System.DateTime);
            this.interruptionFinishTimeTextBox.Leave += new System.EventHandler(this.interruptionFinishTimeTextBox_Leave);
            // 
            // isInterruptionCheckBox
            // 
            this.isInterruptionCheckBox.AutoSize = true;
            this.isInterruptionCheckBox.Location = new System.Drawing.Point(115, 160);
            this.isInterruptionCheckBox.Name = "isInterruptionCheckBox";
            this.isInterruptionCheckBox.Size = new System.Drawing.Size(72, 17);
            this.isInterruptionCheckBox.TabIndex = 6;
            this.isInterruptionCheckBox.Text = "Перерыв";
            this.isInterruptionCheckBox.UseVisualStyleBackColor = true;
            this.isInterruptionCheckBox.Leave += new System.EventHandler(this.isInterruptionCheckBox_Leave);
            // 
            // EditOperatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(269, 246);
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
            this.Controls.Add(this.isInterruptionCheckBox);
            this.Controls.Add(this.interruptionLabel);
            this.Controls.Add(this.interruptionStartTimeTextBox);
            this.Controls.Add(this.interruptionFinishTimeTextBox);
            this.Controls.Add(this.passwordButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
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
        private System.Windows.Forms.MaskedTextBox interruptionStartTimeTextBox;
        private System.Windows.Forms.Label interruptionLabel;
        private System.Windows.Forms.MaskedTextBox interruptionFinishTimeTextBox;
        private System.Windows.Forms.CheckBox isInterruptionCheckBox;
    }
}