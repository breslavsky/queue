namespace Queue.Server
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
            this.portUpDown = new System.Windows.Forms.NumericUpDown();
            this.serverGroupBox = new System.Windows.Forms.GroupBox();
            this.isDebugCheckBox = new System.Windows.Forms.CheckBox();
            this.portLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.databaseGroupBox = new System.Windows.Forms.GroupBox();
            this.isValidateCheckBox = new System.Windows.Forms.CheckBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.databaseMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.importMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.damaskMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).BeginInit();
            this.serverGroupBox.SuspendLayout();
            this.databaseGroupBox.SuspendLayout();
            this.topMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // portUpDown
            // 
            this.portUpDown.Location = new System.Drawing.Point(90, 50);
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
            this.portUpDown.Size = new System.Drawing.Size(50, 20);
            this.portUpDown.TabIndex = 0;
            this.portUpDown.Value = new decimal(new int[] {
            4505,
            0,
            0,
            0});
            // 
            // serverGroupBox
            // 
            this.serverGroupBox.Controls.Add(this.isDebugCheckBox);
            this.serverGroupBox.Controls.Add(this.portLabel);
            this.serverGroupBox.Controls.Add(this.portUpDown);
            this.serverGroupBox.Controls.Add(this.startButton);
            this.serverGroupBox.Enabled = false;
            this.serverGroupBox.Location = new System.Drawing.Point(10, 100);
            this.serverGroupBox.Name = "serverGroupBox";
            this.serverGroupBox.Size = new System.Drawing.Size(245, 80);
            this.serverGroupBox.TabIndex = 0;
            this.serverGroupBox.TabStop = false;
            this.serverGroupBox.Text = "Сервер очереди";
            // 
            // isDebugCheckBox
            // 
            this.isDebugCheckBox.AutoSize = true;
            this.isDebugCheckBox.Location = new System.Drawing.Point(10, 25);
            this.isDebugCheckBox.Name = "isDebugCheckBox";
            this.isDebugCheckBox.Size = new System.Drawing.Size(105, 17);
            this.isDebugCheckBox.TabIndex = 1;
            this.isDebugCheckBox.Text = "Режим отладки";
            this.isDebugCheckBox.UseVisualStyleBackColor = true;
            // 
            // portLabel
            // 
            this.portLabel.AutoSize = true;
            this.portLabel.Location = new System.Drawing.Point(10, 55);
            this.portLabel.Name = "portLabel";
            this.portLabel.Size = new System.Drawing.Size(74, 13);
            this.portLabel.TabIndex = 0;
            this.portLabel.Text = "Порт службы";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(145, 45);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(90, 25);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Запустить";
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // databaseGroupBox
            // 
            this.databaseGroupBox.Controls.Add(this.isValidateCheckBox);
            this.databaseGroupBox.Controls.Add(this.connectButton);
            this.databaseGroupBox.Location = new System.Drawing.Point(10, 30);
            this.databaseGroupBox.Name = "databaseGroupBox";
            this.databaseGroupBox.Size = new System.Drawing.Size(245, 60);
            this.databaseGroupBox.TabIndex = 0;
            this.databaseGroupBox.TabStop = false;
            this.databaseGroupBox.Text = "База данных";
            // 
            // isValidateCheckBox
            // 
            this.isValidateCheckBox.AutoSize = true;
            this.isValidateCheckBox.Checked = true;
            this.isValidateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.isValidateCheckBox.Location = new System.Drawing.Point(125, 30);
            this.isValidateCheckBox.Name = "isValidateCheckBox";
            this.isValidateCheckBox.Size = new System.Drawing.Size(114, 17);
            this.isValidateCheckBox.TabIndex = 0;
            this.isValidateCheckBox.Text = "Проверить схему";
            this.isValidateCheckBox.UseVisualStyleBackColor = true;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(10, 25);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(105, 25);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Подключить";
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // topMenu
            // 
            this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.databaseMenu});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(264, 24);
            this.topMenu.TabIndex = 0;
            this.topMenu.Text = "menuStrip1";
            // 
            // databaseMenu
            // 
            this.databaseMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importMenu});
            this.databaseMenu.Enabled = false;
            this.databaseMenu.Image = ((System.Drawing.Image)(resources.GetObject("databaseMenu.Image")));
            this.databaseMenu.Name = "databaseMenu";
            this.databaseMenu.Size = new System.Drawing.Size(102, 20);
            this.databaseMenu.Text = "База данных";
            // 
            // importMenu
            // 
            this.importMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.damaskMenuItem});
            this.importMenu.Name = "importMenu";
            this.importMenu.Size = new System.Drawing.Size(161, 22);
            this.importMenu.Text = "Импорт данных";
            // 
            // damaskMenuItem
            // 
            this.damaskMenuItem.Name = "damaskMenuItem";
            this.damaskMenuItem.Size = new System.Drawing.Size(115, 22);
            this.damaskMenuItem.Text = "Дамаск";
            this.damaskMenuItem.Click += new System.EventHandler(this.damaskMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 192);
            this.Controls.Add(this.topMenu);
            this.Controls.Add(this.databaseGroupBox);
            this.Controls.Add(this.serverGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.topMenu;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(280, 205);
            this.Name = "MainForm";
            this.Text = "Сервер очереди";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.portUpDown)).EndInit();
            this.serverGroupBox.ResumeLayout(false);
            this.serverGroupBox.PerformLayout();
            this.databaseGroupBox.ResumeLayout(false);
            this.databaseGroupBox.PerformLayout();
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown portUpDown;
        private System.Windows.Forms.GroupBox serverGroupBox;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Label portLabel;
        private System.Windows.Forms.GroupBox databaseGroupBox;
        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.ToolStripMenuItem databaseMenu;
        private System.Windows.Forms.ToolStripMenuItem importMenu;
        private System.Windows.Forms.ToolStripMenuItem damaskMenuItem;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.CheckBox isValidateCheckBox;
        private System.Windows.Forms.CheckBox isDebugCheckBox;
    }
}

