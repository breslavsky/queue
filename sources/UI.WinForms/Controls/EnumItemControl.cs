using Junte.Data.Common;
using Queue.Common;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class EnumItemControl : UserControl
    {
        public EnumItemControl()
        {
            InitializeComponent();
        }

        public event EventHandler<EventArgs> SelectedChanged;

        private bool frozen = true;

        public void Initialize<T>() where T : struct
        {
            frozen = true;

            comboBox.Items.Clear();
            var items = EnumItem<T>.GetItems();
            comboBox.Items.AddRange(items);
            Enabled = items.Length > 0;
            comboBox.SelectedItem = items.FirstOrDefault();

            frozen = false;
        }

        public void Select<T>(T value) where T : struct
        {
            frozen = true;

            comboBox.SelectedItem = new EnumItem<T>(value);

            frozen = false;
        }

        public T Selected<T>() where T : struct
        {
            return (comboBox.SelectedItem as EnumItem<T>).Value;
        }

        public void Clear()
        {
            comboBox.Items.Clear();
            Enabled = false;
        }

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedChanged != null && !frozen)
            {
                SelectedChanged(this, new EventArgs());
            }
        }
    }
}