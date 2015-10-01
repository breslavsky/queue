namespace Queue.Operator
{
    partial class RedirectToOperatorForm
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
            this.redirectOperatorControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.submitButton = new System.Windows.Forms.Button();
            this.redirectOperatorLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // redirectOperatorControl
            // 
            this.redirectOperatorControl.Location = new System.Drawing.Point(135, 10);
            this.redirectOperatorControl.Name = "redirectOperatorControl";
            this.redirectOperatorControl.Size = new System.Drawing.Size(150, 21);
            this.redirectOperatorControl.TabIndex = 10;
            this.redirectOperatorControl.UseResetButton = false;
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(210, 35);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(75, 25);
            this.submitButton.TabIndex = 12;
            this.submitButton.Text = "Передать";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // redirectOperatorLabel
            // 
            this.redirectOperatorLabel.Location = new System.Drawing.Point(5, 5);
            this.redirectOperatorLabel.Name = "redirectOperatorLabel";
            this.redirectOperatorLabel.Size = new System.Drawing.Size(130, 25);
            this.redirectOperatorLabel.TabIndex = 13;
            this.redirectOperatorLabel.Text = "Оператор для передачи";
            this.redirectOperatorLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // RedirectToOperatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 66);
            this.Controls.Add(this.redirectOperatorLabel);
            this.Controls.Add(this.redirectOperatorControl);
            this.Controls.Add(this.submitButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "RedirectToOperatorForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Передать оператору";
            this.Load += new System.EventHandler(this.RedirectToOperatorForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private UI.WinForms.IdentifiedEntityControl redirectOperatorControl;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.Label redirectOperatorLabel;
    }
}