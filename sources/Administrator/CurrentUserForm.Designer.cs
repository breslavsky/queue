namespace Queue.Administrator
{
    partial class CurrentUserForm
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
            this.passwordButton = new System.Windows.Forms.Button();
            this.currentUserLabel = new System.Windows.Forms.Label();
            this.couponPrintersComboBox = new System.Windows.Forms.ComboBox();
            this.couponPrinterLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.serringsGroupBox = new System.Windows.Forms.GroupBox();
            this.serringsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // passwordButton
            // 
            this.passwordButton.Location = new System.Drawing.Point(225, 5);
            this.passwordButton.Name = "passwordButton";
            this.passwordButton.Size = new System.Drawing.Size(115, 25);
            this.passwordButton.TabIndex = 0;
            this.passwordButton.Text = "Сменить пароль";
            this.passwordButton.UseVisualStyleBackColor = true;
            this.passwordButton.Click += new System.EventHandler(this.passwordButton_Click);
            // 
            // currentUserLabel
            // 
            this.currentUserLabel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.currentUserLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.currentUserLabel.Location = new System.Drawing.Point(5, 5);
            this.currentUserLabel.Name = "currentUserLabel";
            this.currentUserLabel.Size = new System.Drawing.Size(215, 25);
            this.currentUserLabel.TabIndex = 1;
            this.currentUserLabel.Text = "-";
            this.currentUserLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // couponPrintersComboBox
            // 
            this.couponPrintersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.couponPrintersComboBox.FormattingEnabled = true;
            this.couponPrintersComboBox.Location = new System.Drawing.Point(160, 25);
            this.couponPrintersComboBox.Name = "couponPrintersComboBox";
            this.couponPrintersComboBox.Size = new System.Drawing.Size(170, 21);
            this.couponPrintersComboBox.TabIndex = 14;
            this.couponPrintersComboBox.Leave += new System.EventHandler(this.couponPrintersComboBox_Leave);
            // 
            // couponPrinterLabel
            // 
            this.couponPrinterLabel.Location = new System.Drawing.Point(5, 20);
            this.couponPrinterLabel.Name = "couponPrinterLabel";
            this.couponPrinterLabel.Size = new System.Drawing.Size(150, 25);
            this.couponPrinterLabel.TabIndex = 15;
            this.couponPrinterLabel.Text = "Принтер для печати чеков";
            this.couponPrinterLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(255, 50);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 16;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // serringsGroupBox
            // 
            this.serringsGroupBox.Controls.Add(this.couponPrintersComboBox);
            this.serringsGroupBox.Controls.Add(this.saveButton);
            this.serringsGroupBox.Controls.Add(this.couponPrinterLabel);
            this.serringsGroupBox.Location = new System.Drawing.Point(5, 35);
            this.serringsGroupBox.Name = "serringsGroupBox";
            this.serringsGroupBox.Size = new System.Drawing.Size(335, 80);
            this.serringsGroupBox.TabIndex = 17;
            this.serringsGroupBox.TabStop = false;
            this.serringsGroupBox.Text = "Настройки программы";
            // 
            // CurrentUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(344, 122);
            this.Controls.Add(this.serringsGroupBox);
            this.Controls.Add(this.currentUserLabel);
            this.Controls.Add(this.passwordButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CurrentUserForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Текущий пользователь";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CurrentUserForm_FormClosing);
            this.Load += new System.EventHandler(this.CurrentUserForm_Load);
            this.serringsGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button passwordButton;
        private System.Windows.Forms.Label currentUserLabel;
        private System.Windows.Forms.ComboBox couponPrintersComboBox;
        private System.Windows.Forms.Label couponPrinterLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox serringsGroupBox;
    }
}