namespace Queue.Administrator
{
    partial class ServiceGroupEditForm
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
            this.saveButton = new System.Windows.Forms.Button();
            this.commentLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.iconImageBox = new System.Windows.Forms.PictureBox();
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.codeLabel = new System.Windows.Forms.Label();
            this.colorButton = new System.Windows.Forms.Button();
            this.colorPanel = new System.Windows.Forms.Panel();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.commentTextBox = new System.Windows.Forms.TextBox();
            this.layoutGroupBox = new System.Windows.Forms.GroupBox();
            this.columnsUpDown = new System.Windows.Forms.NumericUpDown();
            this.rowsUpDown = new System.Windows.Forms.NumericUpDown();
            this.rowsLabel = new System.Windows.Forms.Label();
            this.columnsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.iconImageBox)).BeginInit();
            this.layoutGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(315, 365);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(80, 25);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Сохранить";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // commentLabel
            // 
            this.commentLabel.AutoSize = true;
            this.commentLabel.Location = new System.Drawing.Point(10, 150);
            this.commentLabel.Name = "commentLabel";
            this.commentLabel.Size = new System.Drawing.Size(77, 13);
            this.commentLabel.TabIndex = 0;
            this.commentLabel.Text = "Комментарий";
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(20, 95);
            this.nameTextBox.Multiline = true;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(375, 45);
            this.nameTextBox.TabIndex = 0;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(10, 75);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(83, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Наименование";
            // 
            // iconImageBox
            // 
            this.iconImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.iconImageBox.Location = new System.Drawing.Point(330, 20);
            this.iconImageBox.Name = "iconImageBox";
            this.iconImageBox.Size = new System.Drawing.Size(64, 64);
            this.iconImageBox.TabIndex = 0;
            this.iconImageBox.TabStop = false;
            this.iconImageBox.Click += new System.EventHandler(this.iconImageBox_Click);
            // 
            // codeTextBox
            // 
            this.codeTextBox.Location = new System.Drawing.Point(20, 40);
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(55, 20);
            this.codeTextBox.TabIndex = 0;
            this.codeTextBox.Leave += new System.EventHandler(this.codeTextBox_Leave);
            // 
            // codeLabel
            // 
            this.codeLabel.AutoSize = true;
            this.codeLabel.Location = new System.Drawing.Point(10, 20);
            this.codeLabel.Name = "codeLabel";
            this.codeLabel.Size = new System.Drawing.Size(26, 13);
            this.codeLabel.TabIndex = 0;
            this.codeLabel.Text = "Код";
            // 
            // colorButton
            // 
            this.colorButton.Location = new System.Drawing.Point(180, 35);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(80, 25);
            this.colorButton.TabIndex = 0;
            this.colorButton.Text = "Выбрать";
            this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
            // 
            // colorPanel
            // 
            this.colorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.colorPanel.Location = new System.Drawing.Point(85, 35);
            this.colorPanel.Name = "colorPanel";
            this.colorPanel.Size = new System.Drawing.Size(90, 25);
            this.colorPanel.TabIndex = 0;
            this.colorPanel.Leave += new System.EventHandler(this.colorPanel_Leave);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(10, 245);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(57, 13);
            this.descriptionLabel.TabIndex = 3;
            this.descriptionLabel.Text = "Описание";
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(15, 270);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ReadOnly = true;
            this.descriptionTextBox.Size = new System.Drawing.Size(380, 85);
            this.descriptionTextBox.TabIndex = 4;
            this.descriptionTextBox.Click += new System.EventHandler(this.descriptionTextBox_Click);
            this.descriptionTextBox.Leave += new System.EventHandler(this.descriptionTextBox_Leave);
            // 
            // commentTextBox
            // 
            this.commentTextBox.Location = new System.Drawing.Point(20, 175);
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.Size = new System.Drawing.Size(375, 60);
            this.commentTextBox.TabIndex = 5;
            this.commentTextBox.Leave += new System.EventHandler(this.commentTextBox_Leave);
            // 
            // layoutGroupBox
            // 
            this.layoutGroupBox.Controls.Add(this.columnsUpDown);
            this.layoutGroupBox.Controls.Add(this.rowsUpDown);
            this.layoutGroupBox.Controls.Add(this.rowsLabel);
            this.layoutGroupBox.Controls.Add(this.columnsLabel);
            this.layoutGroupBox.Location = new System.Drawing.Point(15, 365);
            this.layoutGroupBox.Name = "layoutGroupBox";
            this.layoutGroupBox.Size = new System.Drawing.Size(230, 55);
            this.layoutGroupBox.TabIndex = 6;
            this.layoutGroupBox.TabStop = false;
            this.layoutGroupBox.Text = "Расположение услуг";
            // 
            // columnsUpDown
            // 
            this.columnsUpDown.Location = new System.Drawing.Point(65, 20);
            this.columnsUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.columnsUpDown.Name = "columnsUpDown";
            this.columnsUpDown.Size = new System.Drawing.Size(50, 20);
            this.columnsUpDown.TabIndex = 1;
            this.columnsUpDown.Leave += new System.EventHandler(this.columnsUpDown_Leave);
            // 
            // rowsUpDown
            // 
            this.rowsUpDown.Location = new System.Drawing.Point(165, 20);
            this.rowsUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.rowsUpDown.Name = "rowsUpDown";
            this.rowsUpDown.Size = new System.Drawing.Size(50, 20);
            this.rowsUpDown.TabIndex = 3;
            this.rowsUpDown.Leave += new System.EventHandler(this.rowsUpDown_Leave);
            // 
            // rowsLabel
            // 
            this.rowsLabel.AutoSize = true;
            this.rowsLabel.Location = new System.Drawing.Point(125, 25);
            this.rowsLabel.Name = "rowsLabel";
            this.rowsLabel.Size = new System.Drawing.Size(37, 13);
            this.rowsLabel.TabIndex = 4;
            this.rowsLabel.Text = "Строк";
            // 
            // columnsLabel
            // 
            this.columnsLabel.AutoSize = true;
            this.columnsLabel.Location = new System.Drawing.Point(12, 25);
            this.columnsLabel.Name = "columnsLabel";
            this.columnsLabel.Size = new System.Drawing.Size(50, 13);
            this.columnsLabel.TabIndex = 2;
            this.columnsLabel.Text = "Колонок";
            // 
            // ServiceGroupEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 432);
            this.Controls.Add(this.layoutGroupBox);
            this.Controls.Add(this.commentTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.codeLabel);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.iconImageBox);
            this.Controls.Add(this.commentLabel);
            this.Controls.Add(this.colorPanel);
            this.Controls.Add(this.colorButton);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(430, 470);
            this.Name = "ServiceGroupEditForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Редактирование группы услуг";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServiceGroupEditForm_FormClosing);
            this.Load += new System.EventHandler(this.ServiceGroupEdit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.iconImageBox)).EndInit();
            this.layoutGroupBox.ResumeLayout(false);
            this.layoutGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label commentLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.PictureBox iconImageBox;
        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.Label codeLabel;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.Panel colorPanel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.GroupBox layoutGroupBox;
        private System.Windows.Forms.NumericUpDown columnsUpDown;
        private System.Windows.Forms.NumericUpDown rowsUpDown;
        private System.Windows.Forms.Label rowsLabel;
        private System.Windows.Forms.Label columnsLabel;
    }
}