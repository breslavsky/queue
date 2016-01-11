namespace Queue.Administrator
{
    partial class TerminalConfigControl
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
            this.terminallayoutGroupBox = new System.Windows.Forms.GroupBox();
            this.columnsUpDown = new System.Windows.Forms.NumericUpDown();
            this.rowsUpDown = new System.Windows.Forms.NumericUpDown();
            this.terminalRowsLabel = new System.Windows.Forms.Label();
            this.terminalColumnsLabel = new System.Windows.Forms.Label();
            this.PINUpDown = new System.Windows.Forms.NumericUpDown();
            this.saveButton = new System.Windows.Forms.Button();
            this.terminalPINLabel = new System.Windows.Forms.Label();
            this.currentDayRecordingCheckBox = new System.Windows.Forms.CheckBox();
            this.pagesLabel = new System.Windows.Forms.Label();
            this.pagesControl = new Queue.UI.WinForms.EnumFlagsControl();
            this.startPageControl = new Queue.UI.WinForms.EnumItemControl();
            this.startPageLabel = new System.Windows.Forms.Label();
            this.terminallayoutGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PINUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // terminallayoutGroupBox
            // 
            this.terminallayoutGroupBox.Controls.Add(this.columnsUpDown);
            this.terminallayoutGroupBox.Controls.Add(this.rowsUpDown);
            this.terminallayoutGroupBox.Controls.Add(this.terminalRowsLabel);
            this.terminallayoutGroupBox.Controls.Add(this.terminalColumnsLabel);
            this.terminallayoutGroupBox.Location = new System.Drawing.Point(5, 35);
            this.terminallayoutGroupBox.Name = "terminallayoutGroupBox";
            this.terminallayoutGroupBox.Size = new System.Drawing.Size(230, 55);
            this.terminallayoutGroupBox.TabIndex = 10;
            this.terminallayoutGroupBox.TabStop = false;
            this.terminallayoutGroupBox.Text = "Расположение услуг";
            // 
            // columnsUpDown
            // 
            this.columnsUpDown.Location = new System.Drawing.Point(65, 25);
            this.columnsUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.columnsUpDown.Name = "columnsUpDown";
            this.columnsUpDown.Size = new System.Drawing.Size(50, 20);
            this.columnsUpDown.TabIndex = 1;
            this.columnsUpDown.Leave += new System.EventHandler(this.columnsUpDown_Leave);
            // 
            // rowsUpDown
            // 
            this.rowsUpDown.Location = new System.Drawing.Point(165, 25);
            this.rowsUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.rowsUpDown.Name = "rowsUpDown";
            this.rowsUpDown.Size = new System.Drawing.Size(50, 20);
            this.rowsUpDown.TabIndex = 3;
            this.rowsUpDown.Leave += new System.EventHandler(this.rowsUpDown_Leave);
            // 
            // terminalRowsLabel
            // 
            this.terminalRowsLabel.Location = new System.Drawing.Point(120, 15);
            this.terminalRowsLabel.Name = "terminalRowsLabel";
            this.terminalRowsLabel.Size = new System.Drawing.Size(40, 30);
            this.terminalRowsLabel.TabIndex = 4;
            this.terminalRowsLabel.Text = "Строк";
            this.terminalRowsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // terminalColumnsLabel
            // 
            this.terminalColumnsLabel.Location = new System.Drawing.Point(5, 15);
            this.terminalColumnsLabel.Name = "terminalColumnsLabel";
            this.terminalColumnsLabel.Size = new System.Drawing.Size(55, 30);
            this.terminalColumnsLabel.TabIndex = 2;
            this.terminalColumnsLabel.Text = "Колонок";
            this.terminalColumnsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // PINUpDown
            // 
            this.PINUpDown.Location = new System.Drawing.Point(65, 5);
            this.PINUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.PINUpDown.Name = "PINUpDown";
            this.PINUpDown.Size = new System.Drawing.Size(70, 20);
            this.PINUpDown.TabIndex = 6;
            this.PINUpDown.Leave += new System.EventHandler(this.PINUpDown_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(570, 389);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 7;
            this.saveButton.Text = "Записать";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // terminalPINLabel
            // 
            this.terminalPINLabel.Location = new System.Drawing.Point(5, -5);
            this.terminalPINLabel.Name = "terminalPINLabel";
            this.terminalPINLabel.Size = new System.Drawing.Size(60, 30);
            this.terminalPINLabel.TabIndex = 8;
            this.terminalPINLabel.Text = "PIN-код";
            this.terminalPINLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // currentDayRecordingCheckBox
            // 
            this.currentDayRecordingCheckBox.Location = new System.Drawing.Point(10, 95);
            this.currentDayRecordingCheckBox.Name = "currentDayRecordingCheckBox";
            this.currentDayRecordingCheckBox.Size = new System.Drawing.Size(225, 17);
            this.currentDayRecordingCheckBox.TabIndex = 9;
            this.currentDayRecordingCheckBox.Text = "Запись на текущий день";
            this.currentDayRecordingCheckBox.UseVisualStyleBackColor = true;
            this.currentDayRecordingCheckBox.Leave += new System.EventHandler(this.currentDayRecordingCheckBox_Leave);
            // 
            // pagesLabel
            // 
            this.pagesLabel.Location = new System.Drawing.Point(255, 5);
            this.pagesLabel.Name = "pagesLabel";
            this.pagesLabel.Size = new System.Drawing.Size(195, 25);
            this.pagesLabel.TabIndex = 11;
            this.pagesLabel.Text = "Страницы";
            this.pagesLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // pagesControl
            // 
            this.pagesControl.Location = new System.Drawing.Point(270, 35);
            this.pagesControl.Name = "pagesControl";
            this.pagesControl.Size = new System.Drawing.Size(180, 65);
            this.pagesControl.TabIndex = 12;
            this.pagesControl.Leave += new System.EventHandler(this.pagesControl_Leave);
            // 
            // startPageControl
            // 
            this.startPageControl.Location = new System.Drawing.Point(255, 135);
            this.startPageControl.Name = "startPageControl";
            this.startPageControl.Size = new System.Drawing.Size(195, 21);
            this.startPageControl.TabIndex = 13;
            this.startPageControl.Leave += new System.EventHandler(this.startPageControl_Leave);
            // 
            // startPageLabel
            // 
            this.startPageLabel.Location = new System.Drawing.Point(255, 105);
            this.startPageLabel.Name = "startPageLabel";
            this.startPageLabel.Size = new System.Drawing.Size(195, 25);
            this.startPageLabel.TabIndex = 14;
            this.startPageLabel.Text = "Начальная страница";
            this.startPageLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // TerminalConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.startPageLabel);
            this.Controls.Add(this.startPageControl);
            this.Controls.Add(this.pagesLabel);
            this.Controls.Add(this.pagesControl);
            this.Controls.Add(this.terminallayoutGroupBox);
            this.Controls.Add(this.PINUpDown);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.terminalPINLabel);
            this.Controls.Add(this.currentDayRecordingCheckBox);
            this.Name = "TerminalConfigControl";
            this.Size = new System.Drawing.Size(654, 425);
            this.Load += new System.EventHandler(this.TerminalConfigControl_Load);
            this.terminallayoutGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PINUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox terminallayoutGroupBox;
        private System.Windows.Forms.NumericUpDown columnsUpDown;
        private System.Windows.Forms.NumericUpDown rowsUpDown;
        private System.Windows.Forms.Label terminalRowsLabel;
        private System.Windows.Forms.Label terminalColumnsLabel;
        private System.Windows.Forms.NumericUpDown PINUpDown;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label terminalPINLabel;
        private System.Windows.Forms.CheckBox currentDayRecordingCheckBox;
        private System.Windows.Forms.Label pagesLabel;
        private UI.WinForms.EnumFlagsControl pagesControl;
        private UI.WinForms.EnumItemControl startPageControl;
        private System.Windows.Forms.Label startPageLabel;
    }
}
