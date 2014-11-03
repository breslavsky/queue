namespace Queue.Database
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
            this.connectButton = new System.Windows.Forms.Button();
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.importMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.damaskMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.topMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(5, 30);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(105, 25);
            this.connectButton.TabIndex = 0;
            this.connectButton.Text = "Подключить";
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // topMenu
            // 
            this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importMenu});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(264, 24);
            this.topMenu.TabIndex = 0;
            this.topMenu.Text = "menuStrip1";
            // 
            // importMenu
            // 
            this.importMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.damaskMenuItem});
            this.importMenu.Name = "importMenu";
            this.importMenu.Size = new System.Drawing.Size(106, 20);
            this.importMenu.Text = "Импорт данных";
            this.importMenu.Click += new System.EventHandler(this.импортДаннызToolStripMenuItem_Click);
            // 
            // damaskMenuItem
            // 
            this.damaskMenuItem.Name = "damaskMenuItem";
            this.damaskMenuItem.Size = new System.Drawing.Size(152, 22);
            this.damaskMenuItem.Text = "Дамаск";
            this.damaskMenuItem.Click += new System.EventHandler(this.damaskMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(264, 67);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.topMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.topMenu;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(280, 105);
            this.Name = "MainForm";
            this.Text = "База данных";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.ToolStripMenuItem importMenu;
        private System.Windows.Forms.ToolStripMenuItem damaskMenuItem;
    }
}

