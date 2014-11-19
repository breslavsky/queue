﻿namespace Queue.Manager
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.postponeMinutesMinLabel = new System.Windows.Forms.Label();
            this.postponeMinutesLabel = new System.Windows.Forms.Label();
            this.postponeMinutesUpDown = new System.Windows.Forms.NumericUpDown();
            this.operatorsComboBox = new System.Windows.Forms.ComboBox();
            this.operatorLabel = new System.Windows.Forms.Label();
            this.serviceChangeLink = new System.Windows.Forms.LinkLabel();
            this.isPriorityCheckBox = new System.Windows.Forms.CheckBox();
            this.subjectsLabel = new System.Windows.Forms.Label();
            this.subjectsUpDown = new System.Windows.Forms.NumericUpDown();
            this.clientEditLink = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.postponeButton = new System.Windows.Forms.Button();
            this.topMenu = new System.Windows.Forms.MenuStrip();
            this.cancelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restoreMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.couponMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reportMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.subjectsChangeButton = new System.Windows.Forms.Button();
            this.subjectsPanel = new System.Windows.Forms.Panel();
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
            this.serviceStepComboBox = new System.Windows.Forms.ComboBox();
            this.operatorResetButton = new System.Windows.Forms.Button();
            this.serviceStepResetButton = new System.Windows.Forms.Button();
            this.editPanel = new System.Windows.Forms.Panel();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.parametersGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.postponeMinutesUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.topMenu.SuspendLayout();
            this.subjectsPanel.SuspendLayout();
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
            this.parametersLabel.AutoSize = true;
            this.parametersLabel.Location = new System.Drawing.Point(325, 5);
            this.parametersLabel.Name = "parametersLabel";
            this.parametersLabel.Size = new System.Drawing.Size(102, 13);
            this.parametersLabel.TabIndex = 0;
            this.parametersLabel.Text = "Параметры услуги";
            // 
            // parametersGridView
            // 
            this.parametersGridView.AllowUserToAddRows = false;
            this.parametersGridView.AllowUserToDeleteRows = false;
            this.parametersGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle15.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.parametersGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.parametersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parametersGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.parameterNameColumn,
            this.parameterValueColumn});
            this.parametersGridView.Location = new System.Drawing.Point(325, 25);
            this.parametersGridView.MultiSelect = false;
            this.parametersGridView.Name = "parametersGridView";
            this.parametersGridView.ReadOnly = true;
            this.parametersGridView.RowHeadersVisible = false;
            this.parametersGridView.Size = new System.Drawing.Size(295, 160);
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
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle16.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle16.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle16.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle16.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.eventsGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle16;
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
            // postponeMinutesMinLabel
            // 
            this.postponeMinutesMinLabel.Location = new System.Drawing.Point(115, 20);
            this.postponeMinutesMinLabel.Name = "postponeMinutesMinLabel";
            this.postponeMinutesMinLabel.Size = new System.Drawing.Size(30, 20);
            this.postponeMinutesMinLabel.TabIndex = 0;
            this.postponeMinutesMinLabel.Text = "мин.";
            this.postponeMinutesMinLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // postponeMinutesLabel
            // 
            this.postponeMinutesLabel.Location = new System.Drawing.Point(5, 20);
            this.postponeMinutesLabel.Name = "postponeMinutesLabel";
            this.postponeMinutesLabel.Size = new System.Drawing.Size(60, 20);
            this.postponeMinutesLabel.TabIndex = 0;
            this.postponeMinutesLabel.Text = "На время";
            this.postponeMinutesLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // postponeMinutesUpDown
            // 
            this.postponeMinutesUpDown.Location = new System.Drawing.Point(65, 20);
            this.postponeMinutesUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.postponeMinutesUpDown.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.postponeMinutesUpDown.Name = "postponeMinutesUpDown";
            this.postponeMinutesUpDown.Size = new System.Drawing.Size(50, 20);
            this.postponeMinutesUpDown.TabIndex = 0;
            this.postponeMinutesUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // operatorsComboBox
            // 
            this.operatorsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.operatorsComboBox.Location = new System.Drawing.Point(115, 280);
            this.operatorsComboBox.Name = "operatorsComboBox";
            this.operatorsComboBox.Size = new System.Drawing.Size(180, 21);
            this.operatorsComboBox.TabIndex = 0;
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
            this.isPriorityCheckBox.Click += new System.EventHandler(this.isPriorityCheckBox_Click);
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
            this.subjectsUpDown.Location = new System.Drawing.Point(0, 5);
            this.subjectsUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.subjectsUpDown.Name = "subjectsUpDown";
            this.subjectsUpDown.Size = new System.Drawing.Size(60, 20);
            this.subjectsUpDown.TabIndex = 0;
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.postponeButton);
            this.groupBox1.Controls.Add(this.postponeMinutesMinLabel);
            this.groupBox1.Controls.Add(this.postponeMinutesUpDown);
            this.groupBox1.Controls.Add(this.postponeMinutesLabel);
            this.groupBox1.Location = new System.Drawing.Point(325, 195);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(235, 50);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Отложить запрос";
            // 
            // postponeButton
            // 
            this.postponeButton.Location = new System.Drawing.Point(150, 20);
            this.postponeButton.Name = "postponeButton";
            this.postponeButton.Size = new System.Drawing.Size(75, 20);
            this.postponeButton.TabIndex = 0;
            this.postponeButton.Text = "Отложить";
            this.postponeButton.UseVisualStyleBackColor = true;
            this.postponeButton.Click += new System.EventHandler(this.postponeButton_Click);
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
            // subjectsChangeButton
            // 
            this.subjectsChangeButton.Location = new System.Drawing.Point(65, 5);
            this.subjectsChangeButton.Name = "subjectsChangeButton";
            this.subjectsChangeButton.Size = new System.Drawing.Size(75, 20);
            this.subjectsChangeButton.TabIndex = 0;
            this.subjectsChangeButton.Text = "Изменить";
            this.subjectsChangeButton.UseVisualStyleBackColor = true;
            this.subjectsChangeButton.Click += new System.EventHandler(this.subjectsChangeButton_Click);
            // 
            // subjectsPanel
            // 
            this.subjectsPanel.Controls.Add(this.subjectsUpDown);
            this.subjectsPanel.Controls.Add(this.subjectsChangeButton);
            this.subjectsPanel.Location = new System.Drawing.Point(115, 75);
            this.subjectsPanel.Name = "subjectsPanel";
            this.subjectsPanel.Size = new System.Drawing.Size(150, 30);
            this.subjectsPanel.TabIndex = 0;
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
            // serviceStepComboBox
            // 
            this.serviceStepComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.serviceStepComboBox.Location = new System.Drawing.Point(115, 230);
            this.serviceStepComboBox.Name = "serviceStepComboBox";
            this.serviceStepComboBox.Size = new System.Drawing.Size(180, 21);
            this.serviceStepComboBox.TabIndex = 13;
            // 
            // operatorResetButton
            // 
            this.operatorResetButton.Location = new System.Drawing.Point(300, 280);
            this.operatorResetButton.Name = "operatorResetButton";
            this.operatorResetButton.Size = new System.Drawing.Size(20, 21);
            this.operatorResetButton.TabIndex = 14;
            this.operatorResetButton.Text = "X";
            this.operatorResetButton.UseVisualStyleBackColor = true;
            this.operatorResetButton.Click += new System.EventHandler(this.operatorResetButton_Click);
            // 
            // serviceStepResetButton
            // 
            this.serviceStepResetButton.Location = new System.Drawing.Point(300, 230);
            this.serviceStepResetButton.Name = "serviceStepResetButton";
            this.serviceStepResetButton.Size = new System.Drawing.Size(20, 21);
            this.serviceStepResetButton.TabIndex = 15;
            this.serviceStepResetButton.Text = "X";
            this.serviceStepResetButton.UseVisualStyleBackColor = true;
            this.serviceStepResetButton.Click += new System.EventHandler(this.serviceStepResetButton_Click);
            // 
            // editPanel
            // 
            this.editPanel.Controls.Add(this.typeComboBox);
            this.editPanel.Controls.Add(this.parametersGridView);
            this.editPanel.Controls.Add(this.serviceStepResetButton);
            this.editPanel.Controls.Add(this.parametersLabel);
            this.editPanel.Controls.Add(this.operatorResetButton);
            this.editPanel.Controls.Add(this.operatorsComboBox);
            this.editPanel.Controls.Add(this.serviceStepComboBox);
            this.editPanel.Controls.Add(this.operatorLabel);
            this.editPanel.Controls.Add(this.serviceStepLabel);
            this.editPanel.Controls.Add(this.stateLabel);
            this.editPanel.Controls.Add(this.serviceTypeTextBlock);
            this.editPanel.Controls.Add(this.serviceChangeLink);
            this.editPanel.Controls.Add(this.serviceTypeLabel);
            this.editPanel.Controls.Add(this.serviceLabel);
            this.editPanel.Controls.Add(this.clientLabel);
            this.editPanel.Controls.Add(this.requestTimeTextBlock);
            this.editPanel.Controls.Add(this.requestDateLabel);
            this.editPanel.Controls.Add(this.requestTimeLabel);
            this.editPanel.Controls.Add(this.isPriorityCheckBox);
            this.editPanel.Controls.Add(this.stateTextBlock);
            this.editPanel.Controls.Add(this.numberLabel);
            this.editPanel.Controls.Add(this.serviceTextBlock);
            this.editPanel.Controls.Add(this.subjectsLabel);
            this.editPanel.Controls.Add(this.clientTextBlock);
            this.editPanel.Controls.Add(this.clientEditLink);
            this.editPanel.Controls.Add(this.requestDateTextBlock);
            this.editPanel.Controls.Add(this.groupBox1);
            this.editPanel.Controls.Add(this.numberTextBlock);
            this.editPanel.Controls.Add(this.subjectsPanel);
            this.editPanel.Location = new System.Drawing.Point(0, 25);
            this.editPanel.Name = "editPanel";
            this.editPanel.Size = new System.Drawing.Size(635, 315);
            this.editPanel.TabIndex = 16;
            this.editPanel.EnabledChanged += new System.EventHandler(this.editPanel_EnabledChanged);
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.Location = new System.Drawing.Point(180, 55);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(110, 21);
            this.typeComboBox.TabIndex = 16;
            // 
            // EditClientRequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(634, 536);
            this.Controls.Add(this.editPanel);
            this.Controls.Add(this.topMenu);
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
            ((System.ComponentModel.ISupportInitialize)(this.postponeMinutesUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.subjectsPanel.ResumeLayout(false);
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
        private System.Windows.Forms.Label postponeMinutesMinLabel;
        private System.Windows.Forms.Label postponeMinutesLabel;
        private System.Windows.Forms.NumericUpDown postponeMinutesUpDown;
        private System.Windows.Forms.ComboBox operatorsComboBox;
        private System.Windows.Forms.Label operatorLabel;
        private System.Windows.Forms.LinkLabel serviceChangeLink;
        private System.Windows.Forms.CheckBox isPriorityCheckBox;
        private System.Windows.Forms.Label subjectsLabel;
        private System.Windows.Forms.NumericUpDown subjectsUpDown;
        private System.Windows.Forms.LinkLabel clientEditLink;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button postponeButton;
        private System.Windows.Forms.MenuStrip topMenu;
        private System.Windows.Forms.ToolStripMenuItem cancelMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restoreMenuItem;
        private System.Windows.Forms.ToolStripMenuItem couponMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reportMenuItem;
        private System.Windows.Forms.Button subjectsChangeButton;
        private System.Windows.Forms.Panel subjectsPanel;
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
        private System.Windows.Forms.ComboBox serviceStepComboBox;
        private System.Windows.Forms.Button operatorResetButton;
        private System.Windows.Forms.Button serviceStepResetButton;
        private System.Windows.Forms.Panel editPanel;
        private System.Windows.Forms.ComboBox typeComboBox;
    }
}