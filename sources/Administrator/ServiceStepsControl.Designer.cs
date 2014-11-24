﻿namespace Queue.Administrator
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.stepsTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.serviceStepsGridView = new System.Windows.Forms.DataGridView();
            this.deleteColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.addStepButton = new System.Windows.Forms.Button();
            this.stepsTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serviceStepsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // stepsTableLayoutPanel
            // 
            this.stepsTableLayoutPanel.ColumnCount = 1;
            this.stepsTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.stepsTableLayoutPanel.Controls.Add(this.serviceStepsGridView, 0, 0);
            this.stepsTableLayoutPanel.Controls.Add(this.addStepButton, 0, 1);
            this.stepsTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stepsTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.stepsTableLayoutPanel.Name = "stepsTableLayoutPanel";
            this.stepsTableLayoutPanel.RowCount = 2;
            this.stepsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.stepsTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.stepsTableLayoutPanel.Size = new System.Drawing.Size(600, 400);
            this.stepsTableLayoutPanel.TabIndex = 2;
            // 
            // serviceStepsGridView
            // 
            this.serviceStepsGridView.AllowUserToAddRows = false;
            this.serviceStepsGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.serviceStepsGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.serviceStepsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.serviceStepsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deleteColumn,
            this.nameColumn});
            this.serviceStepsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceStepsGridView.Location = new System.Drawing.Point(0, 0);
            this.serviceStepsGridView.Margin = new System.Windows.Forms.Padding(0);
            this.serviceStepsGridView.MultiSelect = false;
            this.serviceStepsGridView.Name = "serviceStepsGridView";
            this.serviceStepsGridView.ReadOnly = true;
            this.serviceStepsGridView.RowHeadersVisible = false;
            this.serviceStepsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.serviceStepsGridView.Size = new System.Drawing.Size(600, 365);
            this.serviceStepsGridView.TabIndex = 0;
            this.serviceStepsGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.serviceStepsGridView_CellClick);
            this.serviceStepsGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.serviceStepsGridView_CellMouseDoubleClick);
            // 
            // deleteColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.deleteColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.deleteColumn.FillWeight = 60F;
            this.deleteColumn.HeaderText = "";
            this.deleteColumn.Name = "deleteColumn";
            this.deleteColumn.ReadOnly = true;
            this.deleteColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.deleteColumn.Text = "[удалить]";
            this.deleteColumn.UseColumnTextForLinkValue = true;
            this.deleteColumn.Width = 60;
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
            this.addStepButton.Location = new System.Drawing.Point(0, 370);
            this.addStepButton.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.addStepButton.Name = "addStepButton";
            this.addStepButton.Size = new System.Drawing.Size(75, 25);
            this.addStepButton.TabIndex = 0;
            this.addStepButton.Text = "Добавить";
            this.addStepButton.Click += new System.EventHandler(this.addStepButton_Click);
            // 
            // ServiceStepsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stepsTableLayoutPanel);
            this.Name = "ServiceStepsControl";
            this.Size = new System.Drawing.Size(600, 400);
            this.stepsTableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.serviceStepsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel stepsTableLayoutPanel;
        private System.Windows.Forms.DataGridView serviceStepsGridView;
        private System.Windows.Forms.DataGridViewLinkColumn deleteColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.Button addStepButton;
    }
}
