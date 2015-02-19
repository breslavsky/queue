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
            this.headerLabel = new System.Windows.Forms.Label();
            this.headerTextBox = new System.Windows.Forms.TextBox();
            this.footerLabel = new System.Windows.Forms.Label();
            this.footerTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // currentDayRecordingCheckBox
            // 
            this.currentDayRecordingCheckBox.Location = new System.Drawing.Point(110, 285);
            this.currentDayRecordingCheckBox.Name = "currentDayRecordingCheckBox";
            this.currentDayRecordingCheckBox.Size = new System.Drawing.Size(275, 20);
            this.currentDayRecordingCheckBox.TabIndex = 1;
            this.currentDayRecordingCheckBox.Text = "Запись на текущий день";
            this.currentDayRecordingCheckBox.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.currentDayRecordingCheckBox.UseVisualStyleBackColor = true;
            this.currentDayRecordingCheckBox.Leave += new System.EventHandler(this.portalCurrentDayRecordingCheckBox_Leave);
            // 
            // headerLabel
            // 
            this.headerLabel.Location = new System.Drawing.Point(40, -10);
            this.headerLabel.Name = "headerLabel";
            this.headerLabel.Size = new System.Drawing.Size(65, 30);
            this.headerLabel.TabIndex = 2;
            this.headerLabel.Text = "Заголовок";
            this.headerLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // headerTextBox
            // 
            this.headerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.headerTextBox.Location = new System.Drawing.Point(110, 5);
            this.headerTextBox.Multiline = true;
            this.headerTextBox.Name = "headerTextBox";
            this.headerTextBox.ReadOnly = true;
            this.headerTextBox.Size = new System.Drawing.Size(560, 130);
            this.headerTextBox.TabIndex = 3;
            this.headerTextBox.Click += new System.EventHandler(this.headerTextBox_Click);
            this.headerTextBox.Leave += new System.EventHandler(this.headerTextBox_Leave);
            // 
            // footerLabel
            // 
            this.footerLabel.Location = new System.Drawing.Point(25, 130);
            this.footerLabel.Name = "footerLabel";
            this.footerLabel.Size = new System.Drawing.Size(80, 30);
            this.footerLabel.TabIndex = 4;
            this.footerLabel.Text = "Нижняя часть";
            this.footerLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // footerTextBox
            // 
            this.footerTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.footerTextBox.Location = new System.Drawing.Point(110, 145);
            this.footerTextBox.Multiline = true;
            this.footerTextBox.Name = "footerTextBox";
            this.footerTextBox.ReadOnly = true;
            this.footerTextBox.Size = new System.Drawing.Size(560, 130);
            this.footerTextBox.TabIndex = 5;
            this.footerTextBox.Click += new System.EventHandler(this.footerTextBox_Click);
            this.footerTextBox.Leave += new System.EventHandler(this.footerTextBox_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(595, 280);
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
            this.Controls.Add(this.headerLabel);
            this.Controls.Add(this.headerTextBox);
            this.Controls.Add(this.footerLabel);
            this.Controls.Add(this.footerTextBox);
            this.Controls.Add(this.saveButton);
            this.Name = "PortalConfigControl";
            this.Size = new System.Drawing.Size(680, 320);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox currentDayRecordingCheckBox;
        private System.Windows.Forms.Label headerLabel;
        private System.Windows.Forms.TextBox headerTextBox;
        private System.Windows.Forms.Label footerLabel;
        private System.Windows.Forms.TextBox footerTextBox;
        private System.Windows.Forms.Button saveButton;
    }
}
