using Junte.Translation;
using System;

namespace Queue.UI.WPF.Types
{
    public class ScreenNumberItem
    {
        public byte Number { get; private set; }

        public ScreenNumberItem(byte number)
        {
            Number = number;
        }

        public override string ToString()
        {
            return String.Format(Translater.Message("ScreenNumberItemText"), Number + 1);
        }
    }
}