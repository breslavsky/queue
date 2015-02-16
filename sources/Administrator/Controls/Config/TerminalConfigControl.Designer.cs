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
            this.windowTemplateEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this.previewLabel = new System.Windows.Forms.LinkLabel();
            this.templateLabel = new System.Windows.Forms.LinkLabel();
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
            this.columnsUpDown.Location = new System.Drawing.Point(65, 20);
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
            this.rowsUpDown.Location = new System.Drawing.Point(165, 20);
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
            this.terminalRowsLabel.AutoSize = true;
            this.terminalRowsLabel.Location = new System.Drawing.Point(125, 25);
            this.terminalRowsLabel.Name = "terminalRowsLabel";
            this.terminalRowsLabel.Size = new System.Drawing.Size(37, 13);
            this.terminalRowsLabel.TabIndex = 4;
            this.terminalRowsLabel.Text = "Строк";
            // 
            // terminalColumnsLabel
            // 
            this.terminalColumnsLabel.AutoSize = true;
            this.terminalColumnsLabel.Location = new System.Drawing.Point(12, 25);
            this.terminalColumnsLabel.Name = "terminalColumnsLabel";
            this.terminalColumnsLabel.Size = new System.Drawing.Size(50, 13);
            this.terminalColumnsLabel.TabIndex = 2;
            this.terminalColumnsLabel.Text = "Колонок";
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
            this.terminalPINLabel.AutoSize = true;
            this.terminalPINLabel.Location = new System.Drawing.Point(7, 10);
            this.terminalPINLabel.Name = "terminalPINLabel";
            this.terminalPINLabel.Size = new System.Drawing.Size(46, 13);
            this.terminalPINLabel.TabIndex = 8;
            this.terminalPINLabel.Text = "PIN-код";
            // 
            // currentDayRecordingCheckBox
            // 
            this.currentDayRecordingCheckBox.AutoSize = true;
            this.currentDayRecordingCheckBox.Location = new System.Drawing.Point(10, 95);
            this.currentDayRecordingCheckBox.Name = "currentDayRecordingCheckBox";
            this.currentDayRecordingCheckBox.Size = new System.Drawing.Size(151, 17);
            this.currentDayRecordingCheckBox.TabIndex = 9;
            this.currentDayRecordingCheckBox.Text = "Запись на текущий день";
            this.currentDayRecordingCheckBox.UseVisualStyleBackColor = true;
            this.currentDayRecordingCheckBox.Leave += new System.EventHandler(this.currentDayRecordingCheckBox_Leave);
            // 
            // windowTemplateEditor
            // 
            this.windowTemplateEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.windowTemplateEditor.IsReadOnly = false;
            this.windowTemplateEditor.LineViewerStyle = ICSharpCode.TextEditor.Document.LineViewerStyle.FullRow;
            this.windowTemplateEditor.Location = new System.Drawing.Point(5, 120);
            this.windowTemplateEditor.Margin = new System.Windows.Forms.Padding(0);
            this.windowTemplateEditor.Name = "windowTemplateEditor";
            this.windowTemplateEditor.Size = new System.Drawing.Size(640, 264);
            this.windowTemplateEditor.TabIndex = 11;
            this.windowTemplateEditor.Leave += new System.EventHandler(this.windowTemplateEditor_Leave);
            // 
            // previewLabel
            // 
            this.previewLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.previewLabel.AutoSize = true;
            this.previewLabel.Location = new System.Drawing.Point(5, 390);
            this.previewLabel.Name = "previewLabel";
            this.previewLabel.Size = new System.Drawing.Size(86, 13);
            this.previewLabel.TabIndex = 12;
            this.previewLabel.TabStop = true;
            this.previewLabel.Text = "[предпросмотр]";
            this.previewLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.previewLabel_LinkClicked);
            // 
            // templateLabel
            // 
            this.templateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.templateLabel.AutoSize = true;
            this.templateLabel.Location = new System.Drawing.Point(90, 390);
            this.templateLabel.Name = "templateLabel";
            this.templateLabel.Size = new System.Drawing.Size(84, 13);
            this.templateLabel.TabIndex = 13;
            this.templateLabel.TabStop = true;
            this.templateLabel.Text = "[по умолчанию]";
            this.templateLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.templateLabel_LinkClicked);
            // 
            // TerminalConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.previewLabel);
            this.Controls.Add(this.templateLabel);
            this.Controls.Add(this.windowTemplateEditor);
            this.Controls.Add(this.terminallayoutGroupBox);
            this.Controls.Add(this.PINUpDown);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.terminalPINLabel);
            this.Controls.Add(this.currentDayRecordingCheckBox);
            this.Name = "TerminalConfigControl";
            this.Size = new System.Drawing.Size(654, 425);
            this.terminallayoutGroupBox.ResumeLayout(false);
            this.terminallayoutGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.columnsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rowsUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PINUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private ICSharpCode.TextEditor.TextEditorControl windowTemplateEditor;
        private System.Windows.Forms.LinkLabel previewLabel;
        private System.Windows.Forms.LinkLabel templateLabel;
    }
}
