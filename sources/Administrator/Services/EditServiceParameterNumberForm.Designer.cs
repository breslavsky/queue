﻿namespace Queue.Administrator
{
    partial class EditServiceParameterNumberForm
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
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.toolTipLabel = new System.Windows.Forms.Label();
            this.toolTipTextBox = new System.Windows.Forms.TextBox();
            this.isRequireCheckBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.Location = new System.Drawing.Point(5, 0);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(195, 18);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "Название";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Location = new System.Drawing.Point(10, 25);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(190, 20);
            this.nameTextBox.TabIndex = 0;
            this.nameTextBox.Leave += new System.EventHandler(this.nameTextBox_Leave);
            // 
            // toolTipLabel
            // 
            this.toolTipLabel.Location = new System.Drawing.Point(5, 50);
            this.toolTipLabel.Name = "toolTipLabel";
            this.toolTipLabel.Size = new System.Drawing.Size(195, 18);
            this.toolTipLabel.TabIndex = 0;
            this.toolTipLabel.Text = "Подсказка";
            this.toolTipLabel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // toolTipTextBox
            // 
            this.toolTipTextBox.Location = new System.Drawing.Point(10, 75);
            this.toolTipTextBox.Multiline = true;
            this.toolTipTextBox.Name = "toolTipTextBox";
            this.toolTipTextBox.Size = new System.Drawing.Size(190, 50);
            this.toolTipTextBox.TabIndex = 1;
            this.toolTipTextBox.Leave += new System.EventHandler(this.toolTipTextBox_Leave);
            // 
            // isRequireCheckBox
            // 
            this.isRequireCheckBox.AutoSize = true;
            this.isRequireCheckBox.Location = new System.Drawing.Point(10, 135);
            this.isRequireCheckBox.Name = "isRequireCheckBox";
            this.isRequireCheckBox.Size = new System.Drawing.Size(183, 17);
            this.isRequireCheckBox.TabIndex = 2;
            this.isRequireCheckBox.Text = "Обязательное для заполнения";
            this.isRequireCheckBox.UseVisualStyleBackColor = true;
            this.isRequireCheckBox.Leave += new System.EventHandler(this.isRequireCheckBox_Leave);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(125, 160);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 25);
            this.saveButton.TabIndex = 3;
            this.saveButton.Text = "Записать";
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // EditServiceParameterNumberForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(209, 196);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(this.toolTipLabel);
            this.Controls.Add(this.toolTipTextBox);
            this.Controls.Add(this.isRequireCheckBox);
            this.Controls.Add(this.saveButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "EditServiceParameterNumberForm";
            this.Text = "Редактирование параметра";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditServiceParameterNumberForm_FormClosing);
            this.Load += new System.EventHandler(this.EditServiceParameterNumberForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.Label toolTipLabel;
        private System.Windows.Forms.TextBox toolTipTextBox;
        private System.Windows.Forms.CheckBox isRequireCheckBox;
        private System.Windows.Forms.Button saveButton;
    }
}