namespace Queue.UI.WinForms
{
    partial class SelectServiceForm
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
            this.serviceControl = new Queue.UI.WinForms.SelectServiceControl();
            this.SuspendLayout();
            // 
            // selectServiceControl
            // 
            this.serviceControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.serviceControl.Location = new System.Drawing.Point(10, 10);
            this.serviceControl.Name = "selectServiceControl";
            this.serviceControl.Size = new System.Drawing.Size(544, 412);
            this.serviceControl.TabIndex = 0;
            this.serviceControl.Selected += new System.EventHandler<System.EventArgs>(this.serviceControl_Selected);
            // 
            // SelectServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 432);
            this.Controls.Add(this.serviceControl);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(580, 470);
            this.Name = "SelectServiceForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Выбор услуги";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServicesForm_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private SelectServiceControl serviceControl;

    }
}