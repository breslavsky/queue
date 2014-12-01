namespace Queue.Operator
{
    partial class DigitalTimer
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.timerTextBlock = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timerTextBlock
            // 
            this.timerTextBlock.BackColor = System.Drawing.Color.White;
            this.timerTextBlock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.timerTextBlock.Location = new System.Drawing.Point(0, 0);
            this.timerTextBlock.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.timerTextBlock.Name = "timerTextBlock";
            this.timerTextBlock.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.timerTextBlock.Size = new System.Drawing.Size(80, 24);
            this.timerTextBlock.TabIndex = 1;
            this.timerTextBlock.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DigitalTimer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.timerTextBlock);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "DigitalTimer";
            this.Size = new System.Drawing.Size(80, 25);
            this.Load += new System.EventHandler(this.DigitalTimer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label timerTextBlock;
    }
}
