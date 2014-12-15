namespace Queue.Administrator
{
    partial class EditClientRequestForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.stateLabel = new System.Windows.Forms.Label();
            this.numberLabel = new System.Windows.Forms.Label();
            this.parametersLabel = new System.Windows.Forms.Label();
            this.parametersGridView = new System.Windows.Forms.DataGridView();
            this.parameterNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parameterValueColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestDateLabel = new System.Windows.Forms.Label();
            this.clientLabel = new System.Windows.Forms.Label();
            this.serviceLabel = new System.Windows.Forms.Label();
            this.eventsGridView = new System.Windows.Forms.DataGridView();
            this.createDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.messageColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventsLabel = new System.Windows.Forms.Label();
            this.operatorLabel = new System.Windows.Forms.Label();
            this.serviceChangeLink = new System.Windows.Forms.LinkLabel();
            this.isPriorityCheckBox = new System.Windows.Forms.CheckBox();
            this.subjectsLabel = new System.Windows.Forms.Label();
            this.subjectsUpDown = new System.Windows.Forms.NumericUpDown();
            this.clientEditLink = new System.Windows.Forms.LinkLabel();
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.cancelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.couponMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numberTextBlock = new System.Windows.Forms.Label();
            this.requestDateTextBlock = new System.Windows.Forms.Label();
            this.clientTextBlock = new System.Windows.Forms.Label();
            this.serviceTextBlock = new System.Windows.Forms.Label();
            this.stateTextBlock = new System.Windows.Forms.Label();
            this.requestTimeTextBlock = new System.Windows.Forms.Label();
            this.requestTimeLabel = new System.Windows.Forms.Label();
            this.serviceTypeLabel = new System.Windows.Forms.Label();
            this.serviceTypeTextBlock = new System.Windows.Forms.Label();
            this.serviceStepLabel = new System.Windows.Forms.Label();
            this.editPanel = new System.Windows.Forms.Panel();
            this.saveButton = new System.Windows.Forms.Button();
            this.operatorControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.serviceStepControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.typeControl = new Queue.UI.WinForms.EnumItemControl();
            ((System.ComponentModel.ISupportInitialize)(this.parametersGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).BeginInit();
            this.topMenu.SuspendLayout();
            this.editPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // stateLabel
            // 
            this.stateLabel.Location = new System.Drawing.Point(5, 255);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(108, 20);
            this.stateLabel.TabIndex = 0;
            this.stateLabel.Text = "Текущее состояние";
            this.stateLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // numberLabel
            // 
            this.numberLabel.Location = new System.Drawing.Point(5, 5);
            this.numberLabel.Name = "numberLabel";
            this.numberLabel.Size = new System.Drawing.Size(110, 20);
            this.numberLabel.TabIndex = 0;
            this.numberLabel.Text = "Номер";
            this.numberLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // parametersLabel
            // 
            this.parametersLabel.Location = new System.Drawing.Point(325, 5);
            this.parametersLabel.Name = "parametersLabel";
            this.parametersLabel.Size = new System.Drawing.Size(295, 20);
            this.parametersLabel.TabIndex = 0;
            this.parametersLabel.Text = "Параметры услуги";
            this.parametersLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // parametersGridView
            // 
            this.parametersGridView.AllowUserToAddRows = false;
            this.parametersGridView.AllowUserToDeleteRows = false;
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
            this.parameterNameColumn,
            this.parameterValueColumn});
            this.parametersGridView.Location = new System.Drawing.Point(325, 30);
            this.parametersGridView.MultiSelect = false;
            this.parametersGridView.Name = "parametersGridView";
            this.parametersGridView.ReadOnly = true;
            this.parametersGridView.RowHeadersVisible = false;
            this.parametersGridView.Size = new System.Drawing.Size(295, 155);
            this.parametersGridView.TabIndex = 0;
            // 
            // parameterNameColumn
            // 
            this.parameterNameColumn.HeaderText = "Название";
            this.parameterNameColumn.Name = "parameterNameColumn";
            this.parameterNameColumn.ReadOnly = true;
            this.parameterNameColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.parameterNameColumn.Width = 150;
            // 
            // parameterValueColumn
            // 
            this.parameterValueColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.parameterValueColumn.FillWeight = 150F;
            this.parameterValueColumn.HeaderText = "Значение";
            this.parameterValueColumn.Name = "parameterValueColumn";
            this.parameterValueColumn.ReadOnly = true;
            this.parameterValueColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // requestDateLabel
            // 
            this.requestDateLabel.Location = new System.Drawing.Point(5, 30);
            this.requestDateLabel.Name = "requestDateLabel";
            this.requestDateLabel.Size = new System.Drawing.Size(110, 20);
            this.requestDateLabel.TabIndex = 0;
            this.requestDateLabel.Text = "Дата запроса";
            this.requestDateLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // clientLabel
            // 
            this.clientLabel.Location = new System.Drawing.Point(5, 105);
            this.clientLabel.Name = "clientLabel";
            this.clientLabel.Size = new System.Drawing.Size(110, 20);
            this.clientLabel.TabIndex = 0;
            this.clientLabel.Text = "Клиент";
            this.clientLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceLabel
            // 
            this.serviceLabel.Location = new System.Drawing.Point(5, 130);
            this.serviceLabel.Name = "serviceLabel";
            this.serviceLabel.Size = new System.Drawing.Size(110, 55);
            this.serviceLabel.TabIndex = 0;
            this.serviceLabel.Text = "Выбранная услуга";
            this.serviceLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // eventsGridView
            // 
            this.eventsGridView.AllowUserToAddRows = false;
            this.eventsGridView.AllowUserToDeleteRows = false;
            this.eventsGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.eventsGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.eventsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.eventsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.createDateColumn,
            this.messageColumn});
            this.eventsGridView.Location = new System.Drawing.Point(10, 375);
            this.eventsGridView.MultiSelect = false;
            this.eventsGridView.Name = "eventsGridView";
            this.eventsGridView.ReadOnly = true;
            this.eventsGridView.RowHeadersVisible = false;
            this.eventsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.eventsGridView.Size = new System.Drawing.Size(615, 150);
            this.eventsGridView.TabIndex = 0;
            // 
            // createDateColumn
            // 
            this.createDateColumn.HeaderText = "Дата";
            this.createDateColumn.Name = "createDateColumn";
            this.createDateColumn.ReadOnly = true;
            this.createDateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.createDateColumn.Width = 130;
            // 
            // messageColumn
            // 
            this.messageColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.messageColumn.HeaderText = "Сообщение";
            this.messageColumn.Name = "messageColumn";
            this.messageColumn.ReadOnly = true;
            this.messageColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // eventsLabel
            // 
            this.eventsLabel.Location = new System.Drawing.Point(10, 350);
            this.eventsLabel.Name = "eventsLabel";
            this.eventsLabel.Size = new System.Drawing.Size(615, 20);
            this.eventsLabel.TabIndex = 0;
            this.eventsLabel.Text = "Журнал событий";
            this.eventsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // operatorLabel
            // 
            this.operatorLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.operatorLabel.Location = new System.Drawing.Point(5, 280);
            this.operatorLabel.Name = "operatorLabel";
            this.operatorLabel.Size = new System.Drawing.Size(110, 20);
            this.operatorLabel.TabIndex = 0;
            this.operatorLabel.Text = "Оператор";
            this.operatorLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceChangeLink
            // 
            this.serviceChangeLink.AutoSize = true;
            this.serviceChangeLink.Location = new System.Drawing.Point(260, 185);
            this.serviceChangeLink.Name = "serviceChangeLink";
            this.serviceChangeLink.Size = new System.Drawing.Size(62, 13);
            this.serviceChangeLink.TabIndex = 0;
            this.serviceChangeLink.TabStop = true;
            this.serviceChangeLink.Text = "[изменить]";
            this.serviceChangeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.serviceChangeLink_LinkClicked);
            // 
            // isPriorityCheckBox
            // 
            this.isPriorityCheckBox.Location = new System.Drawing.Point(180, 5);
            this.isPriorityCheckBox.Name = "isPriorityCheckBox";
            this.isPriorityCheckBox.Size = new System.Drawing.Size(115, 20);
            this.isPriorityCheckBox.TabIndex = 0;
            this.isPriorityCheckBox.Text = "Приоритет вызова";
            this.isPriorityCheckBox.UseVisualStyleBackColor = true;
            this.isPriorityCheckBox.Leave += new System.EventHandler(this.isPriorityCheckBox_Leave);
            // 
            // subjectsLabel
            // 
            this.subjectsLabel.Location = new System.Drawing.Point(5, 80);
            this.subjectsLabel.Name = "subjectsLabel";
            this.subjectsLabel.Size = new System.Drawing.Size(110, 20);
            this.subjectsLabel.TabIndex = 0;
            this.subjectsLabel.Text = "Объектов";
            this.subjectsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // subjectsUpDown
            // 
            this.subjectsUpDown.Location = new System.Drawing.Point(115, 80);
            this.subjectsUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.subjectsUpDown.Name = "subjectsUpDown";
            this.subjectsUpDown.Size = new System.Drawing.Size(60, 20);
            this.subjectsUpDown.TabIndex = 0;
            this.subjectsUpDown.Leave += new System.EventHandler(this.subjectsUpDown_Leave);
            // 
            // clientEditLink
            // 
            this.clientEditLink.AutoSize = true;
            this.clientEditLink.Location = new System.Drawing.Point(260, 110);
            this.clientEditLink.Name = "clientEditLink";
            this.clientEditLink.Size = new System.Drawing.Size(62, 13);
            this.clientEditLink.TabIndex = 0;
            this.clientEditLink.TabStop = true;
            this.clientEditLink.Text = "[изменить]";
            this.clientEditLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.clientEditLink_LinkClicked);
            // 
            // topMenu
            // 
            this.topMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cancelMenuItem,
            this.restoreMenuItem,
            this.couponMenuItem,
            this.reportMenuItem});
            this.topMenu.Location = new System.Drawing.Point(0, 0);
            this.topMenu.Name = "topMenu";
            this.topMenu.Size = new System.Drawing.Size(634, 24);
            this.topMenu.TabIndex = 0;
            this.topMenu.Text = "menuStrip1";
            // 
            // cancelMenuItem
            // 
            this.cancelMenuItem.Enabled = false;
            this.cancelMenuItem.Name = "cancelMenuItem";
            this.cancelMenuItem.Size = new System.Drawing.Size(73, 20);
            this.cancelMenuItem.Text = "Отменить";
            this.cancelMenuItem.Click += new System.EventHandler(this.cancelMenuItem_Click);
            // 
            // restoreMenuItem
            // 
            this.restoreMenuItem.Enabled = false;
            this.restoreMenuItem.Name = "restoreMenuItem";
            this.restoreMenuItem.Size = new System.Drawing.Size(94, 20);
            this.restoreMenuItem.Text = "Восстановить";
            this.restoreMenuItem.Click += new System.EventHandler(this.restoreMenuItem_Click);
            // 
            // couponMenuItem
            // 
            this.couponMenuItem.Name = "couponMenuItem";
            this.couponMenuItem.Size = new System.Drawing.Size(99, 20);
            this.couponMenuItem.Text = "Печать талона";
            this.couponMenuItem.Click += new System.EventHandler(this.couponMenuItem_Click);
            // 
            // reportMenuItem
            // 
            this.reportMenuItem.Name = "reportMenuItem";
            this.reportMenuItem.Size = new System.Drawing.Size(97, 20);
            this.reportMenuItem.Text = "Печать отчета";
            this.reportMenuItem.Click += new System.EventHandler(this.reportMenuItem_Click);
            // 
            // numberTextBlock
            // 
            this.numberTextBlock.BackColor = System.Drawing.Color.White;
            this.numberTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberTextBlock.Location = new System.Drawing.Point(115, 5);
            this.numberTextBlock.Name = "numberTextBlock";
            this.numberTextBlock.Padding = new System.Windows.Forms.Padding(2);
            this.numberTextBlock.Size = new System.Drawing.Size(60, 20);
            this.numberTextBlock.TabIndex = 1;
            // 
            // requestDateTextBlock
            // 
            this.requestDateTextBlock.BackColor = System.Drawing.Color.White;
            this.requestDateTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.requestDateTextBlock.Location = new System.Drawing.Point(115, 30);
            this.requestDateTextBlock.Name = "requestDateTextBlock";
            this.requestDateTextBlock.Padding = new System.Windows.Forms.Padding(2);
            this.requestDateTextBlock.Size = new System.Drawing.Size(85, 20);
            this.requestDateTextBlock.TabIndex = 2;
            // 
            // clientTextBlock
            // 
            this.clientTextBlock.BackColor = System.Drawing.Color.White;
            this.clientTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clientTextBlock.Location = new System.Drawing.Point(115, 105);
            this.clientTextBlock.Name = "clientTextBlock";
            this.clientTextBlock.Padding = new System.Windows.Forms.Padding(2);
            this.clientTextBlock.Size = new System.Drawing.Size(140, 20);
            this.clientTextBlock.TabIndex = 4;
            // 
            // serviceTextBlock
            // 
            this.serviceTextBlock.BackColor = System.Drawing.Color.White;
            this.serviceTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serviceTextBlock.Location = new System.Drawing.Point(115, 130);
            this.serviceTextBlock.Name = "serviceTextBlock";
            this.serviceTextBlock.Padding = new System.Windows.Forms.Padding(2);
            this.serviceTextBlock.Size = new System.Drawing.Size(205, 55);
            this.serviceTextBlock.TabIndex = 5;
            // 
            // stateTextBlock
            // 
            this.stateTextBlock.BackColor = System.Drawing.Color.White;
            this.stateTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stateTextBlock.Location = new System.Drawing.Point(115, 255);
            this.stateTextBlock.Name = "stateTextBlock";
            this.stateTextBlock.Padding = new System.Windows.Forms.Padding(2);
            this.stateTextBlock.Size = new System.Drawing.Size(120, 20);
            this.stateTextBlock.TabIndex = 6;
            // 
            // requestTimeTextBlock
            // 
            this.requestTimeTextBlock.BackColor = System.Drawing.Color.White;
            this.requestTimeTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.requestTimeTextBlock.Location = new System.Drawing.Point(115, 55);
            this.requestTimeTextBlock.Name = "requestTimeTextBlock";
            this.requestTimeTextBlock.Padding = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.requestTimeTextBlock.Size = new System.Drawing.Size(60, 20);
            this.requestTimeTextBlock.TabIndex = 8;
            // 
            // requestTimeLabel
            // 
            this.requestTimeLabel.Location = new System.Drawing.Point(5, 55);
            this.requestTimeLabel.Name = "requestTimeLabel";
            this.requestTimeLabel.Size = new System.Drawing.Size(110, 20);
            this.requestTimeLabel.TabIndex = 9;
            this.requestTimeLabel.Text = "Время запроса";
            this.requestTimeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceTypeLabel
            // 
            this.serviceTypeLabel.Location = new System.Drawing.Point(5, 205);
            this.serviceTypeLabel.Name = "serviceTypeLabel";
            this.serviceTypeLabel.Size = new System.Drawing.Size(110, 20);
            this.serviceTypeLabel.TabIndex = 10;
            this.serviceTypeLabel.Text = "Тип услуги";
            this.serviceTypeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceTypeTextBlock
            // 
            this.serviceTypeTextBlock.BackColor = System.Drawing.Color.White;
            this.serviceTypeTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serviceTypeTextBlock.Location = new System.Drawing.Point(115, 205);
            this.serviceTypeTextBlock.Name = "serviceTypeTextBlock";
            this.serviceTypeTextBlock.Padding = new System.Windows.Forms.Padding(2);
            this.serviceTypeTextBlock.Size = new System.Drawing.Size(205, 20);
            this.serviceTypeTextBlock.TabIndex = 11;
            // 
            // serviceStepLabel
            // 
            this.serviceStepLabel.Location = new System.Drawing.Point(5, 230);
            this.serviceStepLabel.Name = "serviceStepLabel";
            this.serviceStepLabel.Size = new System.Drawing.Size(110, 20);
            this.serviceStepLabel.TabIndex = 12;
            this.serviceStepLabel.Text = "Этап услуги";
            this.serviceStepLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // editPanel
            // 
            this.editPanel.Controls.Add(this.saveButton);
            this.editPanel.Controls.Add(this.operatorControl);
            this.editPanel.Controls.Add(this.serviceStepControl);
            this.editPanel.Controls.Add(this.typeControl);
            this.editPanel.Controls.Add(this.numberLabel);
            this.editPanel.Controls.Add(this.numberTextBlock);
            this.editPanel.Controls.Add(this.isPriorityCheckBox);
            this.editPanel.Controls.Add(this.requestDateLabel);
            this.editPanel.Controls.Add(this.requestDateTextBlock);
            this.editPanel.Controls.Add(this.requestTimeLabel);
            this.editPanel.Controls.Add(this.requestTimeTextBlock);
            this.editPanel.Controls.Add(this.subjectsLabel);
            this.editPanel.Controls.Add(this.subjectsUpDown);
            this.editPanel.Controls.Add(this.clientLabel);
            this.editPanel.Controls.Add(this.clientTextBlock);
            this.editPanel.Controls.Add(this.clientEditLink);
            this.editPanel.Controls.Add(this.serviceLabel);
            this.editPanel.Controls.Add(this.serviceTextBlock);
            this.editPanel.Controls.Add(this.serviceChangeLink);
            this.editPanel.Controls.Add(this.serviceTypeLabel);
            this.editPanel.Controls.Add(this.serviceTypeTextBlock);
            this.editPanel.Controls.Add(this.serviceStepLabel);
            this.editPanel.Controls.Add(this.stateLabel);
            this.editPanel.Controls.Add(this.stateTextBlock);
            this.editPanel.Controls.Add(this.operatorLabel);
            this.editPanel.Controls.Add(this.parametersLabel);
            this.editPanel.Controls.Add(this.parametersGridView);
            this.editPanel.Location = new System.Drawing.Point(0, 25);
            this.editPanel.Name = "editPanel";
            this.editPanel.Size = new System.Drawing.Size(635, 315);
            this.editPanel.TabIndex = 16;
            this.editPanel.EnabledChanged += new System.EventHandler(this.editPanel_EnabledChanged);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(330, 195);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 20;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // operatorControl
            // 
            this.operatorControl.Enabled = false;
            this.operatorControl.Location = new System.Drawing.Point(115, 280);
            this.operatorControl.Name = "operatorControl";
            this.operatorControl.Size = new System.Drawing.Size(205, 21);
            this.operatorControl.TabIndex = 19;
            this.operatorControl.UseResetButton = true;
            this.operatorControl.Leave += new System.EventHandler(this.operatorsControl_Leave);
            // 
            // serviceStepControl
            // 
            this.serviceStepControl.Enabled = false;
            this.serviceStepControl.Location = new System.Drawing.Point(115, 230);
            this.serviceStepControl.Name = "serviceStepControl";
            this.serviceStepControl.Size = new System.Drawing.Size(205, 21);
            this.serviceStepControl.TabIndex = 18;
            this.serviceStepControl.UseResetButton = true;
            this.serviceStepControl.Leave += new System.EventHandler(this.serviceStepControl_Leave);
            // 
            // typeControl
            // 
            this.typeControl.Enabled = false;
            this.typeControl.Location = new System.Drawing.Point(180, 55);
            this.typeControl.Name = "typeControl";
            this.typeControl.Size = new System.Drawing.Size(135, 21);
            this.typeControl.TabIndex = 17;
            this.typeControl.Leave += new System.EventHandler(this.typeControl_Leave);
            // 
            // EditClientRequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 536);
            this.Controls.Add(this.topMenu);
            this.Controls.Add(this.editPanel);
            this.Controls.Add(this.eventsLabel);
            this.Controls.Add(this.eventsGridView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(650, 489);
            this.Name = "EditClientRequestForm";
            this.Text = "Запрос клиента";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditClientRequestForm_FormClosing);
            this.Load += new System.EventHandler(this.EditClientRequestForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.parametersGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).EndInit();
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.editPanel.ResumeLayout(false);
            this.editPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label stateLabel;
        private System.Windows.Forms.Label numberLabel;
        private System.Windows.Forms.Label parametersLabel;
        private System.Windows.Forms.DataGridView parametersGridView;
        private System.Windows.Forms.Label requestDateLabel;
        private System.Windows.Forms.Label clientLabel;
        private System.Windows.Forms.Label serviceLabel;
        private System.Windows.Forms.DataGridView eventsGridView;
        private System.Windows.Forms.Label eventsLabel;
        private System.Windows.Forms.Label operatorLabel;
        private System.Windows.Forms.LinkLabel serviceChangeLink;
        private System.Windows.Forms.CheckBox isPriorityCheckBox;
        private System.Windows.Forms.Label subjectsLabel;
        private System.Windows.Forms.NumericUpDown subjectsUpDown;
        private System.Windows.Forms.LinkLabel clientEditLink;
        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.ToolStripMenuItem cancelMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreMenuItem;
        private System.Windows.Forms.ToolStripMenuItem couponMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportMenuItem;
        private System.Windows.Forms.Label numberTextBlock;
        private System.Windows.Forms.Label requestDateTextBlock;
        private System.Windows.Forms.Label clientTextBlock;
        private System.Windows.Forms.Label serviceTextBlock;
        private System.Windows.Forms.Label stateTextBlock;
        private System.Windows.Forms.DataGridViewTextBoxColumn createDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn messageColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn parameterNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn parameterValueColumn;
        private System.Windows.Forms.Label requestTimeTextBlock;
        private System.Windows.Forms.Label requestTimeLabel;
        private System.Windows.Forms.Label serviceTypeLabel;
        private System.Windows.Forms.Label serviceTypeTextBlock;
        private System.Windows.Forms.Label serviceStepLabel;
        private System.Windows.Forms.Panel editPanel;
        private UI.WinForms.EnumItemControl typeControl;
        private UI.WinForms.IdentifiedEntityControl operatorControl;
        private UI.WinForms.IdentifiedEntityControl serviceStepControl;
        private System.Windows.Forms.Button saveButton;
    }
}