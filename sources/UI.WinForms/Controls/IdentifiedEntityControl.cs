using Junte.UI.WinForms;
using Queue.Services.DTO;
using System;

namespace Queue.UI.WinForms
{
    public partial class IdentifiedEntityControl : RichUserControl
    {
        public bool UseResetButton
        {
            get { return resetButton.Visible; }
            set { resetButton.Visible = value; }
        }

        public event EventHandler<EventArgs> SelectedChanged = delegate { };

        private bool frozen = true;

        public IdentifiedEntityControl()
        {
            InitializeComponent();
        }

        private void IdentifiedEntityControl_Load(object sender, EventArgs e)
        {
            if (!UseResetButton)
            {
                comboBox.Width += resetButton.Width;
            }
        }

        public void Initialize(IdentifiedEntityLink[] entities)
        {
            frozen = true;

            comboBox.Items.Clear();
            comboBox.Items.AddRange(entities);
            comboBox.Enabled = entities.Length > 0;

            frozen = false;
        }

        public T Selected<T>() where T : IdentifiedEntity
        {
            return comboBox.SelectedItem != null
                ? (comboBox.SelectedItem as IdentifiedEntityLink).Cast<T>() : null;
        }

        public void Select<T>(T value) where T : IdentifiedEntity
        {
            frozen = true;

            comboBox.SelectedItem = value;

            frozen = false;
        }

        public void Empty()
        {
            comboBox.SelectedItem = null;
        }

        public void Clear()
        {
            comboBox.Items.Clear();
            comboBox.Enabled = false;
        }

        private void resetButton_Click(object sender, System.EventArgs e)
        {
            Empty();
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (frozen)
            {
                return;
            }

            SelectedChanged(this, null);
        }
    }
}