namespace Queue.Administrator
{
    partial class UsersForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.administratorsTabPage = new System.Windows.Forms.TabPage();
            this.operatorsTabPage = new System.Windows.Forms.TabPage();
            this.usersTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.usersGridView = new System.Windows.Forms.DataGridView();
            this.addUserButton = new System.Windows.Forms.Button();
            this.usersTabs = new System.Windows.Forms.TabControl();
            this.isActiveColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.roleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.surnameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.patronymicColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.emailColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mobileColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.workplaceColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.isInterruptionColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.InterruptionStartTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InterruptionFinishTimeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.operatorsTabPage.SuspendLayout();
            this.usersTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.usersGridView)).BeginInit();
            this.usersTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // administratorsTabPage
            // 
            this.administratorsTabPage.Location = new System.Drawing.Point(4, 26);
            this.administratorsTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.administratorsTabPage.Name = "administratorsTabPage";
            this.administratorsTabPage.Size = new System.Drawing.Size(886, 412);
            this.administratorsTabPage.TabIndex = 0;
            this.administratorsTabPage.Tag = "";
            this.administratorsTabPage.Text = "Администраторы";
            this.administratorsTabPage.UseVisualStyleBackColor = true;
            // 
            // operatorsTabPage
            // 
            this.operatorsTabPage.Controls.Add(this.usersTableLayoutPanel);
            this.operatorsTabPage.Location = new System.Drawing.Point(4, 26);
            this.operatorsTabPage.Margin = new System.Windows.Forms.Padding(0);
            this.operatorsTabPage.Name = "operatorsTabPage";
            this.operatorsTabPage.Size = new System.Drawing.Size(886, 412);
            this.operatorsTabPage.TabIndex = 0;
            this.operatorsTabPage.Tag = "";
            this.operatorsTabPage.Text = "Операторы";
            this.operatorsTabPage.UseVisualStyleBackColor = true;
            // 
            // usersTableLayoutPanel
            // 
            this.usersTableLayoutPanel.ColumnCount = 1;
            this.usersTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.usersTableLayoutPanel.Controls.Add(this.usersGridView, 0, 0);
            this.usersTableLayoutPanel.Controls.Add(this.addUserButton, 0, 1);
            this.usersTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usersTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.usersTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.usersTableLayoutPanel.Name = "usersTableLayoutPanel";
            this.usersTableLayoutPanel.RowCount = 2;
            this.usersTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.usersTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.usersTableLayoutPanel.Size = new System.Drawing.Size(886, 412);
            this.usersTableLayoutPanel.TabIndex = 0;
            this.usersTableLayoutPanel.Tag = "";
            // 
            // usersGridView
            // 
            this.usersGridView.AllowUserToAddRows = false;
            this.usersGridView.AllowUserToResizeColumns = false;
            this.usersGridView.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(2);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.usersGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.usersGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.usersGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.isActiveColumn,
            this.roleColumn,
            this.surnameColumn,
            this.nameColumn,
            this.patronymicColumn,
            this.emailColumn,
            this.mobileColumn,
            this.workplaceColumn,
            this.isInterruptionColumn,
            this.InterruptionStartTimeColumn,
            this.InterruptionFinishTimeColumn});
            this.usersGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usersGridView.Location = new System.Drawing.Point(0, 0);
            this.usersGridView.Margin = new System.Windows.Forms.Padding(0);
            this.usersGridView.MultiSelect = false;
            this.usersGridView.Name = "usersGridView";
            this.usersGridView.ReadOnly = true;
            this.usersGridView.RowHeadersVisible = false;
            this.usersGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.usersGridView.Size = new System.Drawing.Size(886, 377);
            this.usersGridView.TabIndex = 0;
            this.usersGridView.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.usersGridView_CellMouseDoubleClick);
            this.usersGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.usersGridView_UserDeletingRow);
            // 
            // addUserButton
            // 
            this.addUserButton.Location = new System.Drawing.Point(0, 382);
            this.addUserButton.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.addUserButton.Name = "addUserButton";
            this.addUserButton.Size = new System.Drawing.Size(75, 25);
            this.addUserButton.TabIndex = 1;
            this.addUserButton.Text = "Добавить";
            this.addUserButton.Click += new System.EventHandler(this.addUserButton_Click);
            // 
            // usersTabs
            // 
            this.usersTabs.Controls.Add(this.operatorsTabPage);
            this.usersTabs.Controls.Add(this.administratorsTabPage);
            this.usersTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.usersTabs.Location = new System.Drawing.Point(10, 10);
            this.usersTabs.Margin = new System.Windows.Forms.Padding(5);
            this.usersTabs.Name = "usersTabs";
            this.usersTabs.Padding = new System.Drawing.Point(5, 5);
            this.usersTabs.SelectedIndex = 0;
            this.usersTabs.Size = new System.Drawing.Size(894, 442);
            this.usersTabs.TabIndex = 0;
            this.usersTabs.SelectedIndexChanged += new System.EventHandler(this.usersTabs_SelectedIndexChanged);
            // 
            // isActiveColumn
            // 
            this.isActiveColumn.HeaderText = "";
            this.isActiveColumn.Name = "isActiveColumn";
            this.isActiveColumn.ReadOnly = true;
            this.isActiveColumn.Width = 30;
            // 
            // roleColumn
            // 
            this.roleColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.roleColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.roleColumn.FillWeight = 140F;
            this.roleColumn.HeaderText = "Роль";
            this.roleColumn.Name = "roleColumn";
            this.roleColumn.ReadOnly = true;
            this.roleColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.roleColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.roleColumn.Visible = false;
            this.roleColumn.Width = 140;
            // 
            // surnameColumn
            // 
            this.surnameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.surnameColumn.FillWeight = 130F;
            this.surnameColumn.HeaderText = "Фамилия";
            this.surnameColumn.MinimumWidth = 100;
            this.surnameColumn.Name = "surnameColumn";
            this.surnameColumn.ReadOnly = true;
            // 
            // nameColumn
            // 
            this.nameColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.nameColumn.HeaderText = "Имя";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // patronymicColumn
            // 
            this.patronymicColumn.HeaderText = "Отчество";
            this.patronymicColumn.Name = "patronymicColumn";
            this.patronymicColumn.ReadOnly = true;
            // 
            // emailColumn
            // 
            this.emailColumn.FillWeight = 150F;
            this.emailColumn.HeaderText = "Электронный адрес";
            this.emailColumn.Name = "emailColumn";
            this.emailColumn.ReadOnly = true;
            this.emailColumn.Width = 150;
            // 
            // mobileColumn
            // 
            this.mobileColumn.FillWeight = 120F;
            this.mobileColumn.HeaderText = "Мобильный телефон";
            this.mobileColumn.Name = "mobileColumn";
            this.mobileColumn.ReadOnly = true;
            this.mobileColumn.Width = 150;
            // 
            // workplaceColumn
            // 
            this.workplaceColumn.HeaderText = "Рабочее место";
            this.workplaceColumn.Name = "workplaceColumn";
            this.workplaceColumn.ReadOnly = true;
            this.workplaceColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.workplaceColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // isInterruptionColumn
            // 
            this.isInterruptionColumn.HeaderText = "Перерыв";
            this.isInterruptionColumn.Name = "isInterruptionColumn";
            this.isInterruptionColumn.ReadOnly = true;
            this.isInterruptionColumn.Width = 80;
            // 
            // InterruptionStartTimeColumn
            // 
            this.InterruptionStartTimeColumn.HeaderText = "Начало перерыва";
            this.InterruptionStartTimeColumn.Name = "InterruptionStartTimeColumn";
            this.InterruptionStartTimeColumn.ReadOnly = true;
            this.InterruptionStartTimeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.InterruptionStartTimeColumn.Width = 150;
            // 
            // InterruptionFinishTimeColumn
            // 
            this.InterruptionFinishTimeColumn.HeaderText = "Окончание перерыва";
            this.InterruptionFinishTimeColumn.Name = "InterruptionFinishTimeColumn";
            this.InterruptionFinishTimeColumn.ReadOnly = true;
            this.InterruptionFinishTimeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.InterruptionFinishTimeColumn.Width = 150;
            // 
            // UsersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 462);
            this.Controls.Add(this.usersTabs);
            this.MinimumSize = new System.Drawing.Size(930, 500);
            this.Name = "UsersForm";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.Text = "Пользователи";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UsersForm_FormClosing);
            this.Load += new System.EventHandler(this.UsersForm_Load);
            this.operatorsTabPage.ResumeLayout(false);
            this.usersTableLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.usersGridView)).EndInit();
            this.usersTabs.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage administratorsTabPage;
        private System.Windows.Forms.TabPage operatorsTabPage;
        private System.Windows.Forms.TableLayoutPanel usersTableLayoutPanel;
        private System.Windows.Forms.DataGridView usersGridView;
        private System.Windows.Forms.Button addUserButton;
        private System.Windows.Forms.TabControl usersTabs;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isActiveColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn roleColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn surnameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn patronymicColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn emailColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn mobileColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn workplaceColumn;
        private System.Windows.Forms.DataGridViewCheckBoxColumn isInterruptionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn InterruptionStartTimeColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn InterruptionFinishTimeColumn;

    }
}