namespace Queue.UI.WinForms
{
    partial class ScheduleControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.saveButton = new System.Windows.Forms.Button();
            this.addServiceRenderingButton = new System.Windows.Forms.Button();
            this.gridView = new System.Windows.Forms.DataGridView();
            this.renderingModeComboBox = new System.Windows.Forms.ComboBox();
            this.maxClientRequestsMeasureLabel = new System.Windows.Forms.Label();
            this.earlyGroupBox = new System.Windows.Forms.GroupBox();
            this.earlyReservationPercentLabel = new System.Windows.Forms.Label();
            this.earlyStartTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.earlyReservationUpDown = new System.Windows.Forms.NumericUpDown();
            this.earlyFinishTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.earlyReservationLabel = new System.Windows.Forms.Label();
            this.earlyTimeLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.maxClientRequestsUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxClientRequestsLabel = new System.Windows.Forms.Label();
            this.startTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.finishTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.isInterruptionCheckBox = new System.Windows.Forms.CheckBox();
            this.interruptionPanel = new System.Windows.Forms.Panel();
            this.interruptionFinishTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.interruptionStartTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.clientIntervalLabel = new System.Windows.Forms.Label();
            this.clientIntervalUpDown = new System.Windows.Forms.NumericUpDown();
            this.minLabel1 = new System.Windows.Forms.Label();
            this.intersectionLabel = new System.Windows.Forms.Label();
            this.intersectionUpDown = new System.Windows.Forms.NumericUpDown();
            this.minLabel2 = new System.Windows.Forms.Label();
            this.isWorkedCheckBox = new System.Windows.Forms.CheckBox();
            this.schedulePanel = new System.Windows.Forms.Panel();
            this.deleteColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.operatorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serviceStepColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priorityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
            this.earlyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.earlyReservationUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxClientRequestsUpDown)).BeginInit();
            this.interruptionPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientIntervalUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intersectionUpDown)).BeginInit();
            this.schedulePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(135, 265);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // addServiceRenderingButton
            // 
            this.addServiceRenderingButton.Location = new System.Drawing.Point(695, 190);
            this.addServiceRenderingButton.Name = "addServiceRenderingButton";
            this.addServiceRenderingButton.Size = new System.Drawing.Size(80, 25);
            this.addServiceRenderingButton.TabIndex = 0;
            this.addServiceRenderingButton.Text = "Добавить";
            this.addServiceRenderingButton.Click += new System.EventHandler(this.addServiceRenderingButton_Click);
            // 
            // gridView
            // 
            this.gridView.AllowUserToAddRows = false;
            this.gridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deleteColumn,
            this.operatorColumn,
            this.serviceStepColumn,
            this.modeColumn,
            this.priorityColumn});
            this.gridView.Location = new System.Drawing.Point(215, 5);
            this.gridView.MultiSelect = false;
            this.gridView.Name = "gridView";
            this.gridView.ReadOnly = true;
            this.gridView.RowHeadersVisible = false;
            this.gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridView.Size = new System.Drawing.Size(560, 180);
            this.gridView.TabIndex = 0;
            this.gridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.serviceRenderingsGridView_CellClick);
            this.gridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.serviceRenderingsGridView_CellMouseDoubleClick);
            // 
            // renderingModeComboBox
            // 
            this.renderingModeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.renderingModeComboBox.FormattingEnabled = true;
            this.renderingModeComboBox.Location = new System.Drawing.Point(10, 130);
            this.renderingModeComboBox.Name = "renderingModeComboBox";
            this.renderingModeComboBox.Size = new System.Drawing.Size(195, 21);
            this.renderingModeComboBox.TabIndex = 0;
            this.renderingModeComboBox.SelectedIndexChanged += new System.EventHandler(this.renderingModeComboBox_SelectedIndexChanged);
            this.renderingModeComboBox.Leave += new System.EventHandler(this.renderingModeComboBox_Leave);
            // 
            // maxClientRequestsMeasureLabel
            // 
            this.maxClientRequestsMeasureLabel.AutoSize = true;
            this.maxClientRequestsMeasureLabel.Location = new System.Drawing.Point(170, 110);
            this.maxClientRequestsMeasureLabel.Name = "maxClientRequestsMeasureLabel";
            this.maxClientRequestsMeasureLabel.Size = new System.Drawing.Size(23, 13);
            this.maxClientRequestsMeasureLabel.TabIndex = 0;
            this.maxClientRequestsMeasureLabel.Text = "шт.";
            // 
            // earlyGroupBox
            // 
            this.earlyGroupBox.Controls.Add(this.earlyReservationPercentLabel);
            this.earlyGroupBox.Controls.Add(this.earlyStartTimeTextBox);
            this.earlyGroupBox.Controls.Add(this.earlyReservationUpDown);
            this.earlyGroupBox.Controls.Add(this.earlyFinishTimeTextBox);
            this.earlyGroupBox.Controls.Add(this.earlyReservationLabel);
            this.earlyGroupBox.Controls.Add(this.earlyTimeLabel);
            this.earlyGroupBox.Location = new System.Drawing.Point(10, 155);
            this.earlyGroupBox.Name = "earlyGroupBox";
            this.earlyGroupBox.Size = new System.Drawing.Size(195, 75);
            this.earlyGroupBox.TabIndex = 0;
            this.earlyGroupBox.TabStop = false;
            this.earlyGroupBox.Text = "По записи";
            // 
            // earlyReservationPercentLabel
            // 
            this.earlyReservationPercentLabel.AutoSize = true;
            this.earlyReservationPercentLabel.Location = new System.Drawing.Point(155, 50);
            this.earlyReservationPercentLabel.Name = "earlyReservationPercentLabel";
            this.earlyReservationPercentLabel.Size = new System.Drawing.Size(15, 13);
            this.earlyReservationPercentLabel.TabIndex = 0;
            this.earlyReservationPercentLabel.Text = "%";
            // 
            // earlyStartTimeTextBox
            // 
            this.earlyStartTimeTextBox.Location = new System.Drawing.Point(115, 20);
            this.earlyStartTimeTextBox.Mask = "00:00";
            this.earlyStartTimeTextBox.Name = "earlyStartTimeTextBox";
            this.earlyStartTimeTextBox.Size = new System.Drawing.Size(35, 20);
            this.earlyStartTimeTextBox.TabIndex = 0;
            this.earlyStartTimeTextBox.Text = "0000";
            this.earlyStartTimeTextBox.ValidatingType = typeof(System.DateTime);
            this.earlyStartTimeTextBox.Leave += new System.EventHandler(this.earlyStartTimeTextBox_Leave);
            // 
            // earlyReservationUpDown
            // 
            this.earlyReservationUpDown.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.earlyReservationUpDown.Location = new System.Drawing.Point(115, 45);
            this.earlyReservationUpDown.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.earlyReservationUpDown.Name = "earlyReservationUpDown";
            this.earlyReservationUpDown.Size = new System.Drawing.Size(35, 20);
            this.earlyReservationUpDown.TabIndex = 0;
            this.earlyReservationUpDown.Leave += new System.EventHandler(this.earlyReservationUpDown_Leave);
            // 
            // earlyFinishTimeTextBox
            // 
            this.earlyFinishTimeTextBox.Location = new System.Drawing.Point(155, 20);
            this.earlyFinishTimeTextBox.Mask = "00:00";
            this.earlyFinishTimeTextBox.Name = "earlyFinishTimeTextBox";
            this.earlyFinishTimeTextBox.Size = new System.Drawing.Size(35, 20);
            this.earlyFinishTimeTextBox.TabIndex = 0;
            this.earlyFinishTimeTextBox.Text = "0000";
            this.earlyFinishTimeTextBox.ValidatingType = typeof(System.DateTime);
            this.earlyFinishTimeTextBox.Leave += new System.EventHandler(this.earlyFinishTimeTextBox_Leave);
            // 
            // earlyReservationLabel
            // 
            this.earlyReservationLabel.AutoSize = true;
            this.earlyReservationLabel.Location = new System.Drawing.Point(5, 50);
            this.earlyReservationLabel.Name = "earlyReservationLabel";
            this.earlyReservationLabel.Size = new System.Drawing.Size(92, 13);
            this.earlyReservationLabel.TabIndex = 0;
            this.earlyReservationLabel.Text = "Кол-во запросов";
            // 
            // earlyTimeLabel
            // 
            this.earlyTimeLabel.AutoSize = true;
            this.earlyTimeLabel.Location = new System.Drawing.Point(5, 25);
            this.earlyTimeLabel.Name = "earlyTimeLabel";
            this.earlyTimeLabel.Size = new System.Drawing.Size(79, 13);
            this.earlyTimeLabel.TabIndex = 0;
            this.earlyTimeLabel.Text = "Время записи";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(10, 10);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(91, 13);
            this.timeLabel.TabIndex = 0;
            this.timeLabel.Text = "Время оказания";
            // 
            // maxClientRequestsUpDown
            // 
            this.maxClientRequestsUpDown.Location = new System.Drawing.Point(130, 105);
            this.maxClientRequestsUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.maxClientRequestsUpDown.Name = "maxClientRequestsUpDown";
            this.maxClientRequestsUpDown.Size = new System.Drawing.Size(40, 20);
            this.maxClientRequestsUpDown.TabIndex = 0;
            this.maxClientRequestsUpDown.Leave += new System.EventHandler(this.maxClientRequestsUpDown_Leave);
            // 
            // maxClientRequestsLabel
            // 
            this.maxClientRequestsLabel.AutoSize = true;
            this.maxClientRequestsLabel.Location = new System.Drawing.Point(10, 110);
            this.maxClientRequestsLabel.Name = "maxClientRequestsLabel";
            this.maxClientRequestsLabel.Size = new System.Drawing.Size(115, 13);
            this.maxClientRequestsLabel.TabIndex = 0;
            this.maxClientRequestsLabel.Text = "Запросов на клиента";
            // 
            // startTimeTextBox
            // 
            this.startTimeTextBox.Location = new System.Drawing.Point(130, 5);
            this.startTimeTextBox.Mask = "00:00";
            this.startTimeTextBox.Name = "startTimeTextBox";
            this.startTimeTextBox.Size = new System.Drawing.Size(35, 20);
            this.startTimeTextBox.TabIndex = 0;
            this.startTimeTextBox.Text = "0000";
            this.startTimeTextBox.ValidatingType = typeof(System.DateTime);
            this.startTimeTextBox.Leave += new System.EventHandler(this.startTimeTextBox_Leave);
            // 
            // finishTimeTextBox
            // 
            this.finishTimeTextBox.Location = new System.Drawing.Point(170, 5);
            this.finishTimeTextBox.Mask = "00:00";
            this.finishTimeTextBox.Name = "finishTimeTextBox";
            this.finishTimeTextBox.Size = new System.Drawing.Size(35, 20);
            this.finishTimeTextBox.TabIndex = 0;
            this.finishTimeTextBox.Text = "0000";
            this.finishTimeTextBox.ValidatingType = typeof(System.DateTime);
            this.finishTimeTextBox.Leave += new System.EventHandler(this.finishTimeTextBox_Leave);
            // 
            // isInterruptionCheckBox
            // 
            this.isInterruptionCheckBox.AutoSize = true;
            this.isInterruptionCheckBox.Location = new System.Drawing.Point(15, 35);
            this.isInterruptionCheckBox.Name = "isInterruptionCheckBox";
            this.isInterruptionCheckBox.Size = new System.Drawing.Size(72, 17);
            this.isInterruptionCheckBox.TabIndex = 0;
            this.isInterruptionCheckBox.Tag = "1";
            this.isInterruptionCheckBox.Text = "Перерыв";
            this.isInterruptionCheckBox.UseVisualStyleBackColor = true;
            this.isInterruptionCheckBox.CheckedChanged += new System.EventHandler(this.isInterruptionCheckBox_CheckedChanged);
            this.isInterruptionCheckBox.Leave += new System.EventHandler(this.isInterruptionCheckBox_Leave);
            // 
            // interruptionPanel
            // 
            this.interruptionPanel.Controls.Add(this.interruptionFinishTimeTextBox);
            this.interruptionPanel.Controls.Add(this.interruptionStartTimeTextBox);
            this.interruptionPanel.Enabled = false;
            this.interruptionPanel.Location = new System.Drawing.Point(130, 30);
            this.interruptionPanel.Name = "interruptionPanel";
            this.interruptionPanel.Size = new System.Drawing.Size(75, 20);
            this.interruptionPanel.TabIndex = 0;
            // 
            // interruptionFinishTimeTextBox
            // 
            this.interruptionFinishTimeTextBox.Location = new System.Drawing.Point(40, 0);
            this.interruptionFinishTimeTextBox.Mask = "00:00";
            this.interruptionFinishTimeTextBox.Name = "interruptionFinishTimeTextBox";
            this.interruptionFinishTimeTextBox.Size = new System.Drawing.Size(35, 20);
            this.interruptionFinishTimeTextBox.TabIndex = 0;
            this.interruptionFinishTimeTextBox.Text = "0000";
            this.interruptionFinishTimeTextBox.ValidatingType = typeof(System.DateTime);
            this.interruptionFinishTimeTextBox.Leave += new System.EventHandler(this.interruptionFinishTimeTextBox_Leave);
            // 
            // interruptionStartTimeTextBox
            // 
            this.interruptionStartTimeTextBox.Location = new System.Drawing.Point(0, 0);
            this.interruptionStartTimeTextBox.Mask = "00:00";
            this.interruptionStartTimeTextBox.Name = "interruptionStartTimeTextBox";
            this.interruptionStartTimeTextBox.Size = new System.Drawing.Size(35, 20);
            this.interruptionStartTimeTextBox.TabIndex = 0;
            this.interruptionStartTimeTextBox.Text = "0000";
            this.interruptionStartTimeTextBox.ValidatingType = typeof(System.DateTime);
            this.interruptionStartTimeTextBox.Leave += new System.EventHandler(this.interruptionStartTimeTextBox_Leave);
            // 
            // clientIntervalLabel
            // 
            this.clientIntervalLabel.AutoSize = true;
            this.clientIntervalLabel.Location = new System.Drawing.Point(10, 60);
            this.clientIntervalLabel.Name = "clientIntervalLabel";
            this.clientIntervalLabel.Size = new System.Drawing.Size(91, 13);
            this.clientIntervalLabel.TabIndex = 0;
            this.clientIntervalLabel.Text = "Время оказания";
            // 
            // clientIntervalUpDown
            // 
            this.clientIntervalUpDown.Location = new System.Drawing.Point(130, 55);
            this.clientIntervalUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.clientIntervalUpDown.Name = "clientIntervalUpDown";
            this.clientIntervalUpDown.Size = new System.Drawing.Size(40, 20);
            this.clientIntervalUpDown.TabIndex = 0;
            this.clientIntervalUpDown.Leave += new System.EventHandler(this.clientIntervalUpDown_Leave);
            // 
            // minLabel1
            // 
            this.minLabel1.AutoSize = true;
            this.minLabel1.Location = new System.Drawing.Point(170, 65);
            this.minLabel1.Name = "minLabel1";
            this.minLabel1.Size = new System.Drawing.Size(30, 13);
            this.minLabel1.TabIndex = 0;
            this.minLabel1.Text = "мин.";
            // 
            // intersectionLabel
            // 
            this.intersectionLabel.AutoSize = true;
            this.intersectionLabel.Location = new System.Drawing.Point(10, 85);
            this.intersectionLabel.Name = "intersectionLabel";
            this.intersectionLabel.Size = new System.Drawing.Size(99, 13);
            this.intersectionLabel.TabIndex = 0;
            this.intersectionLabel.Text = "Время наложения";
            // 
            // intersectionUpDown
            // 
            this.intersectionUpDown.Location = new System.Drawing.Point(130, 80);
            this.intersectionUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.intersectionUpDown.Name = "intersectionUpDown";
            this.intersectionUpDown.Size = new System.Drawing.Size(40, 20);
            this.intersectionUpDown.TabIndex = 0;
            this.intersectionUpDown.Leave += new System.EventHandler(this.intersectionUpDown_Leave);
            // 
            // minLabel2
            // 
            this.minLabel2.AutoSize = true;
            this.minLabel2.Location = new System.Drawing.Point(170, 85);
            this.minLabel2.Name = "minLabel2";
            this.minLabel2.Size = new System.Drawing.Size(30, 13);
            this.minLabel2.TabIndex = 0;
            this.minLabel2.Text = "мин.";
            // 
            // isWorkedCheckBox
            // 
            this.isWorkedCheckBox.AutoSize = true;
            this.isWorkedCheckBox.Location = new System.Drawing.Point(10, 5);
            this.isWorkedCheckBox.Name = "isWorkedCheckBox";
            this.isWorkedCheckBox.Size = new System.Drawing.Size(134, 17);
            this.isWorkedCheckBox.TabIndex = 0;
            this.isWorkedCheckBox.Text = "Услуги оказываются";
            this.isWorkedCheckBox.CheckedChanged += new System.EventHandler(this.isWorkedCheckBox_CheckedChanged);
            this.isWorkedCheckBox.Leave += new System.EventHandler(this.isWorkedCheckBox_Leave);
            // 
            // schedulePanel
            // 
            this.schedulePanel.Controls.Add(this.timeLabel);
            this.schedulePanel.Controls.Add(this.minLabel2);
            this.schedulePanel.Controls.Add(this.addServiceRenderingButton);
            this.schedulePanel.Controls.Add(this.intersectionUpDown);
            this.schedulePanel.Controls.Add(this.intersectionLabel);
            this.schedulePanel.Controls.Add(this.gridView);
            this.schedulePanel.Controls.Add(this.minLabel1);
            this.schedulePanel.Controls.Add(this.renderingModeComboBox);
            this.schedulePanel.Controls.Add(this.clientIntervalUpDown);
            this.schedulePanel.Controls.Add(this.maxClientRequestsMeasureLabel);
            this.schedulePanel.Controls.Add(this.clientIntervalLabel);
            this.schedulePanel.Controls.Add(this.earlyGroupBox);
            this.schedulePanel.Controls.Add(this.interruptionPanel);
            this.schedulePanel.Controls.Add(this.isInterruptionCheckBox);
            this.schedulePanel.Controls.Add(this.maxClientRequestsUpDown);
            this.schedulePanel.Controls.Add(this.finishTimeTextBox);
            this.schedulePanel.Controls.Add(this.maxClientRequestsLabel);
            this.schedulePanel.Controls.Add(this.startTimeTextBox);
            this.schedulePanel.Location = new System.Drawing.Point(5, 25);
            this.schedulePanel.Name = "schedulePanel";
            this.schedulePanel.Size = new System.Drawing.Size(780, 235);
            this.schedulePanel.TabIndex = 2;
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
            // operatorColumn
            // 
            this.operatorColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.operatorColumn.HeaderText = "Оператор";
            this.operatorColumn.Name = "operatorColumn";
            this.operatorColumn.ReadOnly = true;
            this.operatorColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // serviceStepColumn
            // 
            this.serviceStepColumn.HeaderText = "Этап услуги";
            this.serviceStepColumn.Name = "serviceStepColumn";
            this.serviceStepColumn.ReadOnly = true;
            this.serviceStepColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // modeColumn
            // 
            this.modeColumn.FillWeight = 140F;
            this.modeColumn.HeaderText = "Режим обслуживания";
            this.modeColumn.Name = "modeColumn";
            this.modeColumn.ReadOnly = true;
            this.modeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.modeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.modeColumn.Width = 140;
            // 
            // priorityColumn
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.priorityColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.priorityColumn.HeaderText = "Приоритет";
            this.priorityColumn.Name = "priorityColumn";
            this.priorityColumn.ReadOnly = true;
            this.priorityColumn.Width = 90;
            // 
            // ScheduleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.schedulePanel);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.isWorkedCheckBox);
            this.Name = "ScheduleControl";
            this.Size = new System.Drawing.Size(790, 310);
            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
            this.earlyGroupBox.ResumeLayout(false);
            this.earlyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.earlyReservationUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxClientRequestsUpDown)).EndInit();
            this.interruptionPanel.ResumeLayout(false);
            this.interruptionPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.clientIntervalUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intersectionUpDown)).EndInit();
            this.schedulePanel.ResumeLayout(false);
            this.schedulePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MaskedTextBox interruptionFinishTimeTextBox;
        private System.Windows.Forms.MaskedTextBox interruptionStartTimeTextBox;
        private System.Windows.Forms.NumericUpDown clientIntervalUpDown;
        private System.Windows.Forms.NumericUpDown earlyReservationUpDown;
        private System.Windows.Forms.CheckBox isInterruptionCheckBox;
        private System.Windows.Forms.Label earlyReservationLabel;
        private System.Windows.Forms.MaskedTextBox startTimeTextBox;
        private System.Windows.Forms.CheckBox isWorkedCheckBox;
        private System.Windows.Forms.MaskedTextBox finishTimeTextBox;
        private System.Windows.Forms.Label clientIntervalLabel;
        private System.Windows.Forms.Label intersectionLabel;
        private System.Windows.Forms.NumericUpDown intersectionUpDown;
        private System.Windows.Forms.Label minLabel1;
        private System.Windows.Forms.Label minLabel2;
        private System.Windows.Forms.MaskedTextBox earlyFinishTimeTextBox;
        private System.Windows.Forms.MaskedTextBox earlyStartTimeTextBox;
        private System.Windows.Forms.Label earlyTimeLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Panel interruptionPanel;
        private System.Windows.Forms.Label earlyReservationPercentLabel;
        private System.Windows.Forms.GroupBox earlyGroupBox;
        private System.Windows.Forms.ComboBox renderingModeComboBox;
        private System.Windows.Forms.Label maxClientRequestsLabel;
        private System.Windows.Forms.NumericUpDown maxClientRequestsUpDown;
        private System.Windows.Forms.Label maxClientRequestsMeasureLabel;
        private System.Windows.Forms.Button addServiceRenderingButton;
        private System.Windows.Forms.DataGridView gridView;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Panel schedulePanel;
        private System.Windows.Forms.DataGridViewLinkColumn deleteColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn operatorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serviceStepColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priorityColumn;
    }
}
