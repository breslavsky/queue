namespace Queue.Administrator
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
            this.saveButton = new System.Windows.Forms.Button();
            this.addServiceRenderingButton = new System.Windows.Forms.Button();
            this.serviceRenderingsGridView = new System.Windows.Forms.DataGridView();
            this.operatorColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.serviceStepColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.modeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.priorityColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.maxClientRequestsMeasureLabel = new System.Windows.Forms.Label();
            this.earlyGroupBox = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.earlyClientIntervalUpDown = new System.Windows.Forms.NumericUpDown();
            this.earlyTimeLabel = new System.Windows.Forms.Label();
            this.minLabel3 = new System.Windows.Forms.Label();
            this.earlyStartTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.earlyFinishTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.earlyReservationLabel = new System.Windows.Forms.Label();
            this.earlyReservationUpDown = new System.Windows.Forms.NumericUpDown();
            this.timeLabel = new System.Windows.Forms.Label();
            this.maxClientRequestsUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxClientRequestsLabel = new System.Windows.Forms.Label();
            this.startTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.finishTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.liveClientIntervalLabel = new System.Windows.Forms.Label();
            this.liveClientIntervalUpDown = new System.Windows.Forms.NumericUpDown();
            this.minLabel1 = new System.Windows.Forms.Label();
            this.intersectionLabel = new System.Windows.Forms.Label();
            this.intersectionUpDown = new System.Windows.Forms.NumericUpDown();
            this.minLabel2 = new System.Windows.Forms.Label();
            this.isWorkedCheckBox = new System.Windows.Forms.CheckBox();
            this.schedulePanel = new System.Windows.Forms.Panel();
            this.renderingModeControl = new Queue.UI.WinForms.EnumItemControl();
            this.serviceRenderingsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.serviceRenderingsGridView)).BeginInit();
            this.earlyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.earlyClientIntervalUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.earlyReservationUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxClientRequestsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.liveClientIntervalUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.intersectionUpDown)).BeginInit();
            this.schedulePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(135, 270);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Записать";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // addServiceRenderingButton
            // 
            this.addServiceRenderingButton.Location = new System.Drawing.Point(695, 195);
            this.addServiceRenderingButton.Name = "addServiceRenderingButton";
            this.addServiceRenderingButton.Size = new System.Drawing.Size(80, 25);
            this.addServiceRenderingButton.TabIndex = 0;
            this.addServiceRenderingButton.Text = "Добавить";
            this.addServiceRenderingButton.Click += new System.EventHandler(this.addServiceRenderingButton_Click);
            // 
            // serviceRenderingsGridView
            // 
            this.serviceRenderingsGridView.AllowUserToAddRows = false;
            this.serviceRenderingsGridView.AllowUserToResizeColumns = false;
            this.serviceRenderingsGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.serviceRenderingsGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.serviceRenderingsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.serviceRenderingsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.operatorColumn,
            this.serviceStepColumn,
            this.modeColumn,
            this.priorityColumn});
            this.serviceRenderingsGridView.Location = new System.Drawing.Point(215, 35);
            this.serviceRenderingsGridView.MultiSelect = false;
            this.serviceRenderingsGridView.Name = "serviceRenderingsGridView";
            this.serviceRenderingsGridView.ReadOnly = true;
            this.serviceRenderingsGridView.RowHeadersVisible = false;
            this.serviceRenderingsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.serviceRenderingsGridView.Size = new System.Drawing.Size(560, 155);
            this.serviceRenderingsGridView.TabIndex = 0;
            this.serviceRenderingsGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.serviceRenderingsGridView_CellMouseDoubleClick);
            this.serviceRenderingsGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.serviceRenderingsGridView_UserDeletingRow);
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
            this.serviceStepColumn.Width = 150;
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.priorityColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.priorityColumn.HeaderText = "Приоритет";
            this.priorityColumn.Name = "priorityColumn";
            this.priorityColumn.ReadOnly = true;
            this.priorityColumn.Width = 90;
            // 
            // maxClientRequestsMeasureLabel
            // 
            this.maxClientRequestsMeasureLabel.AutoSize = true;
            this.maxClientRequestsMeasureLabel.Location = new System.Drawing.Point(170, 90);
            this.maxClientRequestsMeasureLabel.Name = "maxClientRequestsMeasureLabel";
            this.maxClientRequestsMeasureLabel.Size = new System.Drawing.Size(23, 13);
            this.maxClientRequestsMeasureLabel.TabIndex = 0;
            this.maxClientRequestsMeasureLabel.Text = "шт.";
            // 
            // earlyGroupBox
            // 
            this.earlyGroupBox.Controls.Add(this.label1);
            this.earlyGroupBox.Controls.Add(this.earlyClientIntervalUpDown);
            this.earlyGroupBox.Controls.Add(this.earlyTimeLabel);
            this.earlyGroupBox.Controls.Add(this.minLabel3);
            this.earlyGroupBox.Controls.Add(this.earlyStartTimeTextBox);
            this.earlyGroupBox.Controls.Add(this.earlyFinishTimeTextBox);
            this.earlyGroupBox.Controls.Add(this.earlyReservationLabel);
            this.earlyGroupBox.Controls.Add(this.earlyReservationUpDown);
            this.earlyGroupBox.Location = new System.Drawing.Point(10, 135);
            this.earlyGroupBox.Name = "earlyGroupBox";
            this.earlyGroupBox.Size = new System.Drawing.Size(195, 95);
            this.earlyGroupBox.TabIndex = 0;
            this.earlyGroupBox.TabStop = false;
            this.earlyGroupBox.Text = "По записи";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Время оказания";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // earlyClientIntervalUpDown
            // 
            this.earlyClientIntervalUpDown.Location = new System.Drawing.Point(115, 70);
            this.earlyClientIntervalUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.earlyClientIntervalUpDown.Name = "earlyClientIntervalUpDown";
            this.earlyClientIntervalUpDown.Size = new System.Drawing.Size(35, 20);
            this.earlyClientIntervalUpDown.TabIndex = 4;
            this.earlyClientIntervalUpDown.Leave += new System.EventHandler(this.earlyClientIntervalUpDown_Leave);
            // 
            // earlyTimeLabel
            // 
            this.earlyTimeLabel.Location = new System.Drawing.Point(5, 15);
            this.earlyTimeLabel.Name = "earlyTimeLabel";
            this.earlyTimeLabel.Size = new System.Drawing.Size(105, 25);
            this.earlyTimeLabel.TabIndex = 0;
            this.earlyTimeLabel.Text = "Время записи";
            this.earlyTimeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // minLabel3
            // 
            this.minLabel3.AutoSize = true;
            this.minLabel3.Location = new System.Drawing.Point(155, 75);
            this.minLabel3.Name = "minLabel3";
            this.minLabel3.Size = new System.Drawing.Size(30, 13);
            this.minLabel3.TabIndex = 5;
            this.minLabel3.Text = "мин.";
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
            this.earlyReservationLabel.Location = new System.Drawing.Point(5, 40);
            this.earlyReservationLabel.Name = "earlyReservationLabel";
            this.earlyReservationLabel.Size = new System.Drawing.Size(105, 25);
            this.earlyReservationLabel.TabIndex = 0;
            this.earlyReservationLabel.Text = "% резервирования";
            this.earlyReservationLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
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
            // timeLabel
            // 
            this.timeLabel.Location = new System.Drawing.Point(10, 5);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(115, 25);
            this.timeLabel.TabIndex = 0;
            this.timeLabel.Text = "Период оказания";
            this.timeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // maxClientRequestsUpDown
            // 
            this.maxClientRequestsUpDown.Location = new System.Drawing.Point(130, 85);
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
            this.maxClientRequestsLabel.Location = new System.Drawing.Point(10, 80);
            this.maxClientRequestsLabel.Name = "maxClientRequestsLabel";
            this.maxClientRequestsLabel.Size = new System.Drawing.Size(115, 25);
            this.maxClientRequestsLabel.TabIndex = 0;
            this.maxClientRequestsLabel.Text = "Запросов на клиента";
            this.maxClientRequestsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // startTimeTextBox
            // 
            this.startTimeTextBox.Location = new System.Drawing.Point(130, 10);
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
            this.finishTimeTextBox.Location = new System.Drawing.Point(170, 10);
            this.finishTimeTextBox.Mask = "00:00";
            this.finishTimeTextBox.Name = "finishTimeTextBox";
            this.finishTimeTextBox.Size = new System.Drawing.Size(35, 20);
            this.finishTimeTextBox.TabIndex = 0;
            this.finishTimeTextBox.Text = "0000";
            this.finishTimeTextBox.ValidatingType = typeof(System.DateTime);
            this.finishTimeTextBox.Leave += new System.EventHandler(this.finishTimeTextBox_Leave);
            // 
            // liveClientIntervalLabel
            // 
            this.liveClientIntervalLabel.Location = new System.Drawing.Point(10, 30);
            this.liveClientIntervalLabel.Name = "liveClientIntervalLabel";
            this.liveClientIntervalLabel.Size = new System.Drawing.Size(115, 25);
            this.liveClientIntervalLabel.TabIndex = 0;
            this.liveClientIntervalLabel.Text = "Время оказания";
            this.liveClientIntervalLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // liveClientIntervalUpDown
            // 
            this.liveClientIntervalUpDown.Location = new System.Drawing.Point(130, 35);
            this.liveClientIntervalUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.liveClientIntervalUpDown.Name = "liveClientIntervalUpDown";
            this.liveClientIntervalUpDown.Size = new System.Drawing.Size(40, 20);
            this.liveClientIntervalUpDown.TabIndex = 0;
            this.liveClientIntervalUpDown.Leave += new System.EventHandler(this.liveClientIntervalUpDown_Leave);
            // 
            // minLabel1
            // 
            this.minLabel1.AutoSize = true;
            this.minLabel1.Location = new System.Drawing.Point(170, 45);
            this.minLabel1.Name = "minLabel1";
            this.minLabel1.Size = new System.Drawing.Size(30, 13);
            this.minLabel1.TabIndex = 0;
            this.minLabel1.Text = "мин.";
            // 
            // intersectionLabel
            // 
            this.intersectionLabel.Location = new System.Drawing.Point(10, 55);
            this.intersectionLabel.Name = "intersectionLabel";
            this.intersectionLabel.Size = new System.Drawing.Size(115, 25);
            this.intersectionLabel.TabIndex = 0;
            this.intersectionLabel.Text = "Время наложения";
            this.intersectionLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // intersectionUpDown
            // 
            this.intersectionUpDown.Location = new System.Drawing.Point(130, 60);
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
            this.minLabel2.Location = new System.Drawing.Point(170, 65);
            this.minLabel2.Name = "minLabel2";
            this.minLabel2.Size = new System.Drawing.Size(30, 13);
            this.minLabel2.TabIndex = 0;
            this.minLabel2.Text = "мин.";
            // 
            // isWorkedCheckBox
            // 
            this.isWorkedCheckBox.Location = new System.Drawing.Point(10, 5);
            this.isWorkedCheckBox.Name = "isWorkedCheckBox";
            this.isWorkedCheckBox.Size = new System.Drawing.Size(320, 17);
            this.isWorkedCheckBox.TabIndex = 0;
            this.isWorkedCheckBox.Text = "Услуги оказываются";
            this.isWorkedCheckBox.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.isWorkedCheckBox.CheckedChanged += new System.EventHandler(this.isWorkedCheckBox_CheckedChanged);
            this.isWorkedCheckBox.Leave += new System.EventHandler(this.isWorkedCheckBox_Leave);
            // 
            // schedulePanel
            // 
            this.schedulePanel.Controls.Add(this.timeLabel);
            this.schedulePanel.Controls.Add(this.startTimeTextBox);
            this.schedulePanel.Controls.Add(this.finishTimeTextBox);
            this.schedulePanel.Controls.Add(this.liveClientIntervalLabel);
            this.schedulePanel.Controls.Add(this.liveClientIntervalUpDown);
            this.schedulePanel.Controls.Add(this.minLabel1);
            this.schedulePanel.Controls.Add(this.intersectionLabel);
            this.schedulePanel.Controls.Add(this.intersectionUpDown);
            this.schedulePanel.Controls.Add(this.minLabel2);
            this.schedulePanel.Controls.Add(this.maxClientRequestsLabel);
            this.schedulePanel.Controls.Add(this.maxClientRequestsUpDown);
            this.schedulePanel.Controls.Add(this.renderingModeControl);
            this.schedulePanel.Controls.Add(this.maxClientRequestsMeasureLabel);
            this.schedulePanel.Controls.Add(this.serviceRenderingsLabel);
            this.schedulePanel.Controls.Add(this.serviceRenderingsGridView);
            this.schedulePanel.Controls.Add(this.earlyGroupBox);
            this.schedulePanel.Controls.Add(this.addServiceRenderingButton);
            this.schedulePanel.Location = new System.Drawing.Point(5, 25);
            this.schedulePanel.Name = "schedulePanel";
            this.schedulePanel.Size = new System.Drawing.Size(780, 240);
            this.schedulePanel.TabIndex = 2;
            // 
            // renderingModeControl
            // 
            this.renderingModeControl.Location = new System.Drawing.Point(10, 110);
            this.renderingModeControl.Name = "renderingModeControl";
            this.renderingModeControl.Size = new System.Drawing.Size(195, 21);
            this.renderingModeControl.TabIndex = 1;
            this.renderingModeControl.Leave += new System.EventHandler(this.renderingModeControl_Leave);
            // 
            // serviceRenderingsLabel
            // 
            this.serviceRenderingsLabel.Location = new System.Drawing.Point(215, 10);
            this.serviceRenderingsLabel.Name = "serviceRenderingsLabel";
            this.serviceRenderingsLabel.Size = new System.Drawing.Size(560, 20);
            this.serviceRenderingsLabel.TabIndex = 2;
            this.serviceRenderingsLabel.Text = "Параметры обслуживания:";
            this.serviceRenderingsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // ScheduleControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.isWorkedCheckBox);
            this.Controls.Add(this.schedulePanel);
            this.Controls.Add(this.saveButton);
            this.Name = "ScheduleControl";
            this.Size = new System.Drawing.Size(790, 305);
            ((System.ComponentModel.ISupportInitialize)(this.serviceRenderingsGridView)).EndInit();
            this.earlyGroupBox.ResumeLayout(false);
            this.earlyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.earlyClientIntervalUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.earlyReservationUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxClientRequestsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.liveClientIntervalUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.intersectionUpDown)).EndInit();
            this.schedulePanel.ResumeLayout(false);
            this.schedulePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown liveClientIntervalUpDown;
        private System.Windows.Forms.NumericUpDown earlyReservationUpDown;
        private System.Windows.Forms.Label earlyReservationLabel;
        private System.Windows.Forms.MaskedTextBox startTimeTextBox;
        private System.Windows.Forms.CheckBox isWorkedCheckBox;
        private System.Windows.Forms.MaskedTextBox finishTimeTextBox;
        private System.Windows.Forms.Label liveClientIntervalLabel;
        private System.Windows.Forms.Label intersectionLabel;
        private System.Windows.Forms.NumericUpDown intersectionUpDown;
        private System.Windows.Forms.Label minLabel1;
        private System.Windows.Forms.Label minLabel2;
        private System.Windows.Forms.MaskedTextBox earlyFinishTimeTextBox;
        private System.Windows.Forms.MaskedTextBox earlyStartTimeTextBox;
        private System.Windows.Forms.Label earlyTimeLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.GroupBox earlyGroupBox;
        private System.Windows.Forms.Label maxClientRequestsLabel;
        private System.Windows.Forms.NumericUpDown maxClientRequestsUpDown;
        private System.Windows.Forms.Label maxClientRequestsMeasureLabel;
        private System.Windows.Forms.Button addServiceRenderingButton;
        private System.Windows.Forms.DataGridView serviceRenderingsGridView;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Panel schedulePanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn operatorColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn serviceStepColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn modeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn priorityColumn;
        private UI.WinForms.EnumItemControl renderingModeControl;
        private System.Windows.Forms.Label serviceRenderingsLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown earlyClientIntervalUpDown;
        private System.Windows.Forms.Label minLabel3;
    }
}
