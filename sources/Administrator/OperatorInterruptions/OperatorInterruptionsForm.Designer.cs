namespace Queue.Administrator
{
    partial class OperatorInterruptionsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.operatorInterruptionsGridView = new System.Windows.Forms.DataGridView();
            this.operatorInterruptionsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.addAdditionalServiceButton = new System.Windows.Forms.Button();
            this.operatorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TargetDate = new Queue.UI.WinForms.DataGridViewTranslatableColumn();
            this.dayOfWeekColumn = new Queue.UI.WinForms.DataGridViewTranslatableColumn();
            this.startTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.finishTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mainTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.operatorInterruptionsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.operatorInterruptionsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.operatorInterruptionsGridView, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.addAdditionalServiceButton, 0, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(10, 10);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(764, 292);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // operatorInterruptionsGridView
            // 
            this.operatorInterruptionsGridView.AllowUserToAddRows = false;
            this.operatorInterruptionsGridView.AllowUserToResizeColumns = false;
            this.operatorInterruptionsGridView.AllowUserToResizeRows = false;
            this.operatorInterruptionsGridView.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.operatorInterruptionsGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.operatorInterruptionsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.operatorInterruptionsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.operatorColumn,
            this.Type,
            this.TargetDate,
            this.dayOfWeekColumn,
            this.startTimeColumn,
            this.finishTimeColumn});
            this.operatorInterruptionsGridView.DataSource = this.operatorInterruptionsBindingSource;
            this.operatorInterruptionsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.operatorInterruptionsGridView.Location = new System.Drawing.Point(0, 0);
            this.operatorInterruptionsGridView.Margin = new System.Windows.Forms.Padding(0);
            this.operatorInterruptionsGridView.MultiSelect = false;
            this.operatorInterruptionsGridView.Name = "operatorInterruptionsGridView";
            this.operatorInterruptionsGridView.ReadOnly = true;
            this.operatorInterruptionsGridView.RowHeadersVisible = false;
            this.operatorInterruptionsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.operatorInterruptionsGridView.Size = new System.Drawing.Size(764, 257);
            this.operatorInterruptionsGridView.TabIndex = 0;
            this.operatorInterruptionsGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.operatorInterruptionsGridView_CellMouseDoubleClick);
            this.operatorInterruptionsGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.operatorInterruptionsGridView_UserDeletingRow);
            // 
            // operatorInterruptionsBindingSource
            // 
            this.operatorInterruptionsBindingSource.DataSource = typeof(Queue.Services.DTO.OperatorInterruption);
            // 
            // addAdditionalServiceButton
            // 
            this.addAdditionalServiceButton.Location = new System.Drawing.Point(0, 262);
            this.addAdditionalServiceButton.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.addAdditionalServiceButton.Name = "addAdditionalServiceButton";
            this.addAdditionalServiceButton.Size = new System.Drawing.Size(75, 25);
            this.addAdditionalServiceButton.TabIndex = 0;
            this.addAdditionalServiceButton.Text = "Добавить";
            this.addAdditionalServiceButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // operatorColumn
            // 
            this.operatorColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.operatorColumn.DataPropertyName = "Operator";
            this.operatorColumn.HeaderText = "Оператор";
            this.operatorColumn.Name = "operatorColumn";
            this.operatorColumn.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "Type";
            this.Type.HeaderText = "Тип";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // TargetDate
            // 
            this.TargetDate.DataPropertyName = "TargetDate";
            dataGridViewCellStyle2.NullValue = null;
            this.TargetDate.DefaultCellStyle = dataGridViewCellStyle2;
            this.TargetDate.HeaderText = "Целевая дата";
            this.TargetDate.Name = "TargetDate";
            this.TargetDate.ReadOnly = true;
            this.TargetDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.TargetDate.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.TargetDate.Width = 150;
            // 
            // dayOfWeekColumn
            // 
            this.dayOfWeekColumn.DataPropertyName = "DayOfWeek";
            this.dayOfWeekColumn.HeaderText = "День недели";
            this.dayOfWeekColumn.Name = "dayOfWeekColumn";
            this.dayOfWeekColumn.ReadOnly = true;
            this.dayOfWeekColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dayOfWeekColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dayOfWeekColumn.Width = 120;
            // 
            // startTimeColumn
            // 
            this.startTimeColumn.DataPropertyName = "StartTime";
            this.startTimeColumn.HeaderText = "Начало";
            this.startTimeColumn.Name = "startTimeColumn";
            this.startTimeColumn.ReadOnly = true;
            // 
            // finishTimeColumn
            // 
            this.finishTimeColumn.DataPropertyName = "FinishTime";
            this.finishTimeColumn.HeaderText = "Окончание";
            this.finishTimeColumn.Name = "finishTimeColumn";
            this.finishTimeColumn.ReadOnly = true;
            // 
            // OperatorInterruptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 312);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(800, 350);
            this.Name = "OperatorInterruptionsForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Перерывы операторов";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OperatorInterruptionsForm_FormClosing);
            this.Load += new System.EventHandler(this.OperatorInterruptionsForm_Load);
            this.mainTableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.operatorInterruptionsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.operatorInterruptionsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Button addAdditionalServiceButton;
        private System.Windows.Forms.DataGridView operatorInterruptionsGridView;
        private System.Windows.Forms.BindingSource operatorInterruptionsBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn operatorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private UI.WinForms.DataGridViewTranslatableColumn TargetDate;
        private UI.WinForms.DataGridViewTranslatableColumn dayOfWeekColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn startTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn finishTimeColumn;

    }
}