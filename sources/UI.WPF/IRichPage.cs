using System;

namespace Queue.UI.WPF.Types
{
    //TODO: why?
    public interface IRichPage
    {
        LoadingControl ShowLoading();

        NoticeControl ShowNotice(object message, Action callback = null);

        WarningControl ShowWarning(object message, Action callback = null);
    }
}