namespace Queue.Administrator
{
    partial class ClientRequestsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientRequestsForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.requestDateCheckBox = new System.Windows.Forms.CheckBox();
            this.requestDatePanel = new System.Windows.Forms.GroupBox();
            this.requestDateLabel = new System.Windows.Forms.Label();
            this.requestDatePicker = new System.Windows.Forms.DateTimePicker();
            this.serviceCheckBox = new System.Windows.Forms.CheckBox();
            this.servicePanel = new System.Windows.Forms.GroupBox();
            this.serviceLabel = new System.Windows.Forms.Label();
            this.serviceControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.operatorCheckBox = new System.Windows.Forms.CheckBox();
            this.operatorPanel = new System.Windows.Forms.GroupBox();
            this.operatorLabel = new System.Windows.Forms.Label();
            this.operatorControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.queryLabel = new System.Windows.Forms.Label();
            this.queryTextBox = new System.Windows.Forms.TextBox();
            this.stateCheckBox = new System.Windows.Forms.CheckBox();
            this.statePanel = new System.Windows.Forms.GroupBox();
            this.stateLabel = new System.Windows.Forms.Label();
            this.stateControl = new Queue.UI.WinForms.EnumItemControl();
            this.findButton = new System.Windows.Forms.Button();
            this.detailsCheckBox = new System.Windows.Forms.CheckBox();
            this.firstButton = new System.Windows.Forms.Button();
            this.prevButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.clientRequestsGridView = new System.Windows.Forms.DataGridView();
            this.numberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestDateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.requestTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waitingStartTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.callingStartTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.waitingTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.сallingFinishTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.callingTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.renderStartTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.renderFinishTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.renderTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.productivityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subjectsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operatorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serviceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mainTableLayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.requestDatePanel.SuspendLayout();
            this.servicePanel.SuspendLayout();
            this.operatorPanel.SuspendLayout();
            this.statePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientRequestsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.panel1, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.clientRequestsGridView, 0, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(10, 10);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(1014, 342);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.requestDateCheckBox);
            this.panel1.Controls.Add(this.requestDatePanel);
            this.panel1.Controls.Add(this.serviceCheckBox);
            this.panel1.Controls.Add(this.servicePanel);
            this.panel1.Controls.Add(this.operatorCheckBox);
            this.panel1.Controls.Add(this.operatorPanel);
            this.panel1.Controls.Add(this.queryLabel);
            this.panel1.Controls.Add(this.queryTextBox);
            this.panel1.Controls.Add(this.stateCheckBox);
            this.panel1.Controls.Add(this.statePanel);
            this.panel1.Controls.Add(this.findButton);
            this.panel1.Controls.Add(this.detailsCheckBox);
            this.panel1.Controls.Add(this.firstButton);
            this.panel1.Controls.Add(this.prevButton);
            this.panel1.Controls.Add(this.nextButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1014, 120);
            this.panel1.TabIndex = 0;
            // 
            // requestDateCheckBox
            // 
            this.requestDateCheckBox.AutoSize = true;
            this.requestDateCheckBox.Checked = true;
            this.requestDateCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.requestDateCheckBox.Location = new System.Drawing.Point(5, 25);
            this.requestDateCheckBox.Name = "requestDateCheckBox";
            this.requestDateCheckBox.Size = new System.Drawing.Size(15, 14);
            this.requestDateCheckBox.TabIndex = 0;
            this.requestDateCheckBox.UseVisualStyleBackColor = true;
            this.requestDateCheckBox.CheckedChanged += new System.EventHandler(this.requestDateCheckBox_CheckedChanged);
            // 
            // requestDatePanel
            // 
            this.requestDatePanel.Controls.Add(this.requestDateLabel);
            this.requestDatePanel.Controls.Add(this.requestDatePicker);
            this.requestDatePanel.Location = new System.Drawing.Point(25, 5);
            this.requestDatePanel.Name = "requestDatePanel";
            this.requestDatePanel.Size = new System.Drawing.Size(200, 45);
            this.requestDatePanel.TabIndex = 0;
            this.requestDatePanel.TabStop = false;
            // 
            // requestDateLabel
            // 
            this.requestDateLabel.Location = new System.Drawing.Point(5, 15);
            this.requestDateLabel.Name = "requestDateLabel";
            this.requestDateLabel.Size = new System.Drawing.Size(80, 18);
            this.requestDateLabel.TabIndex = 0;
            this.requestDateLabel.Text = "Дата запроса";
            this.requestDateLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // requestDatePicker
            // 
            this.requestDatePicker.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.requestDatePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.requestDatePicker.Location = new System.Drawing.Point(90, 15);
            this.requestDatePicker.Name = "requestDatePicker";
            this.requestDatePicker.Size = new System.Drawing.Size(100, 20);
            this.requestDatePicker.TabIndex = 0;
            this.requestDatePicker.ValueChanged += new System.EventHandler(this.requestDatePicker_ValueChanged);
            // 
            // serviceCheckBox
            // 
            this.serviceCheckBox.AutoSize = true;
            this.serviceCheckBox.Location = new System.Drawing.Point(230, 25);
            this.serviceCheckBox.Name = "serviceCheckBox";
            this.serviceCheckBox.Size = new System.Drawing.Size(15, 14);
            this.serviceCheckBox.TabIndex = 1;
            this.serviceCheckBox.UseVisualStyleBackColor = true;
            this.serviceCheckBox.CheckedChanged += new System.EventHandler(this.serviceCheckBox_CheckedChanged);
            // 
            // servicePanel
            // 
            this.servicePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.servicePanel.Controls.Add(this.serviceLabel);
            this.servicePanel.Controls.Add(this.serviceControl);
            this.servicePanel.Enabled = false;
            this.servicePanel.Location = new System.Drawing.Point(250, 5);
            this.servicePanel.Name = "servicePanel";
            this.servicePanel.Size = new System.Drawing.Size(510, 45);
            this.servicePanel.TabIndex = 2;
            this.servicePanel.TabStop = false;
            // 
            // serviceLabel
            // 
            this.serviceLabel.Location = new System.Drawing.Point(5, 20);
            this.serviceLabel.Name = "serviceLabel";
            this.serviceLabel.Size = new System.Drawing.Size(45, 13);
            this.serviceLabel.TabIndex = 0;
            this.serviceLabel.Text = "Услуга";
            this.serviceLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceControl
            // 
            this.serviceControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.serviceControl.Location = new System.Drawing.Point(50, 15);
            this.serviceControl.Name = "serviceControl";
            this.serviceControl.Size = new System.Drawing.Size(455, 21);
            this.serviceControl.TabIndex = 2;
            this.serviceControl.UseResetButton = false;
            this.serviceControl.Leave += new System.EventHandler(this.serviceControl_Leave);
            // 
            // operatorCheckBox
            // 
            this.operatorCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.operatorCheckBox.AutoSize = true;
            this.operatorCheckBox.Location = new System.Drawing.Point(765, 25);
            this.operatorCheckBox.Name = "operatorCheckBox";
            this.operatorCheckBox.Size = new System.Drawing.Size(15, 14);
            this.operatorCheckBox.TabIndex = 3;
            this.operatorCheckBox.UseVisualStyleBackColor = true;
            this.operatorCheckBox.CheckedChanged += new System.EventHandler(this.operatorCheckBox_CheckedChanged);
            // 
            // operatorPanel
            // 
            this.operatorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.operatorPanel.Controls.Add(this.operatorLabel);
            this.operatorPanel.Controls.Add(this.operatorControl);
            this.operatorPanel.Enabled = false;
            this.operatorPanel.Location = new System.Drawing.Point(785, 5);
            this.operatorPanel.Name = "operatorPanel";
            this.operatorPanel.Size = new System.Drawing.Size(220, 45);
            this.operatorPanel.TabIndex = 2;
            this.operatorPanel.TabStop = false;
            // 
            // operatorLabel
            // 
            this.operatorLabel.Location = new System.Drawing.Point(5, 20);
            this.operatorLabel.Name = "operatorLabel";
            this.operatorLabel.Size = new System.Drawing.Size(60, 13);
            this.operatorLabel.TabIndex = 0;
            this.operatorLabel.Text = "Оператор";
            this.operatorLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // operatorControl
            // 
            this.operatorControl.Location = new System.Drawing.Point(65, 15);
            this.operatorControl.Name = "operatorControl";
            this.operatorControl.Size = new System.Drawing.Size(150, 21);
            this.operatorControl.TabIndex = 3;
            this.operatorControl.UseResetButton = false;
            this.operatorControl.Leave += new System.EventHandler(this.operatorControl_Leave);
            // 
            // queryLabel
            // 
            this.queryLabel.Location = new System.Drawing.Point(10, 80);
            this.queryLabel.Name = "queryLabel";
            this.queryLabel.Size = new System.Drawing.Size(50, 13);
            this.queryLabel.TabIndex = 0;
            this.queryLabel.Text = "Запрос:";
            this.queryLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // queryTextBox
            // 
            this.queryTextBox.Location = new System.Drawing.Point(65, 75);
            this.queryTextBox.Name = "queryTextBox";
            this.queryTextBox.Size = new System.Drawing.Size(155, 20);
            this.queryTextBox.TabIndex = 4;
            this.queryTextBox.Leave += new System.EventHandler(this.queryTextBox_Leave);
            // 
            // stateCheckBox
            // 
            this.stateCheckBox.AutoSize = true;
            this.stateCheckBox.Location = new System.Drawing.Point(230, 75);
            this.stateCheckBox.Name = "stateCheckBox";
            this.stateCheckBox.Size = new System.Drawing.Size(15, 14);
            this.stateCheckBox.TabIndex = 5;
            this.stateCheckBox.UseVisualStyleBackColor = true;
            this.stateCheckBox.CheckedChanged += new System.EventHandler(this.stateCheckBox_CheckedChanged);
            // 
            // statePanel
            // 
            this.statePanel.Controls.Add(this.stateLabel);
            this.statePanel.Controls.Add(this.stateControl);
            this.statePanel.Enabled = false;
            this.statePanel.Location = new System.Drawing.Point(250, 55);
            this.statePanel.Name = "statePanel";
            this.statePanel.Size = new System.Drawing.Size(220, 45);
            this.statePanel.TabIndex = 6;
            this.statePanel.TabStop = false;
            // 
            // stateLabel
            // 
            this.stateLabel.Location = new System.Drawing.Point(5, 20);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(65, 15);
            this.stateLabel.TabIndex = 0;
            this.stateLabel.Text = "Состояние";
            this.stateLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // stateControl
            // 
            this.stateControl.Location = new System.Drawing.Point(75, 15);
            this.stateControl.Name = "stateControl";
            this.stateControl.Size = new System.Drawing.Size(140, 21);
            this.stateControl.TabIndex = 6;
            this.stateControl.Leave += new System.EventHandler(this.stateControl_Leave);
            // 
            // findButton
            // 
            this.findButton.Location = new System.Drawing.Point(480, 70);
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(75, 25);
            this.findButton.TabIndex = 7;
            this.findButton.Text = "Найти";
            this.findButton.UseVisualStyleBackColor = true;
            this.findButton.Click += new System.EventHandler(this.findButton_Click);
            // 
            // detailsCheckBox
            // 
            this.detailsCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.detailsCheckBox.AutoSize = true;
            this.detailsCheckBox.Location = new System.Drawing.Point(850, 80);
            this.detailsCheckBox.Name = "detailsCheckBox";
            this.detailsCheckBox.Size = new System.Drawing.Size(76, 17);
            this.detailsCheckBox.TabIndex = 8;
            this.detailsCheckBox.Text = "Подробно";
            this.detailsCheckBox.UseVisualStyleBackColor = true;
            this.detailsCheckBox.CheckedChanged += new System.EventHandler(this.detailsCheckBox_CheckedChanged);
            // 
            // firstButton
            // 
            this.firstButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.firstButton.Image = ((System.Drawing.Image)(resources.GetObject("firstButton.Image")));
            this.firstButton.Location = new System.Drawing.Point(935, 80);
            this.firstButton.Name = "firstButton";
            this.firstButton.Size = new System.Drawing.Size(25, 20);
            this.firstButton.TabIndex = 9;
            this.firstButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // prevButton
            // 
            this.prevButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.prevButton.Image = ((System.Drawing.Image)(resources.GetObject("prevButton.Image")));
            this.prevButton.Location = new System.Drawing.Point(960, 80);
            this.prevButton.Name = "prevButton";
            this.prevButton.Size = new System.Drawing.Size(25, 20);
            this.prevButton.TabIndex = 10;
            this.prevButton.Click += new System.EventHandler(this.prevButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.nextButton.Image = ((System.Drawing.Image)(resources.GetObject("nextButton.Image")));
            this.nextButton.Location = new System.Drawing.Point(985, 80);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(25, 20);
            this.nextButton.TabIndex = 11;
            this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // clientRequestsGridView
            // 
            this.clientRequestsGridView.AllowUserToAddRows = false;
            this.clientRequestsGridView.AllowUserToDeleteRows = false;
            this.clientRequestsGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.clientRequestsGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.clientRequestsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.clientRequestsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numberColumn,
            this.requestDateColumn,
            this.requestTimeColumn,
            this.waitingStartTimeColumn,
            this.callingStartTimeColumn,
            this.waitingTimeColumn,
            this.сallingFinishTimeColumn,
            this.callingTimeColumn,
            this.renderStartTimeColumn,
            this.renderFinishTimeColumn,
            this.renderTimeColumn,
            this.productivityColumn,
            this.subjectsColumn,
            this.clientColumn,
            this.operatorColumn,
            this.serviceColumn,
            this.stateColumn});
            this.clientRequestsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientRequestsGridView.Location = new System.Drawing.Point(0, 120);
            this.clientRequestsGridView.Margin = new System.Windows.Forms.Padding(0);
            this.clientRequestsGridView.MultiSelect = false;
            this.clientRequestsGridView.Name = "clientRequestsGridView";
            this.clientRequestsGridView.ReadOnly = true;
            this.clientRequestsGridView.RowHeadersVisible = false;
            this.clientRequestsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.clientRequestsGridView.Size = new System.Drawing.Size(1014, 222);
            this.clientRequestsGridView.TabIndex = 0;
            this.clientRequestsGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.clientRequestsGridView_CellMouseDoubleClick);
            // 
            // numberColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.numberColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.numberColumn.HeaderText = "Номер";
            this.numberColumn.Name = "numberColumn";
            this.numberColumn.ReadOnly = true;
            this.numberColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.numberColumn.Width = 80;
            // 
            // requestDateColumn
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.requestDateColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.requestDateColumn.HeaderText = "Дата";
            this.requestDateColumn.Name = "requestDateColumn";
            this.requestDateColumn.ReadOnly = true;
            this.requestDateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.requestDateColumn.Width = 80;
            // 
            // requestTimeColumn
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.requestTimeColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.requestTimeColumn.HeaderText = "Время";
            this.requestTimeColumn.Name = "requestTimeColumn";
            this.requestTimeColumn.ReadOnly = true;
            this.requestTimeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.requestTimeColumn.Width = 85;
            // 
            // waitingStartTimeColumn
            // 
            this.waitingStartTimeColumn.HeaderText = "Начало ожидания";
            this.waitingStartTimeColumn.Name = "waitingStartTimeColumn";
            this.waitingStartTimeColumn.ReadOnly = true;
            this.waitingStartTimeColumn.Visible = false;
            // 
            // callingStartTimeColumn
            // 
            this.callingStartTimeColumn.HeaderText = "Начало вызова";
            this.callingStartTimeColumn.Name = "callingStartTimeColumn";
            this.callingStartTimeColumn.ReadOnly = true;
            this.callingStartTimeColumn.Visible = false;
            this.callingStartTimeColumn.Width = 80;
            // 
            // waitingTimeColumn
            // 
            this.waitingTimeColumn.HeaderText = "Время ожидания";
            this.waitingTimeColumn.Name = "waitingTimeColumn";
            this.waitingTimeColumn.ReadOnly = true;
            this.waitingTimeColumn.Visible = false;
            // 
            // сallingFinishTimeColumn
            // 
            this.сallingFinishTimeColumn.HeaderText = "Окончание вызова";
            this.сallingFinishTimeColumn.Name = "сallingFinishTimeColumn";
            this.сallingFinishTimeColumn.ReadOnly = true;
            this.сallingFinishTimeColumn.Visible = false;
            this.сallingFinishTimeColumn.Width = 80;
            // 
            // callingTimeColumn
            // 
            this.callingTimeColumn.HeaderText = "Время вызова";
            this.callingTimeColumn.Name = "callingTimeColumn";
            this.callingTimeColumn.ReadOnly = true;
            this.callingTimeColumn.Visible = false;
            // 
            // renderStartTimeColumn
            // 
            this.renderStartTimeColumn.HeaderText = "Начало обслуживания";
            this.renderStartTimeColumn.Name = "renderStartTimeColumn";
            this.renderStartTimeColumn.ReadOnly = true;
            this.renderStartTimeColumn.Visible = false;
            // 
            // renderFinishTimeColumn
            // 
            this.renderFinishTimeColumn.HeaderText = "Окончание обслуживания";
            this.renderFinishTimeColumn.Name = "renderFinishTimeColumn";
            this.renderFinishTimeColumn.ReadOnly = true;
            this.renderFinishTimeColumn.Visible = false;
            // 
            // renderTimeColumn
            // 
            this.renderTimeColumn.HeaderText = "Время обслуживания";
            this.renderTimeColumn.Name = "renderTimeColumn";
            this.renderTimeColumn.ReadOnly = true;
            this.renderTimeColumn.Visible = false;
            // 
            // productivityColumn
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle5.Format = "0";
            this.productivityColumn.DefaultCellStyle = dataGridViewCellStyle5;
            this.productivityColumn.HeaderText = "Производительность";
            this.productivityColumn.Name = "productivityColumn";
            this.productivityColumn.ReadOnly = true;
            this.productivityColumn.Visible = false;
            this.productivityColumn.Width = 150;
            // 
            // subjectsColumn
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.subjectsColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.subjectsColumn.HeaderText = "Объектов";
            this.subjectsColumn.Name = "subjectsColumn";
            this.subjectsColumn.ReadOnly = true;
            this.subjectsColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // clientColumn
            // 
            this.clientColumn.HeaderText = "Клиент";
            this.clientColumn.Name = "clientColumn";
            this.clientColumn.ReadOnly = true;
            this.clientColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clientColumn.Width = 120;
            // 
            // operatorColumn
            // 
            this.operatorColumn.HeaderText = "Оператор";
            this.operatorColumn.Name = "operatorColumn";
            this.operatorColumn.ReadOnly = true;
            this.operatorColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // serviceColumn
            // 
            this.serviceColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.serviceColumn.HeaderText = "Выбранная услуга";
            this.serviceColumn.MinimumWidth = 150;
            this.serviceColumn.Name = "serviceColumn";
            this.serviceColumn.ReadOnly = true;
            this.serviceColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // stateColumn
            // 
            this.stateColumn.HeaderText = "Состояние";
            this.stateColumn.Name = "stateColumn";
            this.stateColumn.ReadOnly = true;
            this.stateColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ClientRequestsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 362);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(1050, 400);
            this.Name = "ClientRequestsForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Запросы";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientRequestsForm_FormClosing);
            this.Load += new System.EventHandler(this.ClientRequestsForm_Load);
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.requestDatePanel.ResumeLayout(false);
            this.servicePanel.ResumeLayout(false);
            this.operatorPanel.ResumeLayout(false);
            this.statePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clientRequestsGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.DataGridView clientRequestsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestDateColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn requestTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waitingStartTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn callingStartTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn waitingTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn сallingFinishTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn callingTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn renderStartTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn renderFinishTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn renderTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn productivityColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subjectsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn operatorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serviceColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stateColumn;
        private System.Windows.Forms.Panel panel1;
        private UI.WinForms.IdentifiedEntityControl operatorControl;
        private UI.WinForms.IdentifiedEntityControl serviceControl;
        private System.Windows.Forms.CheckBox requestDateCheckBox;
        private System.Windows.Forms.GroupBox requestDatePanel;
        private System.Windows.Forms.Label requestDateLabel;
        private System.Windows.Forms.DateTimePicker requestDatePicker;
        private System.Windows.Forms.CheckBox serviceCheckBox;
        private System.Windows.Forms.GroupBox servicePanel;
        private System.Windows.Forms.Label serviceLabel;
        private System.Windows.Forms.CheckBox operatorCheckBox;
        private System.Windows.Forms.GroupBox operatorPanel;
        private System.Windows.Forms.Label operatorLabel;
        private System.Windows.Forms.GroupBox statePanel;
        private System.Windows.Forms.Label stateLabel;
        private UI.WinForms.EnumItemControl stateControl;
        private System.Windows.Forms.CheckBox detailsCheckBox;
        private System.Windows.Forms.CheckBox stateCheckBox;
        private System.Windows.Forms.TextBox queryTextBox;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button prevButton;
        private System.Windows.Forms.Button firstButton;
        private System.Windows.Forms.Label queryLabel;
        private System.Windows.Forms.Button findButton;
    }
}