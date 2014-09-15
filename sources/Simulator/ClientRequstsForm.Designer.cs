namespace Queue.Simulator
{
    partial class ClientRequstsForm
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
            this.delayUpDown = new System.Windows.Forms.NumericUpDown();
            this.delayLabel = new System.Windows.Forms.Label();
            this.delayUnitDabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.logTextBox = new System.Windows.Forms.TextBox();
            this.workersLabel = new System.Windows.Forms.Label();
            this.workersUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.delayUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workersUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // delayUpDown
            // 
            this.delayUpDown.Location = new System.Drawing.Point(190, 10);
            this.delayUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.delayUpDown.Name = "delayUpDown";
            this.delayUpDown.Size = new System.Drawing.Size(45, 20);
            this.delayUpDown.TabIndex = 0;
            this.delayUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // delayLabel
            // 
            this.delayLabel.AutoSize = true;
            this.delayLabel.Location = new System.Drawing.Point(125, 15);
            this.delayLabel.Name = "delayLabel";
            this.delayLabel.Size = new System.Drawing.Size(58, 13);
            this.delayLabel.TabIndex = 0;
            this.delayLabel.Text = "Задержка";
            // 
            // delayUnitDabel
            // 
            this.delayUnitDabel.AutoSize = true;
            this.delayUnitDabel.Location = new System.Drawing.Point(240, 15);
            this.delayUnitDabel.Name = "delayUnitDabel";
            this.delayUnitDabel.Size = new System.Drawing.Size(28, 13);
            this.delayUnitDabel.TabIndex = 0;
            this.delayUnitDabel.Text = "сек.";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(275, 10);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 25);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Пуск";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Enabled = false;
            this.stopButton.Location = new System.Drawing.Point(355, 10);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(75, 25);
            this.stopButton.TabIndex = 0;
            this.stopButton.Text = "Стоп";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // logTextBox
            // 
            this.logTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logTextBox.Location = new System.Drawing.Point(10, 45);
            this.logTextBox.Multiline = true;
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.Size = new System.Drawing.Size(484, 240);
            this.logTextBox.TabIndex = 0;
            // 
            // workersLabel
            // 
            this.workersLabel.AutoSize = true;
            this.workersLabel.Location = new System.Drawing.Point(10, 15);
            this.workersLabel.Name = "workersLabel";
            this.workersLabel.Size = new System.Drawing.Size(50, 13);
            this.workersLabel.TabIndex = 0;
            this.workersLabel.Text = "Потоков";
            // 
            // workersUpDown
            // 
            this.workersUpDown.Location = new System.Drawing.Point(70, 10);
            this.workersUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.workersUpDown.Name = "workersUpDown";
            this.workersUpDown.Size = new System.Drawing.Size(45, 20);
            this.workersUpDown.TabIndex = 0;
            this.workersUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // ClientRequstsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 292);
            this.Controls.Add(this.workersUpDown);
            this.Controls.Add(this.workersLabel);
            this.Controls.Add(this.logTextBox);
            this.Controls.Add(this.stopButton);
            this.Controls.Add(this.startButton);
            this.Controls.Add(this.delayUnitDabel);
            this.Controls.Add(this.delayLabel);
            this.Controls.Add(this.delayUpDown);
            this.MinimumSize = new System.Drawing.Size(520, 330);
            this.Name = "ClientRequstsForm";
            this.Text = "Симулятор запросов клиентов";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ClientRequstsForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.delayUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workersUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown delayUpDown;
        private System.Windows.Forms.Label delayLabel;
        private System.Windows.Forms.Label delayUnitDabel;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.TextBox logTextBox;
        private System.Windows.Forms.Label workersLabel;
        private System.Windows.Forms.NumericUpDown workersUpDown;
    }
}