using Junte.UI.WinForms;
using System;
using System.Linq;

namespace Queue.UI.WinForms
{
    public partial class EnumFlagsControl : RichUserControl
    {
        public EnumFlagsControl()
        {
            InitializeComponent();
        }

        public void Initialize<T>() where T : struct, IConvertible
        {
            listBox.Items.Clear();

            var items = EnumItem<T>.GetItems().ToList();
            items.RemoveAll(i => (long)Enum.ToObject(typeof(T), i.Value) == 0);

            listBox.Items.AddRange(items.ToArray());
            listBox.Enabled = items.Count > 0;
        }

        public void Select<T>(T value) where T : struct, IConvertible
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

        public T Selected<T>() where T : struct, IConvertible
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