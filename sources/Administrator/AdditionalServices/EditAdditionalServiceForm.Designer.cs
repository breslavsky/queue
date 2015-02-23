namespace Queue.Administrator
{
    partial class EditAdditionalServiceForm
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
            this.components = new System.ComponentModel.Container();
            this.saveButton = new System.Windows.Forms.Button();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.additionalServiceBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nameLabel = new System.Windows.Forms.Label();
            this.measureLabel = new System.Windows.Forms.Label();
            this.measureTextBox = new System.Windows.Forms.TextBox();
            this.priceLabel = new System.Windows.Forms.Label();
            this.priceUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.additionalServiceBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.priceUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(305, 130);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Записать";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.additionalServiceBindingSource, "Name", true));
            this.nameTextBox.Location = new System.Drawing.Point(140, 5);
            this.nameTextBox.Multiline = true;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(240, 60);
            this.nameTextBox.TabIndex = 3;
            // 
            // additionalServiceBindingSource
            // 
            this.additionalServiceBindingSource.DataSource = typeof(Queue.Services.DTO.AdditionalService);
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(5, 5);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(135, 60);
            this.nameLabel.TabIndex = 4;
            this.nameLabel.Text = "Наименование";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // measureLabel
            // 
            this.measureLabel.Location = new System.Drawing.Point(5, 95);
            this.measureLabel.Name = "measureLabel";
            this.measureLabel.Size = new System.Drawing.Size(135, 30);
            this.measureLabel.TabIndex = 6;
            this.measureLabel.Text = "Единица измерения";
            this.measureLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // measureTextBox
            // 
            this.measureTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.additionalServiceBindingSource, "Measure", true));
            this.measureTextBox.Location = new System.Drawing.Point(140, 105);
            this.measureTextBox.Name = "measureTextBox";
            this.measureTextBox.Size = new System.Drawing.Size(77, 20);
            this.measureTextBox.TabIndex = 5;
            // 
            // priceLabel
            // 
            this.priceLabel.Location = new System.Drawing.Point(5, 65);
            this.priceLabel.Name = "priceLabel";
            this.priceLabel.Size = new System.Drawing.Size(135, 30);
            this.priceLabel.TabIndex = 8;
            this.priceLabel.Text = "Стоимость";
            this.priceLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // priceUpDown
            // 
            this.priceUpDown.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.additionalServiceBindingSource, "Price", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "N2"));
            this.priceUpDown.DecimalPlaces = 2;
            this.priceUpDown.Location = new System.Drawing.Point(140, 75);
            this.priceUpDown.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.priceUpDown.Name = "priceUpDown";
            this.priceUpDown.Size = new System.Drawing.Size(60, 20);
            this.priceUpDown.TabIndex = 9;
            // 
            // EditAdditionalServiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 161);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.priceLabel);
            this.Controls.Add(this.priceUpDown);
            this.Controls.Add(this.measureLabel);
            this.Controls.Add(this.measureTextBox);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(100, 130);
            this.Name = "EditAdditionalServiceForm";
            this.Text = "Редактирование дополнительной услуги";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.EditAdditionalServiceForm_FormClosed);
            this.Load += new System.EventHandler(this.EditAdditionalServiceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.additionalServiceBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.priceUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.Label measureLabel;
        private System.Windows.Forms.TextBox measureTextBox;
        private System.Windows.Forms.Label priceLabel;
        private System.Windows.Forms.BindingSource additionalServiceBindingSource;
        private System.Windows.Forms.NumericUpDown priceUpDown;
    }
}