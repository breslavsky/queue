using System.Windows.Media;

namespace Queue.UI.WPF
{
    public class AccentColorComboBoxItem
    {
        public string Name { get; private set; }

        public Brush ColorBrush { get; private set; }

        public AccentColorComboBoxItem(string name, Brush colorBrush)
        {
            Name = name;
            ColorBrush = colorBrush;
        }
    }
}