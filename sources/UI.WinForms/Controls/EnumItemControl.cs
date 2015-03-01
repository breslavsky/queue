using Queue.Common;
using System;
using System.Linq;

namespace Queue.UI.WinForms
{
    public partial class EnumItemControl : RichUserControl
    {
        public event EventHandler<EventArgs> SelectedChanged;

        private bool frozen = true;

        public EnumItemControl()
        {
            InitializeComponent();
        }

        public void Initialize<T>() where T : struct
        {
            frozen = true;
            try
            {
                comboBox.Items.Clear();
                var items = EnumItem<T>.GetItems();
                comboBox.Items.AddRange(items);
                comboBox.Enabled = items.Length > 0;
                comboBox.SelectedItem = items.FirstOrDefault();
            }
            finally
            {
                frozen = false;
            }
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
            comboBox.Enabled = false;
        }

        public void Empty()
        {
            comboBox.SelectedItem = null;
        }

        public override void Translate()
        {
            frozen = true;
            try
            {
                object selected = comboBox.SelectedItem;
                object[] items = new object[comboBox.Items.Count];
                comboBox.Items.CopyTo(items, 0);

                comboBox.Items.Clear();

                comboBox.Items.AddRange(items);
                comboBox.SelectedItem = selected;
            }
            finally
            {
                frozen = false;
            }
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