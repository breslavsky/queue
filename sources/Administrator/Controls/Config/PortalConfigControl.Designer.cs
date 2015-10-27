namespace Queue.Administrator
{
    partial class PortalConfigControl
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
            this.currentDayRecordingCheckBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // currentDayRecordingCheckBox
            // 
            this.currentDayRecordingCheckBox.Location = new System.Drawing.Point(5, 10);
            this.currentDayRecordingCheckBox.Name = "currentDayRecordingCheckBox";
            this.currentDayRecordingCheckBox.Size = new System.Drawing.Size(175, 20);
            this.currentDayRecordingCheckBox.TabIndex = 1;
            this.currentDayRecordingCheckBox.Text = "Запись на текущий день";
            this.currentDayRecordingCheckBox.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.currentDayRecordingCheckBox.UseVisualStyleBackColor = true;
            this.currentDayRecordingCheckBox.Leave += new System.EventHandler(this.portalCurrentDayRecordingCheckBox_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(105, 40);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Записать";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // PortalConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.currentDayRecordingCheckBox);
            this.Controls.Add(this.saveButton);
            this.Name = "PortalConfigControl";
            this.Size = new System.Drawing.Size(190, 70);
            this.Load += new System.EventHandler(this.PortalConfigControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox currentDayRecordingCheckBox;
        private System.Windows.Forms.Button saveButton;
    }
}
