namespace Queue.Administrator
{
    partial class LifeSituationsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LifeSituationsForm));
            this.buttonDown = new System.Windows.Forms.Button();
            this.treeView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addGroupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addLifeSituationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editGroupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editLifeSituationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteGroupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLifeSituationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
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
            // treeView
            // 
            this.treeView.AllowDrop = true;
            this.treeView.CheckBoxes = true;
            this.treeView.ContextMenuStrip = this.contextMenuStrip;
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.HideSelection = false;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Margin = new System.Windows.Forms.Padding(0);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(589, 442);
            this.treeView.TabIndex = 0;
            this.treeView.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterCheck);
            this.treeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterExpand);
            this.treeView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.treeView_ItemDrag);
            this.treeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.treeView.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView_DragDrop);
            this.treeView.DragOver += new System.Windows.Forms.DragEventHandler(this.treeView_DragOver);
            this.treeView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.treeView_MouseUp);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addGroupMenuItem,
            this.addLifeSituationMenuItem,
            this.editGroupMenuItem,
            this.editLifeSituationMenuItem,
            this.deleteGroupMenuItem,
            this.deleteLifeSituationMenuItem});
            this.contextMenuStrip.Name = "serviceCategoriesTreeMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(253, 158);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
            // 
            // addGroupMenuItem
            // 
            this.addGroupMenuItem.Name = "addGroupMenuItem";
            this.addGroupMenuItem.Size = new System.Drawing.Size(252, 22);
            this.addGroupMenuItem.Text = "Добавить группу";
            this.addGroupMenuItem.Click += new System.EventHandler(this.addGroupMenuItem_Click);
            // 
            // addLifeSituationMenuItem
            // 
            this.addLifeSituationMenuItem.Name = "addLifeSituationMenuItem";
            this.addLifeSituationMenuItem.Size = new System.Drawing.Size(252, 22);
            this.addLifeSituationMenuItem.Text = "Добавить жизненную ситуацию";
            this.addLifeSituationMenuItem.Click += new System.EventHandler(this.addLifeSituationMenuItem_Click);
            // 
            // editGroupMenuItem
            // 
            this.editGroupMenuItem.Name = "editGroupMenuItem";
            this.editGroupMenuItem.Size = new System.Drawing.Size(252, 22);
            this.editGroupMenuItem.Text = "Изменить группу";
            this.editGroupMenuItem.Click += new System.EventHandler(this.editGroupMenuItem_Click);
            // 
            // editLifeSituationMenuItem
            // 
            this.editLifeSituationMenuItem.Name = "editLifeSituationMenuItem";
            this.editLifeSituationMenuItem.Size = new System.Drawing.Size(252, 22);
            this.editLifeSituationMenuItem.Text = "Изменить жизненную ситуацию";
            this.editLifeSituationMenuItem.Click += new System.EventHandler(this.editLifeSituationMenuItem_Click);
            // 
            // deleteGroupMenuItem
            // 
            this.deleteGroupMenuItem.Name = "deleteGroupMenuItem";
            this.deleteGroupMenuItem.Size = new System.Drawing.Size(252, 22);
            this.deleteGroupMenuItem.Text = "Удалить группу";
            this.deleteGroupMenuItem.Click += new System.EventHandler(this.deleteGroupMenuItem_Click);
            // 
            // deleteLifeSituationMenuItem
            // 
            this.deleteLifeSituationMenuItem.Name = "deleteLifeSituationMenuItem";
            this.deleteLifeSituationMenuItem.Size = new System.Drawing.Size(252, 22);
            this.deleteLifeSituationMenuItem.Text = "Удалить жизненную ситуацию";
            this.deleteLifeSituationMenuItem.Click += new System.EventHandler(this.deleteLifeSituationMenuItem_Click);
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 2;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.mainLayoutPanel.Controls.Add(this.treeView, 0, 0);
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
            // LifeSituationsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 462);
            this.Controls.Add(this.mainLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(660, 500);
            this.Name = "LifeSituationsForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Настройка жизненных ситуаций";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LifeSituationsForm_FormClosing);
            this.Load += new System.EventHandler(this.LifeSituationsForm_Load);
            this.contextMenuStrip.ResumeLayout(false);
            this.mainLayoutPanel.ResumeLayout(false);
            this.movePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem addGroupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteLifeSituationMenuItem;
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private System.Windows.Forms.ToolStripMenuItem addLifeSituationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editGroupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editLifeSituationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteGroupMenuItem;
        private System.Windows.Forms.Panel movePanel;
        private System.Windows.Forms.Button buttonUp;
    }
}