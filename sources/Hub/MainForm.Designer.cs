namespace Queue.Hub
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
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.serverStateLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.currentDateTimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.ledsTISeriesTabPage = new System.Windows.Forms.TabPage();
            this.ledsTISeriesControl1 = new Queue.Hub.LedsTISeriesControl();
            this.statusBar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.ledsTISeriesTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverStateLabel,
            this.currentDateTimeLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 120);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(384, 22);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(384, 120);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.ledsTISeriesTabPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(10, 10);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.Padding = new System.Drawing.Point(5, 5);
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(364, 100);
            this.tabControl1.TabIndex = 0;
            // 
            // ledsTISeriesTabPage
            // 
            this.ledsTISeriesTabPage.Controls.Add(this.ledsTISeriesControl1);
            this.ledsTISeriesTabPage.Location = new System.Drawing.Point(4, 26);
            this.ledsTISeriesTabPage.Name = "ledsTISeriesTabPage";
            this.ledsTISeriesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ledsTISeriesTabPage.Size = new System.Drawing.Size(356, 70);
            this.ledsTISeriesTabPage.TabIndex = 0;
            this.ledsTISeriesTabPage.Text = "Табло «Световод» серии «Т»";
            this.ledsTISeriesTabPage.UseVisualStyleBackColor = true;
            // 
            // ledsTISeriesControl1
            // 
            this.ledsTISeriesControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ledsTISeriesControl1.Location = new System.Drawing.Point(3, 3);
            this.ledsTISeriesControl1.Name = "ledsTISeriesControl1";
            this.ledsTISeriesControl1.Size = new System.Drawing.Size(350, 64);
            this.ledsTISeriesControl1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 142);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 180);
            this.Name = "MainForm";
            this.Text = "Шина";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ledsTISeriesTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel serverStateLabel;
        private System.Windows.Forms.ToolStripStatusLabel currentDateTimeLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage ledsTISeriesTabPage;
        private LedsTISeriesControl ledsTISeriesControl1;
    }
}

