namespace Queue.Operator
{
    partial class EditClientRequestAdditionalServiceForm
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
            this.additionalServiceLabel = new System.Windows.Forms.Label();
            this.quantityLabel = new System.Windows.Forms.Label();
            this.additionalServiceControl = new Queue.UI.WinForms.IdentifiedEntityControl();
            this.quantityUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.quantityUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(330, 65);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Записать";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // additionalServiceLabel
            // 
            this.additionalServiceLabel.Location = new System.Drawing.Point(5, 10);
            this.additionalServiceLabel.Name = "additionalServiceLabel";
            this.additionalServiceLabel.Size = new System.Drawing.Size(145, 25);
            this.additionalServiceLabel.TabIndex = 6;
            this.additionalServiceLabel.Text = "Дополнительная услуга";
            this.additionalServiceLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // quantityLabel
            // 
            this.quantityLabel.Location = new System.Drawing.Point(5, 35);
            this.quantityLabel.Name = "quantityLabel";
            this.quantityLabel.Size = new System.Drawing.Size(145, 25);
            this.quantityLabel.TabIndex = 7;
            this.quantityLabel.Text = "Количество";
            this.quantityLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // additionalServiceControl
            // 
            this.additionalServiceControl.Location = new System.Drawing.Point(150, 15);
            this.additionalServiceControl.Name = "additionalServiceControl";
            this.additionalServiceControl.Size = new System.Drawing.Size(250, 21);
            this.additionalServiceControl.TabIndex = 10;
            this.additionalServiceControl.UseResetButton = false;
            this.additionalServiceControl.Leave += new System.EventHandler(this.additionalServiceControl_Leave);
            // 
            // quantityUpDown
            // 
            this.quantityUpDown.DecimalPlaces = 2;
            this.quantityUpDown.Location = new System.Drawing.Point(150, 40);
            this.quantityUpDown.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.quantityUpDown.Name = "quantityUpDown";
            this.quantityUpDown.Size = new System.Drawing.Size(60, 20);
            this.quantityUpDown.TabIndex = 11;
            this.quantityUpDown.Leave += new System.EventHandler(this.quantityUpDown_Leave);
            // 
            // EditClientRequestAdditionalServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 96);
            this.Controls.Add(this.quantityUpDown);
            this.Controls.Add(this.additionalServiceControl);
            this.Controls.Add(this.quantityLabel);
            this.Controls.Add(this.additionalServiceLabel);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(100, 130);
            this.Name = "EditClientRequestAdditionalServiceForm";
            this.Text = "Редактирование дополнительной услуги запроса клиента";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditClientRequestAdditionalServiceForm_FormClosed);
            this.Load += new System.EventHandler(this.EditClientRequestAdditionalServiceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.quantityUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Label additionalServiceLabel;
        private System.Windows.Forms.Label quantityLabel;
        private UI.WinForms.IdentifiedEntityControl additionalServiceControl;
        private System.Windows.Forms.NumericUpDown quantityUpDown;
    }
}