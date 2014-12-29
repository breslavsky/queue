namespace Queue.Administrator
{
    partial class OfficesForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.officesGridView = new System.Windows.Forms.DataGridView();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loginColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.manageColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.addOfficeButton = new System.Windows.Forms.Button();
            this.mainTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.officesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.officesGridView, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.addOfficeButton, 0, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(10, 10);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(464, 292);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // officesGridView
            // 
            this.officesGridView.AllowUserToAddRows = false;
            this.officesGridView.AllowUserToResizeColumns = false;
            this.officesGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.officesGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.officesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.officesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameColumn,
            this.loginColumn,
            this.manageColumn});
            this.officesGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.officesGridView.Location = new System.Drawing.Point(0, 0);
            this.officesGridView.Margin = new System.Windows.Forms.Padding(0);
            this.officesGridView.MultiSelect = false;
            this.officesGridView.Name = "officesGridView";
            this.officesGridView.ReadOnly = true;
            this.officesGridView.RowHeadersVisible = false;
            this.officesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.officesGridView.Size = new System.Drawing.Size(464, 257);
            this.officesGridView.TabIndex = 0;
            this.officesGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.officeGridView_CellClick);
            this.officesGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.officesGridView_CellMouseDoubleClick);
            this.officesGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.officesGridView_UserDeletingRow);
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
            // loginColumn
            // 
            this.loginColumn.FillWeight = 60F;
            this.loginColumn.HeaderText = "";
            this.loginColumn.Name = "loginColumn";
            this.loginColumn.ReadOnly = true;
            this.loginColumn.Text = "Войти";
            this.loginColumn.UseColumnTextForButtonValue = true;
            this.loginColumn.Width = 60;
            // 
            // manageColumn
            // 
            this.manageColumn.HeaderText = "";
            this.manageColumn.Name = "manageColumn";
            this.manageColumn.ReadOnly = true;
            this.manageColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.manageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.manageColumn.Text = "Управлять";
            this.manageColumn.UseColumnTextForButtonValue = true;
            // 
            // addOfficeButton
            // 
            this.addOfficeButton.Location = new System.Drawing.Point(0, 262);
            this.addOfficeButton.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.addOfficeButton.Name = "addOfficeButton";
            this.addOfficeButton.Size = new System.Drawing.Size(75, 25);
            this.addOfficeButton.TabIndex = 0;
            this.addOfficeButton.Text = "Добавить";
            this.addOfficeButton.Click += new System.EventHandler(this.addOfficeButton_Click);
            // 
            // OfficesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 312);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(500, 350);
            this.Name = "OfficesForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Филиалы";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OfficesForm_FormClosing);
            this.Load += new System.EventHandler(this.OfficesForm_Load);
            this.mainTableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.officesGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Button addOfficeButton;
        private System.Windows.Forms.DataGridView officesGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewButtonColumn loginColumn;
        private System.Windows.Forms.DataGridViewButtonColumn manageColumn;

    }
}