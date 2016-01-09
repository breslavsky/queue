namespace Queue.Administrator
{
    partial class EditLifeSituationForm
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
            this.commentTextBox = new System.Windows.Forms.TextBox();
            this.commentLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.codeLabel = new System.Windows.Forms.Label();
            this.colorButton = new System.Windows.Forms.Button();
            this.fontSizeTrackBar = new System.Windows.Forms.TrackBar();
            this.fontSizeLabel = new System.Windows.Forms.Label();
            this.fontSizeValueLabel = new System.Windows.Forms.Label();
            this.serviceLabel = new System.Windows.Forms.Label();
            this.serviceChangeLink = new System.Windows.Forms.LinkLabel();
            this.serviceTextBlock = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(285, 455);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(85, 25);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Записать";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // commentTextBox
            // 
            this.commentTextBox.Location = new System.Drawing.Point(20, 245);
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.Size = new System.Drawing.Size(350, 75);
            this.commentTextBox.TabIndex = 5;
            this.commentTextBox.Leave += new System.EventHandler(this.commentTextBox_Leave);
            // 
            // commentLabel
            // 
            this.commentLabel.Location = new System.Drawing.Point(10, 215);
            this.commentLabel.Name = "commentLabel";
            this.commentLabel.Size = new System.Drawing.Size(360, 25);
            this.commentLabel.TabIndex = 0;
            this.commentLabel.Text = "Комментарий";
            this.commentLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(20, 100);
            this.nameTextBox.Multiline = true;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(350, 110);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(10, 70);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(360, 25);
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
            this.codeLabel.Size = new System.Drawing.Size(75, 25);
            this.codeLabel.TabIndex = 0;
            this.codeLabel.Text = "Код";
            this.codeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // colorButton
            // 
            this.colorButton.Location = new System.Drawing.Point(80, 35);
            this.colorButton.Name = "colorButton";
            this.colorButton.Size = new System.Drawing.Size(90, 25);
            this.colorButton.TabIndex = 16;
            this.colorButton.Click += new System.EventHandler(this.colorButton_Click);
            this.colorButton.Leave += new System.EventHandler(this.colorButton_Leave);
            // 
            // fontSizeTrackBar
            // 
            this.fontSizeTrackBar.LargeChange = 10;
            this.fontSizeTrackBar.Location = new System.Drawing.Point(175, 35);
            this.fontSizeTrackBar.Maximum = 300;
            this.fontSizeTrackBar.Name = "fontSizeTrackBar";
            this.fontSizeTrackBar.Size = new System.Drawing.Size(195, 45);
            this.fontSizeTrackBar.TabIndex = 17;
            this.fontSizeTrackBar.Value = 10;
            this.fontSizeTrackBar.ValueChanged += new System.EventHandler(this.fontSizeTrackBar_ValueChanged);
            this.fontSizeTrackBar.Leave += new System.EventHandler(this.fontSizeTrackBar_Leave);
            // 
            // fontSizeLabel
            // 
            this.fontSizeLabel.Location = new System.Drawing.Point(175, 10);
            this.fontSizeLabel.Name = "fontSizeLabel";
            this.fontSizeLabel.Size = new System.Drawing.Size(105, 25);
            this.fontSizeLabel.TabIndex = 18;
            this.fontSizeLabel.Text = "Размер шрифта %";
            this.fontSizeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // fontSizeValueLabel
            // 
            this.fontSizeValueLabel.Location = new System.Drawing.Point(280, 15);
            this.fontSizeValueLabel.Name = "fontSizeValueLabel";
            this.fontSizeValueLabel.Size = new System.Drawing.Size(90, 20);
            this.fontSizeValueLabel.TabIndex = 20;
            this.fontSizeValueLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // serviceLabel
            // 
            this.serviceLabel.Location = new System.Drawing.Point(10, 325);
            this.serviceLabel.Name = "serviceLabel";
            this.serviceLabel.Size = new System.Drawing.Size(360, 25);
            this.serviceLabel.TabIndex = 21;
            this.serviceLabel.Text = "Услуга";
            this.serviceLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceChangeLink
            // 
            this.serviceChangeLink.AutoSize = true;
            this.serviceChangeLink.Location = new System.Drawing.Point(305, 435);
            this.serviceChangeLink.Name = "serviceChangeLink";
            this.serviceChangeLink.Size = new System.Drawing.Size(62, 13);
            this.serviceChangeLink.TabIndex = 23;
            this.serviceChangeLink.TabStop = true;
            this.serviceChangeLink.Text = "[изменить]";
            this.serviceChangeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.serviceChangeLink_LinkClicked);
            // 
            // serviceTextBlock
            // 
            this.serviceTextBlock.BackColor = System.Drawing.Color.White;
            this.serviceTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serviceTextBlock.Location = new System.Drawing.Point(20, 355);
            this.serviceTextBlock.Name = "serviceTextBlock";
            this.serviceTextBlock.Padding = new System.Windows.Forms.Padding(2);
            this.serviceTextBlock.Size = new System.Drawing.Size(350, 80);
            this.serviceTextBlock.TabIndex = 24;
            // 
            // EditLifeSituationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 486);
            this.Controls.Add(this.serviceTextBlock);
            this.Controls.Add(this.serviceChangeLink);
            this.Controls.Add(this.serviceLabel);
            this.Controls.Add(this.fontSizeValueLabel);
            this.Controls.Add(this.fontSizeLabel);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.fontSizeTrackBar);
            this.Controls.Add(this.colorButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.codeLabel);
            this.Controls.Add(this.commentTextBox);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.commentLabel);
            this.Controls.Add(this.nameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditLifeSituationForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Редактирование жизненной ситуации";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditLifeSituationForm_FormClosing);
            this.Load += new System.EventHandler(this.EditLifeSituationForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.Label commentLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.Label codeLabel;
        private System.Windows.Forms.Button colorButton;
        private System.Windows.Forms.TrackBar fontSizeTrackBar;
        private System.Windows.Forms.Label fontSizeLabel;
        private System.Windows.Forms.Label fontSizeValueLabel;
        private System.Windows.Forms.Label serviceLabel;
        private System.Windows.Forms.LinkLabel serviceChangeLink;
        private System.Windows.Forms.Label serviceTextBlock;

    }
}