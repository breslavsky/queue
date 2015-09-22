namespace Queue.Operator
{
    partial class SettingsForm
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
            this.saveButton = new System.Windows.Forms.Button();
            this.hubSettingsUserControl = new Queue.Operator.HubSettingsUserControl();
            this.hubQualityGroupBox = new System.Windows.Forms.GroupBox();
            this.hubQualityGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(220, 95);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Сохранить";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // hubSettingsUserControl
            // 
            this.hubSettingsUserControl.Location = new System.Drawing.Point(5, 20);
            this.hubSettingsUserControl.Name = "hubSettingsUserControl";
            this.hubSettingsUserControl.Size = new System.Drawing.Size(280, 60);
            this.hubSettingsUserControl.TabIndex = 0;
            // 
            // hubQualityGroupBox
            // 
            this.hubQualityGroupBox.Controls.Add(this.hubSettingsUserControl);
            this.hubQualityGroupBox.Location = new System.Drawing.Point(5, 5);
            this.hubQualityGroupBox.Name = "hubQualityGroupBox";
            this.hubQualityGroupBox.Size = new System.Drawing.Size(290, 85);
            this.hubQualityGroupBox.TabIndex = 2;
            this.hubQualityGroupBox.TabStop = false;
            this.hubQualityGroupBox.Text = "Служба качества";
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 121);
            this.Controls.Add(this.hubQualityGroupBox);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.Text = "Настройки программы";
            this.hubQualityGroupBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HubSettingsUserControl hubSettingsUserControl;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.GroupBox hubQualityGroupBox;

    }
}