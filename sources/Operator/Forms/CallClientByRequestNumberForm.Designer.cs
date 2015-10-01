namespace Queue.Operator
{
    partial class CallClientByRequestNumberForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.submitButton = new System.Windows.Forms.Button();
            this.clientRequestNumberLabel = new System.Windows.Forms.Label();
            this.clientRequestNumberUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.clientRequestNumberUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(105, 35);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 25);
            this.submitButton.TabIndex = 12;
            this.submitButton.Text = "Вызвать";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // clientRequestNumberLabel
            // 
            this.clientRequestNumberLabel.Location = new System.Drawing.Point(5, 5);
            this.clientRequestNumberLabel.Name = "clientRequestNumberLabel";
            this.clientRequestNumberLabel.Size = new System.Drawing.Size(100, 25);
            this.clientRequestNumberLabel.TabIndex = 13;
            this.clientRequestNumberLabel.Text = "Номер клиента";
            this.clientRequestNumberLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // clientRequestNumberUpDown
            // 
            this.clientRequestNumberUpDown.Location = new System.Drawing.Point(105, 10);
            this.clientRequestNumberUpDown.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.clientRequestNumberUpDown.Name = "clientRequestNumberUpDown";
            this.clientRequestNumberUpDown.Size = new System.Drawing.Size(75, 20);
            this.clientRequestNumberUpDown.TabIndex = 14;
            // 
            // CallClientByRequestNumberForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 66);
            this.Controls.Add(this.clientRequestNumberUpDown);
            this.Controls.Add(this.clientRequestNumberLabel);
            this.Controls.Add(this.submitButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "CallClientByRequestNumberForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Вызвать по номеру";
            ((System.ComponentModel.ISupportInitialize)(this.clientRequestNumberUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Label clientRequestNumberLabel;
        private System.Windows.Forms.NumericUpDown clientRequestNumberUpDown;
    }
}