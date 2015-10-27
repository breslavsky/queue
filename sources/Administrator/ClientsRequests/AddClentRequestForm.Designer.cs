namespace Queue.Administrator
{
    partial class AddClentRequestForm
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
            this.servicesTreeView = new System.Windows.Forms.TreeView();
            this.addButton = new System.Windows.Forms.Button();
            this.priorityCheckBox = new System.Windows.Forms.CheckBox();
            this.queueTypeEarlyGroupBox = new System.Windows.Forms.GroupBox();
            this.earlyStatusLabel = new System.Windows.Forms.Label();
            this.requestDateLabel = new System.Windows.Forms.Label();
            this.earlyDatePicker = new System.Windows.Forms.DateTimePicker();
            this.freeTimeLabel = new System.Windows.Forms.Label();
            this.freeTimeComboBox = new System.Windows.Forms.ComboBox();
            this.earlyRadioButton = new System.Windows.Forms.RadioButton();
            this.liveRadioButton = new System.Windows.Forms.RadioButton();
            this.clientGroupBox = new System.Windows.Forms.GroupBox();
            this.surnameLabel = new System.Windows.Forms.Label();
            this.clientSurnameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.clientNameTextBox = new System.Windows.Forms.TextBox();
            this.patronymicLabel = new System.Windows.Forms.Label();
            this.clientPatronymicTextBox = new System.Windows.Forms.TextBox();
            this.mobileLabel = new System.Windows.Forms.Label();
            this.clientMobileTextBox = new System.Windows.Forms.MaskedTextBox();
            this.couponAutoPrintCheckBox = new System.Windows.Forms.CheckBox();
            this.queueTypePanel = new System.Windows.Forms.Panel();
            this.clientsListBox = new System.Windows.Forms.ListBox();
            this.clearCurrentClientButton = new System.Windows.Forms.Button();
            this.subjectsLabel = new System.Windows.Forms.Label();
            this.subjectsUpDown = new System.Windows.Forms.NumericUpDown();
            this.queueTypeLiveGroupBox = new System.Windows.Forms.GroupBox();
            this.liveStatusLabel = new System.Windows.Forms.Label();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.queueTypeEarlyGroupBox.SuspendLayout();
            this.clientGroupBox.SuspendLayout();
            this.queueTypePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).BeginInit();
            this.queueTypeLiveGroupBox.SuspendLayout();
            this.mainTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // servicesTreeView
            // 
            this.servicesTreeView.AllowDrop = true;
            this.servicesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.servicesTreeView.HideSelection = false;
            this.servicesTreeView.Location = new System.Drawing.Point(0, 0);
            this.servicesTreeView.Margin = new System.Windows.Forms.Padding(0);
            this.servicesTreeView.Name = "servicesTreeView";
            this.servicesTreeView.Size = new System.Drawing.Size(404, 592);
            this.servicesTreeView.TabIndex = 0;
            this.servicesTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.servicesTreeView_AfterExpand);
            this.servicesTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.servicesTreeView_BeforeSelect);
            this.servicesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.servicesTreeView_AfterSelect);
            // 
            // addButton
            // 
            this.addButton.Enabled = false;
            this.addButton.Location = new System.Drawing.Point(195, 487);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 25);
            this.addButton.TabIndex = 12;
            this.addButton.Text = "Добавить";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // priorityCheckBox
            // 
            this.priorityCheckBox.Location = new System.Drawing.Point(65, 490);
            this.priorityCheckBox.Name = "priorityCheckBox";
            this.priorityCheckBox.Size = new System.Drawing.Size(130, 20);
            this.priorityCheckBox.TabIndex = 11;
            this.priorityCheckBox.Text = "Приоритет вызова";
            this.priorityCheckBox.UseVisualStyleBackColor = true;
            // 
            // queueTypeEarlyGroupBox
            // 
            this.queueTypeEarlyGroupBox.Controls.Add(this.earlyStatusLabel);
            this.queueTypeEarlyGroupBox.Controls.Add(this.requestDateLabel);
            this.queueTypeEarlyGroupBox.Controls.Add(this.earlyDatePicker);
            this.queueTypeEarlyGroupBox.Controls.Add(this.freeTimeLabel);
            this.queueTypeEarlyGroupBox.Controls.Add(this.freeTimeComboBox);
            this.queueTypeEarlyGroupBox.Enabled = false;
            this.queueTypeEarlyGroupBox.Location = new System.Drawing.Point(10, 370);
            this.queueTypeEarlyGroupBox.Margin = new System.Windows.Forms.Padding(5);
            this.queueTypeEarlyGroupBox.Name = "queueTypeEarlyGroupBox";
            this.queueTypeEarlyGroupBox.Size = new System.Drawing.Size(260, 110);
            this.queueTypeEarlyGroupBox.TabIndex = 9;
            this.queueTypeEarlyGroupBox.TabStop = false;
            this.queueTypeEarlyGroupBox.Text = "Предварительная запись";
            // 
            // earlyStatusLabel
            // 
            this.earlyStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.earlyStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.earlyStatusLabel.ForeColor = System.Drawing.Color.Red;
            this.earlyStatusLabel.Location = new System.Drawing.Point(9, 25);
            this.earlyStatusLabel.Name = "earlyStatusLabel";
            this.earlyStatusLabel.Size = new System.Drawing.Size(246, 15);
            this.earlyStatusLabel.TabIndex = 0;
            this.earlyStatusLabel.Text = "-";
            // 
            // requestDateLabel
            // 
            this.requestDateLabel.Location = new System.Drawing.Point(10, 50);
            this.requestDateLabel.Name = "requestDateLabel";
            this.requestDateLabel.Size = new System.Drawing.Size(75, 20);
            this.requestDateLabel.TabIndex = 0;
            this.requestDateLabel.Text = "Дата записи";
            this.requestDateLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // earlyDatePicker
            // 
            this.earlyDatePicker.Location = new System.Drawing.Point(90, 50);
            this.earlyDatePicker.Name = "earlyDatePicker";
            this.earlyDatePicker.Size = new System.Drawing.Size(160, 20);
            this.earlyDatePicker.TabIndex = 9;
            this.earlyDatePicker.ValueChanged += new System.EventHandler(this.earlyDatePicker_ValueChanged);
            // 
            // freeTimeLabel
            // 
            this.freeTimeLabel.Location = new System.Drawing.Point(90, 80);
            this.freeTimeLabel.Name = "freeTimeLabel";
            this.freeTimeLabel.Size = new System.Drawing.Size(85, 20);
            this.freeTimeLabel.TabIndex = 0;
            this.freeTimeLabel.Text = "Время записи";
            this.freeTimeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // freeTimeComboBox
            // 
            this.freeTimeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.freeTimeComboBox.Enabled = false;
            this.freeTimeComboBox.FormattingEnabled = true;
            this.freeTimeComboBox.Location = new System.Drawing.Point(180, 80);
            this.freeTimeComboBox.Name = "freeTimeComboBox";
            this.freeTimeComboBox.Size = new System.Drawing.Size(70, 21);
            this.freeTimeComboBox.TabIndex = 10;
            // 
            // earlyRadioButton
            // 
            this.earlyRadioButton.Location = new System.Drawing.Point(140, 275);
            this.earlyRadioButton.Name = "earlyRadioButton";
            this.earlyRadioButton.Size = new System.Drawing.Size(130, 30);
            this.earlyRadioButton.TabIndex = 9;
            this.earlyRadioButton.Text = "Предварительная запись";
            this.earlyRadioButton.CheckedChanged += new System.EventHandler(this.earlyRadioButton_CheckedChanged);
            // 
            // liveRadioButton
            // 
            this.liveRadioButton.Checked = true;
            this.liveRadioButton.Location = new System.Drawing.Point(15, 275);
            this.liveRadioButton.Name = "liveRadioButton";
            this.liveRadioButton.Size = new System.Drawing.Size(120, 30);
            this.liveRadioButton.TabIndex = 8;
            this.liveRadioButton.TabStop = true;
            this.liveRadioButton.Text = "Живая очередь";
            this.liveRadioButton.UseVisualStyleBackColor = true;
            this.liveRadioButton.CheckedChanged += new System.EventHandler(this.liveRadioButton_CheckedChanged);
            // 
            // clientGroupBox
            // 
            this.clientGroupBox.Controls.Add(this.surnameLabel);
            this.clientGroupBox.Controls.Add(this.clientSurnameTextBox);
            this.clientGroupBox.Controls.Add(this.nameLabel);
            this.clientGroupBox.Controls.Add(this.clientNameTextBox);
            this.clientGroupBox.Controls.Add(this.patronymicLabel);
            this.clientGroupBox.Controls.Add(this.clientPatronymicTextBox);
            this.clientGroupBox.Controls.Add(this.mobileLabel);
            this.clientGroupBox.Controls.Add(this.clientMobileTextBox);
            this.clientGroupBox.Location = new System.Drawing.Point(10, 13);
            this.clientGroupBox.Margin = new System.Windows.Forms.Padding(5);
            this.clientGroupBox.Name = "clientGroupBox";
            this.clientGroupBox.Size = new System.Drawing.Size(260, 152);
            this.clientGroupBox.TabIndex = 0;
            this.clientGroupBox.TabStop = false;
            this.clientGroupBox.Text = "Данные клиента";
            // 
            // surnameLabel
            // 
            this.surnameLabel.Location = new System.Drawing.Point(10, 30);
            this.surnameLabel.Name = "surnameLabel";
            this.surnameLabel.Size = new System.Drawing.Size(70, 20);
            this.surnameLabel.TabIndex = 16;
            this.surnameLabel.Text = "Фамилия";
            this.surnameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // clientSurnameTextBox
            // 
            this.clientSurnameTextBox.Location = new System.Drawing.Point(85, 30);
            this.clientSurnameTextBox.Name = "clientSurnameTextBox";
            this.clientSurnameTextBox.Size = new System.Drawing.Size(135, 20);
            this.clientSurnameTextBox.TabIndex = 1;
            this.clientSurnameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.clientSurnameTextBox_KeyDown);
            this.clientSurnameTextBox.Leave += new System.EventHandler(this.clientSurnameTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(10, 60);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(70, 20);
            this.nameLabel.TabIndex = 17;
            this.nameLabel.Text = "Имя";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // clientNameTextBox
            // 
            this.clientNameTextBox.Location = new System.Drawing.Point(85, 60);
            this.clientNameTextBox.Name = "clientNameTextBox";
            this.clientNameTextBox.Size = new System.Drawing.Size(90, 20);
            this.clientNameTextBox.TabIndex = 2;
            this.clientNameTextBox.Leave += new System.EventHandler(this.clientNameTextBox_Leave);
            // 
            // patronymicLabel
            // 
            this.patronymicLabel.Location = new System.Drawing.Point(10, 90);
            this.patronymicLabel.Name = "patronymicLabel";
            this.patronymicLabel.Size = new System.Drawing.Size(70, 20);
            this.patronymicLabel.TabIndex = 18;
            this.patronymicLabel.Text = "Отчество";
            this.patronymicLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // clientPatronymicTextBox
            // 
            this.clientPatronymicTextBox.Location = new System.Drawing.Point(85, 90);
            this.clientPatronymicTextBox.Name = "clientPatronymicTextBox";
            this.clientPatronymicTextBox.Size = new System.Drawing.Size(135, 20);
            this.clientPatronymicTextBox.TabIndex = 3;
            this.clientPatronymicTextBox.Leave += new System.EventHandler(this.clientPatronymicTextBox_Leave);
            // 
            // mobileLabel
            // 
            this.mobileLabel.Location = new System.Drawing.Point(10, 120);
            this.mobileLabel.Name = "mobileLabel";
            this.mobileLabel.Size = new System.Drawing.Size(70, 20);
            this.mobileLabel.TabIndex = 19;
            this.mobileLabel.Text = "Мобильный";
            this.mobileLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // clientMobileTextBox
            // 
            this.clientMobileTextBox.Location = new System.Drawing.Point(85, 120);
            this.clientMobileTextBox.Mask = "8(999)-000-0000";
            this.clientMobileTextBox.Name = "clientMobileTextBox";
            this.clientMobileTextBox.Size = new System.Drawing.Size(90, 20);
            this.clientMobileTextBox.TabIndex = 4;
            this.clientMobileTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.clientMobileTextBox_KeyDown);
            // 
            // couponAutoPrintCheckBox
            // 
            this.couponAutoPrintCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.couponAutoPrintCheckBox.Checked = true;
            this.couponAutoPrintCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.couponAutoPrintCheckBox.Location = new System.Drawing.Point(5, 560);
            this.couponAutoPrintCheckBox.Margin = new System.Windows.Forms.Padding(5);
            this.couponAutoPrintCheckBox.Name = "couponAutoPrintCheckBox";
            this.couponAutoPrintCheckBox.Size = new System.Drawing.Size(270, 30);
            this.couponAutoPrintCheckBox.TabIndex = 14;
            this.couponAutoPrintCheckBox.Text = "Автоматическая печать";
            this.couponAutoPrintCheckBox.UseVisualStyleBackColor = true;
            // 
            // queueTypePanel
            // 
            this.queueTypePanel.Controls.Add(this.couponAutoPrintCheckBox);
            this.queueTypePanel.Controls.Add(this.clientGroupBox);
            this.queueTypePanel.Controls.Add(this.clientsListBox);
            this.queueTypePanel.Controls.Add(this.clearCurrentClientButton);
            this.queueTypePanel.Controls.Add(this.subjectsLabel);
            this.queueTypePanel.Controls.Add(this.subjectsUpDown);
            this.queueTypePanel.Controls.Add(this.liveRadioButton);
            this.queueTypePanel.Controls.Add(this.earlyRadioButton);
            this.queueTypePanel.Controls.Add(this.queueTypeLiveGroupBox);
            this.queueTypePanel.Controls.Add(this.queueTypeEarlyGroupBox);
            this.queueTypePanel.Controls.Add(this.priorityCheckBox);
            this.queueTypePanel.Controls.Add(this.addButton);
            this.queueTypePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queueTypePanel.Location = new System.Drawing.Point(404, 0);
            this.queueTypePanel.Margin = new System.Windows.Forms.Padding(0);
            this.queueTypePanel.Name = "queueTypePanel";
            this.queueTypePanel.Size = new System.Drawing.Size(280, 592);
            this.queueTypePanel.TabIndex = 1;
            // 
            // clientsListBox
            // 
            this.clientsListBox.FormattingEnabled = true;
            this.clientsListBox.Location = new System.Drawing.Point(20, 175);
            this.clientsListBox.Name = "clientsListBox";
            this.clientsListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.clientsListBox.Size = new System.Drawing.Size(215, 69);
            this.clientsListBox.TabIndex = 5;
            this.clientsListBox.SelectedValueChanged += new System.EventHandler(this.clientsListBox_SelectedValueChanged);
            // 
            // clearCurrentClientButton
            // 
            this.clearCurrentClientButton.Location = new System.Drawing.Point(240, 175);
            this.clearCurrentClientButton.Name = "clearCurrentClientButton";
            this.clearCurrentClientButton.Size = new System.Drawing.Size(30, 70);
            this.clearCurrentClientButton.TabIndex = 6;
            this.clearCurrentClientButton.Text = "-";
            this.clearCurrentClientButton.UseVisualStyleBackColor = true;
            this.clearCurrentClientButton.Click += new System.EventHandler(this.clearCurrentClientButton_Click);
            // 
            // subjectsLabel
            // 
            this.subjectsLabel.Location = new System.Drawing.Point(15, 255);
            this.subjectsLabel.Name = "subjectsLabel";
            this.subjectsLabel.Size = new System.Drawing.Size(65, 13);
            this.subjectsLabel.TabIndex = 0;
            this.subjectsLabel.Text = "Объектов";
            this.subjectsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.subjectsLabel.Click += new System.EventHandler(this.subjectsLabel_Click);
            // 
            // subjectsUpDown
            // 
            this.subjectsUpDown.Location = new System.Drawing.Point(85, 250);
            this.subjectsUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.subjectsUpDown.Name = "subjectsUpDown";
            this.subjectsUpDown.Size = new System.Drawing.Size(50, 20);
            this.subjectsUpDown.TabIndex = 7;
            this.subjectsUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // queueTypeLiveGroupBox
            // 
            this.queueTypeLiveGroupBox.Controls.Add(this.liveStatusLabel);
            this.queueTypeLiveGroupBox.Location = new System.Drawing.Point(10, 315);
            this.queueTypeLiveGroupBox.Margin = new System.Windows.Forms.Padding(5);
            this.queueTypeLiveGroupBox.Name = "queueTypeLiveGroupBox";
            this.queueTypeLiveGroupBox.Size = new System.Drawing.Size(260, 45);
            this.queueTypeLiveGroupBox.TabIndex = 8;
            this.queueTypeLiveGroupBox.TabStop = false;
            this.queueTypeLiveGroupBox.Text = "Живая очередь";
            // 
            // liveStatusLabel
            // 
            this.liveStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.liveStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.liveStatusLabel.ForeColor = System.Drawing.Color.Red;
            this.liveStatusLabel.Location = new System.Drawing.Point(9, 25);
            this.liveStatusLabel.Name = "liveStatusLabel";
            this.liveStatusLabel.Size = new System.Drawing.Size(246, 15);
            this.liveStatusLabel.TabIndex = 0;
            this.liveStatusLabel.Text = "-";
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 2;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 280F));
            this.mainTableLayoutPanel.Controls.Add(this.servicesTreeView, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.queueTypePanel, 1, 0);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(10, 10);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 1;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(684, 592);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // AddClentRequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 612);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(720, 650);
            this.Name = "AddClentRequestForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Добавить запрос";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddClentRequestForm_FormClosing);
            this.Load += new System.EventHandler(this.AddClientRequestForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.AddClentRequestForm_KeyDown);
            this.queueTypeEarlyGroupBox.ResumeLayout(false);
            this.clientGroupBox.ResumeLayout(false);
            this.clientGroupBox.PerformLayout();
            this.queueTypePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).EndInit();
            this.queueTypeLiveGroupBox.ResumeLayout(false);
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView servicesTreeView;
        private System.Windows.Forms.Panel queueTypePanel;
        private System.Windows.Forms.CheckBox couponAutoPrintCheckBox;
        private System.Windows.Forms.GroupBox clientGroupBox;
        private System.Windows.Forms.Label surnameLabel;
        private System.Windows.Forms.TextBox clientSurnameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox clientNameTextBox;
        private System.Windows.Forms.Label patronymicLabel;
        private System.Windows.Forms.TextBox clientPatronymicTextBox;
        private System.Windows.Forms.RadioButton liveRadioButton;
        private System.Windows.Forms.RadioButton earlyRadioButton;
        private System.Windows.Forms.GroupBox queueTypeEarlyGroupBox;
        private System.Windows.Forms.Label requestDateLabel;
        private System.Windows.Forms.DateTimePicker earlyDatePicker;
        private System.Windows.Forms.Label freeTimeLabel;
        private System.Windows.Forms.ComboBox freeTimeComboBox;
        private System.Windows.Forms.CheckBox priorityCheckBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private System.Windows.Forms.Label subjectsLabel;
        private System.Windows.Forms.NumericUpDown subjectsUpDown;
        private System.Windows.Forms.Label liveStatusLabel;
        private System.Windows.Forms.Label earlyStatusLabel;
        private System.Windows.Forms.GroupBox queueTypeLiveGroupBox;
        private System.Windows.Forms.Label mobileLabel;
        private System.Windows.Forms.MaskedTextBox clientMobileTextBox;
        private System.Windows.Forms.ListBox clientsListBox;
        private System.Windows.Forms.Button clearCurrentClientButton;

    }
}