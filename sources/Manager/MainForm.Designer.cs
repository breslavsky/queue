namespace Queue.Manager
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.formsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientRequestMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.addClientRequestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientRequestsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queuePlanMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.queueMonitorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сurrentScheduleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutButton = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.serverStateLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.currentDateTimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.topMenu.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // topMenu
            // 
            this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.formsMenuItem,
            this.clientsMenuItem,
            this.clientRequestMenu,
            this.queuePlanMenu,
            this.logoutButton});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.MdiWindowListItem = this.formsMenuItem;
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(700, 24);
            this.topMenu.TabIndex = 0;
            this.topMenu.Text = "menuStrip1";
            // 
            // formsMenuItem
            // 
            this.formsMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("formsMenuItem.Image")));
            this.formsMenuItem.Name = "formsMenuItem";
            this.formsMenuItem.Size = new System.Drawing.Size(63, 20);
            this.formsMenuItem.Text = "Окна";
            // 
            // clientsMenuItem
            // 
            this.clientsMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("clientsMenuItem.Image")));
            this.clientsMenuItem.Name = "clientsMenuItem";
            this.clientsMenuItem.Size = new System.Drawing.Size(83, 20);
            this.clientsMenuItem.Text = "Клиенты";
            this.clientsMenuItem.Click += new System.EventHandler(this.clientsMenuItem_Click);
            // 
            // clientRequestMenu
            // 
            this.clientRequestMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addClientRequestMenuItem,
            this.clientRequestsMenuItem});
            this.clientRequestMenu.Image = ((System.Drawing.Image)(resources.GetObject("clientRequestMenu.Image")));
            this.clientRequestMenu.Name = "clientRequestMenu";
            this.clientRequestMenu.Size = new System.Drawing.Size(84, 20);
            this.clientRequestMenu.Text = "Запросы";
            // 
            // addClientRequestMenuItem
            // 
            this.addClientRequestMenuItem.Name = "addClientRequestMenuItem";
            this.addClientRequestMenuItem.Size = new System.Drawing.Size(169, 22);
            this.addClientRequestMenuItem.Text = "Добавить запрос";
            this.addClientRequestMenuItem.Click += new System.EventHandler(this.addClientRequestMenuItem_Click);
            // 
            // clientRequestsMenuItem
            // 
            this.clientRequestsMenuItem.Name = "clientRequestsMenuItem";
            this.clientRequestsMenuItem.Size = new System.Drawing.Size(169, 22);
            this.clientRequestsMenuItem.Text = "Список запросов";
            this.clientRequestsMenuItem.Click += new System.EventHandler(this.clientRequestsMenuItem_Click);
            // 
            // queuePlanMenu
            // 
            this.queuePlanMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.queueMonitorMenuItem,
            this.сurrentScheduleMenuItem});
            this.queuePlanMenu.Image = ((System.Drawing.Image)(resources.GetObject("queuePlanMenu.Image")));
            this.queuePlanMenu.Name = "queuePlanMenu";
            this.queuePlanMenu.Size = new System.Drawing.Size(113, 20);
            this.queuePlanMenu.Text = "План очереди";
            // 
            // queueMonitorMenuItem
            // 
            this.queueMonitorMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("queueMonitorMenuItem.Image")));
            this.queueMonitorMenuItem.Name = "queueMonitorMenuItem";
            this.queueMonitorMenuItem.Size = new System.Drawing.Size(190, 22);
            this.queueMonitorMenuItem.Text = "Монитор очереди";
            this.queueMonitorMenuItem.Click += new System.EventHandler(this.queueMonitorMenuItem_Click);
            // 
            // сurrentScheduleMenuItem
            // 
            this.сurrentScheduleMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("сurrentScheduleMenuItem.Image")));
            this.сurrentScheduleMenuItem.Name = "сurrentScheduleMenuItem";
            this.сurrentScheduleMenuItem.Size = new System.Drawing.Size(190, 22);
            this.сurrentScheduleMenuItem.Text = "Текущее расписание";
            this.сurrentScheduleMenuItem.Click += new System.EventHandler(this.сurrentScheduleMenuItem_Click);
            // 
            // logoutButton
            // 
            this.logoutButton.Image = ((System.Drawing.Image)(resources.GetObject("logoutButton.Image")));
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(69, 20);
            this.logoutButton.Text = "Выход";
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverStateLabel,
            this.currentDateTimeLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 328);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(700, 22);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 0;
            this.statusBar.Text = "statusBar";
            // 
            // serverStateLabel
            // 
            this.serverStateLabel.Name = "serverStateLabel";
            this.serverStateLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // currentDateTimeLabel
            // 
            this.currentDateTimeLabel.Name = "currentDateTimeLabel";
            this.currentDateTimeLabel.Size = new System.Drawing.Size(12, 17);
            this.currentDateTimeLabel.Text = "-";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 350);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.topMenu);
            this.IsMdiContainer = true;
            this.Name = "MainForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.ToolStripMenuItem clientRequestMenu;
        private System.Windows.Forms.ToolStripMenuItem addClientRequestMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientRequestsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutButton;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel serverStateLabel;
        private System.Windows.Forms.ToolStripStatusLabel currentDateTimeLabel;
        private System.Windows.Forms.ToolStripMenuItem queuePlanMenu;
        private System.Windows.Forms.ToolStripMenuItem queueMonitorMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сurrentScheduleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formsMenuItem;
    }
}