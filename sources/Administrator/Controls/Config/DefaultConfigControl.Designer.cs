namespace Queue.Administrator
{
    partial class DefaultConfigControl
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
            this.maxRenderingGroupBox = new System.Windows.Forms.GroupBox();
            this.maxRenderingTimeMinLabel = new System.Windows.Forms.Label();
            this.maxClientRequestsUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxRenderingTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.maxClientRequestsLabel = new System.Windows.Forms.Label();
            this.maxRenderingTimeLabel = new System.Windows.Forms.Label();
            this.queueNameLabel = new System.Windows.Forms.Label();
            this.queueNameTextBox = new System.Windows.Forms.TextBox();
            this.workTimeLabel = new System.Windows.Forms.Label();
            this.workStartTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.workFinishTimeTextBox = new System.Windows.Forms.MaskedTextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.isDebugCheckBox = new System.Windows.Forms.CheckBox();
            this.maxRenderingGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxClientRequestsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxRenderingTimeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // maxRenderingGroupBox
            // 
            this.maxRenderingGroupBox.Controls.Add(this.maxRenderingTimeMinLabel);
            this.maxRenderingGroupBox.Controls.Add(this.maxClientRequestsUpDown);
            this.maxRenderingGroupBox.Controls.Add(this.maxRenderingTimeUpDown);
            this.maxRenderingGroupBox.Controls.Add(this.maxClientRequestsLabel);
            this.maxRenderingGroupBox.Controls.Add(this.maxRenderingTimeLabel);
            this.maxRenderingGroupBox.Location = new System.Drawing.Point(365, 10);
            this.maxRenderingGroupBox.Name = "maxRenderingGroupBox";
            this.maxRenderingGroupBox.Size = new System.Drawing.Size(235, 80);
            this.maxRenderingGroupBox.TabIndex = 12;
            this.maxRenderingGroupBox.TabStop = false;
            this.maxRenderingGroupBox.Text = "Максимальное обслуживание";
            // 
            // maxRenderingTimeMinLabel
            // 
            this.maxRenderingTimeMinLabel.AutoSize = true;
            this.maxRenderingTimeMinLabel.Location = new System.Drawing.Point(195, 50);
            this.maxRenderingTimeMinLabel.Name = "maxRenderingTimeMinLabel";
            this.maxRenderingTimeMinLabel.Size = new System.Drawing.Size(30, 13);
            this.maxRenderingTimeMinLabel.TabIndex = 5;
            this.maxRenderingTimeMinLabel.Text = "мин.";
            // 
            // maxClientRequestsUpDown
            // 
            this.maxClientRequestsUpDown.Location = new System.Drawing.Point(140, 20);
            this.maxClientRequestsUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.maxClientRequestsUpDown.Name = "maxClientRequestsUpDown";
            this.maxClientRequestsUpDown.Size = new System.Drawing.Size(55, 20);
            this.maxClientRequestsUpDown.TabIndex = 1;
            this.maxClientRequestsUpDown.Leave += new System.EventHandler(this.maxClientRequestsUpDown_Leave);
            // 
            // maxRenderingTimeUpDown
            // 
            this.maxRenderingTimeUpDown.Location = new System.Drawing.Point(140, 45);
            this.maxRenderingTimeUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.maxRenderingTimeUpDown.Name = "maxRenderingTimeUpDown";
            this.maxRenderingTimeUpDown.Size = new System.Drawing.Size(55, 20);
            this.maxRenderingTimeUpDown.TabIndex = 3;
            this.maxRenderingTimeUpDown.Leave += new System.EventHandler(this.maxRenderingTimeUpDown_Leave);
            // 
            // maxClientRequestsLabel
            // 
            this.maxClientRequestsLabel.Location = new System.Drawing.Point(10, 15);
            this.maxClientRequestsLabel.Name = "maxClientRequestsLabel";
            this.maxClientRequestsLabel.Size = new System.Drawing.Size(117, 25);
            this.maxClientRequestsLabel.TabIndex = 2;
            this.maxClientRequestsLabel.Text = "Количество запросов";
            this.maxClientRequestsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // maxRenderingTimeLabel
            // 
            this.maxRenderingTimeLabel.Location = new System.Drawing.Point(10, 45);
            this.maxRenderingTimeLabel.Name = "maxRenderingTimeLabel";
            this.maxRenderingTimeLabel.Size = new System.Drawing.Size(116, 20);
            this.maxRenderingTimeLabel.TabIndex = 4;
            this.maxRenderingTimeLabel.Text = "Время обслуживания";
            this.maxRenderingTimeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // queueNameLabel
            // 
            this.queueNameLabel.Location = new System.Drawing.Point(5, 5);
            this.queueNameLabel.Name = "queueNameLabel";
            this.queueNameLabel.Size = new System.Drawing.Size(110, 80);
            this.queueNameLabel.TabIndex = 6;
            this.queueNameLabel.Text = "Название очереди";
            this.queueNameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // queueNameTextBox
            // 
            this.queueNameTextBox.Location = new System.Drawing.Point(120, 10);
            this.queueNameTextBox.Multiline = true;
            this.queueNameTextBox.Name = "queueNameTextBox";
            this.queueNameTextBox.Size = new System.Drawing.Size(235, 80);
            this.queueNameTextBox.TabIndex = 7;
            this.queueNameTextBox.Leave += new System.EventHandler(this.queueNameTextBox_Leave);
            // 
            // workTimeLabel
            // 
            this.workTimeLabel.Location = new System.Drawing.Point(5, 90);
            this.workTimeLabel.Name = "workTimeLabel";
            this.workTimeLabel.Size = new System.Drawing.Size(110, 25);
            this.workTimeLabel.TabIndex = 8;
            this.workTimeLabel.Text = "Время работы";
            this.workTimeLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // workStartTimeTextBox
            // 
            this.workStartTimeTextBox.Location = new System.Drawing.Point(120, 100);
            this.workStartTimeTextBox.Mask = "00:00";
            this.workStartTimeTextBox.Name = "workStartTimeTextBox";
            this.workStartTimeTextBox.Size = new System.Drawing.Size(35, 20);
            this.workStartTimeTextBox.TabIndex = 9;
            this.workStartTimeTextBox.Text = "0000";
            this.workStartTimeTextBox.ValidatingType = typeof(System.DateTime);
            this.workStartTimeTextBox.Leave += new System.EventHandler(this.workStartTimeTextBox_Leave);
            // 
            // workFinishTimeTextBox
            // 
            this.workFinishTimeTextBox.Location = new System.Drawing.Point(165, 100);
            this.workFinishTimeTextBox.Mask = "00:00";
            this.workFinishTimeTextBox.Name = "workFinishTimeTextBox";
            this.workFinishTimeTextBox.Size = new System.Drawing.Size(35, 20);
            this.workFinishTimeTextBox.TabIndex = 10;
            this.workFinishTimeTextBox.Text = "0000";
            this.workFinishTimeTextBox.ValidatingType = typeof(System.DateTime);
            this.workFinishTimeTextBox.Leave += new System.EventHandler(this.workFinishTimeTextBox_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(525, 95);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 11;
            this.saveButton.Text = "Записать";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // debugCheckBox
            // 
            this.isDebugCheckBox.AutoSize = true;
            this.isDebugCheckBox.Location = new System.Drawing.Point(415, 100);
            this.isDebugCheckBox.Name = "debugCheckBox";
            this.isDebugCheckBox.Size = new System.Drawing.Size(105, 17);
            this.isDebugCheckBox.TabIndex = 13;
            this.isDebugCheckBox.Text = "Режим отладки";
            this.isDebugCheckBox.UseVisualStyleBackColor = true;
            this.isDebugCheckBox.Leave += new System.EventHandler(this.isDebugCheckBox_Leave);
            // 
            // DefaultConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.isDebugCheckBox);
            this.Controls.Add(this.maxRenderingGroupBox);
            this.Controls.Add(this.queueNameLabel);
            this.Controls.Add(this.queueNameTextBox);
            this.Controls.Add(this.workTimeLabel);
            this.Controls.Add(this.workStartTimeTextBox);
            this.Controls.Add(this.workFinishTimeTextBox);
            this.Controls.Add(this.saveButton);
            this.Name = "DefaultConfigControl";
            this.Size = new System.Drawing.Size(610, 130);
            this.maxRenderingGroupBox.ResumeLayout(false);
            this.maxRenderingGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.maxClientRequestsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxRenderingTimeUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox maxRenderingGroupBox;
        private System.Windows.Forms.Label maxRenderingTimeMinLabel;
        private System.Windows.Forms.NumericUpDown maxClientRequestsUpDown;
        private System.Windows.Forms.NumericUpDown maxRenderingTimeUpDown;
        private System.Windows.Forms.Label maxClientRequestsLabel;
        private System.Windows.Forms.Label maxRenderingTimeLabel;
        private System.Windows.Forms.Label queueNameLabel;
        private System.Windows.Forms.TextBox queueNameTextBox;
        private System.Windows.Forms.Label workTimeLabel;
        private System.Windows.Forms.MaskedTextBox workStartTimeTextBox;
        private System.Windows.Forms.MaskedTextBox workFinishTimeTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox isDebugCheckBox;
    }
}
