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
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.SchemaMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.schemaValidateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.schemaUpdateMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.импортToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testDataMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обновитьСсылкиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.topMenu.Enabled = false;
            this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SchemaMenu,
            this.dataMenu});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(628, 24);
            this.topMenu.TabIndex = 0;
            this.topMenu.Text = "menuStrip1";
            // 
            // logTextBox
            // 
            this.logTextBox.Location = new System.Drawing.Point(5, 60);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(615, 105);
            this.logTextBox.TabIndex = 1;
            // 
            // SchemaMenu
            // 
            this.SchemaMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.schemaValidateMenuItem,
            this.schemaUpdateMenuItem,
            this.обновитьСсылкиToolStripMenuItem});
            this.SchemaMenu.Name = "SchemaMenu";
            this.SchemaMenu.Size = new System.Drawing.Size(75, 20);
            this.SchemaMenu.Text = "Структура";
            // 
            // schemaValidateMenuItem
            // 
            this.schemaValidateMenuItem.Name = "schemaValidateMenuItem";
            this.schemaValidateMenuItem.Size = new System.Drawing.Size(152, 22);
            this.schemaValidateMenuItem.Text = "Проверить";
            this.schemaValidateMenuItem.Click += new System.EventHandler(this.schemaValidateMenu_Click);
            // 
            // schemaUpdateMenuItem
            // 
            this.schemaUpdateMenuItem.Name = "schemaUpdateMenuItem";
            this.schemaUpdateMenuItem.Size = new System.Drawing.Size(152, 22);
            this.schemaUpdateMenuItem.Text = "Обновить";
            this.schemaUpdateMenuItem.Click += new System.EventHandler(this.schemaUpdateMenuItem_Click);
            // 
            // dataMenu
            // 
            this.dataMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testDataMenuItem,
            this.импортToolStripMenuItem});
            this.dataMenu.Name = "dataMenu";
            this.dataMenu.Size = new System.Drawing.Size(62, 20);
            this.dataMenu.Text = "Данные";
            // 
            // импортToolStripMenuItem
            // 
            this.импортToolStripMenuItem.Name = "импортToolStripMenuItem";
            this.импортToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
            this.импортToolStripMenuItem.Text = "Импорт";
            // 
            // testDataMenuItem
            // 
            this.testDataMenuItem.Name = "testDataMenuItem";
            this.testDataMenuItem.Size = new System.Drawing.Size(170, 22);
            this.testDataMenuItem.Text = "Тестовые данные";
            // 
            // обновитьСсылкиToolStripMenuItem
            // 
            this.обновитьСсылкиToolStripMenuItem.Name = "обновитьСсылкиToolStripMenuItem";
            this.обновитьСсылкиToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.обновитьСсылкиToolStripMenuItem.Text = "Обновить связи";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 173);
            this.Controls.Add(this.logTextBox);
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
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.ToolStripMenuItem SchemaMenu;
        private System.Windows.Forms.ToolStripMenuItem schemaValidateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem schemaUpdateMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataMenu;
        private System.Windows.Forms.ToolStripMenuItem testDataMenuItem;
        private System.Windows.Forms.ToolStripMenuItem импортToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обновитьСсылкиToolStripMenuItem;
    }
}

