using Queue.UI.WinForms;
namespace Queue.Administrator
{
    partial class EditServiceRenderingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.operatorLabel = new System.Windows.Forms.Label();
            this.serviceStepLabel = new System.Windows.Forms.Label();
            this.priorityLabel = new System.Windows.Forms.Label();
            this.priorityUpDown = new System.Windows.Forms.NumericUpDown();
            this.saveButton = new System.Windows.Forms.Button();
            this.mode = new System.Windows.Forms.Label();
            this.serviceStepControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.operatorControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.modeСontrol = new Queue.UI.WinForms.EnumItemControl();
            ((System.ComponentModel.ISupportInitialize)(this.priorityUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // operatorLabel
            // 
            this.operatorLabel.Location = new System.Drawing.Point(10, 10);
            this.operatorLabel.Name = "operatorLabel";
            this.operatorLabel.Size = new System.Drawing.Size(95, 20);
            this.operatorLabel.TabIndex = 0;
            this.operatorLabel.Text = "Оператор";
            this.operatorLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceStepLabel
            // 
            this.serviceStepLabel.Location = new System.Drawing.Point(10, 35);
            this.serviceStepLabel.Name = "serviceStepLabel";
            this.serviceStepLabel.Size = new System.Drawing.Size(95, 20);
            this.serviceStepLabel.TabIndex = 0;
            this.serviceStepLabel.Text = "Этап услуги";
            this.serviceStepLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // priorityLabel
            // 
            this.priorityLabel.Location = new System.Drawing.Point(10, 85);
            this.priorityLabel.Name = "priorityLabel";
            this.priorityLabel.Size = new System.Drawing.Size(95, 20);
            this.priorityLabel.TabIndex = 0;
            this.priorityLabel.Text = "Приоритет";
            this.priorityLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // priorityUpDown
            // 
            this.priorityUpDown.Location = new System.Drawing.Point(110, 85);
            this.priorityUpDown.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.priorityUpDown.Name = "priorityUpDown";
            this.priorityUpDown.Size = new System.Drawing.Size(60, 20);
            this.priorityUpDown.TabIndex = 3;
            this.priorityUpDown.Leave += new System.EventHandler(this.priorityUpDown_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(240, 110);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // mode
            // 
            this.mode.Location = new System.Drawing.Point(10, 60);
            this.mode.Name = "mode";
            this.mode.Size = new System.Drawing.Size(95, 20);
            this.mode.TabIndex = 0;
            this.mode.Text = "Режим";
            this.mode.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // serviceStepControl
            // 
            this.serviceStepControl.Location = new System.Drawing.Point(110, 35);
            this.serviceStepControl.Name = "serviceStepControl";
            this.serviceStepControl.Size = new System.Drawing.Size(205, 21);
            this.serviceStepControl.TabIndex = 1;
            this.serviceStepControl.UseResetButton = true;
            this.serviceStepControl.Leave += new System.EventHandler(this.serviceStepControl_Leave);
            // 
            // operatorControl
            // 
            this.operatorControl.Location = new System.Drawing.Point(110, 10);
            this.operatorControl.Name = "operatorControl";
            this.operatorControl.Size = new System.Drawing.Size(205, 21);
            this.operatorControl.TabIndex = 0;
            this.operatorControl.UseResetButton = false;
            this.operatorControl.Leave += new System.EventHandler(this.operatorControl_Leave);
            // 
            // modeСontrol
            // 
            this.modeСontrol.Location = new System.Drawing.Point(110, 60);
            this.modeСontrol.Name = "modeСontrol";
            this.modeСontrol.Size = new System.Drawing.Size(205, 21);
            this.modeСontrol.TabIndex = 2;
            this.modeСontrol.Leave += new System.EventHandler(this.modeСontrol_Leave);
            // 
            // EditServiceRenderingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 141);
            this.Controls.Add(this.operatorLabel);
            this.Controls.Add(this.operatorControl);
            this.Controls.Add(this.serviceStepLabel);
            this.Controls.Add(this.serviceStepControl);
            this.Controls.Add(this.mode);
            this.Controls.Add(this.modeСontrol);
            this.Controls.Add(this.priorityLabel);
            this.Controls.Add(this.priorityUpDown);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditServiceRenderingForm";
            this.Text = "Изменение обслуживания";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditServiceRenderingForm_FormClosing);
            this.Load += new System.EventHandler(this.EditServiceRenderingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.priorityUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label operatorLabel;
        private System.Windows.Forms.Label serviceStepLabel;
        private System.Windows.Forms.Label priorityLabel;
        private System.Windows.Forms.NumericUpDown priorityUpDown;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label mode;
        private IdentifiedEntityControl serviceStepControl;
        private IdentifiedEntityControl operatorControl;
        private EnumItemControl modeСontrol;
    }
}