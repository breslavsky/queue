namespace Queue.Simulator
{
    partial class SimulatorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimulatorForm));
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.clientRequestsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.opeatorsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.topMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // topMenu
            // 
            this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientRequestsMenuItem,
            this.opeatorsMenuItem,
            this.logoutButton});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(684, 24);
            this.topMenu.TabIndex = 0;
            this.topMenu.Text = "menuStrip1";
            // 
            // clientRequestsMenuItem
            // 
            this.clientRequestsMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("clientRequestsMenuItem.Image")));
            this.clientRequestsMenuItem.Name = "clientRequestsMenuItem";
            this.clientRequestsMenuItem.Size = new System.Drawing.Size(151, 20);
            this.clientRequestsMenuItem.Text = "Симулятор запросов";
            this.clientRequestsMenuItem.Click += new System.EventHandler(this.clientRequestsMenuItem_Click);
            // 
            // opeatorsMenuItem
            // 
            this.opeatorsMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("opeatorsMenuItem.Image")));
            this.opeatorsMenuItem.Name = "opeatorsMenuItem";
            this.opeatorsMenuItem.Size = new System.Drawing.Size(165, 20);
            this.opeatorsMenuItem.Text = "Симулятор операторов";
            this.opeatorsMenuItem.Click += new System.EventHandler(this.opeatorsMenuItem_Click);
            // 
            // logoutButton
            // 
            this.logoutButton.Image = ((System.Drawing.Image)(resources.GetObject("logoutButton.Image")));
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(69, 20);
            this.logoutButton.Text = "Выход";
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 362);
            this.Controls.Add(this.topMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.topMenu;
            this.Name = "MainForm";
            this.Text = "Симулятор";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.ToolStripMenuItem clientRequestsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutButton;
        private System.Windows.Forms.ToolStripMenuItem opeatorsMenuItem;
    }
}

