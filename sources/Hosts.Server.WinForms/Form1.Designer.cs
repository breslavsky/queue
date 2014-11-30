namespace Queue.Hosts.Server.WinForms
{
    partial class Form1
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
            this.editDatabaseSettingsUserControl1 = new Junte.UI.WinForms.NHibernate.EditDatabaseSettingsUserControl();
            this.SuspendLayout();
            // 
            // editDatabaseSettingsUserControl1
            // 
            this.editDatabaseSettingsUserControl1.Location = new System.Drawing.Point(45, 25);
            this.editDatabaseSettingsUserControl1.Name = "editDatabaseSettingsUserControl1";
            this.editDatabaseSettingsUserControl1.Settings = null;
            this.editDatabaseSettingsUserControl1.Size = new System.Drawing.Size(340, 186);
            this.editDatabaseSettingsUserControl1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 262);
            this.Controls.Add(this.editDatabaseSettingsUserControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Junte.UI.WinForms.NHibernate.EditDatabaseSettingsUserControl editDatabaseSettingsUserControl1;

    }
}