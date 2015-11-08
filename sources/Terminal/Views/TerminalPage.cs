using Queue.UI.WPF;
using System;

namespace Queue.Terminal.Views
{
    public abstract class TerminalPage : RichPage
    {
        protected abstract Type ModelType { get; }

        public TerminalPage()
            : base()
        {
            DataContext = Activator.CreateInstance(ModelType);
        }
    }
}