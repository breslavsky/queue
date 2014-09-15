namespace Queue.Manager
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
            this.clientMobileTextBox = new System.Windows.Forms.MaskedTextBox();
            this.mobileLabel = new System.Windows.Forms.Label();
            this.surnameLabel = new System.Windows.Forms.Label();
            this.clientSurnameTextBox = new System.Windows.Forms.TextBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.clientNameTextBox = new System.Windows.Forms.TextBox();
            this.patronymicLabel = new System.Windows.Forms.Label();
            this.clientPatronymicTextBox = new System.Windows.Forms.TextBox();
            this.couponAutoPrintCheckBox = new System.Windows.Forms.CheckBox();
            this.queueTypePanel = new System.Windows.Forms.Panel();
            this.clearCurrentClientButton = new System.Windows.Forms.Button();
            this.clientsListBox = new System.Windows.Forms.ListBox();
            this.queueTypeLiveGroupBox = new System.Windows.Forms.GroupBox();
            this.liveStatusLabel = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.printersComboBox = new System.Windows.Forms.ComboBox();
            this.printButton = new System.Windows.Forms.Button();
            this.subjectsUpDown = new System.Windows.Forms.NumericUpDown();
            this.subjectsLabel = new System.Windows.Forms.Label();
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.queueTypeEarlyGroupBox.SuspendLayout();
            this.clientGroupBox.SuspendLayout();
            this.queueTypePanel.SuspendLayout();
            this.queueTypeLiveGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).BeginInit();
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
            this.addButton.TabIndex = 0;
            this.addButton.Text = "Добавить";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // priorityCheckBox
            // 
            this.priorityCheckBox.AutoSize = true;
            this.priorityCheckBox.Location = new System.Drawing.Point(70, 490);
            this.priorityCheckBox.Name = "priorityCheckBox";
            this.priorityCheckBox.Size = new System.Drawing.Size(121, 17);
            this.priorityCheckBox.TabIndex = 0;
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
            this.queueTypeEarlyGroupBox.TabIndex = 0;
            this.queueTypeEarlyGroupBox.TabStop = false;
            this.queueTypeEarlyGroupBox.Text = "Предварительная запись";
            // 
            // earlyStatusLabel
            // 
            this.earlyStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.earlyStatusLabel.AutoSize = true;
            this.earlyStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.earlyStatusLabel.ForeColor = System.Drawing.Color.Red;
            this.earlyStatusLabel.Location = new System.Drawing.Point(9, 25);
            this.earlyStatusLabel.Name = "earlyStatusLabel";
            this.earlyStatusLabel.Size = new System.Drawing.Size(10, 13);
            this.earlyStatusLabel.TabIndex = 0;
            this.earlyStatusLabel.Text = "-";
            // 
            // requestDateLabel
            // 
            this.requestDateLabel.AutoSize = true;
            this.requestDateLabel.Location = new System.Drawing.Point(10, 50);
            this.requestDateLabel.Name = "requestDateLabel";
            this.requestDateLabel.Size = new System.Drawing.Size(72, 13);
            this.requestDateLabel.TabIndex = 0;
            this.requestDateLabel.Text = "Дата записи";
            // 
            // earlyDatePicker
            // 
            this.earlyDatePicker.Location = new System.Drawing.Point(90, 50);
            this.earlyDatePicker.Name = "earlyDatePicker";
            this.earlyDatePicker.Size = new System.Drawing.Size(160, 20);
            this.earlyDatePicker.TabIndex = 0;
            this.earlyDatePicker.ValueChanged += new System.EventHandler(this.earlyDatePicker_ValueChanged);
            // 
            // freeTimeLabel
            // 
            this.freeTimeLabel.AutoSize = true;
            this.freeTimeLabel.Location = new System.Drawing.Point(90, 80);
            this.freeTimeLabel.Name = "freeTimeLabel";
            this.freeTimeLabel.Size = new System.Drawing.Size(79, 13);
            this.freeTimeLabel.TabIndex = 0;
            this.freeTimeLabel.Text = "Время записи";
            // 
            // freeTimeComboBox
            // 
            this.freeTimeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.freeTimeComboBox.Enabled = false;
            this.freeTimeComboBox.FormattingEnabled = true;
            this.freeTimeComboBox.Location = new System.Drawing.Point(180, 80);
            this.freeTimeComboBox.Name = "freeTimeComboBox";
            this.freeTimeComboBox.Size = new System.Drawing.Size(70, 21);
            this.freeTimeComboBox.TabIndex = 0;
            // 
            // earlyRadioButton
            // 
            this.earlyRadioButton.AutoSize = true;
            this.earlyRadioButton.Location = new System.Drawing.Point(122, 285);
            this.earlyRadioButton.Name = "earlyRadioButton";
            this.earlyRadioButton.Size = new System.Drawing.Size(155, 17);
            this.earlyRadioButton.TabIndex = 0;
            this.earlyRadioButton.Text = "Предварительная запись";
            this.earlyRadioButton.CheckedChanged += new System.EventHandler(this.earlyRadioButton_CheckedChanged);
            // 
            // liveRadioButton
            // 
            this.liveRadioButton.AutoSize = true;
            this.liveRadioButton.Checked = true;
            this.liveRadioButton.Location = new System.Drawing.Point(15, 285);
            this.liveRadioButton.Name = "liveRadioButton";
            this.liveRadioButton.Size = new System.Drawing.Size(104, 17);
            this.liveRadioButton.TabIndex = 0;
            this.liveRadioButton.TabStop = true;
            this.liveRadioButton.Text = "Живая очередь";
            this.liveRadioButton.UseVisualStyleBackColor = true;
            this.liveRadioButton.CheckedChanged += new System.EventHandler(this.liveRadioButton_CheckedChanged);
            // 
            // clientGroupBox
            // 
            this.clientGroupBox.Controls.Add(this.clientMobileTextBox);
            this.clientGroupBox.Controls.Add(this.mobileLabel);
            this.clientGroupBox.Controls.Add(this.surnameLabel);
            this.clientGroupBox.Controls.Add(this.clientSurnameTextBox);
            this.clientGroupBox.Controls.Add(this.nameLabel);
            this.clientGroupBox.Controls.Add(this.clientNameTextBox);
            this.clientGroupBox.Controls.Add(this.patronymicLabel);
            this.clientGroupBox.Controls.Add(this.clientPatronymicTextBox);
            this.clientGroupBox.Location = new System.Drawing.Point(10, 13);
            this.clientGroupBox.Margin = new System.Windows.Forms.Padding(5);
            this.clientGroupBox.Name = "clientGroupBox";
            this.clientGroupBox.Size = new System.Drawing.Size(260, 152);
            this.clientGroupBox.TabIndex = 0;
            this.clientGroupBox.TabStop = false;
            this.clientGroupBox.Text = "Данные клиента";
            // 
            // clientMobileTextBox
            // 
            this.clientMobileTextBox.Location = new System.Drawing.Point(80, 120);
            this.clientMobileTextBox.Mask = "8(999)-000-0000";
            this.clientMobileTextBox.Name = "clientMobileTextBox";
            this.clientMobileTextBox.Size = new System.Drawing.Size(90, 20);
            this.clientMobileTextBox.TabIndex = 3;
            this.clientMobileTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.clientMobileTextBox_KeyDown);
            // 
            // mobileLabel
            // 
            this.mobileLabel.AutoSize = true;
            this.mobileLabel.Location = new System.Drawing.Point(10, 125);
            this.mobileLabel.Name = "mobileLabel";
            this.mobileLabel.Size = new System.Drawing.Size(66, 13);
            this.mobileLabel.TabIndex = 1;
            this.mobileLabel.Text = "Мобильный";
            // 
            // surnameLabel
            // 
            this.surnameLabel.AutoSize = true;
            this.surnameLabel.Location = new System.Drawing.Point(10, 30);
            this.surnameLabel.Name = "surnameLabel";
            this.surnameLabel.Size = new System.Drawing.Size(56, 13);
            this.surnameLabel.TabIndex = 0;
            this.surnameLabel.Text = "Фамилия";
            // 
            // clientSurnameTextBox
            // 
            this.clientSurnameTextBox.Location = new System.Drawing.Point(80, 30);
            this.clientSurnameTextBox.Name = "clientSurnameTextBox";
            this.clientSurnameTextBox.Size = new System.Drawing.Size(135, 20);
            this.clientSurnameTextBox.TabIndex = 0;
            this.clientSurnameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.clientSurnameTextBox_KeyDown);
            this.clientSurnameTextBox.Leave += new System.EventHandler(this.clientSurnameTextBox_Leave);
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(10, 60);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(29, 13);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Имя";
            // 
            // clientNameTextBox
            // 
            this.clientNameTextBox.Location = new System.Drawing.Point(80, 60);
            this.clientNameTextBox.Name = "clientNameTextBox";
            this.clientNameTextBox.Size = new System.Drawing.Size(90, 20);
            this.clientNameTextBox.TabIndex = 0;
            this.clientNameTextBox.Leave += new System.EventHandler(this.clientNameTextBox_Leave);
            // 
            // patronymicLabel
            // 
            this.patronymicLabel.AutoSize = true;
            this.patronymicLabel.Location = new System.Drawing.Point(10, 90);
            this.patronymicLabel.Name = "patronymicLabel";
            this.patronymicLabel.Size = new System.Drawing.Size(54, 13);
            this.patronymicLabel.TabIndex = 0;
            this.patronymicLabel.Text = "Отчество";
            // 
            // clientPatronymicTextBox
            // 
            this.clientPatronymicTextBox.Location = new System.Drawing.Point(80, 90);
            this.clientPatronymicTextBox.Name = "clientPatronymicTextBox";
            this.clientPatronymicTextBox.Size = new System.Drawing.Size(135, 20);
            this.clientPatronymicTextBox.TabIndex = 0;
            this.clientPatronymicTextBox.Leave += new System.EventHandler(this.clientPatronymicTextBox_Leave);
            // 
            // couponAutoPrintCheckBox
            // 
            this.couponAutoPrintCheckBox.AutoSize = true;
            this.couponAutoPrintCheckBox.Checked = true;
            this.couponAutoPrintCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.couponAutoPrintCheckBox.Location = new System.Drawing.Point(35, 35);
            this.couponAutoPrintCheckBox.Margin = new System.Windows.Forms.Padding(5);
            this.couponAutoPrintCheckBox.Name = "couponAutoPrintCheckBox";
            this.couponAutoPrintCheckBox.Size = new System.Drawing.Size(147, 17);
            this.couponAutoPrintCheckBox.TabIndex = 0;
            this.couponAutoPrintCheckBox.Text = "Автоматическая печать";
            this.couponAutoPrintCheckBox.UseVisualStyleBackColor = true;
            // 
            // queueTypePanel
            // 
            this.queueTypePanel.Controls.Add(this.clearCurrentClientButton);
            this.queueTypePanel.Controls.Add(this.clientsListBox);
            this.queueTypePanel.Controls.Add(this.queueTypeLiveGroupBox);
            this.queueTypePanel.Controls.Add(this.panel1);
            this.queueTypePanel.Controls.Add(this.subjectsUpDown);
            this.queueTypePanel.Controls.Add(this.subjectsLabel);
            this.queueTypePanel.Controls.Add(this.clientGroupBox);
            this.queueTypePanel.Controls.Add(this.liveRadioButton);
            this.queueTypePanel.Controls.Add(this.earlyRadioButton);
            this.queueTypePanel.Controls.Add(this.queueTypeEarlyGroupBox);
            this.queueTypePanel.Controls.Add(this.priorityCheckBox);
            this.queueTypePanel.Controls.Add(this.addButton);
            this.queueTypePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.queueTypePanel.Location = new System.Drawing.Point(404, 0);
            this.queueTypePanel.Margin = new System.Windows.Forms.Padding(0);
            this.queueTypePanel.Name = "queueTypePanel";
            this.queueTypePanel.Size = new System.Drawing.Size(280, 592);
            this.queueTypePanel.TabIndex = 0;
            // 
            // clearCurrentClientButton
            // 
            this.clearCurrentClientButton.Location = new System.Drawing.Point(240, 175);
            this.clearCurrentClientButton.Name = "clearCurrentClientButton";
            this.clearCurrentClientButton.Size = new System.Drawing.Size(30, 70);
            this.clearCurrentClientButton.TabIndex = 2;
            this.clearCurrentClientButton.Text = "-";
            this.clearCurrentClientButton.UseVisualStyleBackColor = true;
            this.clearCurrentClientButton.Click += new System.EventHandler(this.clearCurrentClientButton_Click);
            // 
            // clientsListBox
            // 
            this.clientsListBox.FormattingEnabled = true;
            this.clientsListBox.Location = new System.Drawing.Point(20, 175);
            this.clientsListBox.Name = "clientsListBox";
            this.clientsListBox.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.clientsListBox.Size = new System.Drawing.Size(215, 69);
            this.clientsListBox.TabIndex = 1;
            this.clientsListBox.SelectedValueChanged += new System.EventHandler(this.clientsListBox_SelectedValueChanged);
            // 
            // queueTypeLiveGroupBox
            // 
            this.queueTypeLiveGroupBox.Controls.Add(this.liveStatusLabel);
            this.queueTypeLiveGroupBox.Location = new System.Drawing.Point(10, 315);
            this.queueTypeLiveGroupBox.Margin = new System.Windows.Forms.Padding(5);
            this.queueTypeLiveGroupBox.Name = "queueTypeLiveGroupBox";
            this.queueTypeLiveGroupBox.Size = new System.Drawing.Size(260, 45);
            this.queueTypeLiveGroupBox.TabIndex = 0;
            this.queueTypeLiveGroupBox.TabStop = false;
            this.queueTypeLiveGroupBox.Text = "Живая очередь";
            // 
            // liveStatusLabel
            // 
            this.liveStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.liveStatusLabel.AutoSize = true;
            this.liveStatusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.liveStatusLabel.ForeColor = System.Drawing.Color.Red;
            this.liveStatusLabel.Location = new System.Drawing.Point(9, 25);
            this.liveStatusLabel.Name = "liveStatusLabel";
            this.liveStatusLabel.Size = new System.Drawing.Size(10, 13);
            this.liveStatusLabel.TabIndex = 0;
            this.liveStatusLabel.Text = "-";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.printersComboBox);
            this.panel1.Controls.Add(this.printButton);
            this.panel1.Controls.Add(this.couponAutoPrintCheckBox);
            this.panel1.Location = new System.Drawing.Point(0, 530);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(280, 60);
            this.panel1.TabIndex = 0;
            // 
            // printersComboBox
            // 
            this.printersComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.printersComboBox.FormattingEnabled = true;
            this.printersComboBox.Location = new System.Drawing.Point(15, 5);
            this.printersComboBox.Name = "printersComboBox";
            this.printersComboBox.Size = new System.Drawing.Size(250, 21);
            this.printersComboBox.TabIndex = 0;
            this.printersComboBox.SelectionChangeCommitted += new System.EventHandler(this.printersComboBox_SelectionChangeCommitted);
            // 
            // printButton
            // 
            this.printButton.Location = new System.Drawing.Point(190, 30);
            this.printButton.Name = "printButton";
            this.printButton.Size = new System.Drawing.Size(75, 25);
            this.printButton.TabIndex = 0;
            this.printButton.Text = "Печать";
            this.printButton.Click += new System.EventHandler(this.printButton_Click);
            // 
            // subjectsUpDown
            // 
            this.subjectsUpDown.Location = new System.Drawing.Point(85, 255);
            this.subjectsUpDown.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.subjectsUpDown.Name = "subjectsUpDown";
            this.subjectsUpDown.Size = new System.Drawing.Size(50, 20);
            this.subjectsUpDown.TabIndex = 0;
            this.subjectsUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // subjectsLabel
            // 
            this.subjectsLabel.AutoSize = true;
            this.subjectsLabel.Location = new System.Drawing.Point(15, 260);
            this.subjectsLabel.Name = "subjectsLabel";
            this.subjectsLabel.Size = new System.Drawing.Size(57, 13);
            this.subjectsLabel.TabIndex = 0;
            this.subjectsLabel.Text = "Объектов";
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
            this.queueTypeEarlyGroupBox.PerformLayout();
            this.clientGroupBox.ResumeLayout(false);
            this.clientGroupBox.PerformLayout();
            this.queueTypePanel.ResumeLayout(false);
            this.queueTypePanel.PerformLayout();
            this.queueTypeLiveGroupBox.ResumeLayout(false);
            this.queueTypeLiveGroupBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.subjectsUpDown)).EndInit();
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
        private System.Windows.Forms.ComboBox printersComboBox;
        private System.Windows.Forms.Button printButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label liveStatusLabel;
        private System.Windows.Forms.Label earlyStatusLabel;
        private System.Windows.Forms.GroupBox queueTypeLiveGroupBox;
        private System.Windows.Forms.Label mobileLabel;
        private System.Windows.Forms.MaskedTextBox clientMobileTextBox;
        private System.Windows.Forms.ListBox clientsListBox;
        private System.Windows.Forms.Button clearCurrentClientButton;

    }
}