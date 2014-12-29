using System;
namespace Queue.Administrator
{
    partial class CurrentScheduleForm
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
            this.mainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.currentSchedulePanel = new System.Windows.Forms.Panel();
            this.currentScheduleCheckBox = new System.Windows.Forms.CheckBox();
            this.currentScheduleControl = new Queue.Administrator.ScheduleControl();
            this.selectServiceControl = new Queue.UI.WinForms.SelectServiceControl();
            this.mainTableLayoutPanel.SuspendLayout();
            this.currentSchedulePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTableLayoutPanel
            // 
            this.mainTableLayoutPanel.ColumnCount = 1;
            this.mainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.Controls.Add(this.selectServiceControl, 0, 0);
            this.mainTableLayoutPanel.Controls.Add(this.currentSchedulePanel, 0, 1);
            this.mainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTableLayoutPanel.Location = new System.Drawing.Point(5, 5);
            this.mainTableLayoutPanel.Name = "mainTableLayoutPanel";
            this.mainTableLayoutPanel.RowCount = 2;
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 340F));
            this.mainTableLayoutPanel.Size = new System.Drawing.Size(794, 556);
            this.mainTableLayoutPanel.TabIndex = 0;
            // 
            // currentSchedulePanel
            // 
            this.currentSchedulePanel.Controls.Add(this.currentScheduleCheckBox);
            this.currentSchedulePanel.Controls.Add(this.currentScheduleControl);
            this.currentSchedulePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.currentSchedulePanel.Enabled = false;
            this.currentSchedulePanel.Location = new System.Drawing.Point(0, 216);
            this.currentSchedulePanel.Margin = new System.Windows.Forms.Padding(0);
            this.currentSchedulePanel.Name = "currentSchedulePanel";
            this.currentSchedulePanel.Size = new System.Drawing.Size(794, 340);
            this.currentSchedulePanel.TabIndex = 0;
            // 
            // currentScheduleCheckBox
            // 
            this.currentScheduleCheckBox.AutoSize = true;
            this.currentScheduleCheckBox.Location = new System.Drawing.Point(15, 5);
            this.currentScheduleCheckBox.Name = "currentScheduleCheckBox";
            this.currentScheduleCheckBox.Size = new System.Drawing.Size(150, 17);
            this.currentScheduleCheckBox.TabIndex = 0;
            this.currentScheduleCheckBox.Text = "Определить расписание";
            this.currentScheduleCheckBox.UseVisualStyleBackColor = true;
            this.currentScheduleCheckBox.CheckedChanged += new System.EventHandler(this.currentScheduleCheckBox_CheckedChanged);
            this.currentScheduleCheckBox.Click += new System.EventHandler(this.currentScheduleCheckBox_Click);
            // 
            // currentScheduleControl
            // 
            this.currentScheduleControl.Enabled = false;
            this.currentScheduleControl.Location = new System.Drawing.Point(5, 30);
            this.currentScheduleControl.Name = "currentScheduleControl";
            this.currentScheduleControl.Schedule = null;
            this.currentScheduleControl.Size = new System.Drawing.Size(790, 310);
            this.currentScheduleControl.TabIndex = 0;
            // 
            // selectServiceControl
            // 
            this.selectServiceControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectServiceControl.Location = new System.Drawing.Point(3, 3);
            this.selectServiceControl.Name = "selectServiceControl";
            this.selectServiceControl.Size = new System.Drawing.Size(788, 210);
            this.selectServiceControl.TabIndex = 1;
            this.selectServiceControl.Selected += new System.EventHandler<System.EventArgs>(this.selectServiceControl_ServiceSelected);
            // 
            // CurrentScheduleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 566);
            this.Controls.Add(this.mainTableLayoutPanel);
            this.MinimumSize = new System.Drawing.Size(820, 600);
            this.Name = "CurrentScheduleForm";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "Текущее расписание";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServicesForm_FormClosing);
            this.mainTableLayoutPanel.ResumeLayout(false);
            this.currentSchedulePanel.ResumeLayout(false);
            this.currentSchedulePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainTableLayoutPanel;
        private Administrator.ScheduleControl currentScheduleControl;
        private System.Windows.Forms.CheckBox currentScheduleCheckBox;
        private System.Windows.Forms.Panel currentSchedulePanel;
        private UI.WinForms.SelectServiceControl selectServiceControl;
    }
}