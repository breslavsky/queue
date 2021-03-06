﻿namespace Queue.Administrator
{
    partial class WorkplacesForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.workplacesGridView = new System.Windows.Forms.DataGridView();
            this.addWorkplaceButton = new System.Windows.Forms.Button();
            this.typeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.numberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modificatorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.commentColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.displayDeviceIdColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qualityPanelDeviceId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mainTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.workplacesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.workplacesGridView, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.addWorkplaceButton, 0, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(10, 10);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(814, 442);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // workplacesGridView
            // 
            this.workplacesGridView.AllowUserToAddRows = false;
            this.workplacesGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.workplacesGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.workplacesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.workplacesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.typeColumn,
            this.numberColumn,
            this.modificatorColumn,
            this.commentColumn,
            this.displayDeviceIdColumn,
            this.qualityPanelDeviceId});
            this.workplacesGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workplacesGridView.Location = new System.Drawing.Point(0, 0);
            this.workplacesGridView.Margin = new System.Windows.Forms.Padding(0);
            this.workplacesGridView.MultiSelect = false;
            this.workplacesGridView.Name = "workplacesGridView";
            this.workplacesGridView.ReadOnly = true;
            this.workplacesGridView.RowHeadersVisible = false;
            this.workplacesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.workplacesGridView.Size = new System.Drawing.Size(814, 407);
            this.workplacesGridView.TabIndex = 0;
            this.workplacesGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.workplacesGridView_CellMouseDoubleClick);
            this.workplacesGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.workplacesGridView_UserDeletingRow);
            // 
            // addWorkplaceButton
            // 
            this.addWorkplaceButton.Location = new System.Drawing.Point(0, 412);
            this.addWorkplaceButton.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.addWorkplaceButton.Name = "addWorkplaceButton";
            this.addWorkplaceButton.Size = new System.Drawing.Size(75, 25);
            this.addWorkplaceButton.TabIndex = 1;
            this.addWorkplaceButton.Text = "Добавить";
            this.addWorkplaceButton.Click += new System.EventHandler(this.addWorkplaceButton_Click);
            // 
            // typeColumn
            // 
            this.typeColumn.FillWeight = 130F;
            this.typeColumn.HeaderText = "Тип";
            this.typeColumn.Name = "typeColumn";
            this.typeColumn.ReadOnly = true;
            this.typeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.typeColumn.Width = 130;
            // 
            // numberColumn
            // 
            this.numberColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.NullValue = null;
            this.numberColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.numberColumn.FillWeight = 80F;
            this.numberColumn.HeaderText = "Номер";
            this.numberColumn.Name = "numberColumn";
            this.numberColumn.ReadOnly = true;
            this.numberColumn.Width = 80;
            // 
            // modificatorColumn
            // 
            this.modificatorColumn.FillWeight = 120F;
            this.modificatorColumn.HeaderText = "Модификатор";
            this.modificatorColumn.Name = "modificatorColumn";
            this.modificatorColumn.ReadOnly = true;
            this.modificatorColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.modificatorColumn.Width = 120;
            // 
            // commentColumn
            // 
            this.commentColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.commentColumn.HeaderText = "Комментарий";
            this.commentColumn.Name = "commentColumn";
            this.commentColumn.ReadOnly = true;
            // 
            // displayDeviceIdColumn
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.displayDeviceIdColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.displayDeviceIdColumn.HeaderText = "Номер табло";
            this.displayDeviceIdColumn.Name = "displayDeviceIdColumn";
            this.displayDeviceIdColumn.ReadOnly = true;
            this.displayDeviceIdColumn.Width = 130;
            // 
            // qualityPanelDeviceId
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.qualityPanelDeviceId.DefaultCellStyle = dataGridViewCellStyle4;
            this.qualityPanelDeviceId.HeaderText = "Номер пульта";
            this.qualityPanelDeviceId.Name = "qualityPanelDeviceId";
            this.qualityPanelDeviceId.ReadOnly = true;
            this.qualityPanelDeviceId.Width = 120;
            // 
            // WorkplacesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(834, 462);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(849, 499);
            this.Name = "WorkplacesForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Рабочие места";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkplacesForm_FormClosing);
            this.Load += new System.EventHandler(this.WorkplacesForm_Load);
            this.mainTableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.workplacesGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.DataGridView workplacesGridView;
        private System.Windows.Forms.Button addWorkplaceButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn typeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modificatorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn commentColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn displayDeviceIdColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn qualityPanelDeviceId;
    }
}