namespace Queue.Administrator
{
    partial class ServiceParametersControl
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
            this.listBox = new System.Windows.Forms.ListBox();
            this.parameterTypeComboBox = new System.Windows.Forms.ComboBox();
            this.addButton = new System.Windows.Forms.Button();
            this.parameterGroupBox = new System.Windows.Forms.GroupBox();
            this.parameterNamelabel = new System.Windows.Forms.Label();
            this.parameterNameTextBox = new System.Windows.Forms.TextBox();
            this.parameterToolTipLabel = new System.Windows.Forms.Label();
            this.parameterToolTipTextBox = new System.Windows.Forms.TextBox();
            this.parameterIsRequireCheckBox = new System.Windows.Forms.CheckBox();
            this.parameterLengthGroupBox = new System.Windows.Forms.GroupBox();
            this.parameterMinLengthLabel = new System.Windows.Forms.Label();
            this.parameterMinLengthUpDown = new System.Windows.Forms.NumericUpDown();
            this.parameterMaxLengthLabel = new System.Windows.Forms.Label();
            this.parameterMaxLengthUpDown = new System.Windows.Forms.NumericUpDown();
            this.deleteButton = new System.Windows.Forms.Button();
            this.serviceParameterOptionsLabel = new System.Windows.Forms.Label();
            this.parameterOptionsTextBox = new System.Windows.Forms.TextBox();
            this.parameterIsMultipleCheckBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.parameterGroupBox.SuspendLayout();
            this.parameterLengthGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parameterMinLengthUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.parameterMaxLengthUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // listBox
            // 
            this.listBox.FormattingEnabled = true;
            this.listBox.Location = new System.Drawing.Point(5, 5);
            this.listBox.Name = "listBox";
            this.listBox.Size = new System.Drawing.Size(210, 160);
            this.listBox.TabIndex = 1;
            this.listBox.SelectedIndexChanged += new System.EventHandler(this.listBox_SelectedIndexChanged);
            // 
            // parameterTypeComboBox
            // 
            this.parameterTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.parameterTypeComboBox.FormattingEnabled = true;
            this.parameterTypeComboBox.Location = new System.Drawing.Point(5, 175);
            this.parameterTypeComboBox.Name = "parameterTypeComboBox";
            this.parameterTypeComboBox.Size = new System.Drawing.Size(120, 21);
            this.parameterTypeComboBox.TabIndex = 2;
            // 
            // addButton
            // 
            this.addButton.Location = new System.Drawing.Point(135, 175);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(80, 25);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Добавить";
            this.addButton.Click += new System.EventHandler(this.addButton_Click);
            // 
            // parameterGroupBox
            // 
            this.parameterGroupBox.Controls.Add(this.parameterNamelabel);
            this.parameterGroupBox.Controls.Add(this.parameterNameTextBox);
            this.parameterGroupBox.Controls.Add(this.parameterToolTipLabel);
            this.parameterGroupBox.Controls.Add(this.parameterToolTipTextBox);
            this.parameterGroupBox.Controls.Add(this.parameterIsRequireCheckBox);
            this.parameterGroupBox.Controls.Add(this.parameterLengthGroupBox);
            this.parameterGroupBox.Controls.Add(this.deleteButton);
            this.parameterGroupBox.Controls.Add(this.serviceParameterOptionsLabel);
            this.parameterGroupBox.Controls.Add(this.parameterOptionsTextBox);
            this.parameterGroupBox.Controls.Add(this.parameterIsMultipleCheckBox);
            this.parameterGroupBox.Controls.Add(this.saveButton);
            this.parameterGroupBox.Enabled = false;
            this.parameterGroupBox.Location = new System.Drawing.Point(225, 5);
            this.parameterGroupBox.Name = "parameterGroupBox";
            this.parameterGroupBox.Size = new System.Drawing.Size(390, 310);
            this.parameterGroupBox.TabIndex = 4;
            this.parameterGroupBox.TabStop = false;
            this.parameterGroupBox.Text = "Параметр";
            // 
            // parameterNamelabel
            // 
            this.parameterNamelabel.AutoSize = true;
            this.parameterNamelabel.Location = new System.Drawing.Point(10, 20);
            this.parameterNamelabel.Name = "parameterNamelabel";
            this.parameterNamelabel.Size = new System.Drawing.Size(57, 13);
            this.parameterNamelabel.TabIndex = 0;
            this.parameterNamelabel.Text = "Название";
            // 
            // parameterNameTextBox
            // 
            this.parameterNameTextBox.Location = new System.Drawing.Point(20, 40);
            this.parameterNameTextBox.Name = "parameterNameTextBox";
            this.parameterNameTextBox.Size = new System.Drawing.Size(190, 20);
            this.parameterNameTextBox.TabIndex = 0;
            // 
            // parameterToolTipLabel
            // 
            this.parameterToolTipLabel.AutoSize = true;
            this.parameterToolTipLabel.Location = new System.Drawing.Point(10, 70);
            this.parameterToolTipLabel.Name = "parameterToolTipLabel";
            this.parameterToolTipLabel.Size = new System.Drawing.Size(63, 13);
            this.parameterToolTipLabel.TabIndex = 0;
            this.parameterToolTipLabel.Text = "Подсказка";
            // 
            // parameterToolTipTextBox
            // 
            this.parameterToolTipTextBox.Location = new System.Drawing.Point(20, 90);
            this.parameterToolTipTextBox.Multiline = true;
            this.parameterToolTipTextBox.Name = "parameterToolTipTextBox";
            this.parameterToolTipTextBox.Size = new System.Drawing.Size(190, 50);
            this.parameterToolTipTextBox.TabIndex = 0;
            // 
            // parameterIsRequireCheckBox
            // 
            this.parameterIsRequireCheckBox.AutoSize = true;
            this.parameterIsRequireCheckBox.Location = new System.Drawing.Point(10, 150);
            this.parameterIsRequireCheckBox.Name = "parameterIsRequireCheckBox";
            this.parameterIsRequireCheckBox.Size = new System.Drawing.Size(183, 17);
            this.parameterIsRequireCheckBox.TabIndex = 0;
            this.parameterIsRequireCheckBox.Text = "Обязательное для заполнения";
            this.parameterIsRequireCheckBox.UseVisualStyleBackColor = true;
            // 
            // parameterLengthGroupBox
            // 
            this.parameterLengthGroupBox.Controls.Add(this.parameterMinLengthLabel);
            this.parameterLengthGroupBox.Controls.Add(this.parameterMinLengthUpDown);
            this.parameterLengthGroupBox.Controls.Add(this.parameterMaxLengthLabel);
            this.parameterLengthGroupBox.Controls.Add(this.parameterMaxLengthUpDown);
            this.parameterLengthGroupBox.Location = new System.Drawing.Point(10, 180);
            this.parameterLengthGroupBox.Name = "parameterLengthGroupBox";
            this.parameterLengthGroupBox.Size = new System.Drawing.Size(200, 85);
            this.parameterLengthGroupBox.TabIndex = 0;
            this.parameterLengthGroupBox.TabStop = false;
            this.parameterLengthGroupBox.Text = "Ограничение длины";
            // 
            // parameterMinLengthLabel
            // 
            this.parameterMinLengthLabel.AutoSize = true;
            this.parameterMinLengthLabel.Location = new System.Drawing.Point(10, 30);
            this.parameterMinLengthLabel.Name = "parameterMinLengthLabel";
            this.parameterMinLengthLabel.Size = new System.Drawing.Size(111, 13);
            this.parameterMinLengthLabel.TabIndex = 0;
            this.parameterMinLengthLabel.Text = "Минимальная длина";
            // 
            // parameterMinLengthUpDown
            // 
            this.parameterMinLengthUpDown.Location = new System.Drawing.Point(150, 25);
            this.parameterMinLengthUpDown.Name = "parameterMinLengthUpDown";
            this.parameterMinLengthUpDown.Size = new System.Drawing.Size(40, 20);
            this.parameterMinLengthUpDown.TabIndex = 0;
            this.parameterMinLengthUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // parameterMaxLengthLabel
            // 
            this.parameterMaxLengthLabel.AutoSize = true;
            this.parameterMaxLengthLabel.Location = new System.Drawing.Point(10, 55);
            this.parameterMaxLengthLabel.Name = "parameterMaxLengthLabel";
            this.parameterMaxLengthLabel.Size = new System.Drawing.Size(117, 13);
            this.parameterMaxLengthLabel.TabIndex = 0;
            this.parameterMaxLengthLabel.Text = "Максимальная длина";
            // 
            // parameterMaxLengthUpDown
            // 
            this.parameterMaxLengthUpDown.Location = new System.Drawing.Point(150, 50);
            this.parameterMaxLengthUpDown.Name = "parameterMaxLengthUpDown";
            this.parameterMaxLengthUpDown.Size = new System.Drawing.Size(40, 20);
            this.parameterMaxLengthUpDown.TabIndex = 0;
            this.parameterMaxLengthUpDown.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(10, 275);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(75, 25);
            this.deleteButton.TabIndex = 0;
            this.deleteButton.Text = "Удалить";
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // serviceParameterOptionsLabel
            // 
            this.serviceParameterOptionsLabel.AutoSize = true;
            this.serviceParameterOptionsLabel.Location = new System.Drawing.Point(220, 20);
            this.serviceParameterOptionsLabel.Name = "serviceParameterOptionsLabel";
            this.serviceParameterOptionsLabel.Size = new System.Drawing.Size(57, 13);
            this.serviceParameterOptionsLabel.TabIndex = 0;
            this.serviceParameterOptionsLabel.Text = "Варианты";
            // 
            // parameterOptionsTextBox
            // 
            this.parameterOptionsTextBox.Location = new System.Drawing.Point(220, 40);
            this.parameterOptionsTextBox.Multiline = true;
            this.parameterOptionsTextBox.Name = "parameterOptionsTextBox";
            this.parameterOptionsTextBox.Size = new System.Drawing.Size(160, 190);
            this.parameterOptionsTextBox.TabIndex = 0;
            // 
            // parameterIsMultipleCheckBox
            // 
            this.parameterIsMultipleCheckBox.AutoSize = true;
            this.parameterIsMultipleCheckBox.Location = new System.Drawing.Point(220, 240);
            this.parameterIsMultipleCheckBox.Name = "parameterIsMultipleCheckBox";
            this.parameterIsMultipleCheckBox.Size = new System.Drawing.Size(145, 17);
            this.parameterIsMultipleCheckBox.TabIndex = 0;
            this.parameterIsMultipleCheckBox.Text = "Множественный выбор";
            this.parameterIsMultipleCheckBox.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(310, 275);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Сохранить";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // ServiceParametersControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listBox);
            this.Controls.Add(this.parameterTypeComboBox);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.parameterGroupBox);
            this.Name = "ServiceParametersControl";
            this.Size = new System.Drawing.Size(620, 320);
            this.parameterGroupBox.ResumeLayout(false);
            this.parameterGroupBox.PerformLayout();
            this.parameterLengthGroupBox.ResumeLayout(false);
            this.parameterLengthGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parameterMinLengthUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.parameterMaxLengthUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox;
        private System.Windows.Forms.ComboBox parameterTypeComboBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.GroupBox parameterGroupBox;
        private System.Windows.Forms.Label parameterNamelabel;
        private System.Windows.Forms.TextBox parameterNameTextBox;
        private System.Windows.Forms.Label parameterToolTipLabel;
        private System.Windows.Forms.TextBox parameterToolTipTextBox;
        private System.Windows.Forms.CheckBox parameterIsRequireCheckBox;
        private System.Windows.Forms.GroupBox parameterLengthGroupBox;
        private System.Windows.Forms.Label parameterMinLengthLabel;
        private System.Windows.Forms.NumericUpDown parameterMinLengthUpDown;
        private System.Windows.Forms.Label parameterMaxLengthLabel;
        private System.Windows.Forms.NumericUpDown parameterMaxLengthUpDown;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Label serviceParameterOptionsLabel;
        private System.Windows.Forms.TextBox parameterOptionsTextBox;
        private System.Windows.Forms.CheckBox parameterIsMultipleCheckBox;
        private System.Windows.Forms.Button saveButton;
    }
}
