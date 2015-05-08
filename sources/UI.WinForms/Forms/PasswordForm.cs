using Junte.UI.WinForms;
using System;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class PasswordForm : RichForm
    {
        public string Password
        {
            get { return passwordTextBox.Text; }
        }

        public PasswordForm()
            : base()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (passwordTextBox.Text != confirmTextBox.Text)
            {
                UIHelper.Warning("Пароль и его подтверждение не совпадают");
            }
            else
            {
                DialogResult = DialogResult.OK;
            }
        }
    }
}