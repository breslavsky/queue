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
            this.databaseGroupBox = new System.Windows.Forms.GroupBox();
            this.isValidateCheckBox = new System.Windows.Forms.CheckBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.databaseMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.importMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.damaskMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.databaseGroupBox.SuspendLayout();
            this.topMenu.SuspendLayout();
            this.SuspendLayout();
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.topMenu;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(280, 205);
            this.Name = "MainForm";
            this.Text = "Сервер очереди";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.databaseGroupBox.ResumeLayout(false);
            this.databaseGroupBox.PerformLayout();
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox databaseGroupBox;
        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.ToolStripMenuItem databaseMenu;
        private System.Windows.Forms.ToolStripMenuItem importMenu;
        private System.Windows.Forms.ToolStripMenuItem damaskMenuItem;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.CheckBox isValidateCheckBox;
    }
}

