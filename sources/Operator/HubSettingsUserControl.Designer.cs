namespace Queue.Operator
{
    partial class HubSettingsUserControl
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
            this.endpointLabel = new System.Windows.Forms.Label();
            this.endpointTextBox = new System.Windows.Forms.TextBox();
            this.enabledCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // endpointLabel
            // 
            this.endpointLabel.Location = new System.Drawing.Point(5, 25);
            this.endpointLabel.Name = "endpointLabel";
            this.endpointLabel.Size = new System.Drawing.Size(95, 25);
            this.endpointLabel.TabIndex = 5;
            this.endpointLabel.Text = "Концентратор";
            this.endpointLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // endpointTextBox
            // 
            this.endpointTextBox.Location = new System.Drawing.Point(100, 30);
            this.endpointTextBox.Name = "endpointTextBox";
            this.endpointTextBox.Size = new System.Drawing.Size(175, 20);
            this.endpointTextBox.TabIndex = 4;
            this.endpointTextBox.Leave += new System.EventHandler(this.endpointTextBox_Leave);
            // 
            // enabledCheckBox
            // 
            this.enabledCheckBox.AutoSize = true;
            this.enabledCheckBox.Location = new System.Drawing.Point(5, 5);
            this.enabledCheckBox.Name = "enabledCheckBox";
            this.enabledCheckBox.Size = new System.Drawing.Size(121, 17);
            this.enabledCheckBox.TabIndex = 3;
            this.enabledCheckBox.Text = "Разрешить сервис";
            this.enabledCheckBox.UseVisualStyleBackColor = true;
            this.enabledCheckBox.Leave += new System.EventHandler(this.enabledCheckBox_Leave);
            // 
            // HubSettingsUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.endpointLabel);
            this.Controls.Add(this.endpointTextBox);
            this.Controls.Add(this.enabledCheckBox);
            this.Name = "HubSettingsUserControl";
            this.Size = new System.Drawing.Size(280, 55);
            this.Load += new System.EventHandler(this.HubSettingsUserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label endpointLabel;
        private System.Windows.Forms.TextBox endpointTextBox;
        private System.Windows.Forms.CheckBox enabledCheckBox;
    }
}
