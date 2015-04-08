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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.stateLabel = new System.Windows.Forms.Label();
            this.numberLabel = new System.Windows.Forms.Label();
            this.parametersGridView = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parametersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.requestDateLabel = new System.Windows.Forms.Label();
            this.clientLabel = new System.Windows.Forms.Label();
            this.serviceLabel = new System.Windows.Forms.Label();
            this.eventsGridView = new System.Windows.Forms.DataGridView();
            this.createDateDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.messageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eventsBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
            this.clientTextBlock = new System.Windows.Forms.Label();
            this.serviceTextBlock = new System.Windows.Forms.Label();
            this.stateTextBlock = new System.Windows.Forms.Label();
            this.requestTimeLabel = new System.Windows.Forms.Label();
            this.serviceTypeLabel = new System.Windows.Forms.Label();
            this.serviceTypeTextBlock = new System.Windows.Forms.Label();
            this.serviceStepLabel = new System.Windows.Forms.Label();
            this.editPanel = new System.Windows.Forms.Panel();
            this.requestDatePicker = new System.Windows.Forms.DateTimePicker();
            this.requestTimePicker = new Queue.UI.WinForms.TimePicker();
            this.typeControl = new Queue.UI.WinForms.EnumItemControl();
            this.saveButton = new System.Windows.Forms.Button();
            this.serviceStepControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.operatorControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.additionalServicesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.clientRequestTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.additionalServicesGridView = new System.Windows.Forms.DataGridView();
            this.additionalServiceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.parametersGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parametersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventsGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).BeginInit();
            this.topMenu.SuspendLayout();
            this.editPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesBindingSource)).BeginInit();
            this.clientRequestTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesGridView)).BeginInit();
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
            this.numberLabel.Size = new System.Drawing.Size(105, 20);
            this.numberLabel.TabIndex = 0;
            this.numberLabel.Text = "Номер";
            this.numberLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // parametersGridView
            // 
            this.parametersGridView.AllowUserToAddRows = false;
            this.parametersGridView.AllowUserToDeleteRows = false;
            this.parametersGridView.AllowUserToResizeRows = false;
            this.parametersGridView.AutoGenerateColumns = false;
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
            this.nameDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn});
            this.parametersGridView.DataSource = this.parametersBindingSource;
            this.parametersGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parametersGridView.Location = new System.Drawing.Point(0, 0);
            this.parametersGridView.MultiSelect = false;
            this.parametersGridView.Name = "parametersGridView";
            this.parametersGridView.ReadOnly = true;
            this.parametersGridView.RowHeadersVisible = false;
            this.parametersGridView.Size = new System.Drawing.Size(537, 305);
            this.parametersGridView.TabIndex = 0;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.FillWeight = 120F;
            this.nameDataGridViewTextBoxColumn.HeaderText = "Наименование";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Width = 120;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Значение";
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            this.valueDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // parametersBindingSource
            // 
            this.parametersBindingSource.DataSource = typeof(Queue.Services.DTO.ClientRequestParameter);
            // 
            // requestDateLabel
            // 
            this.requestDateLabel.Location = new System.Drawing.Point(5, 30);
            this.requestDateLabel.Name = "requestDateLabel";
            this.requestDateLabel.Size = new System.Drawing.Size(105, 20);
            this.requestDateLabel.TabIndex = 0;
            this.requestDateLabel.Text = "Дата запроса";
            this.requestDateLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // clientLabel
            // 
            this.clientLabel.Location = new System.Drawing.Point(5, 105);
            this.clientLabel.Name = "clientLabel";
            this.clientLabel.Size = new System.Drawing.Size(105, 20);
            this.clientLabel.TabIndex = 0;
            this.clientLabel.Text = "Клиент";
            this.clientLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceLabel
            // 
            this.serviceLabel.Location = new System.Drawing.Point(5, 130);
            this.serviceLabel.Name = "serviceLabel";
            this.serviceLabel.Size = new System.Drawing.Size(105, 55);
            this.serviceLabel.TabIndex = 0;
            this.serviceLabel.Text = "Выбранная услуга";
            this.serviceLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // eventsGridView
            // 
            this.eventsGridView.AllowUserToAddRows = false;
            this.eventsGridView.AllowUserToDeleteRows = false;
            this.eventsGridView.AllowUserToResizeRows = false;
            this.eventsGridView.AutoGenerateColumns = false;
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
            this.createDateDataGridViewTextBoxColumn,
            this.messageDataGridViewTextBoxColumn});
            this.eventsGridView.DataSource = this.eventsBindingSource;
            this.eventsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eventsGridView.Location = new System.Drawing.Point(0, 0);
            this.eventsGridView.MultiSelect = false;
            this.eventsGridView.Name = "eventsGridView";
            this.eventsGridView.ReadOnly = true;
            this.eventsGridView.RowHeadersVisible = false;
            this.eventsGridView.Size = new System.Drawing.Size(542, 305);
            this.eventsGridView.TabIndex = 0;
            // 
            // createDateDataGridViewTextBoxColumn
            // 
            this.createDateDataGridViewTextBoxColumn.DataPropertyName = "CreateDate";
            this.createDateDataGridViewTextBoxColumn.HeaderText = "Дата";
            this.createDateDataGridViewTextBoxColumn.Name = "createDateDataGridViewTextBoxColumn";
            this.createDateDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // messageDataGridViewTextBoxColumn
            // 
            this.messageDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.messageDataGridViewTextBoxColumn.DataPropertyName = "Message";
            this.messageDataGridViewTextBoxColumn.HeaderText = "Сообщение";
            this.messageDataGridViewTextBoxColumn.Name = "messageDataGridViewTextBoxColumn";
            this.messageDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // eventsBindingSource
            // 
            this.eventsBindingSource.DataSource = typeof(Queue.Services.DTO.ClientRequestEvent);
            // 
            // operatorLabel
            // 
            this.operatorLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.operatorLabel.Location = new System.Drawing.Point(5, 280);
            this.operatorLabel.Name = "operatorLabel";
            this.operatorLabel.Size = new System.Drawing.Size(105, 20);
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
            this.serviceChangeLink.TabIndex = 9;
            this.serviceChangeLink.TabStop = true;
            this.serviceChangeLink.Text = "[изменить]";
            this.serviceChangeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.serviceChangeLink_LinkClicked);
            // 
            // isPriorityCheckBox
            // 
            this.isPriorityCheckBox.Location = new System.Drawing.Point(180, 5);
            this.isPriorityCheckBox.Name = "isPriorityCheckBox";
            this.isPriorityCheckBox.Size = new System.Drawing.Size(115, 20);
            this.isPriorityCheckBox.TabIndex = 1;
            this.isPriorityCheckBox.Text = "Приоритет вызова";
            this.isPriorityCheckBox.UseVisualStyleBackColor = true;
            this.isPriorityCheckBox.Leave += new System.EventHandler(this.isPriorityCheckBox_Leave);
            // 
            // subjectsLabel
            // 
            this.subjectsLabel.Location = new System.Drawing.Point(5, 80);
            this.subjectsLabel.Name = "subjectsLabel";
            this.subjectsLabel.Size = new System.Drawing.Size(105, 20);
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
            this.subjectsUpDown.TabIndex = 5;
            this.subjectsUpDown.Leave += new System.EventHandler(this.subjectsUpDown_Leave);
            // 
            // clientEditLink
            // 
            this.clientEditLink.AutoSize = true;
            this.clientEditLink.Location = new System.Drawing.Point(260, 110);
            this.clientEditLink.Name = "clientEditLink";
            this.clientEditLink.Size = new System.Drawing.Size(62, 13);
            this.clientEditLink.TabIndex = 7;
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
            this.topMenu.Size = new System.Drawing.Size(884, 24);
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
            this.numberTextBlock.TabIndex = 0;
            // 
            // clientTextBlock
            // 
            this.clientTextBlock.BackColor = System.Drawing.Color.White;
            this.clientTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clientTextBlock.Location = new System.Drawing.Point(115, 105);
            this.clientTextBlock.Name = "clientTextBlock";
            this.clientTextBlock.Padding = new System.Windows.Forms.Padding(2);
            this.clientTextBlock.Size = new System.Drawing.Size(140, 20);
            this.clientTextBlock.TabIndex = 6;
            // 
            // serviceTextBlock
            // 
            this.serviceTextBlock.BackColor = System.Drawing.Color.White;
            this.serviceTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serviceTextBlock.Location = new System.Drawing.Point(115, 130);
            this.serviceTextBlock.Name = "serviceTextBlock";
            this.serviceTextBlock.Padding = new System.Windows.Forms.Padding(2);
            this.serviceTextBlock.Size = new System.Drawing.Size(205, 55);
            this.serviceTextBlock.TabIndex = 8;
            // 
            // stateTextBlock
            // 
            this.stateTextBlock.BackColor = System.Drawing.Color.White;
            this.stateTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stateTextBlock.Location = new System.Drawing.Point(115, 255);
            this.stateTextBlock.Name = "stateTextBlock";
            this.stateTextBlock.Padding = new System.Windows.Forms.Padding(2);
            this.stateTextBlock.Size = new System.Drawing.Size(120, 20);
            this.stateTextBlock.TabIndex = 12;
            // 
            // requestTimeLabel
            // 
            this.requestTimeLabel.Location = new System.Drawing.Point(5, 55);
            this.requestTimeLabel.Name = "requestTimeLabel";
            this.requestTimeLabel.Size = new System.Drawing.Size(105, 20);
            this.requestTimeLabel.TabIndex = 0;
            this.requestTimeLabel.Text = "Время запроса";
            this.requestTimeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceTypeLabel
            // 
            this.serviceTypeLabel.Location = new System.Drawing.Point(5, 205);
            this.serviceTypeLabel.Name = "serviceTypeLabel";
            this.serviceTypeLabel.Size = new System.Drawing.Size(105, 20);
            this.serviceTypeLabel.TabIndex = 0;
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
            this.serviceTypeTextBlock.TabIndex = 10;
            // 
            // serviceStepLabel
            // 
            this.serviceStepLabel.Location = new System.Drawing.Point(5, 230);
            this.serviceStepLabel.Name = "serviceStepLabel";
            this.serviceStepLabel.Size = new System.Drawing.Size(105, 20);
            this.serviceStepLabel.TabIndex = 0;
            this.serviceStepLabel.Text = "Этап услуги";
            this.serviceStepLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // editPanel
            // 
            this.editPanel.Controls.Add(this.numberLabel);
            this.editPanel.Controls.Add(this.numberTextBlock);
            this.editPanel.Controls.Add(this.isPriorityCheckBox);
            this.editPanel.Controls.Add(this.requestDateLabel);
            this.editPanel.Controls.Add(this.requestDatePicker);
            this.editPanel.Controls.Add(this.requestTimeLabel);
            this.editPanel.Controls.Add(this.requestTimePicker);
            this.editPanel.Controls.Add(this.typeControl);
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
            this.editPanel.Controls.Add(this.saveButton);
            this.editPanel.Controls.Add(this.serviceStepLabel);
            this.editPanel.Controls.Add(this.serviceStepControl);
            this.editPanel.Controls.Add(this.stateLabel);
            this.editPanel.Controls.Add(this.stateTextBlock);
            this.editPanel.Controls.Add(this.operatorLabel);
            this.editPanel.Controls.Add(this.operatorControl);
            this.editPanel.Location = new System.Drawing.Point(0, 25);
            this.editPanel.Name = "editPanel";
            this.editPanel.Size = new System.Drawing.Size(330, 340);
            this.editPanel.TabIndex = 16;
            this.editPanel.EnabledChanged += new System.EventHandler(this.editPanel_EnabledChanged);
            // 
            // requestDatePicker
            // 
            this.requestDatePicker.Location = new System.Drawing.Point(115, 30);
            this.requestDatePicker.Name = "requestDatePicker";
            this.requestDatePicker.Size = new System.Drawing.Size(150, 20);
            this.requestDatePicker.TabIndex = 2;
            this.requestDatePicker.Leave += new System.EventHandler(this.requestDatePicker_Leave);
            // 
            // requestTimePicker
            // 
            this.requestTimePicker.Location = new System.Drawing.Point(115, 55);
            this.requestTimePicker.Name = "requestTimePicker";
            this.requestTimePicker.Size = new System.Drawing.Size(35, 20);
            this.requestTimePicker.TabIndex = 3;
            this.requestTimePicker.Value = System.TimeSpan.Parse("00:00:00");
            this.requestTimePicker.Leave += new System.EventHandler(this.requestTimePicker_Leave);
            // 
            // typeControl
            // 
            this.typeControl.Location = new System.Drawing.Point(155, 55);
            this.typeControl.Name = "typeControl";
            this.typeControl.Size = new System.Drawing.Size(115, 21);
            this.typeControl.TabIndex = 4;
            this.typeControl.Leave += new System.EventHandler(this.typeControl_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(240, 310);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 14;
            this.saveButton.Text = "Записать";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // serviceStepControl
            // 
            this.serviceStepControl.Location = new System.Drawing.Point(115, 230);
            this.serviceStepControl.Name = "serviceStepControl";
            this.serviceStepControl.Size = new System.Drawing.Size(205, 21);
            this.serviceStepControl.TabIndex = 11;
            this.serviceStepControl.UseResetButton = true;
            this.serviceStepControl.Leave += new System.EventHandler(this.serviceStepControl_Leave);
            // 
            // operatorControl
            // 
            this.operatorControl.Location = new System.Drawing.Point(115, 280);
            this.operatorControl.Name = "operatorControl";
            this.operatorControl.Size = new System.Drawing.Size(205, 21);
            this.operatorControl.TabIndex = 13;
            this.operatorControl.UseResetButton = true;
            this.operatorControl.Leave += new System.EventHandler(this.operatorsControl_Leave);
            // 
            // additionalServicesBindingSource
            // 
            this.additionalServicesBindingSource.DataSource = typeof(Queue.Services.DTO.ClientRequestAdditionalService);
            // 
            // clientRequestTabControl
            // 
            this.clientRequestTabControl.Controls.Add(this.tabPage1);
            this.clientRequestTabControl.Controls.Add(this.tabPage2);
            this.clientRequestTabControl.Controls.Add(this.tabPage3);
            this.clientRequestTabControl.Location = new System.Drawing.Point(330, 25);
            this.clientRequestTabControl.Name = "clientRequestTabControl";
            this.clientRequestTabControl.Padding = new System.Drawing.Point(5, 5);
            this.clientRequestTabControl.SelectedIndex = 0;
            this.clientRequestTabControl.Size = new System.Drawing.Size(550, 335);
            this.clientRequestTabControl.TabIndex = 17;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.eventsGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(542, 305);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "События";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.parametersGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(537, 305);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Параметры";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.additionalServicesGridView);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(537, 305);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Дополнительные услуги";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // additionalServicesGridView
            // 
            this.additionalServicesGridView.AllowUserToAddRows = false;
            this.additionalServicesGridView.AllowUserToDeleteRows = false;
            this.additionalServicesGridView.AllowUserToResizeRows = false;
            this.additionalServicesGridView.AutoGenerateColumns = false;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.additionalServicesGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.additionalServicesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.additionalServicesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.additionalServiceDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.Sum});
            this.additionalServicesGridView.DataSource = this.additionalServicesBindingSource;
            this.additionalServicesGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.additionalServicesGridView.Location = new System.Drawing.Point(0, 0);
            this.additionalServicesGridView.MultiSelect = false;
            this.additionalServicesGridView.Name = "additionalServicesGridView";
            this.additionalServicesGridView.ReadOnly = true;
            this.additionalServicesGridView.RowHeadersVisible = false;
            this.additionalServicesGridView.Size = new System.Drawing.Size(537, 305);
            this.additionalServicesGridView.TabIndex = 1;
            // 
            // additionalServiceDataGridViewTextBoxColumn
            // 
            this.additionalServiceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.additionalServiceDataGridViewTextBoxColumn.DataPropertyName = "AdditionalService";
            this.additionalServiceDataGridViewTextBoxColumn.HeaderText = "Дополнительная услуга";
            this.additionalServiceDataGridViewTextBoxColumn.Name = "additionalServiceDataGridViewTextBoxColumn";
            this.additionalServiceDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.quantityDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.quantityDataGridViewTextBoxColumn.FillWeight = 70F;
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Кол-во";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            this.quantityDataGridViewTextBoxColumn.Width = 70;
            // 
            // Sum
            // 
            this.Sum.DataPropertyName = "Sum";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "C2";
            dataGridViewCellStyle5.NullValue = null;
            this.Sum.DefaultCellStyle = dataGridViewCellStyle5;
            this.Sum.HeaderText = "Сумма";
            this.Sum.Name = "Sum";
            this.Sum.ReadOnly = true;
            // 
            // EditClientRequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 366);
            this.Controls.Add(this.clientRequestTabControl);
            this.Controls.Add(this.topMenu);
            this.Controls.Add(this.editPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(900, 400);
            this.Name = "EditClientRequestForm";
            this.Text = "Запрос клиента";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditClientRequestForm_FormClosing);
            this.Load += new System.EventHandler(this.EditClientRequestForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.parametersGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parametersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventsGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eventsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).EndInit();
            this.topMenu.ResumeLayout(false);
            this.topMenu.PerformLayout();
            this.editPanel.ResumeLayout(false);
            this.editPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesBindingSource)).EndInit();
            this.clientRequestTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label stateLabel;
        private System.Windows.Forms.Label numberLabel;
        private System.Windows.Forms.DataGridView parametersGridView;
        private System.Windows.Forms.Label requestDateLabel;
        private System.Windows.Forms.Label clientLabel;
        private System.Windows.Forms.Label serviceLabel;
        private System.Windows.Forms.DataGridView eventsGridView;
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
        private System.Windows.Forms.Label clientTextBlock;
        private System.Windows.Forms.Label serviceTextBlock;
        private System.Windows.Forms.Label stateTextBlock;
        private System.Windows.Forms.Label requestTimeLabel;
        private System.Windows.Forms.Label serviceTypeLabel;
        private System.Windows.Forms.Label serviceTypeTextBlock;
        private System.Windows.Forms.Label serviceStepLabel;
        private System.Windows.Forms.Panel editPanel;
        private UI.WinForms.EnumItemControl typeControl;
        private UI.WinForms.IdentifiedEntityControl operatorControl;
        private UI.WinForms.IdentifiedEntityControl serviceStepControl;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.DateTimePicker requestDatePicker;
        private UI.WinForms.TimePicker requestTimePicker;
        private System.Windows.Forms.BindingSource eventsBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn createDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn messageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource parametersBindingSource;
        private System.Windows.Forms.BindingSource additionalServicesBindingSource;
        private System.Windows.Forms.TabControl clientRequestTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.DataGridView additionalServicesGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn additionalServiceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sum;
    }
}