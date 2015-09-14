namespace Queue.Administrator
{
    partial class EditServiceForm
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
            this.parametersTabPage = new System.Windows.Forms.TabPage();
            this.serviceParametersControl = new Queue.Administrator.ServiceParametersControl();
            this.exceptionScheduleTabPage = new System.Windows.Forms.TabPage();
            this.exceptionScheduleDatePicker = new System.Windows.Forms.DateTimePicker();
            this.exceptionScheduleCheckBox = new System.Windows.Forms.CheckBox();
            this.exceptionScheduleControl = new Queue.Administrator.ScheduleControl();
            this.commonTabPage = new System.Windows.Forms.TabPage();
            this.servicePropertiesTabControl = new System.Windows.Forms.TabControl();
            this.commonPropertiesTabPage = new System.Windows.Forms.TabPage();
            this.codeLabel = new System.Windows.Forms.Label();
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.tagsLabel = new System.Windows.Forms.Label();
            this.tagsTextBox = new System.Windows.Forms.TextBox();
            this.liveRegistratorLabel = new System.Windows.Forms.Label();
            this.liveRegistratorFlagsControl = new Queue.UI.WinForms.EnumFlagsControl();
            this.earlyRegistratorLabel = new System.Windows.Forms.Label();
            this.earlyRegistratorFlagsControl = new Queue.UI.WinForms.EnumFlagsControl();
            this.commentLabel = new System.Windows.Forms.Label();
            this.commentTextBox = new System.Windows.Forms.TextBox();
            this.additionalPropertiesTabPage = new System.Windows.Forms.TabPage();
            this.clientCallDelayLabel = new System.Windows.Forms.Label();
            this.clientCallDelayUpDown = new System.Windows.Forms.NumericUpDown();
            this.clientCallDelaySecondsLabel = new System.Windows.Forms.Label();
            this.timeIntervalRoundingLabel = new System.Windows.Forms.Label();
            this.timeIntervalRoundingUpDown = new System.Windows.Forms.NumericUpDown();
            this.timeIntervalRoundingMinLabel = new System.Windows.Forms.Label();
            this.maxSubjectsLabel = new System.Windows.Forms.Label();
            this.maxSubjectsUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxEarlyDaysLabel = new System.Windows.Forms.Label();
            this.maxEarlyDaysUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxEarlyDaysDaysLabel = new System.Windows.Forms.Label();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.priorityUpDown = new System.Windows.Forms.NumericUpDown();
            this.clientRequireCheckBox = new System.Windows.Forms.CheckBox();
            this.isPlanSubjectsCheckBox = new System.Windows.Forms.CheckBox();
            this.isUseTypeCheckBox = new System.Windows.Forms.CheckBox();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.linkLabel = new System.Windows.Forms.Label();
            this.linkTextBox = new System.Windows.Forms.TextBox();
            this.designPropertiesTabControl = new System.Windows.Forms.TabPage();
            this.fontSizeLabel = new System.Windows.Forms.Label();
            this.fontSizeTrackBar = new System.Windows.Forms.TrackBar();
            this.saveButton = new System.Windows.Forms.Button();
            this.serviceTabControl = new System.Windows.Forms.TabControl();
            this.stepsTabPage = new System.Windows.Forms.TabPage();
            this.serviceStepsControl = new Queue.Administrator.ServiceStepsControl();
            this.weekdayScheduleTabPage = new System.Windows.Forms.TabPage();
            this.weekdayTabControl = new System.Windows.Forms.TabControl();
            this.mondayTabPage = new System.Windows.Forms.TabPage();
            this.weekdaySchedulePanel = new System.Windows.Forms.Panel();
            this.weekdayScheduleCheckBox = new System.Windows.Forms.CheckBox();
            this.weekdayScheduleControl = new Queue.Administrator.ScheduleControl();
            this.tuesdayTabPage = new System.Windows.Forms.TabPage();
            this.wednesdayTabPage = new System.Windows.Forms.TabPage();
            this.thursdayTabPage = new System.Windows.Forms.TabPage();
            this.fridayTabPage = new System.Windows.Forms.TabPage();
            this.saturdayTabPage = new System.Windows.Forms.TabPage();
            this.sundayTabPage = new System.Windows.Forms.TabPage();
            this.parametersTabPage.SuspendLayout();
            this.exceptionScheduleTabPage.SuspendLayout();
            this.commonTabPage.SuspendLayout();
            this.servicePropertiesTabControl.SuspendLayout();
            this.commonPropertiesTabPage.SuspendLayout();
            this.additionalPropertiesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientCallDelayUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeIntervalRoundingUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSubjectsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxEarlyDaysUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.priorityUpDown)).BeginInit();
            this.designPropertiesTabControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeTrackBar)).BeginInit();
            this.serviceTabControl.SuspendLayout();
            this.stepsTabPage.SuspendLayout();
            this.weekdayScheduleTabPage.SuspendLayout();
            this.weekdayTabControl.SuspendLayout();
            this.mondayTabPage.SuspendLayout();
            this.weekdaySchedulePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // parametersTabPage
            // 
            this.parametersTabPage.Controls.Add(this.serviceParametersControl);
            this.parametersTabPage.Location = new System.Drawing.Point(4, 26);
            this.parametersTabPage.Name = "parametersTabPage";
            this.parametersTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.parametersTabPage.Size = new System.Drawing.Size(836, 456);
            this.parametersTabPage.TabIndex = 0;
            this.parametersTabPage.Text = "Параметры услуги";
            this.parametersTabPage.UseVisualStyleBackColor = true;
            // 
            // serviceParametersControl
            // 
            this.serviceParametersControl.CurrentUser = null;
            this.serviceParametersControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceParametersControl.Location = new System.Drawing.Point(3, 3);
            this.serviceParametersControl.Name = "serviceParametersControl";
            this.serviceParametersControl.ServerService = null;
            this.serviceParametersControl.Service = null;
            this.serviceParametersControl.Size = new System.Drawing.Size(830, 450);
            this.serviceParametersControl.TabIndex = 0;
            // 
            // exceptionScheduleTabPage
            // 
            this.exceptionScheduleTabPage.Controls.Add(this.exceptionScheduleDatePicker);
            this.exceptionScheduleTabPage.Controls.Add(this.exceptionScheduleCheckBox);
            this.exceptionScheduleTabPage.Controls.Add(this.exceptionScheduleControl);
            this.exceptionScheduleTabPage.Location = new System.Drawing.Point(4, 26);
            this.exceptionScheduleTabPage.Name = "exceptionScheduleTabPage";
            this.exceptionScheduleTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.exceptionScheduleTabPage.Size = new System.Drawing.Size(836, 456);
            this.exceptionScheduleTabPage.TabIndex = 0;
            this.exceptionScheduleTabPage.Text = "Исключения в расписании";
            this.exceptionScheduleTabPage.UseVisualStyleBackColor = true;
            // 
            // exceptionScheduleDatePicker
            // 
            this.exceptionScheduleDatePicker.Location = new System.Drawing.Point(10, 10);
            this.exceptionScheduleDatePicker.Name = "exceptionScheduleDatePicker";
            this.exceptionScheduleDatePicker.Size = new System.Drawing.Size(185, 20);
            this.exceptionScheduleDatePicker.TabIndex = 0;
            this.exceptionScheduleDatePicker.Value = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            this.exceptionScheduleDatePicker.ValueChanged += new System.EventHandler(this.exceptionScheduleDatePicker_ValueChanged);
            // 
            // exceptionScheduleCheckBox
            // 
            this.exceptionScheduleCheckBox.AutoSize = true;
            this.exceptionScheduleCheckBox.Location = new System.Drawing.Point(20, 40);
            this.exceptionScheduleCheckBox.Name = "exceptionScheduleCheckBox";
            this.exceptionScheduleCheckBox.Size = new System.Drawing.Size(150, 17);
            this.exceptionScheduleCheckBox.TabIndex = 1;
            this.exceptionScheduleCheckBox.Tag = "2";
            this.exceptionScheduleCheckBox.Text = "Определить расписание";
            this.exceptionScheduleCheckBox.UseVisualStyleBackColor = true;
            this.exceptionScheduleCheckBox.CheckedChanged += new System.EventHandler(this.exceptionScheduleCheckBox_CheckedChanged);
            this.exceptionScheduleCheckBox.Click += new System.EventHandler(this.exceptionScheduleCheckBox_Click);
            // 
            // exceptionScheduleControl
            // 
            this.exceptionScheduleControl.CurrentUser = null;
            this.exceptionScheduleControl.Location = new System.Drawing.Point(10, 65);
            this.exceptionScheduleControl.Name = "exceptionScheduleControl";
            this.exceptionScheduleControl.Schedule = null;
            this.exceptionScheduleControl.ServerService = null;
            this.exceptionScheduleControl.Size = new System.Drawing.Size(790, 320);
            this.exceptionScheduleControl.TabIndex = 2;
            // 
            // commonTabPage
            // 
            this.commonTabPage.Controls.Add(this.servicePropertiesTabControl);
            this.commonTabPage.Controls.Add(this.saveButton);
            this.commonTabPage.Location = new System.Drawing.Point(4, 26);
            this.commonTabPage.Name = "commonTabPage";
            this.commonTabPage.Size = new System.Drawing.Size(836, 456);
            this.commonTabPage.TabIndex = 0;
            this.commonTabPage.Text = "Общая информация";
            this.commonTabPage.UseVisualStyleBackColor = true;
            // 
            // servicePropertiesTabControl
            // 
            this.servicePropertiesTabControl.Controls.Add(this.commonPropertiesTabPage);
            this.servicePropertiesTabControl.Controls.Add(this.additionalPropertiesTabPage);
            this.servicePropertiesTabControl.Controls.Add(this.designPropertiesTabControl);
            this.servicePropertiesTabControl.Location = new System.Drawing.Point(5, 5);
            this.servicePropertiesTabControl.Margin = new System.Windows.Forms.Padding(5);
            this.servicePropertiesTabControl.Multiline = true;
            this.servicePropertiesTabControl.Name = "servicePropertiesTabControl";
            this.servicePropertiesTabControl.Padding = new System.Drawing.Point(5, 5);
            this.servicePropertiesTabControl.SelectedIndex = 0;
            this.servicePropertiesTabControl.Size = new System.Drawing.Size(825, 410);
            this.servicePropertiesTabControl.TabIndex = 0;
            // 
            // commonPropertiesTabPage
            // 
            this.commonPropertiesTabPage.Controls.Add(this.codeLabel);
            this.commonPropertiesTabPage.Controls.Add(this.codeTextBox);
            this.commonPropertiesTabPage.Controls.Add(this.nameLabel);
            this.commonPropertiesTabPage.Controls.Add(this.nameTextBox);
            this.commonPropertiesTabPage.Controls.Add(this.tagsLabel);
            this.commonPropertiesTabPage.Controls.Add(this.tagsTextBox);
            this.commonPropertiesTabPage.Controls.Add(this.liveRegistratorLabel);
            this.commonPropertiesTabPage.Controls.Add(this.liveRegistratorFlagsControl);
            this.commonPropertiesTabPage.Controls.Add(this.earlyRegistratorLabel);
            this.commonPropertiesTabPage.Controls.Add(this.earlyRegistratorFlagsControl);
            this.commonPropertiesTabPage.Controls.Add(this.commentLabel);
            this.commonPropertiesTabPage.Controls.Add(this.commentTextBox);
            this.commonPropertiesTabPage.Location = new System.Drawing.Point(4, 26);
            this.commonPropertiesTabPage.Name = "commonPropertiesTabPage";
            this.commonPropertiesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.commonPropertiesTabPage.Size = new System.Drawing.Size(817, 380);
            this.commonPropertiesTabPage.TabIndex = 0;
            this.commonPropertiesTabPage.Text = "Основные параметры";
            this.commonPropertiesTabPage.UseVisualStyleBackColor = true;
            // 
            // codeLabel
            // 
            this.codeLabel.Location = new System.Drawing.Point(10, 5);
            this.codeLabel.Name = "codeLabel";
            this.codeLabel.Size = new System.Drawing.Size(340, 20);
            this.codeLabel.TabIndex = 0;
            this.codeLabel.Text = "Код";
            this.codeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // codeTextBox
            // 
            this.codeTextBox.Location = new System.Drawing.Point(20, 35);
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(60, 20);
            this.codeTextBox.TabIndex = 0;
            this.codeTextBox.Leave += new System.EventHandler(this.codeTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(7, 62);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(343, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Наименование";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(20, 85);
            this.nameTextBox.Multiline = true;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(330, 110);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // tagsLabel
            // 
            this.tagsLabel.Location = new System.Drawing.Point(370, 5);
            this.tagsLabel.Name = "tagsLabel";
            this.tagsLabel.Size = new System.Drawing.Size(190, 18);
            this.tagsLabel.TabIndex = 0;
            this.tagsLabel.Text = "Поисковые тэги";
            this.tagsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // tagsTextBox
            // 
            this.tagsTextBox.Location = new System.Drawing.Point(385, 30);
            this.tagsTextBox.Multiline = true;
            this.tagsTextBox.Name = "tagsTextBox";
            this.tagsTextBox.Size = new System.Drawing.Size(175, 80);
            this.tagsTextBox.TabIndex = 2;
            this.tagsTextBox.Leave += new System.EventHandler(this.tagsTextBox_Leave);
            // 
            // liveRegistratorLabel
            // 
            this.liveRegistratorLabel.Location = new System.Drawing.Point(365, 120);
            this.liveRegistratorLabel.Name = "liveRegistratorLabel";
            this.liveRegistratorLabel.Size = new System.Drawing.Size(195, 18);
            this.liveRegistratorLabel.TabIndex = 0;
            this.liveRegistratorLabel.Text = "Регистраторы живой очереди";
            this.liveRegistratorLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // liveRegistratorFlagsControl
            // 
            this.liveRegistratorFlagsControl.Location = new System.Drawing.Point(380, 145);
            this.liveRegistratorFlagsControl.Name = "liveRegistratorFlagsControl";
            this.liveRegistratorFlagsControl.Size = new System.Drawing.Size(180, 65);
            this.liveRegistratorFlagsControl.TabIndex = 3;
            this.liveRegistratorFlagsControl.Leave += new System.EventHandler(this.liveRegistratorFlagsControl_Leave);
            // 
            // earlyRegistratorLabel
            // 
            this.earlyRegistratorLabel.Location = new System.Drawing.Point(590, 120);
            this.earlyRegistratorLabel.Name = "earlyRegistratorLabel";
            this.earlyRegistratorLabel.Size = new System.Drawing.Size(190, 18);
            this.earlyRegistratorLabel.TabIndex = 0;
            this.earlyRegistratorLabel.Text = "Регистраторы по записи";
            this.earlyRegistratorLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // earlyRegistratorFlagsControl
            // 
            this.earlyRegistratorFlagsControl.Location = new System.Drawing.Point(600, 145);
            this.earlyRegistratorFlagsControl.Name = "earlyRegistratorFlagsControl";
            this.earlyRegistratorFlagsControl.Size = new System.Drawing.Size(180, 65);
            this.earlyRegistratorFlagsControl.TabIndex = 4;
            this.earlyRegistratorFlagsControl.Leave += new System.EventHandler(this.earlyRegistratorFlagsControl_Leave);
            // 
            // commentLabel
            // 
            this.commentLabel.Location = new System.Drawing.Point(10, 205);
            this.commentLabel.Name = "commentLabel";
            this.commentLabel.Size = new System.Drawing.Size(340, 18);
            this.commentLabel.TabIndex = 0;
            this.commentLabel.Text = "Комментарий";
            this.commentLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // commentTextBox
            // 
            this.commentTextBox.Location = new System.Drawing.Point(20, 230);
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.Size = new System.Drawing.Size(785, 135);
            this.commentTextBox.TabIndex = 5;
            this.commentTextBox.Leave += new System.EventHandler(this.commentTextBox_Leave);
            // 
            // additionalPropertiesTabPage
            // 
            this.additionalPropertiesTabPage.Controls.Add(this.clientCallDelayLabel);
            this.additionalPropertiesTabPage.Controls.Add(this.clientCallDelayUpDown);
            this.additionalPropertiesTabPage.Controls.Add(this.clientCallDelaySecondsLabel);
            this.additionalPropertiesTabPage.Controls.Add(this.timeIntervalRoundingLabel);
            this.additionalPropertiesTabPage.Controls.Add(this.timeIntervalRoundingUpDown);
            this.additionalPropertiesTabPage.Controls.Add(this.timeIntervalRoundingMinLabel);
            this.additionalPropertiesTabPage.Controls.Add(this.maxSubjectsLabel);
            this.additionalPropertiesTabPage.Controls.Add(this.maxSubjectsUpDown);
            this.additionalPropertiesTabPage.Controls.Add(this.maxEarlyDaysLabel);
            this.additionalPropertiesTabPage.Controls.Add(this.maxEarlyDaysUpDown);
            this.additionalPropertiesTabPage.Controls.Add(this.maxEarlyDaysDaysLabel);
            this.additionalPropertiesTabPage.Controls.Add(this.priorityLabel);
            this.additionalPropertiesTabPage.Controls.Add(this.priorityUpDown);
            this.additionalPropertiesTabPage.Controls.Add(this.clientRequireCheckBox);
            this.additionalPropertiesTabPage.Controls.Add(this.isPlanSubjectsCheckBox);
            this.additionalPropertiesTabPage.Controls.Add(this.isUseTypeCheckBox);
            this.additionalPropertiesTabPage.Controls.Add(this.descriptionLabel);
            this.additionalPropertiesTabPage.Controls.Add(this.descriptionTextBox);
            this.additionalPropertiesTabPage.Controls.Add(this.linkLabel);
            this.additionalPropertiesTabPage.Controls.Add(this.linkTextBox);
            this.additionalPropertiesTabPage.Location = new System.Drawing.Point(4, 26);
            this.additionalPropertiesTabPage.Name = "additionalPropertiesTabPage";
            this.additionalPropertiesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.additionalPropertiesTabPage.Size = new System.Drawing.Size(817, 380);
            this.additionalPropertiesTabPage.TabIndex = 1;
            this.additionalPropertiesTabPage.Text = "Дополнительные параметры";
            this.additionalPropertiesTabPage.UseVisualStyleBackColor = true;
            // 
            // clientCallDelayLabel
            // 
            this.clientCallDelayLabel.Location = new System.Drawing.Point(5, 10);
            this.clientCallDelayLabel.Name = "clientCallDelayLabel";
            this.clientCallDelayLabel.Size = new System.Drawing.Size(205, 30);
            this.clientCallDelayLabel.TabIndex = 0;
            this.clientCallDelayLabel.Text = "Задержка вызова клиента";
            this.clientCallDelayLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // clientCallDelayUpDown
            // 
            this.clientCallDelayUpDown.Location = new System.Drawing.Point(215, 20);
            this.clientCallDelayUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.clientCallDelayUpDown.Name = "clientCallDelayUpDown";
            this.clientCallDelayUpDown.Size = new System.Drawing.Size(50, 20);
            this.clientCallDelayUpDown.TabIndex = 0;
            this.clientCallDelayUpDown.Leave += new System.EventHandler(this.clientCallDelayUpDown_Leave);
            // 
            // clientCallDelaySecondsLabel
            // 
            this.clientCallDelaySecondsLabel.AutoSize = true;
            this.clientCallDelaySecondsLabel.Location = new System.Drawing.Point(270, 25);
            this.clientCallDelaySecondsLabel.Name = "clientCallDelaySecondsLabel";
            this.clientCallDelaySecondsLabel.Size = new System.Drawing.Size(28, 13);
            this.clientCallDelaySecondsLabel.TabIndex = 0;
            this.clientCallDelaySecondsLabel.Text = "сек.";
            // 
            // timeIntervalRoundingLabel
            // 
            this.timeIntervalRoundingLabel.Location = new System.Drawing.Point(5, 40);
            this.timeIntervalRoundingLabel.Name = "timeIntervalRoundingLabel";
            this.timeIntervalRoundingLabel.Size = new System.Drawing.Size(205, 30);
            this.timeIntervalRoundingLabel.TabIndex = 0;
            this.timeIntervalRoundingLabel.Text = "Округление временных интервалов";
            this.timeIntervalRoundingLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // timeIntervalRoundingUpDown
            // 
            this.timeIntervalRoundingUpDown.Location = new System.Drawing.Point(215, 50);
            this.timeIntervalRoundingUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.timeIntervalRoundingUpDown.Name = "timeIntervalRoundingUpDown";
            this.timeIntervalRoundingUpDown.Size = new System.Drawing.Size(50, 20);
            this.timeIntervalRoundingUpDown.TabIndex = 1;
            this.timeIntervalRoundingUpDown.Leave += new System.EventHandler(this.timeIntervalRoundingUpDown_Leave);
            // 
            // timeIntervalRoundingMinLabel
            // 
            this.timeIntervalRoundingMinLabel.AutoSize = true;
            this.timeIntervalRoundingMinLabel.Location = new System.Drawing.Point(270, 55);
            this.timeIntervalRoundingMinLabel.Name = "timeIntervalRoundingMinLabel";
            this.timeIntervalRoundingMinLabel.Size = new System.Drawing.Size(27, 13);
            this.timeIntervalRoundingMinLabel.TabIndex = 0;
            this.timeIntervalRoundingMinLabel.Text = "мин";
            // 
            // maxSubjectsLabel
            // 
            this.maxSubjectsLabel.Location = new System.Drawing.Point(5, 70);
            this.maxSubjectsLabel.Name = "maxSubjectsLabel";
            this.maxSubjectsLabel.Size = new System.Drawing.Size(205, 30);
            this.maxSubjectsLabel.TabIndex = 0;
            this.maxSubjectsLabel.Text = "Максимальное кол-во объектов";
            this.maxSubjectsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // maxSubjectsUpDown
            // 
            this.maxSubjectsUpDown.Location = new System.Drawing.Point(215, 80);
            this.maxSubjectsUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.maxSubjectsUpDown.Name = "maxSubjectsUpDown";
            this.maxSubjectsUpDown.Size = new System.Drawing.Size(50, 20);
            this.maxSubjectsUpDown.TabIndex = 2;
            this.maxSubjectsUpDown.Leave += new System.EventHandler(this.maxSubjectsUpDown_Leave);
            // 
            // maxEarlyDaysLabel
            // 
            this.maxEarlyDaysLabel.Location = new System.Drawing.Point(5, 100);
            this.maxEarlyDaysLabel.Name = "maxEarlyDaysLabel";
            this.maxEarlyDaysLabel.Size = new System.Drawing.Size(205, 30);
            this.maxEarlyDaysLabel.TabIndex = 0;
            this.maxEarlyDaysLabel.Text = "Предварительная запись доступна на";
            this.maxEarlyDaysLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // maxEarlyDaysUpDown
            // 
            this.maxEarlyDaysUpDown.Location = new System.Drawing.Point(215, 110);
            this.maxEarlyDaysUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.maxEarlyDaysUpDown.Name = "maxEarlyDaysUpDown";
            this.maxEarlyDaysUpDown.Size = new System.Drawing.Size(50, 20);
            this.maxEarlyDaysUpDown.TabIndex = 3;
            this.maxEarlyDaysUpDown.Leave += new System.EventHandler(this.maxEarlyDaysUpDown_Leave);
            // 
            // maxEarlyDaysDaysLabel
            // 
            this.maxEarlyDaysDaysLabel.AutoSize = true;
            this.maxEarlyDaysDaysLabel.Location = new System.Drawing.Point(270, 115);
            this.maxEarlyDaysDaysLabel.Name = "maxEarlyDaysDaysLabel";
            this.maxEarlyDaysDaysLabel.Size = new System.Drawing.Size(31, 13);
            this.maxEarlyDaysDaysLabel.TabIndex = 0;
            this.maxEarlyDaysDaysLabel.Text = "дней";
            // 
            // priorityLabel
            // 
            this.priorityLabel.Location = new System.Drawing.Point(5, 130);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(205, 30);
            this.priorityLabel.TabIndex = 0;
            this.priorityLabel.Text = "Приоритет услуги";
            this.priorityLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // priorityUpDown
            // 
            this.priorityUpDown.Location = new System.Drawing.Point(215, 140);
            this.priorityUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.priorityUpDown.Name = "priorityUpDown";
            this.priorityUpDown.Size = new System.Drawing.Size(50, 20);
            this.priorityUpDown.TabIndex = 4;
            this.priorityUpDown.Leave += new System.EventHandler(this.priorityUpDown_Leave);
            // 
            // clientRequireCheckBox
            // 
            this.clientRequireCheckBox.Location = new System.Drawing.Point(305, 10);
            this.clientRequireCheckBox.Name = "clientRequireCheckBox";
            this.clientRequireCheckBox.Size = new System.Drawing.Size(180, 30);
            this.clientRequireCheckBox.TabIndex = 5;
            this.clientRequireCheckBox.Text = "Требовать клиента";
            this.clientRequireCheckBox.UseVisualStyleBackColor = true;
            this.clientRequireCheckBox.Leave += new System.EventHandler(this.clientRequireCheckBox_Leave);
            // 
            // isPlanSubjectsCheckBox
            // 
            this.isPlanSubjectsCheckBox.Location = new System.Drawing.Point(305, 40);
            this.isPlanSubjectsCheckBox.Name = "isPlanSubjectsCheckBox";
            this.isPlanSubjectsCheckBox.Size = new System.Drawing.Size(180, 30);
            this.isPlanSubjectsCheckBox.TabIndex = 6;
            this.isPlanSubjectsCheckBox.Text = "Учитывать при планировании кол-во объектов";
            this.isPlanSubjectsCheckBox.UseVisualStyleBackColor = true;
            this.isPlanSubjectsCheckBox.Leave += new System.EventHandler(this.isPlanSubjectsCheckBox_Leave);
            // 
            // isUseTypeCheckBox
            // 
            this.isUseTypeCheckBox.AutoSize = true;
            this.isUseTypeCheckBox.Location = new System.Drawing.Point(305, 80);
            this.isUseTypeCheckBox.Name = "isUseTypeCheckBox";
            this.isUseTypeCheckBox.Size = new System.Drawing.Size(159, 17);
            this.isUseTypeCheckBox.TabIndex = 7;
            this.isUseTypeCheckBox.Text = "Разделить по типам услуг";
            this.isUseTypeCheckBox.UseVisualStyleBackColor = true;
            this.isUseTypeCheckBox.Leave += new System.EventHandler(this.isUseTypeCheckBox_Leave);
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.Location = new System.Drawing.Point(10, 180);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(200, 18);
            this.descriptionLabel.TabIndex = 0;
            this.descriptionLabel.Text = "Описание услуги";
            this.descriptionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(15, 210);
            this.descriptionTextBox.Multiline = true;
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.ReadOnly = true;
            this.descriptionTextBox.Size = new System.Drawing.Size(790, 75);
            this.descriptionTextBox.TabIndex = 8;
            this.descriptionTextBox.Click += new System.EventHandler(this.descriptionTextBox_Click);
            // 
            // linkLabel
            // 
            this.linkLabel.Location = new System.Drawing.Point(5, 290);
            this.linkLabel.Name = "linkLabel";
            this.linkLabel.Size = new System.Drawing.Size(200, 18);
            this.linkLabel.TabIndex = 0;
            this.linkLabel.Text = "Ссылка на информацию";
            this.linkLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // linkTextBox
            // 
            this.linkTextBox.Location = new System.Drawing.Point(15, 320);
            this.linkTextBox.Multiline = true;
            this.linkTextBox.Name = "linkTextBox";
            this.linkTextBox.Size = new System.Drawing.Size(790, 50);
            this.linkTextBox.TabIndex = 9;
            this.linkTextBox.Leave += new System.EventHandler(this.linkTextBox_Leave);
            // 
            // designPropertiesTabControl
            // 
            this.designPropertiesTabControl.Controls.Add(this.fontSizeLabel);
            this.designPropertiesTabControl.Controls.Add(this.fontSizeTrackBar);
            this.designPropertiesTabControl.Location = new System.Drawing.Point(4, 26);
            this.designPropertiesTabControl.Name = "designPropertiesTabControl";
            this.designPropertiesTabControl.Size = new System.Drawing.Size(817, 380);
            this.designPropertiesTabControl.TabIndex = 2;
            this.designPropertiesTabControl.Text = "Параметры дизайна";
            this.designPropertiesTabControl.UseVisualStyleBackColor = true;
            // 
            // fontSizeLabel
            // 
            this.fontSizeLabel.Location = new System.Drawing.Point(5, 10);
            this.fontSizeLabel.Name = "fontSizeLabel";
            this.fontSizeLabel.Size = new System.Drawing.Size(110, 45);
            this.fontSizeLabel.TabIndex = 13;
            this.fontSizeLabel.Text = "Размер шрифта %";
            this.fontSizeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // fontSizeTrackBar
            // 
            this.fontSizeTrackBar.LargeChange = 10;
            this.fontSizeTrackBar.Location = new System.Drawing.Point(110, 10);
            this.fontSizeTrackBar.Maximum = 300;
            this.fontSizeTrackBar.Minimum = 10;
            this.fontSizeTrackBar.Name = "fontSizeTrackBar";
            this.fontSizeTrackBar.Size = new System.Drawing.Size(245, 45);
            this.fontSizeTrackBar.TabIndex = 12;
            this.fontSizeTrackBar.Value = 100;
            this.fontSizeTrackBar.Leave += new System.EventHandler(this.fontSizeTrackBar_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(745, 420);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(85, 25);
            this.saveButton.TabIndex = 10;
            this.saveButton.Text = "Записать";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // serviceTabControl
            // 
            this.serviceTabControl.Controls.Add(this.commonTabPage);
            this.serviceTabControl.Controls.Add(this.stepsTabPage);
            this.serviceTabControl.Controls.Add(this.weekdayScheduleTabPage);
            this.serviceTabControl.Controls.Add(this.exceptionScheduleTabPage);
            this.serviceTabControl.Controls.Add(this.parametersTabPage);
            this.serviceTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceTabControl.Location = new System.Drawing.Point(10, 10);
            this.serviceTabControl.Margin = new System.Windows.Forms.Padding(5);
            this.serviceTabControl.Name = "serviceTabControl";
            this.serviceTabControl.Padding = new System.Drawing.Point(5, 5);
            this.serviceTabControl.SelectedIndex = 0;
            this.serviceTabControl.Size = new System.Drawing.Size(844, 486);
            this.serviceTabControl.TabIndex = 0;
            this.serviceTabControl.SelectedIndexChanged += new System.EventHandler(this.serviceTabControl_SelectedIndexChanged);
            // 
            // stepsTabPage
            // 
            this.stepsTabPage.Controls.Add(this.serviceStepsControl);
            this.stepsTabPage.Location = new System.Drawing.Point(4, 26);
            this.stepsTabPage.Name = "stepsTabPage";
            this.stepsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.stepsTabPage.Size = new System.Drawing.Size(836, 456);
            this.stepsTabPage.TabIndex = 1;
            this.stepsTabPage.Text = "Этапы услуги";
            this.stepsTabPage.UseVisualStyleBackColor = true;
            // 
            // serviceStepsControl
            // 
            this.serviceStepsControl.CurrentUser = null;
            this.serviceStepsControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceStepsControl.Location = new System.Drawing.Point(3, 3);
            this.serviceStepsControl.Name = "serviceStepsControl";
            this.serviceStepsControl.ServerService = null;
            this.serviceStepsControl.Service = null;
            this.serviceStepsControl.Size = new System.Drawing.Size(830, 450);
            this.serviceStepsControl.TabIndex = 0;
            // 
            // weekdayScheduleTabPage
            // 
            this.weekdayScheduleTabPage.Controls.Add(this.weekdayTabControl);
            this.weekdayScheduleTabPage.Location = new System.Drawing.Point(4, 26);
            this.weekdayScheduleTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.weekdayScheduleTabPage.Name = "weekdayScheduleTabPage";
            this.weekdayScheduleTabPage.Padding = new System.Windows.Forms.Padding(5);
            this.weekdayScheduleTabPage.Size = new System.Drawing.Size(836, 456);
            this.weekdayScheduleTabPage.TabIndex = 0;
            this.weekdayScheduleTabPage.Text = "Регулярное расписание";
            this.weekdayScheduleTabPage.UseVisualStyleBackColor = true;
            // 
            // weekdayTabControl
            // 
            this.weekdayTabControl.Controls.Add(this.mondayTabPage);
            this.weekdayTabControl.Controls.Add(this.tuesdayTabPage);
            this.weekdayTabControl.Controls.Add(this.wednesdayTabPage);
            this.weekdayTabControl.Controls.Add(this.thursdayTabPage);
            this.weekdayTabControl.Controls.Add(this.fridayTabPage);
            this.weekdayTabControl.Controls.Add(this.saturdayTabPage);
            this.weekdayTabControl.Controls.Add(this.sundayTabPage);
            this.weekdayTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.weekdayTabControl.Location = new System.Drawing.Point(5, 5);
            this.weekdayTabControl.Multiline = true;
            this.weekdayTabControl.Name = "weekdayTabControl";
            this.weekdayTabControl.Padding = new System.Drawing.Point(5, 5);
            this.weekdayTabControl.SelectedIndex = 0;
            this.weekdayTabControl.Size = new System.Drawing.Size(826, 446);
            this.weekdayTabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.weekdayTabControl.TabIndex = 0;
            this.weekdayTabControl.Tag = "";
            this.weekdayTabControl.SelectedIndexChanged += new System.EventHandler(this.weekdayTabControl_SelectedIndexChanged);
            // 
            // mondayTabPage
            // 
            this.mondayTabPage.BackColor = System.Drawing.Color.Transparent;
            this.mondayTabPage.Controls.Add(this.weekdaySchedulePanel);
            this.mondayTabPage.Location = new System.Drawing.Point(4, 26);
            this.mondayTabPage.Name = "mondayTabPage";
            this.mondayTabPage.Size = new System.Drawing.Size(818, 416);
            this.mondayTabPage.TabIndex = 0;
            this.mondayTabPage.Tag = "1";
            this.mondayTabPage.Text = "Понедельник";
            this.mondayTabPage.UseVisualStyleBackColor = true;
            // 
            // weekdaySchedulePanel
            // 
            this.weekdaySchedulePanel.Controls.Add(this.weekdayScheduleCheckBox);
            this.weekdaySchedulePanel.Controls.Add(this.weekdayScheduleControl);
            this.weekdaySchedulePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.weekdaySchedulePanel.Location = new System.Drawing.Point(0, 0);
            this.weekdaySchedulePanel.Name = "weekdaySchedulePanel";
            this.weekdaySchedulePanel.Size = new System.Drawing.Size(818, 416);
            this.weekdaySchedulePanel.TabIndex = 1;
            // 
            // weekdayScheduleCheckBox
            // 
            this.weekdayScheduleCheckBox.AutoSize = true;
            this.weekdayScheduleCheckBox.Location = new System.Drawing.Point(15, 15);
            this.weekdayScheduleCheckBox.Name = "weekdayScheduleCheckBox";
            this.weekdayScheduleCheckBox.Size = new System.Drawing.Size(150, 17);
            this.weekdayScheduleCheckBox.TabIndex = 0;
            this.weekdayScheduleCheckBox.Tag = "2";
            this.weekdayScheduleCheckBox.Text = "Определить расписание";
            this.weekdayScheduleCheckBox.UseVisualStyleBackColor = true;
            this.weekdayScheduleCheckBox.CheckedChanged += new System.EventHandler(this.weekdayScheduleCheckBox_CheckedChanged);
            this.weekdayScheduleCheckBox.Click += new System.EventHandler(this.weekdayScheduleCheckBox_Click);
            // 
            // weekdayScheduleControl
            // 
            this.weekdayScheduleControl.CurrentUser = null;
            this.weekdayScheduleControl.Location = new System.Drawing.Point(5, 45);
            this.weekdayScheduleControl.Name = "weekdayScheduleControl";
            this.weekdayScheduleControl.Schedule = null;
            this.weekdayScheduleControl.ServerService = null;
            this.weekdayScheduleControl.Size = new System.Drawing.Size(790, 320);
            this.weekdayScheduleControl.TabIndex = 0;
            // 
            // tuesdayTabPage
            // 
            this.tuesdayTabPage.Location = new System.Drawing.Point(4, 26);
            this.tuesdayTabPage.Name = "tuesdayTabPage";
            this.tuesdayTabPage.Size = new System.Drawing.Size(818, 416);
            this.tuesdayTabPage.TabIndex = 0;
            this.tuesdayTabPage.Tag = "2";
            this.tuesdayTabPage.Text = "Вторник";
            this.tuesdayTabPage.UseVisualStyleBackColor = true;
            // 
            // wednesdayTabPage
            // 
            this.wednesdayTabPage.Location = new System.Drawing.Point(4, 26);
            this.wednesdayTabPage.Name = "wednesdayTabPage";
            this.wednesdayTabPage.Size = new System.Drawing.Size(818, 416);
            this.wednesdayTabPage.TabIndex = 0;
            this.wednesdayTabPage.Tag = "3";
            this.wednesdayTabPage.Text = "Среда";
            this.wednesdayTabPage.UseVisualStyleBackColor = true;
            // 
            // thursdayTabPage
            // 
            this.thursdayTabPage.Location = new System.Drawing.Point(4, 26);
            this.thursdayTabPage.Name = "thursdayTabPage";
            this.thursdayTabPage.Size = new System.Drawing.Size(818, 416);
            this.thursdayTabPage.TabIndex = 0;
            this.thursdayTabPage.Tag = "4";
            this.thursdayTabPage.Text = "Четверг";
            this.thursdayTabPage.UseVisualStyleBackColor = true;
            // 
            // fridayTabPage
            // 
            this.fridayTabPage.Location = new System.Drawing.Point(4, 26);
            this.fridayTabPage.Name = "fridayTabPage";
            this.fridayTabPage.Size = new System.Drawing.Size(818, 416);
            this.fridayTabPage.TabIndex = 0;
            this.fridayTabPage.Tag = "5";
            this.fridayTabPage.Text = "Пятница";
            this.fridayTabPage.UseVisualStyleBackColor = true;
            // 
            // saturdayTabPage
            // 
            this.saturdayTabPage.Location = new System.Drawing.Point(4, 26);
            this.saturdayTabPage.Name = "saturdayTabPage";
            this.saturdayTabPage.Size = new System.Drawing.Size(818, 416);
            this.saturdayTabPage.TabIndex = 0;
            this.saturdayTabPage.Tag = "6";
            this.saturdayTabPage.Text = "Суббота";
            this.saturdayTabPage.UseVisualStyleBackColor = true;
            // 
            // sundayTabPage
            // 
            this.sundayTabPage.Location = new System.Drawing.Point(4, 26);
            this.sundayTabPage.Name = "sundayTabPage";
            this.sundayTabPage.Size = new System.Drawing.Size(818, 416);
            this.sundayTabPage.TabIndex = 0;
            this.sundayTabPage.Tag = "0";
            this.sundayTabPage.Text = "Воскресенье";
            this.sundayTabPage.UseVisualStyleBackColor = true;
            // 
            // EditServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 506);
            this.Controls.Add(this.serviceTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(820, 540);
            this.Name = "EditServiceForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Редактирование услуги";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditServiceForm_FormClosing);
            this.Load += new System.EventHandler(this.EditServiceForm_Load);
            this.parametersTabPage.ResumeLayout(false);
            this.exceptionScheduleTabPage.ResumeLayout(false);
            this.exceptionScheduleTabPage.PerformLayout();
            this.commonTabPage.ResumeLayout(false);
            this.servicePropertiesTabControl.ResumeLayout(false);
            this.commonPropertiesTabPage.ResumeLayout(false);
            this.commonPropertiesTabPage.PerformLayout();
            this.additionalPropertiesTabPage.ResumeLayout(false);
            this.additionalPropertiesTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientCallDelayUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeIntervalRoundingUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxSubjectsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxEarlyDaysUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.priorityUpDown)).EndInit();
            this.designPropertiesTabControl.ResumeLayout(false);
            this.designPropertiesTabControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fontSizeTrackBar)).EndInit();
            this.serviceTabControl.ResumeLayout(false);
            this.stepsTabPage.ResumeLayout(false);
            this.weekdayScheduleTabPage.ResumeLayout(false);
            this.weekdayTabControl.ResumeLayout(false);
            this.mondayTabPage.ResumeLayout(false);
            this.weekdaySchedulePanel.ResumeLayout(false);
            this.weekdaySchedulePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl serviceTabControl;
        private System.Windows.Forms.TabPage commonTabPage;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TabPage exceptionScheduleTabPage;
        private System.Windows.Forms.DateTimePicker exceptionScheduleDatePicker;
        private System.Windows.Forms.CheckBox exceptionScheduleCheckBox;
        private System.Windows.Forms.TabPage parametersTabPage;
        private System.Windows.Forms.TabPage weekdayScheduleTabPage;
        private System.Windows.Forms.TabControl weekdayTabControl;
        private System.Windows.Forms.TabPage mondayTabPage;
        private Administrator.ScheduleControl weekdayScheduleControl;
        private System.Windows.Forms.TabPage tuesdayTabPage;
        private System.Windows.Forms.TabPage wednesdayTabPage;
        private System.Windows.Forms.TabPage thursdayTabPage;
        private System.Windows.Forms.TabPage fridayTabPage;
        private System.Windows.Forms.TabPage saturdayTabPage;
        private System.Windows.Forms.TabPage sundayTabPage;
        private System.Windows.Forms.CheckBox weekdayScheduleCheckBox;
        private System.Windows.Forms.Label liveRegistratorLabel;
        private System.Windows.Forms.Label earlyRegistratorLabel;
        private System.Windows.Forms.NumericUpDown maxSubjectsUpDown;
        private System.Windows.Forms.Label maxSubjectsLabel;
        private System.Windows.Forms.Label maxEarlyDaysLabel;
        private System.Windows.Forms.NumericUpDown maxEarlyDaysUpDown;
        private System.Windows.Forms.Label maxEarlyDaysDaysLabel;
        private System.Windows.Forms.CheckBox clientRequireCheckBox;
        private System.Windows.Forms.NumericUpDown timeIntervalRoundingUpDown;
        private System.Windows.Forms.Label timeIntervalRoundingMinLabel;
        private System.Windows.Forms.Label timeIntervalRoundingLabel;
        private System.Windows.Forms.TabPage stepsTabPage;
        private Administrator.ScheduleControl exceptionScheduleControl;
        private System.Windows.Forms.Panel weekdaySchedulePanel;
        private ServiceStepsControl serviceStepsControl;
        private ServiceParametersControl serviceParametersControl;
        private System.Windows.Forms.Label clientCallDelaySecondsLabel;
        private System.Windows.Forms.Label clientCallDelayLabel;
        private System.Windows.Forms.NumericUpDown clientCallDelayUpDown;
        private System.Windows.Forms.TabControl servicePropertiesTabControl;
        private System.Windows.Forms.TabPage commonPropertiesTabPage;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.Label commentLabel;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label tagsLabel;
        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.TextBox tagsTextBox;
        private System.Windows.Forms.Label codeLabel;
        private System.Windows.Forms.TabPage additionalPropertiesTabPage;
        private System.Windows.Forms.NumericUpDown priorityUpDown;
        private System.Windows.Forms.Label priorityLabel;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.CheckBox isPlanSubjectsCheckBox;
        private System.Windows.Forms.TextBox linkTextBox;
        private System.Windows.Forms.Label linkLabel;
        private UI.WinForms.EnumFlagsControl earlyRegistratorFlagsControl;
        private UI.WinForms.EnumFlagsControl liveRegistratorFlagsControl;
        private System.Windows.Forms.CheckBox isUseTypeCheckBox;
        private System.Windows.Forms.TabPage designPropertiesTabControl;
        private System.Windows.Forms.Label fontSizeLabel;
        private System.Windows.Forms.TrackBar fontSizeTrackBar;

    }
}