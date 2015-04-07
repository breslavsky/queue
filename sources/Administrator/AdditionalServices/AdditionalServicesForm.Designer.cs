namespace Queue.Administrator
{
    partial class AdditionalServicesForm
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
            this.additionalServicesGridView = new System.Windows.Forms.DataGridView();
            this.additionalServicesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.addButton = new System.Windows.Forms.Button();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.measureColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mainTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.additionalServicesGridView, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.addButton, 0, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(10, 10);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(464, 292);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // additionalServicesGridView
            // 
            this.additionalServicesGridView.AllowUserToAddRows = false;
            this.additionalServicesGridView.AllowUserToResizeColumns = false;
            this.additionalServicesGridView.AllowUserToResizeRows = false;
            this.additionalServicesGridView.AutoGenerateColumns = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.additionalServicesGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.additionalServicesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.additionalServicesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.priceColumn,
            this.measureColumn});
            this.additionalServicesGridView.DataSource = this.additionalServicesBindingSource;
            this.additionalServicesGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.additionalServicesGridView.Location = new System.Drawing.Point(0, 0);
            this.additionalServicesGridView.Margin = new System.Windows.Forms.Padding(0);
            this.additionalServicesGridView.MultiSelect = false;
            this.additionalServicesGridView.Name = "additionalServicesGridView";
            this.additionalServicesGridView.ReadOnly = true;
            this.additionalServicesGridView.RowHeadersVisible = false;
            this.additionalServicesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.additionalServicesGridView.Size = new System.Drawing.Size(464, 257);
            this.additionalServicesGridView.TabIndex = 0;
            this.additionalServicesGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.additionalServicesGridView_CellMouseDoubleClick);
            this.additionalServicesGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.additionalServicesGridView_UserDeletingRow);
            // 
            // additionalServicesBindingSource
            // 
            this.additionalServicesBindingSource.DataSource = typeof(Queue.Services.DTO.AdditionalService);
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(0, 262);
            this.addButton.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 25);
            this.addButton.TabIndex = 0;
            this.addButton.Text = "Добавить";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameColumn.DataPropertyName = "Name";
            this.nameColumn.HeaderText = "Название";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // priceColumn
            // 
            this.priceColumn.DataPropertyName = "Price";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "N2";
            dataGridViewCellStyle2.NullValue = null;
            this.priceColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.priceColumn.HeaderText = "Цена";
            this.priceColumn.Name = "priceColumn";
            this.priceColumn.ReadOnly = true;
            // 
            // measureColumn
            // 
            this.measureColumn.DataPropertyName = "Measure";
            this.measureColumn.HeaderText = "Ед. изм.";
            this.measureColumn.Name = "measureColumn";
            this.measureColumn.ReadOnly = true;
            // 
            // AdditionalServicesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 312);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "AdditionalServicesForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Дополнительные услуги";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdditionalServicesForm_FormClosing);
            this.Load += new System.EventHandler(this.AdditionalServicesForm_Load);
            this.mainTableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.DataGridView additionalServicesGridView;
        private System.Windows.Forms.BindingSource additionalServicesBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priceColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn measureColumn;

    }
}