namespace Queue.Administrator
{
    partial class ServiceStepsControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceStepsControl));
            this.stepsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.stepsGridView = new System.Windows.Forms.DataGridView();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addStepButton = new System.Windows.Forms.Button();
            this.movePanel = new System.Windows.Forms.Panel();
            this.serviceStepUpButton = new System.Windows.Forms.Button();
            this.serviceStepDownButton = new System.Windows.Forms.Button();
            this.stepsTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stepsGridView)).BeginInit();
            this.movePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // stepsTableLayoutPanel
            // 
            this.stepsTableLayoutPanel.ColumnCount = 2;
            this.stepsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.stepsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.stepsTableLayoutPanel.Controls.Add(this.stepsGridView, 0, 0);
            this.stepsTableLayoutPanel.Controls.Add(this.addStepButton, 0, 1);
            this.stepsTableLayoutPanel.Controls.Add(this.movePanel, 1, 0);
            this.stepsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stepsTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.stepsTableLayoutPanel.Name = "stepsTableLayoutPanel";
            this.stepsTableLayoutPanel.RowCount = 2;
            this.stepsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.stepsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.stepsTableLayoutPanel.Size = new System.Drawing.Size(600, 400);
            this.stepsTableLayoutPanel.TabIndex = 2;
            // 
            // stepsGridView
            // 
            this.stepsGridView.AllowUserToAddRows = false;
            this.stepsGridView.AllowUserToResizeColumns = false;
            this.stepsGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.stepsGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.stepsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.stepsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn});
            this.stepsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stepsGridView.Location = new System.Drawing.Point(0, 0);
            this.stepsGridView.Margin = new System.Windows.Forms.Padding(0);
            this.stepsGridView.MultiSelect = false;
            this.stepsGridView.Name = "stepsGridView";
            this.stepsGridView.ReadOnly = true;
            this.stepsGridView.RowHeadersVisible = false;
            this.stepsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.stepsGridView.Size = new System.Drawing.Size(565, 370);
            this.stepsGridView.TabIndex = 0;
            this.stepsGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.stepsGridView_CellMouseDoubleClick);
            this.stepsGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.stepsGridView_UserDeletingRow);
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameColumn.FillWeight = 80F;
            this.nameColumn.HeaderText = "Название";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            this.nameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // addStepButton
            // 
            this.addStepButton.Location = new System.Drawing.Point(0, 375);
            this.addStepButton.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.addStepButton.Name = "addStepButton";
            this.addStepButton.Size = new System.Drawing.Size(75, 25);
            this.addStepButton.TabIndex = 0;
            this.addStepButton.Text = "Добавить";
            this.addStepButton.Click += new System.EventHandler(this.addStepButton_Click);
            // 
            // movePanel
            // 
            this.movePanel.Controls.Add(this.serviceStepUpButton);
            this.movePanel.Controls.Add(this.serviceStepDownButton);
            this.movePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.movePanel.Location = new System.Drawing.Point(568, 3);
            this.movePanel.Name = "movePanel";
            this.movePanel.Size = new System.Drawing.Size(29, 364);
            this.movePanel.TabIndex = 1;
            // 
            // serviceStepUpButton
            // 
            this.serviceStepUpButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceStepUpButton.Image = ((System.Drawing.Image)(resources.GetObject("serviceStepUpButton.Image")));
            this.serviceStepUpButton.Location = new System.Drawing.Point(0, 0);
            this.serviceStepUpButton.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.serviceStepUpButton.Name = "serviceStepUpButton";
            this.serviceStepUpButton.Size = new System.Drawing.Size(30, 25);
            this.serviceStepUpButton.TabIndex = 3;
            this.serviceStepUpButton.Click += new System.EventHandler(this.serviceStepUpButton_Click);
            // 
            // serviceStepDownButton
            // 
            this.serviceStepDownButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceStepDownButton.Image = ((System.Drawing.Image)(resources.GetObject("serviceStepDownButton.Image")));
            this.serviceStepDownButton.Location = new System.Drawing.Point(0, 30);
            this.serviceStepDownButton.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.serviceStepDownButton.Name = "serviceStepDownButton";
            this.serviceStepDownButton.Size = new System.Drawing.Size(30, 25);
            this.serviceStepDownButton.TabIndex = 4;
            this.serviceStepDownButton.Click += new System.EventHandler(this.serviceStepDownButton_Click);
            // 
            // ServiceStepsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stepsTableLayoutPanel);
            this.Name = "ServiceStepsControl";
            this.Size = new System.Drawing.Size(600, 400);
            this.stepsTableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stepsGridView)).EndInit();
            this.movePanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel stepsTableLayoutPanel;
        private System.Windows.Forms.DataGridView stepsGridView;
        private System.Windows.Forms.Button addStepButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.Panel movePanel;
        private System.Windows.Forms.Button serviceStepUpButton;
        private System.Windows.Forms.Button serviceStepDownButton;
    }
}
