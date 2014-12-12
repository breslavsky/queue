namespace Queue.Administrator
{
    partial class CouponConfigControl
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
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.couponTemplateEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.previewLabel = new System.Windows.Forms.LinkLabel();
            this.templateLabel = new System.Windows.Forms.LinkLabel();
            this.saveButton = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.couponTemplateEditor, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(618, 435);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // couponTemplateEditor
            // 
            this.couponTemplateEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.couponTemplateEditor.IsReadOnly = false;
            this.couponTemplateEditor.LineViewerStyle = ICSharpCode.TextEditor.Document.LineViewerStyle.FullRow;
            this.couponTemplateEditor.Location = new System.Drawing.Point(0, 0);
            this.couponTemplateEditor.Margin = new System.Windows.Forms.Padding(0);
            this.couponTemplateEditor.Name = "couponTemplateEditor";
            this.couponTemplateEditor.Size = new System.Drawing.Size(618, 400);
            this.couponTemplateEditor.TabIndex = 0;
            this.couponTemplateEditor.Leave += new System.EventHandler(this.couponTemplateEditor_Leave);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.previewLabel);
            this.panel1.Controls.Add(this.templateLabel);
            this.panel1.Controls.Add(this.saveButton);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 400);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(618, 35);
            this.panel1.TabIndex = 0;
            // 
            // previewLabel
            // 
            this.previewLabel.AutoSize = true;
            this.previewLabel.Location = new System.Drawing.Point(5, 5);
            this.previewLabel.Name = "previewLabel";
            this.previewLabel.Size = new System.Drawing.Size(86, 13);
            this.previewLabel.TabIndex = 0;
            this.previewLabel.TabStop = true;
            this.previewLabel.Text = "[предпросмотр]";
            this.previewLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.previewLabel_LinkClicked);
            // 
            // templateLabel
            // 
            this.templateLabel.AutoSize = true;
            this.templateLabel.Location = new System.Drawing.Point(90, 5);
            this.templateLabel.Name = "templateLabel";
            this.templateLabel.Size = new System.Drawing.Size(84, 13);
            this.templateLabel.TabIndex = 0;
            this.templateLabel.TabStop = true;
            this.templateLabel.Text = "[по умолчанию]";
            this.templateLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.templateLabel_LinkClicked);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(537, 5);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Сохранить";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // CouponConfigControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel);
            this.Name = "CouponConfigControl";
            this.Size = new System.Drawing.Size(618, 435);
            this.tableLayoutPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private ICSharpCode.TextEditor.TextEditorControl couponTemplateEditor;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel previewLabel;
        private System.Windows.Forms.LinkLabel templateLabel;
        private System.Windows.Forms.Button saveButton;
    }
}
