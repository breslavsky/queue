using System.Windows;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public delegate void VirtualKeyboardHandler(object sender, VirtualKeyboardEvent e);

    public partial class VirtualKeyboard : UserControl
    {
        public static DependencyProperty OnTypingProperty = DependencyProperty.Register("OnTyping", typeof(string), typeof(VirtualKeyboard));
        public static DependencyProperty OnBackspaceProperty = DependencyProperty.Register("OnBackspace", typeof(string), typeof(VirtualKeyboard));

        public event VirtualKeyboardHandler OnTyping;

        public event VirtualKeyboardHandler OnBackspace;

        public VirtualKeyboard()
        {
            InitializeComponent();

            foreach (var b in mainStackGrid.FindChildren<Button>())
            {
                b.Focusable = false;

                if (b.Name != "backspaceButton")
                {
                    b.Click += (s, e) =>
                    {
                        if (OnTyping != null)
                        {
                            VirtualKeyboardEvent virtualKeyboardEvent = new VirtualKeyboardEvent();
                            virtualKeyboardEvent.Letter = ((Button)s).Content.ToString();
                            OnTyping(this, virtualKeyboardEvent);
                        }
                    };
                }
            }
        }

        private void backspaceButton_Click(object sender, RoutedEventArgs e)
        {
            OnBackspace(this, new VirtualKeyboardEvent());
        }
    }

    public class VirtualKeyboardEvent
    {
        public string Letter;
    }
}