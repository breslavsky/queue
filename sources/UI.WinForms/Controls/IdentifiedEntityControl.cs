using Queue.Services.DTO;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class IdentifiedEntityControl : UserControl
    {
        public bool UserCanReset
        {
            get
            {
                return resetButton.Visible;
            }
            set
            {
                resetButton.Visible = value;
            }
        }

        public IdentifiedEntity[] Entities
        {
            set
            {
                comboBox.Items.Clear();
                comboBox.Items.AddRange(value);
            }
        }

        public IdentifiedEntity Selected
        {
            get
            {
                return comboBox.SelectedItem as IdentifiedEntity;
            }
            set
            {
                comboBox.SelectedItem = value;
            }
        }

        public IdentifiedEntityControl()
        {
            InitializeComponent();
        }

        private void resetButton_VisibleChanged(object sender, System.EventArgs e)
        {
            //comboBox.Width += resetButton.Visible ? -resetButton.Width : resetButton.Width;
        }

        private void resetButton_Click(object sender, System.EventArgs e)
        {
            Selected = null;
        }
    }
}