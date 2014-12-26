namespace Queue.Administrator
{
    partial class EditMediaConfigFileForm
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
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.uploadMediaFileProgressBar = new System.Windows.Forms.ProgressBar();
            this.uploadButton = new System.Windows.Forms.Button();
            this.selectMediaFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(205, 90);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(100, 5);
            this.nameTextBox.Multiline = true;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(180, 50);
            this.nameTextBox.TabIndex = 0;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(5, 5);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(90, 20);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Наименование";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // uploadMediaFileProgressBar
            // 
            this.uploadMediaFileProgressBar.Location = new System.Drawing.Point(5, 60);
            this.uploadMediaFileProgressBar.Name = "uploadMediaFileProgressBar";
            this.uploadMediaFileProgressBar.Size = new System.Drawing.Size(195, 25);
            this.uploadMediaFileProgressBar.TabIndex = 0;
            // 
            // uploadButton
            // 
            this.uploadButton.Location = new System.Drawing.Point(205, 60);
            this.uploadButton.Name = "uploadButton";
            this.uploadButton.Size = new System.Drawing.Size(75, 25);
            this.uploadButton.TabIndex = 1;
            this.uploadButton.Text = "Загрузить";
            this.uploadButton.UseVisualStyleBackColor = true;
            this.uploadButton.Click += new System.EventHandler(this.uploadButton_Click);
            // 
            // selectMediaFileDialog
            // 
            this.selectMediaFileDialog.FileName = "openFileDialog1";
            // 
            // EditMediaConfigFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 121);
            this.Controls.Add(this.uploadButton);
            this.Controls.Add(this.uploadMediaFileProgressBar);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.nameLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditMediaConfigFileForm";
            this.Text = "Изменить видео-файл";
            this.Load += new System.EventHandler(this.EditMediaConfigFileForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.ProgressBar uploadMediaFileProgressBar;
        private System.Windows.Forms.Button uploadButton;
        private System.Windows.Forms.OpenFileDialog selectMediaFileDialog;
    }
}