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
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.connectButton = new System.Windows.Forms.ToolStripMenuItem();
            this.schemaMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.schemaValidateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schemaUpdateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.constraintUpdateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.checkPatchesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.installPatchesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.initDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demoDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.damaskImportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.topMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // topMenu
            // 
            this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectButton,
            this.schemaMenu,
            this.dataMenu});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(628, 24);
            this.topMenu.TabIndex = 0;
            this.topMenu.Text = "menuStrip1";
            // 
            // connectButton
            // 
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(89, 20);
            this.connectButton.Text = "Подключить";
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // schemaMenu
            // 
            this.schemaMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.schemaValidateMenuItem,
            this.schemaUpdateMenuItem,
            this.constraintUpdateMenuItem});
            this.schemaMenu.Enabled = false;
            this.schemaMenu.Name = "schemaMenu";
            this.schemaMenu.Size = new System.Drawing.Size(75, 20);
            this.schemaMenu.Text = "Структура";
            // 
            // schemaValidateMenuItem
            // 
            this.schemaValidateMenuItem.Name = "schemaValidateMenuItem";
            this.schemaValidateMenuItem.Size = new System.Drawing.Size(191, 22);
            this.schemaValidateMenuItem.Text = "Проверить структуру";
            this.schemaValidateMenuItem.Click += new System.EventHandler(this.schemaValidateMenu_Click);
            // 
            // schemaUpdateMenuItem
            // 
            this.schemaUpdateMenuItem.Name = "schemaUpdateMenuItem";
            this.schemaUpdateMenuItem.Size = new System.Drawing.Size(191, 22);
            this.schemaUpdateMenuItem.Text = "Обновить структуру";
            this.schemaUpdateMenuItem.Click += new System.EventHandler(this.schemaUpdateMenuItem_Click);
            // 
            // constraintUpdateMenuItem
            // 
            this.constraintUpdateMenuItem.Name = "constraintUpdateMenuItem";
            this.constraintUpdateMenuItem.Size = new System.Drawing.Size(191, 22);
            this.constraintUpdateMenuItem.Text = "Обновить триггеры";
            this.constraintUpdateMenuItem.Click += new System.EventHandler(this.triggersUpdateMenuItem_Click);
            // 
            // dataMenu
            // 
            this.dataMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkPatchesMenuItem,
            this.installPatchesMenuItem,
            this.initDataMenuItem,
            this.demoDataMenuItem,
            this.importDataMenuItem});
            this.dataMenu.Enabled = false;
            this.dataMenu.Name = "dataMenu";
            this.dataMenu.Size = new System.Drawing.Size(62, 20);
            this.dataMenu.Text = "Данные";
            // 
            // checkPatchesMenuItem
            // 
            this.checkPatchesMenuItem.Name = "checkPatchesMenuItem";
            this.checkPatchesMenuItem.Size = new System.Drawing.Size(229, 22);
            this.checkPatchesMenuItem.Text = "Проверка обновлений";
            this.checkPatchesMenuItem.Click += new System.EventHandler(this.checkPatchesMenuItem_Click);
            // 
            // installPatchesMenuItem
            // 
            this.installPatchesMenuItem.Name = "installPatchesMenuItem";
            this.installPatchesMenuItem.Size = new System.Drawing.Size(229, 22);
            this.installPatchesMenuItem.Text = "Установка обновлений";
            this.installPatchesMenuItem.Click += new System.EventHandler(this.installPatchesMenuItem_Click);
            // 
            // initDataMenuItem
            // 
            this.initDataMenuItem.Name = "initDataMenuItem";
            this.initDataMenuItem.Size = new System.Drawing.Size(229, 22);
            this.initDataMenuItem.Text = "Инициализировать данные";
            this.initDataMenuItem.Click += new System.EventHandler(this.initDataMenuItem_Click);
            // 
            // demoDataMenuItem
            // 
            this.demoDataMenuItem.Name = "demoDataMenuItem";
            this.demoDataMenuItem.Size = new System.Drawing.Size(229, 22);
            this.demoDataMenuItem.Text = "Демонстрационные данные";
            this.demoDataMenuItem.Click += new System.EventHandler(this.demoDataMenuItem_Click);
            // 
            // importDataMenuItem
            // 
            this.importDataMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.damaskImportMenuItem});
            this.importDataMenuItem.Name = "importDataMenuItem";
            this.importDataMenuItem.Size = new System.Drawing.Size(229, 22);
            this.importDataMenuItem.Text = "Импорт данных";
            // 
            // damaskImportMenuItem
            // 
            this.damaskImportMenuItem.Name = "damaskImportMenuItem";
            this.damaskImportMenuItem.Size = new System.Drawing.Size(187, 22);
            this.damaskImportMenuItem.Text = "Импорт из \"Дамаск\"";
            this.damaskImportMenuItem.Click += new System.EventHandler(this.damaskImportMenuItem_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(5, 30);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ReadOnly = true;
            this.logTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.logTextBox.Size = new System.Drawing.Size(620, 290);
            this.logTextBox.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 326);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.topMenu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MainMenuStrip = this.topMenu;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(280, 105);
            this.Name = "MainForm";
            this.Text = "База данных";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.ToolStripMenuItem schemaMenu;
        private System.Windows.Forms.ToolStripMenuItem schemaValidateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem schemaUpdateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataMenu;
        private System.Windows.Forms.ToolStripMenuItem initDataMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importDataMenuItem;
        private System.Windows.Forms.ToolStripMenuItem constraintUpdateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem demoDataMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkPatchesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem installPatchesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem damaskImportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectButton;
    }
}

