namespace Queue.Administrator
{
    partial class AdministratorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdministratorForm));
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.formsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dictionariesMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.usersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.workplacesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.defaultScheduleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.servicesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientRequestsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.clientRequestsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addClientRequestMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queuePlanMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.queueMonitorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.сurrentScheduleMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serviceRatingReportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operatorsRatingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scheduleReportMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.exceptionScheduleReportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.officesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logoutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            this.aboutMenuItem,
            this.formsMenuItem,
            this.configMenuItem,
            this.dictionariesMenu,
            this.clientsMenuItem,
            this.clientRequestsMenu,
            this.queuePlanMenu,
            this.reportsMenuItem,
            this.officesMenuItem,
            this.logoutMenuItem});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.MdiWindowListItem = this.formsMenuItem;
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(884, 24);
            this.topMenu.TabIndex = 0;
            // 
            // aboutMenuItem
            // 
            this.aboutMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("aboutMenuItem.Image")));
            this.aboutMenuItem.Name = "aboutMenuItem";
            this.aboutMenuItem.Size = new System.Drawing.Size(28, 20);
            this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
            // 
            // formsMenuItem
            // 
            this.formsMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("formsMenuItem.Image")));
            this.formsMenuItem.Name = "formsMenuItem";
            this.formsMenuItem.Size = new System.Drawing.Size(64, 20);
            this.formsMenuItem.Text = "Окно";
            // 
            // configMenuItem
            // 
            this.configMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("configMenuItem.Image")));
            this.configMenuItem.Name = "configMenuItem";
            this.configMenuItem.ShortcutKeyDisplayString = "";
            this.configMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.configMenuItem.Size = new System.Drawing.Size(95, 20);
            this.configMenuItem.Tag = "Config";
            this.configMenuItem.Text = "Настройки";
            this.configMenuItem.Click += new System.EventHandler(this.configMenuItem_Click);
            // 
            // dictionariesMenu
            // 
            this.dictionariesMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.usersMenuItem,
            this.workplacesMenuItem,
            this.defaultScheduleMenuItem,
            this.servicesMenuItem});
            this.dictionariesMenu.Image = ((System.Drawing.Image)(resources.GetObject("dictionariesMenu.Image")));
            this.dictionariesMenu.Name = "dictionariesMenu";
            this.dictionariesMenu.Size = new System.Drawing.Size(110, 20);
            this.dictionariesMenu.Text = "Справочники";
            // 
            // usersMenuItem
            // 
            this.usersMenuItem.Name = "usersMenuItem";
            this.usersMenuItem.Size = new System.Drawing.Size(181, 22);
            this.usersMenuItem.Tag = "Users";
            this.usersMenuItem.Text = "Пользователи";
            this.usersMenuItem.Click += new System.EventHandler(this.usersMenuItem_Click);
            // 
            // workplacesMenuItem
            // 
            this.workplacesMenuItem.Name = "workplacesMenuItem";
            this.workplacesMenuItem.Size = new System.Drawing.Size(181, 22);
            this.workplacesMenuItem.Tag = "Workplaces";
            this.workplacesMenuItem.Text = "Рабочие места";
            this.workplacesMenuItem.Click += new System.EventHandler(this.workplacesMenuItem_Click);
            // 
            // defaultScheduleMenuItem
            // 
            this.defaultScheduleMenuItem.Name = "defaultScheduleMenuItem";
            this.defaultScheduleMenuItem.Size = new System.Drawing.Size(181, 22);
            this.defaultScheduleMenuItem.Tag = "DefaultSchedule";
            this.defaultScheduleMenuItem.Text = "Общее расписание";
            this.defaultScheduleMenuItem.Click += new System.EventHandler(this.defaultScheduleMenuItem_Click);
            // 
            // servicesMenuItem
            // 
            this.servicesMenuItem.Name = "servicesMenuItem";
            this.servicesMenuItem.Size = new System.Drawing.Size(181, 22);
            this.servicesMenuItem.Tag = "Services";
            this.servicesMenuItem.Text = "Настройка услуг";
            this.servicesMenuItem.Click += new System.EventHandler(this.servicesMenuItem_Click);
            // 
            // clientsMenuItem
            // 
            this.clientsMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("clientsMenuItem.Image")));
            this.clientsMenuItem.Name = "clientsMenuItem";
            this.clientsMenuItem.Size = new System.Drawing.Size(83, 20);
            this.clientsMenuItem.Tag = "Clients";
            this.clientsMenuItem.Text = "Клиенты";
            this.clientsMenuItem.Click += new System.EventHandler(this.clientsMenuItem_Click);
            // 
            // clientRequestsMenu
            // 
            this.clientRequestsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientRequestsMenuItem,
            this.addClientRequestMenuItem});
            this.clientRequestsMenu.Image = ((System.Drawing.Image)(resources.GetObject("clientRequestsMenu.Image")));
            this.clientRequestsMenu.Name = "clientRequestsMenu";
            this.clientRequestsMenu.Size = new System.Drawing.Size(84, 20);
            this.clientRequestsMenu.Tag = "ClientsRequests";
            this.clientRequestsMenu.Text = "Запросы";
            // 
            // clientRequestsMenuItem
            // 
            this.clientRequestsMenuItem.Name = "clientRequestsMenuItem";
            this.clientRequestsMenuItem.Size = new System.Drawing.Size(169, 22);
            this.clientRequestsMenuItem.Text = "Список запросов";
            this.clientRequestsMenuItem.Click += new System.EventHandler(this.clientRequestsMenuItem_Click);
            // 
            // addClientRequestMenuItem
            // 
            this.addClientRequestMenuItem.Name = "addClientRequestMenuItem";
            this.addClientRequestMenuItem.Size = new System.Drawing.Size(169, 22);
            this.addClientRequestMenuItem.Text = "Добавить запрос";
            this.addClientRequestMenuItem.Click += new System.EventHandler(this.addClientRequestMenuItem_Click);
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
            this.queueMonitorMenuItem.Tag = "QueuePlan";
            this.queueMonitorMenuItem.Text = "Монитор очереди";
            this.queueMonitorMenuItem.Click += new System.EventHandler(this.queueMonitorMenuItem_Click);
            // 
            // сurrentScheduleMenuItem
            // 
            this.сurrentScheduleMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("сurrentScheduleMenuItem.Image")));
            this.сurrentScheduleMenuItem.Name = "сurrentScheduleMenuItem";
            this.сurrentScheduleMenuItem.Size = new System.Drawing.Size(190, 22);
            this.сurrentScheduleMenuItem.Tag = "CurrentSchedule";
            this.сurrentScheduleMenuItem.Text = "Текущее расписание";
            this.сurrentScheduleMenuItem.Click += new System.EventHandler(this.сurrentScheduleMenuItem_Click);
            // 
            // reportsMenuItem
            // 
            this.reportsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serviceRatingReportMenuItem,
            this.operatorsRatingToolStripMenuItem,
            this.scheduleReportMenu});
            this.reportsMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("reportsMenuItem.Image")));
            this.reportsMenuItem.Name = "reportsMenuItem";
            this.reportsMenuItem.Size = new System.Drawing.Size(76, 20);
            this.reportsMenuItem.Tag = "Reports";
            this.reportsMenuItem.Text = "Отчеты";
            // 
            // serviceRatingReportMenuItem
            // 
            this.serviceRatingReportMenuItem.Name = "serviceRatingReportMenuItem";
            this.serviceRatingReportMenuItem.Size = new System.Drawing.Size(186, 22);
            this.serviceRatingReportMenuItem.Text = "Рейтинг услуг";
            this.serviceRatingReportMenuItem.Click += new System.EventHandler(this.serviceRatingReportMenuItem_Click);
            // 
            // operatorsRatingToolStripMenuItem
            // 
            this.operatorsRatingToolStripMenuItem.Name = "operatorsRatingToolStripMenuItem";
            this.operatorsRatingToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.operatorsRatingToolStripMenuItem.Text = "Рейтинг операторов";
            this.operatorsRatingToolStripMenuItem.Click += new System.EventHandler(this.operatorsRatingToolStripMenuItem_Click);
            // 
            // scheduleReportMenu
            // 
            this.scheduleReportMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exceptionScheduleReportMenuItem});
            this.scheduleReportMenu.Name = "scheduleReportMenu";
            this.scheduleReportMenu.Size = new System.Drawing.Size(186, 22);
            this.scheduleReportMenu.Text = "Расписание услуг";
            // 
            // exceptionScheduleReportMenuItem
            // 
            this.exceptionScheduleReportMenuItem.Name = "exceptionScheduleReportMenuItem";
            this.exceptionScheduleReportMenuItem.Size = new System.Drawing.Size(223, 22);
            this.exceptionScheduleReportMenuItem.Text = "Исключения в расписании";
            this.exceptionScheduleReportMenuItem.Click += new System.EventHandler(this.exceptionScheduleReportMenuItem_Click);
            // 
            // officesMenuItem
            // 
            this.officesMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("officesMenuItem.Image")));
            this.officesMenuItem.Name = "officesMenuItem";
            this.officesMenuItem.Size = new System.Drawing.Size(87, 20);
            this.officesMenuItem.Tag = "Offices";
            this.officesMenuItem.Text = "Филиалы";
            this.officesMenuItem.Click += new System.EventHandler(this.officesMenuItem_Click);
            // 
            // logoutMenuItem
            // 
            this.logoutMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("logoutMenuItem.Image")));
            this.logoutMenuItem.Name = "logoutMenuItem";
            this.logoutMenuItem.Size = new System.Drawing.Size(69, 20);
            this.logoutMenuItem.Text = "Выход";
            this.logoutMenuItem.Click += new System.EventHandler(this.logoutMenuItem_Click);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverStateLabel,
            this.currentDateTimeLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 390);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(884, 22);
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
            // AdministratorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 412);
            this.Controls.Add(this.topMenu);
            this.Controls.Add(this.statusBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MinimumSize = new System.Drawing.Size(900, 450);
            this.Name = "AdministratorForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
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
        private System.Windows.Forms.ToolStripMenuItem configMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dictionariesMenu;
        private System.Windows.Forms.ToolStripMenuItem usersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem workplacesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultScheduleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem servicesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientRequestsMenu;
        private System.Windows.Forms.ToolStripMenuItem addClientRequestMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientRequestsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queuePlanMenu;
        private System.Windows.Forms.ToolStripMenuItem reportsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem serviceRatingReportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem officesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logoutMenuItem;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel serverStateLabel;
        private System.Windows.Forms.ToolStripStatusLabel currentDateTimeLabel;
        private System.Windows.Forms.ToolStripMenuItem scheduleReportMenu;
        private System.Windows.Forms.ToolStripMenuItem exceptionScheduleReportMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queueMonitorMenuItem;
        private System.Windows.Forms.ToolStripMenuItem сurrentScheduleMenuItem;
        private System.Windows.Forms.ToolStripMenuItem formsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operatorsRatingToolStripMenuItem;

    }
}