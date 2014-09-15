namespace Queue.Manager
{
    partial class EventsForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.eventsGridView = new System.Windows.Forms.DataGridView();
            this.createDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.messageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.eventsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // eventsGridView
            // 
            this.eventsGridView.AllowUserToAddRows = false;
            this.eventsGridView.AllowUserToDeleteRows = false;
            this.eventsGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.eventsGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.eventsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.eventsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.createDateColumn,
            this.messageColumn});
            this.eventsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsGridView.Location = new System.Drawing.Point(10, 10);
            this.eventsGridView.Margin = new System.Windows.Forms.Padding(0);
            this.eventsGridView.MultiSelect = false;
            this.eventsGridView.Name = "eventsGridView";
            this.eventsGridView.ReadOnly = true;
            this.eventsGridView.RowHeadersVisible = false;
            this.eventsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.eventsGridView.Size = new System.Drawing.Size(589, 242);
            this.eventsGridView.TabIndex = 0;
            // 
            // createDateColumn
            // 
            this.createDateColumn.HeaderText = "Дата и время";
            this.createDateColumn.Name = "createDateColumn";
            this.createDateColumn.ReadOnly = true;
            this.createDateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.createDateColumn.Width = 110;
            // 
            // messageColumn
            // 
            this.messageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.messageColumn.HeaderText = "Сообщение";
            this.messageColumn.Name = "messageColumn";
            this.messageColumn.ReadOnly = true;
            this.messageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // EventsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 262);
            this.Controls.Add(this.eventsGridView);
            this.MinimumSize = new System.Drawing.Size(625, 300);
            this.Name = "EventsForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "События";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EventsForm_FormClosed);
            this.Load += new System.EventHandler(this.Events_Load);
            ((System.ComponentModel.ISupportInitialize)(this.eventsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView eventsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn createDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn messageColumn;
    }
}