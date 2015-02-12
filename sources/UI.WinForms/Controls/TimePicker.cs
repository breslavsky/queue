using Junte.UI.WinForms;
using System;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class TimePicker : RichUserControl
    {
        private TimeSpan value;

        public TimeSpan Value
        {
            get
            {
                return value;
            }
            set
            {
                valueTextBox.Text = value.ToString("hh\\:mm");
            }
        }

        public TimePicker()
        {
            InitializeComponent();
        }

        private void TimePicker_Leave(object sender, EventArgs e)
        {
            try
            {
                value = TimeSpan.Parse(valueTextBox.Text);
            }
            catch
            {
                UIHelper.Warning("Ошибочный формат времени");
            }
        }
    }
}