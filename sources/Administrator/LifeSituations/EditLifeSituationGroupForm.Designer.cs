namespace Queue.Administrator
{
    partial class EditLifeSituationGroupForm
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
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.codeLabel = new System.Windows.Forms.Label();
            this.colorButton = new System.Windows.Forms.Button();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.commentTextBox = new System.Windows.Forms.TextBox();
            this.layoutGroupBox = new System.Windows.Forms.GroupBox();
            this.columnsLabel = new System.Windows.Forms.Label();
            this.columnsUpDown = new System.Windows.Forms.NumericUpDown();
            this.rowsLabel = new System.Windows.Forms.Label();
            this.rowsUpDown = new System.Windows.Forms.NumericUpDown();
            this.fontSizeLabel = new System.Windows.Forms.Label();
            this.fontSizeValueLabel = new System.Windows.Forms.Label();
            this.fontSizeTrackBar = new System.Windows.Forms.TrackBar();
            this.layoutGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(325, 420);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(80, 25);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Записать";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // commentLabel
            // 
            this.commentLabel.Location = new System.Drawing.Point(10, 150);
            this.commentLabel.Name = "commentLabel";
            this.commentLabel.Size = new System.Drawing.Size(395, 18);
            this.commentLabel.TabIndex = 0;
            this.commentLabel.Text = "Комментарий";
            this.commentLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(20, 95);
            this.nameTextBox.Multiline = true;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(385, 45);
            this.nameTextBox.TabIndex = 2;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(10, 70);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(395, 18);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Наименование";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
            this.codeLabel.Location = new System.Drawing.Point(10, 10);
            this.codeLabel.Name = "codeLabel";
            this.codeLabel.Size = new System.Drawing.Size(65, 25);
            this.codeLabel.TabIndex = 0;
            this.codeLabel.Text = "Код";
            this.codeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // colorButton
            // 
            this.colorButton.Location = new System.Drawing.Point(80, 35);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(90, 25);
            this.colorButton.TabIndex = 1;
            this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
            this.colorButton.Leave += new System.EventHandler(this.colorButton_Leave);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Location = new System.Drawing.Point(10, 245);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(395, 18);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Описание";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(15, 270);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ReadOnly = true;
            this.descriptionTextBox.Size = new System.Drawing.Size(390, 85);
            this.descriptionTextBox.TabIndex = 4;
            this.descriptionTextBox.Click += new System.EventHandler(this.descriptionTextBox_Click);
            this.descriptionTextBox.Leave += new System.EventHandler(this.descriptionTextBox_Leave);
            // 
            // commentTextBox
            // 
            this.commentTextBox.Location = new System.Drawing.Point(20, 175);
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.Size = new System.Drawing.Size(385, 60);
            this.commentTextBox.TabIndex = 3;
            this.commentTextBox.Leave += new System.EventHandler(this.commentTextBox_Leave);
            // 
            // layoutGroupBox
            // 
            this.layoutGroupBox.Controls.Add(this.columnsLabel);
            this.layoutGroupBox.Controls.Add(this.columnsUpDown);
            this.layoutGroupBox.Controls.Add(this.rowsLabel);
            this.layoutGroupBox.Controls.Add(this.rowsUpDown);
            this.layoutGroupBox.Location = new System.Drawing.Point(15, 360);
            this.layoutGroupBox.Name = "layoutGroupBox";
            this.layoutGroupBox.Size = new System.Drawing.Size(230, 55);
            this.layoutGroupBox.TabIndex = 5;
            this.layoutGroupBox.TabStop = false;
            this.layoutGroupBox.Text = "Расположение";
            // 
            // columnsLabel
            // 
            this.columnsLabel.Location = new System.Drawing.Point(5, 25);
            this.columnsLabel.Name = "columnsLabel";
            this.columnsLabel.Size = new System.Drawing.Size(55, 13);
            this.columnsLabel.TabIndex = 0;
            this.columnsLabel.Text = "Колонок";
            this.columnsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
            this.columnsUpDown.TabIndex = 5;
            this.columnsUpDown.Leave += new System.EventHandler(this.columnsUpDown_Leave);
            // 
            // rowsLabel
            // 
            this.rowsLabel.Location = new System.Drawing.Point(120, 25);
            this.rowsLabel.Name = "rowsLabel";
            this.rowsLabel.Size = new System.Drawing.Size(40, 13);
            this.rowsLabel.TabIndex = 0;
            this.rowsLabel.Text = "Строк";
            this.rowsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
            this.rowsUpDown.TabIndex = 6;
            this.rowsUpDown.Leave += new System.EventHandler(this.rowsUpDown_Leave);
            // 
            // fontSizeLabel
            // 
            this.fontSizeLabel.Location = new System.Drawing.Point(175, 10);
            this.fontSizeLabel.Name = "fontSizeLabel";
            this.fontSizeLabel.Size = new System.Drawing.Size(110, 25);
            this.fontSizeLabel.TabIndex = 15;
            this.fontSizeLabel.Text = "Размер шрифта %";
            this.fontSizeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // fontSizeValueLabel
            // 
            this.fontSizeValueLabel.Location = new System.Drawing.Point(285, 10);
            this.fontSizeValueLabel.Name = "fontSizeValueLabel";
            this.fontSizeValueLabel.Size = new System.Drawing.Size(115, 25);
            this.fontSizeValueLabel.TabIndex = 19;
            this.fontSizeValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // fontSizeTrackBar
            // 
            this.fontSizeTrackBar.LargeChange = 10;
            this.fontSizeTrackBar.Location = new System.Drawing.Point(170, 35);
            this.fontSizeTrackBar.Maximum = 300;
            this.fontSizeTrackBar.Minimum = 10;
            this.fontSizeTrackBar.Name = "fontSizeTrackBar";
            this.fontSizeTrackBar.Size = new System.Drawing.Size(230, 45);
            this.fontSizeTrackBar.TabIndex = 18;
            this.fontSizeTrackBar.Value = 10;
            this.fontSizeTrackBar.ValueChanged += new System.EventHandler(this.fontSizeTrackBar_ValueChanged);
            this.fontSizeTrackBar.Leave += new System.EventHandler(this.fontSizeTrackBar_Leave);
            // 
            // EditLifeSituationGroupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 451);
            this.Controls.Add(this.fontSizeValueLabel);
            this.Controls.Add(this.fontSizeTrackBar);
            this.Controls.Add(this.fontSizeLabel);
            this.Controls.Add(this.codeLabel);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.colorButton);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.commentLabel);
            this.Controls.Add(this.commentTextBox);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.layoutGroupBox);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 470);
            this.Name = "EditLifeSituationGroupForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Редактирование жизненной ситуации";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServiceGroupEditForm_FormClosing);
            this.Load += new System.EventHandler(this.ServiceGroupEdit_Load);
            this.layoutGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label commentLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.Label codeLabel;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.GroupBox layoutGroupBox;
        private System.Windows.Forms.NumericUpDown columnsUpDown;
        private System.Windows.Forms.NumericUpDown rowsUpDown;
        private System.Windows.Forms.Label rowsLabel;
        private System.Windows.Forms.Label columnsLabel;
        private System.Windows.Forms.Label fontSizeLabel;
        private System.Windows.Forms.Label fontSizeValueLabel;
        private System.Windows.Forms.TrackBar fontSizeTrackBar;
    }
}