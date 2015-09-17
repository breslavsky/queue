namespace Queue.Administrator
{
    partial class QueueMonitorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueueMonitorForm));
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.searchGroupBox = new System.Windows.Forms.GroupBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.numberUpDown = new System.Windows.Forms.NumericUpDown();
            this.numberLabel = new System.Windows.Forms.Label();
            this.reloadSecondLabel = new System.Windows.Forms.Label();
            this.reloadIntervalUpDown = new System.Windows.Forms.NumericUpDown();
            this.reloadCheckBox = new System.Windows.Forms.CheckBox();
            this.operatorOnlneCheckBox = new System.Windows.Forms.CheckBox();
            this.planDateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.loadButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.elementHost = new System.Windows.Forms.Integration.ElementHost();
            this.queueMonitorControl = new Queue.Administrator.QueueMonitorControl();
            this.mainTableLayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.searchGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numberUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.reloadIntervalUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 830F));
            this.mainTableLayoutPanel.Controls.Add(this.panel1, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.elementHost, 0, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(10, 10);
            this.mainTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(830, 580);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.searchGroupBox);
            this.panel1.Controls.Add(this.reloadSecondLabel);
            this.panel1.Controls.Add(this.reloadIntervalUpDown);
            this.panel1.Controls.Add(this.reloadCheckBox);
            this.panel1.Controls.Add(this.operatorOnlneCheckBox);
            this.panel1.Controls.Add(this.planDateTimePicker);
            this.panel1.Controls.Add(this.loadButton);
            this.panel1.Controls.Add(this.refreshButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(830, 65);
            this.panel1.TabIndex = 0;
            // 
            // searchGroupBox
            // 
            this.searchGroupBox.Controls.Add(this.searchButton);
            this.searchGroupBox.Controls.Add(this.numberUpDown);
            this.searchGroupBox.Controls.Add(this.numberLabel);
            this.searchGroupBox.Location = new System.Drawing.Point(325, 5);
            this.searchGroupBox.Name = "searchGroupBox";
            this.searchGroupBox.Size = new System.Drawing.Size(200, 55);
            this.searchGroupBox.TabIndex = 9;
            this.searchGroupBox.TabStop = false;
            this.searchGroupBox.Text = "Поиск";
            // 
            // searchButton
            // 
            this.searchButton.Location = new System.Drawing.Point(120, 20);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 25);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "Найти";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.searchButton_Click);
            // 
            // numberUpDown
            // 
            this.numberUpDown.Location = new System.Drawing.Point(50, 21);
            this.numberUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numberUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numberUpDown.Name = "numberUpDown";
            this.numberUpDown.Size = new System.Drawing.Size(65, 20);
            this.numberUpDown.TabIndex = 2;
            this.numberUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numberLabel
            // 
            this.numberLabel.Location = new System.Drawing.Point(5, 25);
            this.numberLabel.Name = "numberLabel";
            this.numberLabel.Size = new System.Drawing.Size(44, 13);
            this.numberLabel.TabIndex = 0;
            this.numberLabel.Text = "Номер";
            this.numberLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // reloadSecondLabel
            // 
            this.reloadSecondLabel.AutoSize = true;
            this.reloadSecondLabel.Location = new System.Drawing.Point(285, 45);
            this.reloadSecondLabel.Name = "reloadSecondLabel";
            this.reloadSecondLabel.Size = new System.Drawing.Size(28, 13);
            this.reloadSecondLabel.TabIndex = 8;
            this.reloadSecondLabel.Text = "сек.";
            // 
            // reloadIntervalUpDown
            // 
            this.reloadIntervalUpDown.Location = new System.Drawing.Point(240, 40);
            this.reloadIntervalUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.reloadIntervalUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.reloadIntervalUpDown.Name = "reloadIntervalUpDown";
            this.reloadIntervalUpDown.Size = new System.Drawing.Size(40, 20);
            this.reloadIntervalUpDown.TabIndex = 7;
            this.reloadIntervalUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // reloadCheckBox
            // 
            this.reloadCheckBox.AutoSize = true;
            this.reloadCheckBox.Location = new System.Drawing.Point(120, 40);
            this.reloadCheckBox.Name = "reloadCheckBox";
            this.reloadCheckBox.Size = new System.Drawing.Size(116, 17);
            this.reloadCheckBox.TabIndex = 6;
            this.reloadCheckBox.Text = "Автоообновление";
            this.reloadCheckBox.UseVisualStyleBackColor = true;
            this.reloadCheckBox.CheckedChanged += new System.EventHandler(this.reloadCheckBox_CheckedChanged);
            // 
            // operatorOnlneCheckBox
            // 
            this.operatorOnlneCheckBox.AutoSize = true;
            this.operatorOnlneCheckBox.Location = new System.Drawing.Point(10, 40);
            this.operatorOnlneCheckBox.Name = "operatorOnlneCheckBox";
            this.operatorOnlneCheckBox.Size = new System.Drawing.Size(102, 17);
            this.operatorOnlneCheckBox.TabIndex = 5;
            this.operatorOnlneCheckBox.Text = "Только онлайн";
            this.operatorOnlneCheckBox.UseVisualStyleBackColor = true;
            this.operatorOnlneCheckBox.Leave += new System.EventHandler(this.operatorOnlneCheckBox_Leave);
            // 
            // planDateTimePicker
            // 
            this.planDateTimePicker.Location = new System.Drawing.Point(5, 5);
            this.planDateTimePicker.Name = "planDateTimePicker";
            this.planDateTimePicker.Size = new System.Drawing.Size(140, 20);
            this.planDateTimePicker.TabIndex = 0;
            this.planDateTimePicker.Value = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            this.planDateTimePicker.Leave += new System.EventHandler(this.planDateTimePicker_Leave);
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(150, 5);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(165, 25);
            this.loadButton.TabIndex = 1;
            this.loadButton.Text = "Загрузить";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshButton.Image = ((System.Drawing.Image)(resources.GetObject("refreshButton.Image")));
            this.refreshButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.refreshButton.Location = new System.Drawing.Point(705, 5);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(120, 25);
            this.refreshButton.TabIndex = 4;
            this.refreshButton.Text = "Перезагрузить";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // elementHost
            // 
            this.elementHost.AccessibleRole = System.Windows.Forms.AccessibleRole.Window;
            this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost.Location = new System.Drawing.Point(0, 65);
            this.elementHost.Margin = new System.Windows.Forms.Padding(0);
            this.elementHost.Name = "elementHost";
            this.elementHost.Size = new System.Drawing.Size(830, 515);
            this.elementHost.TabIndex = 5;
            this.elementHost.Text = "elementHost1";
            this.elementHost.Child = this.queueMonitorControl;
            // 
            // QueueMonitorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(850, 600);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Name = "QueueMonitorForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Монитор очереди";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.QueueMonitorForm_FormClosing);
            this.Load += new System.EventHandler(this.QueueMonitorForm_Load);
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.searchGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numberUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.reloadIntervalUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Integration.ElementHost elementHost;
        private Queue.Administrator.QueueMonitorControl queueMonitorControl;
        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker planDateTimePicker;
        private System.Windows.Forms.NumericUpDown numberUpDown;
        private System.Windows.Forms.Label numberLabel;
        private System.Windows.Forms.Button searchButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.CheckBox operatorOnlneCheckBox;
        private System.Windows.Forms.Label reloadSecondLabel;
        private System.Windows.Forms.NumericUpDown reloadIntervalUpDown;
        private System.Windows.Forms.CheckBox reloadCheckBox;
        private System.Windows.Forms.GroupBox searchGroupBox;
    }
}