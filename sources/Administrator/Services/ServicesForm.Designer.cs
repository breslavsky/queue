namespace Queue.Administrator
{
    partial class ServicesForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServicesForm));
            this.buttonDown = new System.Windows.Forms.Button();
            this.servicesTreeView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addServiceGroupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addServiceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editServiceGroupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editServiceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteServiceGroupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteServiceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.movePanel = new System.Windows.Forms.Panel();
            this.buttonUp = new System.Windows.Forms.Button();
            this.contextMenuStrip.SuspendLayout();
            this.mainLayoutPanel.SuspendLayout();
            this.movePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonDown
            // 
            this.buttonDown.Image = ((System.Drawing.Image)(resources.GetObject("buttonDown.Image")));
            this.buttonDown.Location = new System.Drawing.Point(0, 30);
            this.buttonDown.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(30, 25);
            this.buttonDown.TabIndex = 0;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // servicesTreeView
            // 
            this.servicesTreeView.AllowDrop = true;
            this.servicesTreeView.CheckBoxes = true;
            this.servicesTreeView.ContextMenuStrip = this.contextMenuStrip;
            this.servicesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.servicesTreeView.HideSelection = false;
            this.servicesTreeView.Location = new System.Drawing.Point(0, 0);
            this.servicesTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.servicesTreeView.Name = "servicesTreeView";
            this.servicesTreeView.Size = new System.Drawing.Size(589, 442);
            this.servicesTreeView.TabIndex = 0;
            this.servicesTreeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.serviceGroupsTreeView_AfterCheck);
            this.servicesTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.servicesTreeView_AfterExpand);
            this.servicesTreeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.serviceGroupsTreeView_ItemDrag);
            this.servicesTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.serviceGroupsTreeView_NodeMouseClick);
            this.servicesTreeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.serviceGroupsTreeView_DragDrop);
            this.servicesTreeView.DragOver += new System.Windows.Forms.DragEventHandler(this.serviceGroupsTreeView_DragOver);
            this.servicesTreeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.serviceGroupsTreeView_MouseUp);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addServiceGroupMenuItem,
            this.addServiceMenuItem,
            this.editServiceGroupMenuItem,
            this.editServiceMenuItem,
            this.deleteServiceGroupMenuItem,
            this.deleteServiceMenuItem});
            this.contextMenuStrip.Name = "serviceCategoriesTreeMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(203, 136);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // addServiceGroupMenuItem
            // 
            this.addServiceGroupMenuItem.Name = "addServiceGroupMenuItem";
            this.addServiceGroupMenuItem.Size = new System.Drawing.Size(202, 22);
            this.addServiceGroupMenuItem.Text = "Добавить группу услуг";
            this.addServiceGroupMenuItem.Click += new System.EventHandler(this.addServiceGroupMenuItem_Click);
            // 
            // addServiceMenuItem
            // 
            this.addServiceMenuItem.Name = "addServiceMenuItem";
            this.addServiceMenuItem.Size = new System.Drawing.Size(202, 22);
            this.addServiceMenuItem.Text = "Добавить услугу";
            this.addServiceMenuItem.Click += new System.EventHandler(this.addServiceMenuItem_Click);
            // 
            // editServiceGroupMenuItem
            // 
            this.editServiceGroupMenuItem.Name = "editServiceGroupMenuItem";
            this.editServiceGroupMenuItem.Size = new System.Drawing.Size(202, 22);
            this.editServiceGroupMenuItem.Text = "Изменить группу услуг";
            this.editServiceGroupMenuItem.Click += new System.EventHandler(this.editServiceGroupMenuItem_Click);
            // 
            // editServiceMenuItem
            // 
            this.editServiceMenuItem.Name = "editServiceMenuItem";
            this.editServiceMenuItem.Size = new System.Drawing.Size(202, 22);
            this.editServiceMenuItem.Text = "Изменить услугу";
            this.editServiceMenuItem.Click += new System.EventHandler(this.editServiceMenuItem_Click);
            // 
            // deleteServiceGroupMenuItem
            // 
            this.deleteServiceGroupMenuItem.Name = "deleteServiceGroupMenuItem";
            this.deleteServiceGroupMenuItem.Size = new System.Drawing.Size(202, 22);
            this.deleteServiceGroupMenuItem.Text = "Удалить группу услуг";
            this.deleteServiceGroupMenuItem.Click += new System.EventHandler(this.deleteServiceGroupMenuItem_Click);
            // 
            // deleteServiceMenuItem
            // 
            this.deleteServiceMenuItem.Name = "deleteServiceMenuItem";
            this.deleteServiceMenuItem.Size = new System.Drawing.Size(202, 22);
            this.deleteServiceMenuItem.Text = "Удалить услугу";
            this.deleteServiceMenuItem.Click += new System.EventHandler(this.deleteServiceMenuItem_Click);
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 2;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.mainLayoutPanel.Controls.Add(this.servicesTreeView, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.movePanel, 1, 0);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(10, 10);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 1;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(624, 442);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // movePanel
            // 
            this.movePanel.Controls.Add(this.buttonUp);
            this.movePanel.Controls.Add(this.buttonDown);
            this.movePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.movePanel.Location = new System.Drawing.Point(592, 3);
            this.movePanel.Name = "movePanel";
            this.movePanel.Size = new System.Drawing.Size(29, 436);
            this.movePanel.TabIndex = 1;
            // 
            // buttonUp
            // 
            this.buttonUp.Image = ((System.Drawing.Image)(resources.GetObject("buttonUp.Image")));
            this.buttonUp.Location = new System.Drawing.Point(0, 0);
            this.buttonUp.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(30, 25);
            this.buttonUp.TabIndex = 1;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // ServicesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 462);
            this.Controls.Add(this.mainLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(660, 500);
            this.Name = "ServicesForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Настройка услуг";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServicesForm_FormClosing);
            this.Load += new System.EventHandler(this.ServicesForm_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.mainLayoutPanel.ResumeLayout(false);
            this.movePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.TreeView servicesTreeView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addServiceGroupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteServiceMenuItem;
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.ToolStripMenuItem addServiceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editServiceGroupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editServiceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteServiceGroupMenuItem;
        private System.Windows.Forms.Panel movePanel;
        private System.Windows.Forms.Button buttonUp;
    }
}