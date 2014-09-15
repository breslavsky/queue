using System;

namespace Queue.UI.WPF.Types
{
    public interface IRichPage
    {
        LoadingControl ShowLoading();

        NoticeControl ShowNotice(object message, Action callback = null);

        WarningControl ShowWarning(object message, Action callback = null);
    }
}