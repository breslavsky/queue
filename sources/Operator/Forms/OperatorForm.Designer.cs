using Queue.UI.WinForms;
namespace Queue.Operator
{
    partial class OperatorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OperatorForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.serverStateLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.qualityStateLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.currentDateTimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.separator1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.standingTextLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.standingLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.separator2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.versionLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ratingLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.settingsButton = new System.Windows.Forms.ToolStripSplitButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.logoutButton = new System.Windows.Forms.Button();
            this.mainTabControl = new System.Windows.Forms.TabControl();
            this.currentClientRequestTab = new System.Windows.Forms.TabPage();
            this.actionsMenu = new System.Windows.Forms.MenuStrip();
            this.additionalMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.callClientByRequestNumberMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redirectToOperatorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.numberLabel = new System.Windows.Forms.Label();
            this.numberTextBlock = new System.Windows.Forms.Label();
            this.isPriorityCheckBox = new System.Windows.Forms.CheckBox();
            this.requestTimeLabel = new System.Windows.Forms.Label();
            this.requestTimeTextBlock = new System.Windows.Forms.Label();
            this.typeTextBlock = new System.Windows.Forms.Label();
            this.subjectsLabel = new System.Windows.Forms.Label();
            this.subjectsPanel = new System.Windows.Forms.Panel();
            this.subjectsChangeLink = new System.Windows.Forms.LinkLabel();
            this.subjectsUpDown = new System.Windows.Forms.NumericUpDown();
            this.clientLabel = new System.Windows.Forms.Label();
            this.clientTextBlock = new System.Windows.Forms.Label();
            this.serviceLabel = new System.Windows.Forms.Label();
            this.serviceTextBlock = new System.Windows.Forms.Label();
            this.serviceChangeLink = new System.Windows.Forms.LinkLabel();
            this.servceTypeLabel = new System.Windows.Forms.Label();
            this.serviceTypeControl = new Queue.UI.WinForms.EnumItemControl();
            this.serviceStepLabel = new System.Windows.Forms.Label();
            this.serviceStepControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.stateLabel = new System.Windows.Forms.Label();
            this.stateTextBlock = new System.Windows.Forms.Label();
            this.commentLabel = new System.Windows.Forms.Label();
            this.commentTextBox = new System.Windows.Forms.TextBox();
            this.commentSaveLink = new System.Windows.Forms.LinkLabel();
            this.digitalTimer = new Queue.UI.WinForms.DigitalTimer();
            this.clientRequestTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.parametersGridView = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parametersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.addAdditionalServiceButton = new System.Windows.Forms.Button();
            this.additionalServicesGridView = new System.Windows.Forms.DataGridView();
            this.additionalServiceDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.quantityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Sum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.additionalServicesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.stepPanel = new System.Windows.Forms.Panel();
            this.step1Panel = new System.Windows.Forms.Panel();
            this.callClientButton = new System.Windows.Forms.Button();
            this.step2Panel = new System.Windows.Forms.Panel();
            this.recallingButton = new System.Windows.Forms.Button();
            this.renderingButton = new System.Windows.Forms.Button();
            this.absenceButton = new System.Windows.Forms.Button();
            this.step3Panel = new System.Windows.Forms.Panel();
            this.renderedButton = new System.Windows.Forms.Button();
            this.returnButton = new System.Windows.Forms.Button();
            this.postponeGroupBox = new System.Windows.Forms.GroupBox();
            this.postponeButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.postponeMinutesUpDown = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.clientRequestsTab = new System.Windows.Forms.TabPage();
            this.clientRequestsGridView = new System.Windows.Forms.DataGridView();
            this.numberColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subjectsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.timeIntervalColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clientColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serviceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.stateColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusBar.SuspendLayout();
            this.panel1.SuspendLayout();
            this.mainTabControl.SuspendLayout();
            this.currentClientRequestTab.SuspendLayout();
            this.actionsMenu.SuspendLayout();
            this.subjectsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).BeginInit();
            this.clientRequestTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parametersGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parametersBindingSource)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesBindingSource)).BeginInit();
            this.stepPanel.SuspendLayout();
            this.step1Panel.SuspendLayout();
            this.step2Panel.SuspendLayout();
            this.step3Panel.SuspendLayout();
            this.postponeGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.postponeMinutesUpDown)).BeginInit();
            this.clientRequestsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientRequestsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverStateLabel,
            this.qualityStateLabel,
            this.currentDateTimeLabel,
            this.separator1,
            this.standingTextLabel,
            this.standingLabel,
            this.separator2,
            this.versionLabel,
            this.ratingLabel,
            this.settingsButton});
            this.statusBar.Location = new System.Drawing.Point(0, 550);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(524, 22);
            this.statusBar.SizingGrip = false;
            this.statusBar.TabIndex = 0;
            this.statusBar.Text = "statusBar";
            // 
            // serverStateLabel
            // 
            this.serverStateLabel.Name = "serverStateLabel";
            this.serverStateLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // qualityStateLabel
            // 
            this.qualityStateLabel.Name = "qualityStateLabel";
            this.qualityStateLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // currentDateTimeLabel
            // 
            this.currentDateTimeLabel.Name = "currentDateTimeLabel";
            this.currentDateTimeLabel.Size = new System.Drawing.Size(34, 17);
            this.currentDateTimeLabel.Text = "00:00";
            // 
            // separator1
            // 
            this.separator1.Name = "separator1";
            this.separator1.Size = new System.Drawing.Size(10, 17);
            this.separator1.Text = "|";
            // 
            // standingTextLabel
            // 
            this.standingTextLabel.Name = "standingTextLabel";
            this.standingTextLabel.Size = new System.Drawing.Size(62, 17);
            this.standingTextLabel.Text = "в очереди";
            // 
            // standingLabel
            // 
            this.standingLabel.Name = "standingLabel";
            this.standingLabel.Size = new System.Drawing.Size(13, 17);
            this.standingLabel.Text = "0";
            // 
            // separator2
            // 
            this.separator2.Name = "separator2";
            this.separator2.Size = new System.Drawing.Size(10, 17);
            this.separator2.Text = "|";
            // 
            // versionLabel
            // 
            this.versionLabel.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(12, 17);
            this.versionLabel.Text = "-";
            // 
            // ratingLabel
            // 
            this.ratingLabel.ForeColor = System.Drawing.SystemColors.ButtonShadow;
            this.ratingLabel.Name = "ratingLabel";
            this.ratingLabel.Size = new System.Drawing.Size(12, 17);
            this.ratingLabel.Text = "-";
            // 
            // settingsButton
            // 
            this.settingsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.settingsButton.DropDownButtonWidth = 0;
            this.settingsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.settingsButton.Name = "settingsButton";
            this.settingsButton.Size = new System.Drawing.Size(72, 20);
            this.settingsButton.Text = "Настройки";
            this.settingsButton.ButtonClick += new System.EventHandler(this.settingsButton_ButtonClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.logoutButton);
            this.panel1.Controls.Add(this.mainTabControl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10, 30, 10, 10);
            this.panel1.Size = new System.Drawing.Size(524, 550);
            this.panel1.TabIndex = 0;
            // 
            // logoutButton
            // 
            this.logoutButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.logoutButton.Image = ((System.Drawing.Image)(resources.GetObject("logoutButton.Image")));
            this.logoutButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.logoutButton.Location = new System.Drawing.Point(425, 5);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(85, 25);
            this.logoutButton.TabIndex = 3;
            this.logoutButton.Text = "Выход";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // mainTabControl
            // 
            this.mainTabControl.Controls.Add(this.currentClientRequestTab);
            this.mainTabControl.Controls.Add(this.clientRequestsTab);
            this.mainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabControl.Location = new System.Drawing.Point(10, 30);
            this.mainTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.mainTabControl.Name = "mainTabControl";
            this.mainTabControl.Padding = new System.Drawing.Point(5, 5);
            this.mainTabControl.SelectedIndex = 0;
            this.mainTabControl.Size = new System.Drawing.Size(504, 510);
            this.mainTabControl.TabIndex = 1;
            // 
            // currentClientRequestTab
            // 
            this.currentClientRequestTab.Controls.Add(this.actionsMenu);
            this.currentClientRequestTab.Controls.Add(this.numberLabel);
            this.currentClientRequestTab.Controls.Add(this.numberTextBlock);
            this.currentClientRequestTab.Controls.Add(this.isPriorityCheckBox);
            this.currentClientRequestTab.Controls.Add(this.requestTimeLabel);
            this.currentClientRequestTab.Controls.Add(this.requestTimeTextBlock);
            this.currentClientRequestTab.Controls.Add(this.typeTextBlock);
            this.currentClientRequestTab.Controls.Add(this.subjectsLabel);
            this.currentClientRequestTab.Controls.Add(this.subjectsPanel);
            this.currentClientRequestTab.Controls.Add(this.clientLabel);
            this.currentClientRequestTab.Controls.Add(this.clientTextBlock);
            this.currentClientRequestTab.Controls.Add(this.serviceLabel);
            this.currentClientRequestTab.Controls.Add(this.serviceTextBlock);
            this.currentClientRequestTab.Controls.Add(this.serviceChangeLink);
            this.currentClientRequestTab.Controls.Add(this.servceTypeLabel);
            this.currentClientRequestTab.Controls.Add(this.serviceTypeControl);
            this.currentClientRequestTab.Controls.Add(this.serviceStepLabel);
            this.currentClientRequestTab.Controls.Add(this.serviceStepControl);
            this.currentClientRequestTab.Controls.Add(this.stateLabel);
            this.currentClientRequestTab.Controls.Add(this.stateTextBlock);
            this.currentClientRequestTab.Controls.Add(this.commentLabel);
            this.currentClientRequestTab.Controls.Add(this.commentTextBox);
            this.currentClientRequestTab.Controls.Add(this.commentSaveLink);
            this.currentClientRequestTab.Controls.Add(this.digitalTimer);
            this.currentClientRequestTab.Controls.Add(this.clientRequestTabControl);
            this.currentClientRequestTab.Controls.Add(this.stepPanel);
            this.currentClientRequestTab.Location = new System.Drawing.Point(4, 26);
            this.currentClientRequestTab.Name = "currentClientRequestTab";
            this.currentClientRequestTab.Size = new System.Drawing.Size(496, 480);
            this.currentClientRequestTab.TabIndex = 0;
            this.currentClientRequestTab.Text = "Текущий запрос клиента";
            this.currentClientRequestTab.UseVisualStyleBackColor = true;
            // 
            // actionsMenu
            // 
            this.actionsMenu.Dock = System.Windows.Forms.DockStyle.None;
            this.actionsMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.actionsMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.additionalMenu});
            this.actionsMenu.Location = new System.Drawing.Point(330, 5);
            this.actionsMenu.Name = "actionsMenu";
            this.actionsMenu.Size = new System.Drawing.Size(135, 24);
            this.actionsMenu.TabIndex = 15;
            this.actionsMenu.Text = "menuStrip1";
            // 
            // additionalMenu
            // 
            this.additionalMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.callClientByRequestNumberMenuItem,
            this.redirectToOperatorMenuItem});
            this.additionalMenu.Image = ((System.Drawing.Image)(resources.GetObject("additionalMenu.Image")));
            this.additionalMenu.Name = "additionalMenu";
            this.additionalMenu.Size = new System.Drawing.Size(123, 20);
            this.additionalMenu.Text = "Дополнительно";
            // 
            // callClientByRequestNumberMenuItem
            // 
            this.callClientByRequestNumberMenuItem.Name = "callClientByRequestNumberMenuItem";
            this.callClientByRequestNumberMenuItem.Size = new System.Drawing.Size(186, 22);
            this.callClientByRequestNumberMenuItem.Text = "Вызвать но номеру";
            this.callClientByRequestNumberMenuItem.Click += new System.EventHandler(this.callClientByRequestNumberMenuItem_Click);
            // 
            // redirectToOperatorMenuItem
            // 
            this.redirectToOperatorMenuItem.Name = "redirectToOperatorMenuItem";
            this.redirectToOperatorMenuItem.Size = new System.Drawing.Size(186, 22);
            this.redirectToOperatorMenuItem.Text = "Передать оператору";
            this.redirectToOperatorMenuItem.Click += new System.EventHandler(this.redirectToOperatorMenuItem_Click);
            // 
            // numberLabel
            // 
            this.numberLabel.Location = new System.Drawing.Point(5, 15);
            this.numberLabel.Name = "numberLabel";
            this.numberLabel.Size = new System.Drawing.Size(115, 20);
            this.numberLabel.TabIndex = 0;
            this.numberLabel.Text = "Номер";
            this.numberLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // numberTextBlock
            // 
            this.numberTextBlock.BackColor = System.Drawing.Color.White;
            this.numberTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numberTextBlock.Location = new System.Drawing.Point(120, 14);
            this.numberTextBlock.Name = "numberTextBlock";
            this.numberTextBlock.Padding = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.numberTextBlock.Size = new System.Drawing.Size(60, 20);
            this.numberTextBlock.TabIndex = 0;
            // 
            // isPriorityCheckBox
            // 
            this.isPriorityCheckBox.AutoSize = true;
            this.isPriorityCheckBox.Enabled = false;
            this.isPriorityCheckBox.Location = new System.Drawing.Point(185, 14);
            this.isPriorityCheckBox.Name = "isPriorityCheckBox";
            this.isPriorityCheckBox.Size = new System.Drawing.Size(121, 17);
            this.isPriorityCheckBox.TabIndex = 0;
            this.isPriorityCheckBox.Text = "Приоритет вызова";
            this.isPriorityCheckBox.UseVisualStyleBackColor = true;
            // 
            // requestTimeLabel
            // 
            this.requestTimeLabel.Location = new System.Drawing.Point(5, 45);
            this.requestTimeLabel.Name = "requestTimeLabel";
            this.requestTimeLabel.Size = new System.Drawing.Size(115, 20);
            this.requestTimeLabel.TabIndex = 0;
            this.requestTimeLabel.Text = "Время запроса";
            this.requestTimeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // requestTimeTextBlock
            // 
            this.requestTimeTextBlock.BackColor = System.Drawing.Color.White;
            this.requestTimeTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.requestTimeTextBlock.Location = new System.Drawing.Point(120, 45);
            this.requestTimeTextBlock.Name = "requestTimeTextBlock";
            this.requestTimeTextBlock.Padding = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.requestTimeTextBlock.Size = new System.Drawing.Size(60, 20);
            this.requestTimeTextBlock.TabIndex = 0;
            // 
            // typeTextBlock
            // 
            this.typeTextBlock.BackColor = System.Drawing.Color.White;
            this.typeTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.typeTextBlock.Location = new System.Drawing.Point(185, 45);
            this.typeTextBlock.Name = "typeTextBlock";
            this.typeTextBlock.Padding = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.typeTextBlock.Size = new System.Drawing.Size(135, 20);
            this.typeTextBlock.TabIndex = 0;
            // 
            // subjectsLabel
            // 
            this.subjectsLabel.Location = new System.Drawing.Point(5, 75);
            this.subjectsLabel.Name = "subjectsLabel";
            this.subjectsLabel.Size = new System.Drawing.Size(110, 20);
            this.subjectsLabel.TabIndex = 2;
            this.subjectsLabel.Text = "Объектов";
            this.subjectsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // subjectsPanel
            // 
            this.subjectsPanel.Controls.Add(this.subjectsChangeLink);
            this.subjectsPanel.Controls.Add(this.subjectsUpDown);
            this.subjectsPanel.Location = new System.Drawing.Point(115, 70);
            this.subjectsPanel.Name = "subjectsPanel";
            this.subjectsPanel.Size = new System.Drawing.Size(210, 30);
            this.subjectsPanel.TabIndex = 5;
            // 
            // subjectsChangeLink
            // 
            this.subjectsChangeLink.AutoSize = true;
            this.subjectsChangeLink.Location = new System.Drawing.Point(70, 10);
            this.subjectsChangeLink.Name = "subjectsChangeLink";
            this.subjectsChangeLink.Size = new System.Drawing.Size(62, 13);
            this.subjectsChangeLink.TabIndex = 16;
            this.subjectsChangeLink.TabStop = true;
            this.subjectsChangeLink.Text = "[изменить]";
            this.subjectsChangeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.subjectsChangeLink_LinkClicked);
            // 
            // subjectsUpDown
            // 
            this.subjectsUpDown.Location = new System.Drawing.Point(5, 5);
            this.subjectsUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.subjectsUpDown.Name = "subjectsUpDown";
            this.subjectsUpDown.Size = new System.Drawing.Size(60, 20);
            this.subjectsUpDown.TabIndex = 1;
            // 
            // clientLabel
            // 
            this.clientLabel.Location = new System.Drawing.Point(5, 105);
            this.clientLabel.Name = "clientLabel";
            this.clientLabel.Size = new System.Drawing.Size(115, 20);
            this.clientLabel.TabIndex = 0;
            this.clientLabel.Text = "Клиент";
            this.clientLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // clientTextBlock
            // 
            this.clientTextBlock.BackColor = System.Drawing.Color.White;
            this.clientTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.clientTextBlock.Location = new System.Drawing.Point(120, 105);
            this.clientTextBlock.Name = "clientTextBlock";
            this.clientTextBlock.Padding = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.clientTextBlock.Size = new System.Drawing.Size(200, 20);
            this.clientTextBlock.TabIndex = 0;
            // 
            // serviceLabel
            // 
            this.serviceLabel.Location = new System.Drawing.Point(5, 135);
            this.serviceLabel.Name = "serviceLabel";
            this.serviceLabel.Size = new System.Drawing.Size(115, 55);
            this.serviceLabel.TabIndex = 0;
            this.serviceLabel.Text = "Выбранная услуга";
            this.serviceLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceTextBlock
            // 
            this.serviceTextBlock.BackColor = System.Drawing.Color.White;
            this.serviceTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.serviceTextBlock.Location = new System.Drawing.Point(120, 135);
            this.serviceTextBlock.Name = "serviceTextBlock";
            this.serviceTextBlock.Padding = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.serviceTextBlock.Size = new System.Drawing.Size(200, 55);
            this.serviceTextBlock.TabIndex = 0;
            // 
            // serviceChangeLink
            // 
            this.serviceChangeLink.AutoSize = true;
            this.serviceChangeLink.Location = new System.Drawing.Point(260, 190);
            this.serviceChangeLink.Name = "serviceChangeLink";
            this.serviceChangeLink.Size = new System.Drawing.Size(62, 13);
            this.serviceChangeLink.TabIndex = 0;
            this.serviceChangeLink.TabStop = true;
            this.serviceChangeLink.Text = "[изменить]";
            this.serviceChangeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.serviceChangeLink_LinkClicked);
            // 
            // servceTypeLabel
            // 
            this.servceTypeLabel.Location = new System.Drawing.Point(5, 210);
            this.servceTypeLabel.Name = "servceTypeLabel";
            this.servceTypeLabel.Size = new System.Drawing.Size(115, 20);
            this.servceTypeLabel.TabIndex = 0;
            this.servceTypeLabel.Text = "Тип услуги";
            this.servceTypeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceTypeControl
            // 
            this.serviceTypeControl.Location = new System.Drawing.Point(120, 210);
            this.serviceTypeControl.Name = "serviceTypeControl";
            this.serviceTypeControl.Size = new System.Drawing.Size(200, 21);
            this.serviceTypeControl.TabIndex = 9;
            this.serviceTypeControl.SelectedChanged += new System.EventHandler<System.EventArgs>(this.serviceTypeControl_SelectedChanged);
            // 
            // serviceStepLabel
            // 
            this.serviceStepLabel.Location = new System.Drawing.Point(5, 240);
            this.serviceStepLabel.Name = "serviceStepLabel";
            this.serviceStepLabel.Size = new System.Drawing.Size(115, 20);
            this.serviceStepLabel.TabIndex = 6;
            this.serviceStepLabel.Text = "Этап услуги";
            this.serviceStepLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceStepControl
            // 
            this.serviceStepControl.Location = new System.Drawing.Point(120, 240);
            this.serviceStepControl.Name = "serviceStepControl";
            this.serviceStepControl.Size = new System.Drawing.Size(200, 21);
            this.serviceStepControl.TabIndex = 10;
            this.serviceStepControl.UseResetButton = false;
            this.serviceStepControl.SelectedChanged += new System.EventHandler<System.EventArgs>(this.serviceStepControl_SelectedChanged);
            // 
            // stateLabel
            // 
            this.stateLabel.Location = new System.Drawing.Point(5, 270);
            this.stateLabel.Name = "stateLabel";
            this.stateLabel.Size = new System.Drawing.Size(115, 20);
            this.stateLabel.TabIndex = 0;
            this.stateLabel.Text = "Текущее состояние";
            this.stateLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // stateTextBlock
            // 
            this.stateTextBlock.BackColor = System.Drawing.Color.White;
            this.stateTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.stateTextBlock.Location = new System.Drawing.Point(120, 270);
            this.stateTextBlock.Name = "stateTextBlock";
            this.stateTextBlock.Padding = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.stateTextBlock.Size = new System.Drawing.Size(135, 20);
            this.stateTextBlock.TabIndex = 0;
            // 
            // commentLabel
            // 
            this.commentLabel.Location = new System.Drawing.Point(5, 295);
            this.commentLabel.Name = "commentLabel";
            this.commentLabel.Size = new System.Drawing.Size(115, 40);
            this.commentLabel.TabIndex = 13;
            this.commentLabel.Text = "Комментарий";
            this.commentLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // commentTextBox
            // 
            this.commentTextBox.Location = new System.Drawing.Point(120, 295);
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.Size = new System.Drawing.Size(200, 40);
            this.commentTextBox.TabIndex = 12;
            // 
            // commentSaveLink
            // 
            this.commentSaveLink.AutoSize = true;
            this.commentSaveLink.Location = new System.Drawing.Point(260, 340);
            this.commentSaveLink.Name = "commentSaveLink";
            this.commentSaveLink.Size = new System.Drawing.Size(65, 13);
            this.commentSaveLink.TabIndex = 14;
            this.commentSaveLink.TabStop = true;
            this.commentSaveLink.Text = "[сохранить]";
            this.commentSaveLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.commentSaveLink_LinkClicked);
            // 
            // digitalTimer
            // 
            this.digitalTimer.Location = new System.Drawing.Point(260, 270);
            this.digitalTimer.Margin = new System.Windows.Forms.Padding(4);
            this.digitalTimer.Name = "digitalTimer";
            this.digitalTimer.Size = new System.Drawing.Size(60, 20);
            this.digitalTimer.TabIndex = 3;
            // 
            // clientRequestTabControl
            // 
            this.clientRequestTabControl.Controls.Add(this.tabPage1);
            this.clientRequestTabControl.Controls.Add(this.tabPage2);
            this.clientRequestTabControl.Location = new System.Drawing.Point(5, 355);
            this.clientRequestTabControl.Name = "clientRequestTabControl";
            this.clientRequestTabControl.Padding = new System.Drawing.Point(5, 5);
            this.clientRequestTabControl.SelectedIndex = 0;
            this.clientRequestTabControl.Size = new System.Drawing.Size(485, 120);
            this.clientRequestTabControl.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.parametersGridView);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(477, 90);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Параметры услуги";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            this.parametersGridView.Location = new System.Drawing.Point(3, 3);
            this.parametersGridView.MultiSelect = false;
            this.parametersGridView.Name = "parametersGridView";
            this.parametersGridView.ReadOnly = true;
            this.parametersGridView.RowHeadersVisible = false;
            this.parametersGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.parametersGridView.Size = new System.Drawing.Size(471, 84);
            this.parametersGridView.TabIndex = 0;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.FillWeight = 150F;
            this.nameDataGridViewTextBoxColumn.HeaderText = "Название";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.nameDataGridViewTextBoxColumn.Width = 150;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Значение";
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            this.valueDataGridViewTextBoxColumn.ReadOnly = true;
            this.valueDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // parametersBindingSource
            // 
            this.parametersBindingSource.DataSource = typeof(Queue.Services.DTO.ClientRequestParameter);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.addAdditionalServiceButton);
            this.tabPage2.Controls.Add(this.additionalServicesGridView);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(477, 90);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Дополнительные услуги";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // addAdditionalServiceButton
            // 
            this.addAdditionalServiceButton.Location = new System.Drawing.Point(5, 100);
            this.addAdditionalServiceButton.Name = "addAdditionalServiceButton";
            this.addAdditionalServiceButton.Size = new System.Drawing.Size(75, 25);
            this.addAdditionalServiceButton.TabIndex = 13;
            this.addAdditionalServiceButton.Text = "Добавить";
            this.addAdditionalServiceButton.UseVisualStyleBackColor = true;
            this.addAdditionalServiceButton.Click += new System.EventHandler(this.addAdditionalServiceButton_Click);
            // 
            // additionalServicesGridView
            // 
            this.additionalServicesGridView.AllowUserToAddRows = false;
            this.additionalServicesGridView.AllowUserToResizeRows = false;
            this.additionalServicesGridView.AutoGenerateColumns = false;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.additionalServicesGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.additionalServicesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.additionalServicesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.additionalServiceDataGridViewTextBoxColumn,
            this.quantityDataGridViewTextBoxColumn,
            this.Sum});
            this.additionalServicesGridView.DataSource = this.additionalServicesBindingSource;
            this.additionalServicesGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.additionalServicesGridView.Location = new System.Drawing.Point(3, 3);
            this.additionalServicesGridView.MultiSelect = false;
            this.additionalServicesGridView.Name = "additionalServicesGridView";
            this.additionalServicesGridView.ReadOnly = true;
            this.additionalServicesGridView.RowHeadersVisible = false;
            this.additionalServicesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.additionalServicesGridView.Size = new System.Drawing.Size(471, 84);
            this.additionalServicesGridView.TabIndex = 12;
            this.additionalServicesGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.additionalServicesGridView_CellMouseDoubleClick);
            this.additionalServicesGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.additionalServicesGridView_UserDeletingRow);
            // 
            // additionalServiceDataGridViewTextBoxColumn
            // 
            this.additionalServiceDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.additionalServiceDataGridViewTextBoxColumn.DataPropertyName = "AdditionalService";
            this.additionalServiceDataGridViewTextBoxColumn.HeaderText = "Дополнительная услуга";
            this.additionalServiceDataGridViewTextBoxColumn.Name = "additionalServiceDataGridViewTextBoxColumn";
            this.additionalServiceDataGridViewTextBoxColumn.ReadOnly = true;
            this.additionalServiceDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // quantityDataGridViewTextBoxColumn
            // 
            this.quantityDataGridViewTextBoxColumn.DataPropertyName = "Quantity";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.quantityDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.quantityDataGridViewTextBoxColumn.FillWeight = 70F;
            this.quantityDataGridViewTextBoxColumn.HeaderText = "Кол-во";
            this.quantityDataGridViewTextBoxColumn.Name = "quantityDataGridViewTextBoxColumn";
            this.quantityDataGridViewTextBoxColumn.ReadOnly = true;
            this.quantityDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.quantityDataGridViewTextBoxColumn.Width = 70;
            // 
            // Sum
            // 
            this.Sum.DataPropertyName = "Sum";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle4.Format = "C2";
            dataGridViewCellStyle4.NullValue = null;
            this.Sum.DefaultCellStyle = dataGridViewCellStyle4;
            this.Sum.HeaderText = "Сумма";
            this.Sum.Name = "Sum";
            this.Sum.ReadOnly = true;
            this.Sum.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // additionalServicesBindingSource
            // 
            this.additionalServicesBindingSource.DataSource = typeof(Queue.Services.DTO.ClientRequestAdditionalService);
            // 
            // stepPanel
            // 
            this.stepPanel.Controls.Add(this.step1Panel);
            this.stepPanel.Controls.Add(this.step2Panel);
            this.stepPanel.Controls.Add(this.step3Panel);
            this.stepPanel.Location = new System.Drawing.Point(330, 35);
            this.stepPanel.Margin = new System.Windows.Forms.Padding(0);
            this.stepPanel.Name = "stepPanel";
            this.stepPanel.Size = new System.Drawing.Size(155, 335);
            this.stepPanel.TabIndex = 0;
            // 
            // step1Panel
            // 
            this.step1Panel.Controls.Add(this.callClientButton);
            this.step1Panel.Location = new System.Drawing.Point(0, 0);
            this.step1Panel.Name = "step1Panel";
            this.step1Panel.Size = new System.Drawing.Size(155, 165);
            this.step1Panel.TabIndex = 0;
            // 
            // callClientButton
            // 
            this.callClientButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.callClientButton.Location = new System.Drawing.Point(0, 0);
            this.callClientButton.Name = "callClientButton";
            this.callClientButton.Size = new System.Drawing.Size(155, 165);
            this.callClientButton.TabIndex = 2;
            this.callClientButton.Text = "Вызвать клиента";
            this.callClientButton.Click += new System.EventHandler(this.callClientButton_Click);
            // 
            // step2Panel
            // 
            this.step2Panel.Controls.Add(this.recallingButton);
            this.step2Panel.Controls.Add(this.renderingButton);
            this.step2Panel.Controls.Add(this.absenceButton);
            this.step2Panel.Location = new System.Drawing.Point(0, 0);
            this.step2Panel.Margin = new System.Windows.Forms.Padding(0);
            this.step2Panel.Name = "step2Panel";
            this.step2Panel.Size = new System.Drawing.Size(155, 165);
            this.step2Panel.TabIndex = 0;
            // 
            // recallingButton
            // 
            this.recallingButton.Location = new System.Drawing.Point(0, 0);
            this.recallingButton.Name = "recallingButton";
            this.recallingButton.Size = new System.Drawing.Size(155, 51);
            this.recallingButton.TabIndex = 0;
            this.recallingButton.Text = "Повторный вызов";
            this.recallingButton.Click += new System.EventHandler(this.recallingButton_Click);
            // 
            // renderingButton
            // 
            this.renderingButton.Location = new System.Drawing.Point(0, 55);
            this.renderingButton.Name = "renderingButton";
            this.renderingButton.Size = new System.Drawing.Size(155, 51);
            this.renderingButton.TabIndex = 0;
            this.renderingButton.Text = "Начать обслуживание";
            this.renderingButton.Click += new System.EventHandler(this.renderingButton_Click);
            // 
            // absenceButton
            // 
            this.absenceButton.Location = new System.Drawing.Point(0, 110);
            this.absenceButton.Name = "absenceButton";
            this.absenceButton.Size = new System.Drawing.Size(155, 51);
            this.absenceButton.TabIndex = 0;
            this.absenceButton.Text = "Клиент не подошел";
            this.absenceButton.Click += new System.EventHandler(this.absenceButton_Click);
            // 
            // step3Panel
            // 
            this.step3Panel.Controls.Add(this.renderedButton);
            this.step3Panel.Controls.Add(this.returnButton);
            this.step3Panel.Controls.Add(this.postponeGroupBox);
            this.step3Panel.Location = new System.Drawing.Point(0, 165);
            this.step3Panel.Margin = new System.Windows.Forms.Padding(0);
            this.step3Panel.Name = "step3Panel";
            this.step3Panel.Size = new System.Drawing.Size(155, 210);
            this.step3Panel.TabIndex = 0;
            // 
            // renderedButton
            // 
            this.renderedButton.Location = new System.Drawing.Point(0, 0);
            this.renderedButton.Name = "renderedButton";
            this.renderedButton.Size = new System.Drawing.Size(155, 40);
            this.renderedButton.TabIndex = 3;
            this.renderedButton.Text = "Закончить обслуживание";
            this.renderedButton.Click += new System.EventHandler(this.renderedButton_Click);
            // 
            // returnButton
            // 
            this.returnButton.Location = new System.Drawing.Point(0, 45);
            this.returnButton.Name = "returnButton";
            this.returnButton.Size = new System.Drawing.Size(155, 40);
            this.returnButton.TabIndex = 4;
            this.returnButton.Text = "Вернуть в очередь";
            this.returnButton.Click += new System.EventHandler(this.returnButton_Click);
            // 
            // postponeGroupBox
            // 
            this.postponeGroupBox.Controls.Add(this.postponeButton);
            this.postponeGroupBox.Controls.Add(this.label1);
            this.postponeGroupBox.Controls.Add(this.postponeMinutesUpDown);
            this.postponeGroupBox.Controls.Add(this.label2);
            this.postponeGroupBox.Location = new System.Drawing.Point(0, 80);
            this.postponeGroupBox.Name = "postponeGroupBox";
            this.postponeGroupBox.Size = new System.Drawing.Size(155, 80);
            this.postponeGroupBox.TabIndex = 10;
            this.postponeGroupBox.TabStop = false;
            // 
            // postponeButton
            // 
            this.postponeButton.Location = new System.Drawing.Point(5, 35);
            this.postponeButton.Name = "postponeButton";
            this.postponeButton.Size = new System.Drawing.Size(145, 40);
            this.postponeButton.TabIndex = 5;
            this.postponeButton.Text = "Отложить вызов";
            this.postponeButton.Click += new System.EventHandler(this.postponeButton_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "На время";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // postponeMinutesUpDown
            // 
            this.postponeMinutesUpDown.Location = new System.Drawing.Point(65, 13);
            this.postponeMinutesUpDown.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.postponeMinutesUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.postponeMinutesUpDown.Name = "postponeMinutesUpDown";
            this.postponeMinutesUpDown.Size = new System.Drawing.Size(50, 20);
            this.postponeMinutesUpDown.TabIndex = 6;
            this.postponeMinutesUpDown.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(115, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "мин.";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // clientRequestsTab
            // 
            this.clientRequestsTab.Controls.Add(this.clientRequestsGridView);
            this.clientRequestsTab.Location = new System.Drawing.Point(4, 26);
            this.clientRequestsTab.Name = "clientRequestsTab";
            this.clientRequestsTab.Size = new System.Drawing.Size(496, 480);
            this.clientRequestsTab.TabIndex = 0;
            this.clientRequestsTab.Text = "Список клиентов";
            this.clientRequestsTab.UseVisualStyleBackColor = true;
            // 
            // clientRequestsGridView
            // 
            this.clientRequestsGridView.AllowUserToAddRows = false;
            this.clientRequestsGridView.AllowUserToDeleteRows = false;
            this.clientRequestsGridView.AllowUserToResizeColumns = false;
            this.clientRequestsGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.clientRequestsGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.clientRequestsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.clientRequestsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numberColumn,
            this.subjectsColumn,
            this.StartTimeColumn,
            this.timeIntervalColumn,
            this.clientColumn,
            this.serviceColumn,
            this.stateColumn});
            this.clientRequestsGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clientRequestsGridView.Location = new System.Drawing.Point(0, 0);
            this.clientRequestsGridView.Margin = new System.Windows.Forms.Padding(0);
            this.clientRequestsGridView.MultiSelect = false;
            this.clientRequestsGridView.Name = "clientRequestsGridView";
            this.clientRequestsGridView.ReadOnly = true;
            this.clientRequestsGridView.RowHeadersVisible = false;
            this.clientRequestsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.clientRequestsGridView.Size = new System.Drawing.Size(496, 480);
            this.clientRequestsGridView.TabIndex = 0;
            // 
            // numberColumn
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.numberColumn.DefaultCellStyle = dataGridViewCellStyle6;
            this.numberColumn.HeaderText = "Номер";
            this.numberColumn.Name = "numberColumn";
            this.numberColumn.ReadOnly = true;
            this.numberColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.numberColumn.Width = 70;
            // 
            // subjectsColumn
            // 
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.subjectsColumn.DefaultCellStyle = dataGridViewCellStyle7;
            this.subjectsColumn.HeaderText = "Объектов";
            this.subjectsColumn.Name = "subjectsColumn";
            this.subjectsColumn.ReadOnly = true;
            this.subjectsColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // StartTimeColumn
            // 
            this.StartTimeColumn.HeaderText = "Время начала";
            this.StartTimeColumn.Name = "StartTimeColumn";
            this.StartTimeColumn.ReadOnly = true;
            this.StartTimeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StartTimeColumn.Width = 110;
            // 
            // timeIntervalColumn
            // 
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.timeIntervalColumn.DefaultCellStyle = dataGridViewCellStyle8;
            this.timeIntervalColumn.HeaderText = "Интервал (мин)";
            this.timeIntervalColumn.Name = "timeIntervalColumn";
            this.timeIntervalColumn.ReadOnly = true;
            this.timeIntervalColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.timeIntervalColumn.Width = 110;
            // 
            // clientColumn
            // 
            this.clientColumn.HeaderText = "Клиент";
            this.clientColumn.Name = "clientColumn";
            this.clientColumn.ReadOnly = true;
            this.clientColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.clientColumn.Width = 130;
            // 
            // serviceColumn
            // 
            this.serviceColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.serviceColumn.FillWeight = 150F;
            this.serviceColumn.HeaderText = "Выбранная услуга";
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
            // OperatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 572);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.actionsMenu;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(840, 610);
            this.MinimumSize = new System.Drawing.Size(540, 610);
            this.Name = "OperatorForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.mainTabControl.ResumeLayout(false);
            this.currentClientRequestTab.ResumeLayout(false);
            this.currentClientRequestTab.PerformLayout();
            this.actionsMenu.ResumeLayout(false);
            this.actionsMenu.PerformLayout();
            this.subjectsPanel.ResumeLayout(false);
            this.subjectsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).EndInit();
            this.clientRequestTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.parametersGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parametersBindingSource)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.additionalServicesBindingSource)).EndInit();
            this.stepPanel.ResumeLayout(false);
            this.step1Panel.ResumeLayout(false);
            this.step2Panel.ResumeLayout(false);
            this.step3Panel.ResumeLayout(false);
            this.postponeGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.postponeMinutesUpDown)).EndInit();
            this.clientRequestsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.clientRequestsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel serverStateLabel;
        private System.Windows.Forms.ToolStripStatusLabel currentDateTimeLabel;
        private System.Windows.Forms.ToolStripStatusLabel versionLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl mainTabControl;
        private System.Windows.Forms.TabPage currentClientRequestTab;
        private System.Windows.Forms.LinkLabel serviceChangeLink;
        private System.Windows.Forms.Label servceTypeLabel;
        private System.Windows.Forms.CheckBox isPriorityCheckBox;
        private System.Windows.Forms.Label numberLabel;
        private System.Windows.Forms.Label requestTimeLabel;
        private System.Windows.Forms.Label clientLabel;
        private System.Windows.Forms.Label serviceLabel;
        private System.Windows.Forms.Label stateLabel;
        private System.Windows.Forms.DataGridView parametersGridView;
        private System.Windows.Forms.Panel stepPanel;
        private System.Windows.Forms.Panel step1Panel;
        private System.Windows.Forms.Button callClientButton;
        private System.Windows.Forms.Panel step2Panel;
        private System.Windows.Forms.Button recallingButton;
        private System.Windows.Forms.Button renderingButton;
        private System.Windows.Forms.Button absenceButton;
        private System.Windows.Forms.Panel step3Panel;
        private System.Windows.Forms.Button returnButton;
        private System.Windows.Forms.Button renderedButton;
        private System.Windows.Forms.Button postponeButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown postponeMinutesUpDown;
        private System.Windows.Forms.TabPage clientRequestsTab;
        private System.Windows.Forms.DataGridView clientRequestsGridView;
        private System.Windows.Forms.Label numberTextBlock;
        private System.Windows.Forms.Label typeTextBlock;
        private System.Windows.Forms.Label requestTimeTextBlock;
        private System.Windows.Forms.Label clientTextBlock;
        private System.Windows.Forms.Label stateTextBlock;
        private System.Windows.Forms.Label serviceTextBlock;
        private System.Windows.Forms.NumericUpDown subjectsUpDown;
        private System.Windows.Forms.Label subjectsLabel;
        private DigitalTimer digitalTimer;
        private System.Windows.Forms.ToolStripStatusLabel standingLabel;
        private System.Windows.Forms.ToolStripStatusLabel standingTextLabel;
        private System.Windows.Forms.ToolStripStatusLabel separator1;
        private System.Windows.Forms.ToolStripStatusLabel separator2;
        private System.Windows.Forms.Panel subjectsPanel;
        private System.Windows.Forms.Label serviceStepLabel;
        private System.Windows.Forms.Button logoutButton;
        private IdentifiedEntityControl serviceStepControl;
        private EnumItemControl serviceTypeControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn subjectsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn timeIntervalColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn clientColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serviceColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn stateColumn;
        private System.Windows.Forms.TabControl clientRequestTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView additionalServicesGridView;
        private System.Windows.Forms.Button addAdditionalServiceButton;
        private System.Windows.Forms.BindingSource additionalServicesBindingSource;
        private System.Windows.Forms.BindingSource parametersBindingSource;
        private System.Windows.Forms.ToolStripStatusLabel qualityStateLabel;
        private System.Windows.Forms.ToolStripStatusLabel ratingLabel;
        private System.Windows.Forms.ToolStripSplitButton settingsButton;
        private System.Windows.Forms.GroupBox postponeGroupBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn additionalServiceDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quantityDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Sum;
        private System.Windows.Forms.Label commentLabel;
        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.LinkLabel commentSaveLink;
        private System.Windows.Forms.MenuStrip actionsMenu;
        private System.Windows.Forms.ToolStripMenuItem additionalMenu;
        private System.Windows.Forms.ToolStripMenuItem callClientByRequestNumberMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redirectToOperatorMenuItem;
        private System.Windows.Forms.LinkLabel subjectsChangeLink;

    }
}