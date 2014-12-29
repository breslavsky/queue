using Junte.Data.Common;
using Queue.Common;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Queue.UI.WinForms.Controls
{
    public partial class EnumFlagsControl : UserControl
    {
        public EnumFlagsControl()
        {
            InitializeComponent();
        }

        public void Initialize<T>() where T : struct
        {
            listBox.Items.Clear();
            var items = EnumItem<T>.GetItems();
            items.Where(i => (long)Enum.ToObject(typeof(T), i.Value) != 0);
            listBox.Items.AddRange(items);
            listBox.Enabled = items.Length > 0;
        }

        public void Select<T>(T value) where T : struct
        {
            long flags = (long)Enum.ToObject(typeof(T), value);

            var items = listBox.Items;
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i] as EnumItem<T>;
                var check = (long)Enum.ToObject(typeof(T), item.Value);
                if ((flags & check) != 0)
                {
                    listBox.SetItemChecked(i, true);
                }
            }
        }

        public T Selected<T>() where T : struct
        {
            long value = 0;

            foreach (EnumItem<T> item in listBox.CheckedItems)
            {
                value |= (long)Enum.ToObject(typeof(T), item.Value);
            }

            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}