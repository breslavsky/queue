namespace Queue.Administrator
{
    partial class ServiceParametersControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.addButton = new System.Windows.Forms.Button();
            this.parametersGridView = new System.Windows.Forms.DataGridView();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTipColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isRequireColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.addPanel = new System.Windows.Forms.Panel();
            this.parameterTypeControl = new Queue.UI.WinForms.EnumItemControl();
            ((System.ComponentModel.ISupportInitialize)(this.parametersGridView)).BeginInit();
            this.mainTableLayoutPanel.SuspendLayout();
            this.addPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(125, 0);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Добавить";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // parametersGridView
            // 
            this.parametersGridView.AllowUserToAddRows = false;
            this.parametersGridView.AllowUserToResizeColumns = false;
            this.parametersGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.parametersGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.parametersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parametersGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.toolTipColumn,
            this.isRequireColumn});
            this.parametersGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersGridView.Location = new System.Drawing.Point(0, 0);
            this.parametersGridView.Margin = new System.Windows.Forms.Padding(0);
            this.parametersGridView.MultiSelect = false;
            this.parametersGridView.Name = "parametersGridView";
            this.parametersGridView.ReadOnly = true;
            this.parametersGridView.RowHeadersVisible = false;
            this.parametersGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.parametersGridView.Size = new System.Drawing.Size(620, 290);
            this.parametersGridView.TabIndex = 5;
            this.parametersGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.parametersGridView_CellMouseDoubleClick);
            this.parametersGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.parametersGridView_UserDeletingRow);
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameColumn.HeaderText = "Имя";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            this.nameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // toolTipColumn
            // 
            this.toolTipColumn.HeaderText = "Подсказка";
            this.toolTipColumn.Name = "toolTipColumn";
            this.toolTipColumn.ReadOnly = true;
            this.toolTipColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.toolTipColumn.Width = 110;
            // 
            // isRequireColumn
            // 
            this.isRequireColumn.HeaderText = "Обязательное";
            this.isRequireColumn.Name = "isRequireColumn";
            this.isRequireColumn.ReadOnly = true;
            this.isRequireColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.isRequireColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.parametersGridView, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.addPanel, 0, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(620, 320);
            this.mainTableLayoutPanel.TabIndex = 6;
            // 
            // addPanel
            // 
            this.addPanel.Controls.Add(this.addButton);
            this.addPanel.Controls.Add(this.parameterTypeControl);
            this.addPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addPanel.Location = new System.Drawing.Point(3, 293);
            this.addPanel.Name = "addPanel";
            this.addPanel.Size = new System.Drawing.Size(614, 24);
            this.addPanel.TabIndex = 6;
            // 
            // parameterTypeControl
            // 
            this.parameterTypeControl.Enabled = false;
            this.parameterTypeControl.Location = new System.Drawing.Point(2, 0);
            this.parameterTypeControl.Name = "parameterTypeControl";
            this.parameterTypeControl.Size = new System.Drawing.Size(125, 21);
            this.parameterTypeControl.TabIndex = 4;
            // 
            // ServiceParametersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTableLayoutPanel);
            this.Name = "ServiceParametersControl";
            this.Size = new System.Drawing.Size(620, 320);
            ((System.ComponentModel.ISupportInitialize)(this.parametersGridView)).EndInit();
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.addPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button addButton;
        private UI.WinForms.EnumItemControl parameterTypeControl;
        private System.Windows.Forms.DataGridView parametersGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn toolTipColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isRequireColumn;
        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Panel addPanel;
    }
}
