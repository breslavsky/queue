using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public interface IMainWindow
    {
        void Invoke(Action action);

        LoadingControl ShowLoading();

        T ShowMessageBox<T>(Func<T> ctor) where T : UserControl;

        NoticeControl Notice(object message, Action callback = null);

        WarningControl Warning(object message, Action callback = null);

        void HideMessageBox(UserControl control);

        void HideActiveMessageBox();

        void HideLoading();

        void AttachControl(UserControl control);

        void DetachControl(UserControl control);

        void MakeFullScreen();

        void Navigate(Page page);

        Task<T> ExecuteLongTask<T>(Func<Task<T>> task);

        Task ExecuteLongTask(Func<Task> task);
    }
}