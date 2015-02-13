namespace Queue.Administrator
{
    partial class NotificationConfigControl
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
            this.saveButton = new System.Windows.Forms.Button();
            this.clientRequestsLengthUpDown = new System.Windows.Forms.NumericUpDown();
            this.clientRequestsLengthLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.clientRequestsLengthUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(135, 40);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Записать";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // clientRequestsLengthUpDown
            // 
            this.clientRequestsLengthUpDown.Location = new System.Drawing.Point(140, 15);
            this.clientRequestsLengthUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.clientRequestsLengthUpDown.Name = "clientRequestsLengthUpDown";
            this.clientRequestsLengthUpDown.Size = new System.Drawing.Size(70, 20);
            this.clientRequestsLengthUpDown.TabIndex = 4;
            this.clientRequestsLengthUpDown.Leave += new System.EventHandler(this.clientRequestsLengthUpDown_Leave);
            // 
            // clientRequestsLengthLabel
            // 
            this.clientRequestsLengthLabel.Location = new System.Drawing.Point(5, 5);
            this.clientRequestsLengthLabel.Name = "clientRequestsLengthLabel";
            this.clientRequestsLengthLabel.Size = new System.Drawing.Size(135, 30);
            this.clientRequestsLengthLabel.TabIndex = 5;
            this.clientRequestsLengthLabel.Text = "Длина списка запросов";
            this.clientRequestsLengthLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // NotificationConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.clientRequestsLengthUpDown);
            this.Controls.Add(this.clientRequestsLengthLabel);
            this.Name = "NotificationConfigControl";
            this.Size = new System.Drawing.Size(220, 70);
            ((System.ComponentModel.ISupportInitialize)(this.clientRequestsLengthUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.NumericUpDown clientRequestsLengthUpDown;
        private System.Windows.Forms.Label clientRequestsLengthLabel;
    }
}
