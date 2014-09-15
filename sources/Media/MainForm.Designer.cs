namespace Queue.Media
{
    partial class MainForm
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.UACButton = new System.Windows.Forms.ToolStripMenuItem();
            this.openButton = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.serverGroupBox = new System.Windows.Forms.GroupBox();
            this.selectFolderButton = new System.Windows.Forms.Button();
            this.folderTextBox = new System.Windows.Forms.TextBox();
            this.folderLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.portUpDown = new System.Windows.Forms.NumericUpDown();
            this.portLabel = new System.Windows.Forms.Label();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.topMenu.SuspendLayout();
            this.serverGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // topMenu
            // 
            this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UACButton,
            this.openButton,
            this.logoutButton});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(279, 24);
            this.topMenu.TabIndex = 0;
            this.topMenu.Text = "menuStrip1";
            // 
            // UACButton
            // 
            this.UACButton.Image = ((System.Drawing.Image)(resources.GetObject("UACButton.Image")));
            this.UACButton.Name = "UACButton";
            this.UACButton.Size = new System.Drawing.Size(59, 20);
            this.UACButton.Text = "UAC";
            this.UACButton.ToolTipText = "Повысить права";
            this.UACButton.Click += new System.EventHandler(this.UACButton_Click);
            // 
            // openButton
            // 
            this.openButton.Enabled = false;
            this.openButton.Image = ((System.Drawing.Image)(resources.GetObject("openButton.Image")));
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(82, 20);
            this.openButton.Text = "Открыть";
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // logoutButton
            // 
            this.logoutButton.Image = ((System.Drawing.Image)(resources.GetObject("logoutButton.Image")));
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(70, 20);
            this.logoutButton.Text = "Выйти";
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // serverGroupBox
            // 
            this.serverGroupBox.Controls.Add(this.folderLabel);
            this.serverGroupBox.Controls.Add(this.folderTextBox);
            this.serverGroupBox.Controls.Add(this.selectFolderButton);
            this.serverGroupBox.Controls.Add(this.portLabel);
            this.serverGroupBox.Controls.Add(this.portUpDown);
            this.serverGroupBox.Controls.Add(this.startButton);
            this.serverGroupBox.Location = new System.Drawing.Point(10, 35);
            this.serverGroupBox.Name = "serverGroupBox";
            this.serverGroupBox.Size = new System.Drawing.Size(260, 85);
            this.serverGroupBox.TabIndex = 0;
            this.serverGroupBox.TabStop = false;
            this.serverGroupBox.Text = "Параметры службы";
            // 
            // selectFolderButton
            // 
            this.selectFolderButton.Location = new System.Drawing.Point(215, 20);
            this.selectFolderButton.Name = "selectFolderButton";
            this.selectFolderButton.Size = new System.Drawing.Size(35, 25);
            this.selectFolderButton.TabIndex = 0;
            this.selectFolderButton.Text = "...";
            this.selectFolderButton.Click += new System.EventHandler(this.selectFolderButton_Click);
            // 
            // folderTextBox
            // 
            this.folderTextBox.Location = new System.Drawing.Point(115, 20);
            this.folderTextBox.Name = "folderTextBox";
            this.folderTextBox.ReadOnly = true;
            this.folderTextBox.Size = new System.Drawing.Size(95, 20);
            this.folderTextBox.TabIndex = 0;
            // 
            // folderLabel
            // 
            this.folderLabel.Location = new System.Drawing.Point(10, 25);
            this.folderLabel.Name = "folderLabel";
            this.folderLabel.Size = new System.Drawing.Size(100, 15);
            this.folderLabel.TabIndex = 0;
            this.folderLabel.Text = "Папка контента";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(175, 50);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 25);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Запуск";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // portUpDown
            // 
            this.portUpDown.Location = new System.Drawing.Point(115, 55);
            this.portUpDown.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.portUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.portUpDown.Name = "portUpDown";
            this.portUpDown.Size = new System.Drawing.Size(55, 20);
            this.portUpDown.TabIndex = 0;
            this.portUpDown.Value = new decimal(new int[] {
            9091,
            0,
            0,
            0});
            // 
            // portLabel
            // 
            this.portLabel.Location = new System.Drawing.Point(10, 60);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(100, 15);
            this.portLabel.TabIndex = 0;
            this.portLabel.Text = "HTTP порт";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(279, 132);
            this.Controls.Add(this.topMenu);
            this.Controls.Add(this.serverGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(295, 170);
            this.Name = "MainForm";
            this.Text = "Служба мультимедиа";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.serverGroupBox.ResumeLayout(false);
            this.serverGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.ToolStripMenuItem openButton;
        private System.Windows.Forms.ToolStripMenuItem logoutButton;
        private System.Windows.Forms.GroupBox serverGroupBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.NumericUpDown portUpDown;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.ToolStripMenuItem UACButton;
        private System.Windows.Forms.Label folderLabel;
        private System.Windows.Forms.Button selectFolderButton;
        private System.Windows.Forms.TextBox folderTextBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

