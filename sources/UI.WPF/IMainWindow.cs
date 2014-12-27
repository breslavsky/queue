using System;

namespace Queue.UI.WPF.Types
{
    public interface IMainWindow
    {
        LoadingControl ShowLoading();

        NoticeControl ShowNotice(object message, Action callback = null);

        WarningControl ShowWarning(object message, Action callback = null);
    }
}