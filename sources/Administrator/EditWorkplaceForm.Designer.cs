namespace Queue.Administrator
{
    partial class EditWorkplaceForm
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
            this.commentTextBox = new System.Windows.Forms.TextBox();
            this.typeLabel = new System.Windows.Forms.Label();
            this.numberLabel = new System.Windows.Forms.Label();
            this.numberUpDown = new System.Windows.Forms.NumericUpDown();
            this.modificatorLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.displayLabel = new System.Windows.Forms.Label();
            this.segmentsLabel = new System.Windows.Forms.Label();
            this.segmentsUpDown = new System.Windows.Forms.NumericUpDown();
            this.displayUpDown = new System.Windows.Forms.NumericUpDown();
            this.saveButton = new System.Windows.Forms.Button();
            this.typeControl = new Queue.UI.WinForms.EnumItemControl();
            this.modificatorControl = new Queue.UI.WinForms.EnumItemControl();
            ((System.ComponentModel.ISupportInitialize)(this.numberUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.segmentsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // commentTextBox
            // 
            this.commentTextBox.Location = new System.Drawing.Point(10, 120);
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.Size = new System.Drawing.Size(245, 65);
            this.commentTextBox.TabIndex = 4;
            this.commentTextBox.Leave += new System.EventHandler(this.commentTextBox_Leave);
            // 
            // typeLabel
            // 
            this.typeLabel.Location = new System.Drawing.Point(10, 10);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(90, 20);
            this.typeLabel.TabIndex = 5;
            this.typeLabel.Text = "Тип";
            this.typeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // numberLabel
            // 
            this.numberLabel.Location = new System.Drawing.Point(10, 40);
            this.numberLabel.Name = "numberLabel";
            this.numberLabel.Size = new System.Drawing.Size(90, 20);
            this.numberLabel.TabIndex = 6;
            this.numberLabel.Text = "Номер";
            this.numberLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // numberUpDown
            // 
            this.numberUpDown.Location = new System.Drawing.Point(105, 40);
            this.numberUpDown.Name = "numberUpDown";
            this.numberUpDown.Size = new System.Drawing.Size(60, 20);
            this.numberUpDown.TabIndex = 2;
            this.numberUpDown.Leave += new System.EventHandler(this.numberUpDown_Leave);
            // 
            // modificatorLabel
            // 
            this.modificatorLabel.Location = new System.Drawing.Point(10, 65);
            this.modificatorLabel.Name = "modificatorLabel";
            this.modificatorLabel.Size = new System.Drawing.Size(90, 20);
            this.modificatorLabel.TabIndex = 8;
            this.modificatorLabel.Text = "Модификатор";
            this.modificatorLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 20);
            this.label1.TabIndex = 9;
            this.label1.Text = "Комментарий";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // displayLabel
            // 
            this.displayLabel.Location = new System.Drawing.Point(10, 190);
            this.displayLabel.Name = "displayLabel";
            this.displayLabel.Size = new System.Drawing.Size(100, 20);
            this.displayLabel.TabIndex = 10;
            this.displayLabel.Text = "Номер табло";
            this.displayLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // segmentsLabel
            // 
            this.segmentsLabel.Location = new System.Drawing.Point(10, 215);
            this.segmentsLabel.Name = "segmentsLabel";
            this.segmentsLabel.Size = new System.Drawing.Size(100, 20);
            this.segmentsLabel.TabIndex = 12;
            this.segmentsLabel.Text = "Кол-во сегментов";
            this.segmentsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // segmentsUpDown
            // 
            this.segmentsUpDown.Location = new System.Drawing.Point(115, 215);
            this.segmentsUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.segmentsUpDown.Name = "segmentsUpDown";
            this.segmentsUpDown.Size = new System.Drawing.Size(60, 20);
            this.segmentsUpDown.TabIndex = 6;
            this.segmentsUpDown.Leave += new System.EventHandler(this.segmentsUpDown_Leave);
            // 
            // displayUpDown
            // 
            this.displayUpDown.Location = new System.Drawing.Point(115, 190);
            this.displayUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.displayUpDown.Name = "displayUpDown";
            this.displayUpDown.Size = new System.Drawing.Size(60, 20);
            this.displayUpDown.TabIndex = 5;
            this.displayUpDown.Leave += new System.EventHandler(this.displayUpDown_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(180, 240);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Записать";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // typeControl
            // 
            this.typeControl.Enabled = false;
            this.typeControl.Location = new System.Drawing.Point(105, 5);
            this.typeControl.Name = "typeControl";
            this.typeControl.Size = new System.Drawing.Size(150, 21);
            this.typeControl.TabIndex = 1;
            this.typeControl.Leave += new System.EventHandler(this.typeControl_Leave);
            // 
            // modificatorControl
            // 
            this.modificatorControl.Enabled = false;
            this.modificatorControl.Location = new System.Drawing.Point(105, 65);
            this.modificatorControl.Name = "modificatorControl";
            this.modificatorControl.Size = new System.Drawing.Size(150, 21);
            this.modificatorControl.TabIndex = 3;
            this.modificatorControl.Leave += new System.EventHandler(this.modificatorControl_Leave);
            // 
            // EditWorkplaceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 271);
            this.Controls.Add(this.modificatorControl);
            this.Controls.Add(this.typeControl);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.displayUpDown);
            this.Controls.Add(this.segmentsUpDown);
            this.Controls.Add(this.segmentsLabel);
            this.Controls.Add(this.displayLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.modificatorLabel);
            this.Controls.Add(this.numberUpDown);
            this.Controls.Add(this.numberLabel);
            this.Controls.Add(this.typeLabel);
            this.Controls.Add(this.commentTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditWorkplaceForm";
            this.Text = "Изменение рабочего места";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditWorkplaceForm_FormClosing);
            this.Load += new System.EventHandler(this.EditWorkplaceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numberUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.segmentsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.displayUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.Label numberLabel;
        private System.Windows.Forms.NumericUpDown numberUpDown;
        private System.Windows.Forms.Label modificatorLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label displayLabel;
        private System.Windows.Forms.Label segmentsLabel;
        private System.Windows.Forms.NumericUpDown segmentsUpDown;
        private System.Windows.Forms.NumericUpDown displayUpDown;
        private System.Windows.Forms.Button saveButton;
        private UI.WinForms.EnumItemControl typeControl;
        private UI.WinForms.EnumItemControl modificatorControl;
    }
}