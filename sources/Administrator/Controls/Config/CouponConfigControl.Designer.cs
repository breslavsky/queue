namespace Queue.Administrator
{
    partial class CouponConfigControl
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
            this.sectionsLabel = new System.Windows.Forms.Label();
            this.sectionsControl = new Queue.UI.WinForms.EnumFlagsControl();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // sectionsLabel
            // 
            this.sectionsLabel.Location = new System.Drawing.Point(4, 11);
            this.sectionsLabel.Name = "sectionsLabel";
            this.sectionsLabel.Size = new System.Drawing.Size(195, 18);
            this.sectionsLabel.TabIndex = 4;
            this.sectionsLabel.Text = "Разделы талона";
            this.sectionsLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // sectionsControl
            // 
            this.sectionsControl.Location = new System.Drawing.Point(19, 36);
            this.sectionsControl.Name = "sectionsControl";
            this.sectionsControl.Size = new System.Drawing.Size(180, 125);
            this.sectionsControl.TabIndex = 5;
            this.sectionsControl.Leave += new System.EventHandler(this.sectionsControl_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(124, 167);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 9;
            this.saveButton.Text = "Записать";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // CouponConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.sectionsLabel);
            this.Controls.Add(this.sectionsControl);
            this.Name = "CouponConfigControl";
            this.Size = new System.Drawing.Size(219, 208);
            this.Load += new System.EventHandler(this.CouponConfigControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label sectionsLabel;
        private UI.WinForms.EnumFlagsControl sectionsControl;
        private System.Windows.Forms.Button saveButton;

    }
}
