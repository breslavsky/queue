namespace Queue.UI.WinForms
{
    partial class HtmlEditorForm
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
            this.htmlEditorControl = new ICSharpCode.TextEditor.TextEditorControl();
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.okCancelPanel = new Junte.UI.WinForms.OkCancelPanel();
            this.mainLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // htmlEditorControl
            // 
            this.htmlEditorControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlEditorControl.IsReadOnly = false;
            this.htmlEditorControl.Location = new System.Drawing.Point(3, 3);
            this.htmlEditorControl.Name = "htmlEditorControl";
            this.htmlEditorControl.Size = new System.Drawing.Size(558, 346);
            this.htmlEditorControl.TabIndex = 0;
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.Controls.Add(this.htmlEditorControl, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.okCancelPanel, 0, 1);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(10, 10);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(564, 392);
            this.mainLayoutPanel.TabIndex = 0;
            // 
            // okCancelPanel
            // 
            this.okCancelPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.okCancelPanel.Location = new System.Drawing.Point(3, 355);
            this.okCancelPanel.Name = "okCancelPanel";
            this.okCancelPanel.Size = new System.Drawing.Size(558, 34);
            this.okCancelPanel.TabIndex = 0;
            // 
            // HtmlEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.mainLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(600, 450);
            this.Name = "HtmlEditorForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Текстовый редактор";
            this.mainLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ICSharpCode.TextEditor.TextEditorControl htmlEditorControl;
        private System.Windows.Forms.TableLayoutPanel mainLayoutPanel;
        private Junte.UI.WinForms.OkCancelPanel okCancelPanel;
    }
}