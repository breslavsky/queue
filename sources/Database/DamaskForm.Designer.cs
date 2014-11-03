namespace Queue.Database
{
    partial class DamaskForm
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
            this.connectionStringTextBox = new System.Windows.Forms.TextBox();
            this.loadButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.importButton = new System.Windows.Forms.Button();
            this.selectCodesFileButton = new System.Windows.Forms.Button();
            this.selectCodesFileTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // connectionStringTextBox
            // 
            this.connectionStringTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionStringTextBox.Location = new System.Drawing.Point(20, 30);
            this.connectionStringTextBox.Multiline = true;
            this.connectionStringTextBox.Name = "connectionStringTextBox";
            this.connectionStringTextBox.Size = new System.Drawing.Size(555, 80);
            this.connectionStringTextBox.TabIndex = 0;
            this.connectionStringTextBox.Text = "Provider=SQLOLEDB;\r\nData Source=localhost\\sqlexpress;\r\nInitial Catalog=damask;\r\nU" +
    "ser ID=damask;\r\nPassword=damask\r\n";
            // 
            // loadButton
            // 
            this.loadButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.loadButton.Location = new System.Drawing.Point(480, 115);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(90, 25);
            this.loadButton.TabIndex = 0;
            this.loadButton.Text = "Подключение";
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(20, 220);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(550, 150);
            this.logTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Строка подключения";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 195);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Журнал импорта";
            // 
            // importButton
            // 
            this.importButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.importButton.Enabled = false;
            this.importButton.Location = new System.Drawing.Point(480, 375);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(90, 25);
            this.importButton.TabIndex = 0;
            this.importButton.Text = "Импорт";
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // selectCodesFileButton
            // 
            this.selectCodesFileButton.Location = new System.Drawing.Point(125, 135);
            this.selectCodesFileButton.Name = "selectCodesFileButton";
            this.selectCodesFileButton.Size = new System.Drawing.Size(145, 25);
            this.selectCodesFileButton.TabIndex = 0;
            this.selectCodesFileButton.Text = "Файл соответствия";
            this.selectCodesFileButton.UseVisualStyleBackColor = true;
            this.selectCodesFileButton.Click += new System.EventHandler(this.selectCodesFileButton_Click);
            // 
            // selectCodesFileTextBox
            // 
            this.selectCodesFileTextBox.Location = new System.Drawing.Point(20, 135);
            this.selectCodesFileTextBox.Name = "selectCodesFileTextBox";
            this.selectCodesFileTextBox.Size = new System.Drawing.Size(100, 20);
            this.selectCodesFileTextBox.TabIndex = 0;
            // 
            // DamaskForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.selectCodesFileTextBox);
            this.Controls.Add(this.selectCodesFileButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.connectionStringTextBox);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.importButton);
            this.MinimumSize = new System.Drawing.Size(470, 350);
            this.Name = "DamaskForm";
            this.Text = "Импорт данных";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox connectionStringTextBox;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button importButton;
        private System.Windows.Forms.Button selectCodesFileButton;
        private System.Windows.Forms.TextBox selectCodesFileTextBox;
    }
}