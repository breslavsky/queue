namespace Queue.Administrator
{
    partial class MediaConfigControl
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tickerGroupBox = new System.Windows.Forms.GroupBox();
            this.tickerTextBox = new System.Windows.Forms.TextBox();
            this.tickerSpeedLabel = new System.Windows.Forms.Label();
            this.TickerLabel = new System.Windows.Forms.Label();
            this.tickerSpeedTrackBar = new System.Windows.Forms.TrackBar();
            this.serviceUrlLabel = new System.Windows.Forms.Label();
            this.serviceUrlTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.uploadMediaFileProgressBar = new System.Windows.Forms.ProgressBar();
            this.mediaFilesGridView = new System.Windows.Forms.DataGridView();
            this.deleteColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uploadColumn = new System.Windows.Forms.DataGridViewButtonColumn();
            this.addMediaFileButton = new System.Windows.Forms.Button();
            this.selectMediaFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.tickerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tickerSpeedTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaFilesGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tickerGroupBox
            // 
            this.tickerGroupBox.Controls.Add(this.tickerTextBox);
            this.tickerGroupBox.Controls.Add(this.tickerSpeedLabel);
            this.tickerGroupBox.Controls.Add(this.TickerLabel);
            this.tickerGroupBox.Controls.Add(this.tickerSpeedTrackBar);
            this.tickerGroupBox.Location = new System.Drawing.Point(10, 35);
            this.tickerGroupBox.Name = "tickerGroupBox";
            this.tickerGroupBox.Size = new System.Drawing.Size(395, 160);
            this.tickerGroupBox.TabIndex = 12;
            this.tickerGroupBox.TabStop = false;
            this.tickerGroupBox.Text = "Бегущая строка";
            // 
            // tickerTextBox
            // 
            this.tickerTextBox.Location = new System.Drawing.Point(75, 25);
            this.tickerTextBox.Multiline = true;
            this.tickerTextBox.Name = "tickerTextBox";
            this.tickerTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tickerTextBox.Size = new System.Drawing.Size(310, 55);
            this.tickerTextBox.TabIndex = 2;
            this.tickerTextBox.Leave += new System.EventHandler(this.tickerTextBox_Leave);
            // 
            // tickerSpeedLabel
            // 
            this.tickerSpeedLabel.AutoSize = true;
            this.tickerSpeedLabel.Location = new System.Drawing.Point(10, 105);
            this.tickerSpeedLabel.Name = "tickerSpeedLabel";
            this.tickerSpeedLabel.Size = new System.Drawing.Size(55, 13);
            this.tickerSpeedLabel.TabIndex = 4;
            this.tickerSpeedLabel.Text = "Скорость";
            // 
            // TickerLabel
            // 
            this.TickerLabel.AutoSize = true;
            this.TickerLabel.Location = new System.Drawing.Point(10, 25);
            this.TickerLabel.Name = "TickerLabel";
            this.TickerLabel.Size = new System.Drawing.Size(37, 13);
            this.TickerLabel.TabIndex = 1;
            this.TickerLabel.Text = "Текст";
            // 
            // tickerSpeedTrackBar
            // 
            this.tickerSpeedTrackBar.BackColor = System.Drawing.Color.White;
            this.tickerSpeedTrackBar.Location = new System.Drawing.Point(75, 100);
            this.tickerSpeedTrackBar.Minimum = 1;
            this.tickerSpeedTrackBar.Name = "tickerSpeedTrackBar";
            this.tickerSpeedTrackBar.Size = new System.Drawing.Size(310, 45);
            this.tickerSpeedTrackBar.TabIndex = 3;
            this.tickerSpeedTrackBar.Value = 5;
            this.tickerSpeedTrackBar.Leave += new System.EventHandler(this.tickerSpeedTrackBar_Leave);
            // 
            // serviceUrlLabel
            // 
            this.serviceUrlLabel.AutoSize = true;
            this.serviceUrlLabel.Location = new System.Drawing.Point(5, 10);
            this.serviceUrlLabel.Name = "serviceUrlLabel";
            this.serviceUrlLabel.Size = new System.Drawing.Size(115, 13);
            this.serviceUrlLabel.TabIndex = 6;
            this.serviceUrlLabel.Text = "Адрес медиа службы";
            // 
            // serviceUrlTextBox
            // 
            this.serviceUrlTextBox.Location = new System.Drawing.Point(120, 5);
            this.serviceUrlTextBox.Name = "serviceUrlTextBox";
            this.serviceUrlTextBox.Size = new System.Drawing.Size(285, 20);
            this.serviceUrlTextBox.TabIndex = 7;
            this.serviceUrlTextBox.Leave += new System.EventHandler(this.serviceUrlTextBox_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(330, 200);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 8;
            this.saveButton.Text = "Сохранить";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // uploadMediaFileProgressBar
            // 
            this.uploadMediaFileProgressBar.Location = new System.Drawing.Point(15, 230);
            this.uploadMediaFileProgressBar.Name = "uploadMediaFileProgressBar";
            this.uploadMediaFileProgressBar.Size = new System.Drawing.Size(390, 15);
            this.uploadMediaFileProgressBar.TabIndex = 9;
            // 
            // mediaFilesGridView
            // 
            this.mediaFilesGridView.AllowUserToAddRows = false;
            this.mediaFilesGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.mediaFilesGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.mediaFilesGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.mediaFilesGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.deleteColumn,
            this.nameColumn,
            this.uploadColumn});
            this.mediaFilesGridView.Location = new System.Drawing.Point(13, 255);
            this.mediaFilesGridView.Margin = new System.Windows.Forms.Padding(0);
            this.mediaFilesGridView.MultiSelect = false;
            this.mediaFilesGridView.Name = "mediaFilesGridView";
            this.mediaFilesGridView.RowHeadersVisible = false;
            this.mediaFilesGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.mediaFilesGridView.Size = new System.Drawing.Size(392, 88);
            this.mediaFilesGridView.TabIndex = 10;
            this.mediaFilesGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.mediaFilesGridView_CellClick);
            this.mediaFilesGridView.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.mediaFilesGridView_CellEndEdit);
            // 
            // deleteColumn
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.deleteColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.deleteColumn.FillWeight = 60F;
            this.deleteColumn.HeaderText = "";
            this.deleteColumn.Name = "deleteColumn";
            this.deleteColumn.ReadOnly = true;
            this.deleteColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.deleteColumn.Text = "[удалить]";
            this.deleteColumn.UseColumnTextForLinkValue = true;
            this.deleteColumn.Width = 60;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameColumn.FillWeight = 80F;
            this.nameColumn.HeaderText = "Название";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // uploadColumn
            // 
            this.uploadColumn.FillWeight = 60F;
            this.uploadColumn.HeaderText = "";
            this.uploadColumn.Name = "uploadColumn";
            this.uploadColumn.Text = "Загрузить";
            this.uploadColumn.UseColumnTextForButtonValue = true;
            this.uploadColumn.Width = 80;
            // 
            // addMediaFileButton
            // 
            this.addMediaFileButton.Location = new System.Drawing.Point(13, 348);
            this.addMediaFileButton.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.addMediaFileButton.Name = "addMediaFileButton";
            this.addMediaFileButton.Size = new System.Drawing.Size(75, 25);
            this.addMediaFileButton.TabIndex = 11;
            this.addMediaFileButton.Text = "Добавить";
            this.addMediaFileButton.Click += new System.EventHandler(this.addMediaFileButton_Click);
            // 
            // selectMediaFileDialog
            // 
            this.selectMediaFileDialog.Filter = "Windows Media Video |*.wmv";
            // 
            // MediaConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tickerGroupBox);
            this.Controls.Add(this.serviceUrlLabel);
            this.Controls.Add(this.serviceUrlTextBox);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.uploadMediaFileProgressBar);
            this.Controls.Add(this.mediaFilesGridView);
            this.Controls.Add(this.addMediaFileButton);
            this.Name = "MediaConfigControl";
            this.Size = new System.Drawing.Size(410, 380);
            this.tickerGroupBox.ResumeLayout(false);
            this.tickerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tickerSpeedTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaFilesGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox tickerGroupBox;
        private System.Windows.Forms.TextBox tickerTextBox;
        private System.Windows.Forms.Label tickerSpeedLabel;
        private System.Windows.Forms.Label TickerLabel;
        private System.Windows.Forms.TrackBar tickerSpeedTrackBar;
        private System.Windows.Forms.Label serviceUrlLabel;
        private System.Windows.Forms.TextBox serviceUrlTextBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.ProgressBar uploadMediaFileProgressBar;
        private System.Windows.Forms.DataGridView mediaFilesGridView;
        private System.Windows.Forms.DataGridViewLinkColumn deleteColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewButtonColumn uploadColumn;
        private System.Windows.Forms.Button addMediaFileButton;
        private System.Windows.Forms.OpenFileDialog selectMediaFileDialog;
    }
}
