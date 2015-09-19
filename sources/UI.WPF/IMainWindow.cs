using System;
using System.Windows.Controls;

namespace Queue.UI.WPF.Types
{
    public interface IMainWindow
    {
        void Invoke(Action action);

        LoadingControl ShowLoading();

        T ShowMessageBox<T>(Func<T> ctor) where T : UserControl;

        NoticeControl Notice(object message, Action callback = null);

        WarningControl Warning(object message, Action callback = null);

        void HideMessageBox(UserControl control);

        void HideLoading();

        void AttachControl(UserControl control);

        void DetachControl(UserControl control);
    }
}