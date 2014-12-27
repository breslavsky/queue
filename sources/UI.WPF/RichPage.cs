using Queue.UI.WPF.Types;
using System;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public class RichPage : Page, IMainWindow
    {
        private LoadingControl loading;
        private NoticeControl notice;
        private WarningControl warning;

        public LoadingControl ShowLoading()
        {
            if (loading == null)
            {
                loading = new LoadingControl();
                (((Grid)((Border)Content).Child)).Children.Add(loading);

                loading.Hide();
            }

            return loading.Show();
        }

        public NoticeControl ShowNotice(object message, Action callback = null)
        {
            if (notice == null)
            {
                notice = new NoticeControl();
                (((Grid)((Border)Content).Child)).Children.Add(notice);
                notice.Hide();
            }

            return notice.Show(message.ToString(), callback);
        }

        public WarningControl ShowWarning(object message, Action callback = null)
        {
            if (warning == null)
            {
                warning = new WarningControl();
                (((Grid)((Border)Content).Child)).Children.Add(warning);
                warning.Hide();
            }

            return warning.Show(message.ToString(), callback);
        }
    }
}