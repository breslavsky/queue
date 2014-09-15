using System;
using System.Windows.Controls;

namespace Queue.UI.Silverlight
{
    public class RichPage : Page
    {
        protected LoadingControl loading;
        protected NoticeControl notice;
        protected WarningControl warning;

        public RichPage()
            : base()
        {
            Loaded += (s, e) =>
            {
                loading = new LoadingControl();
                (((Grid)((Border)Content).Child)).Children.Add(loading);
                loading.Hide();

                notice = new NoticeControl();
                (((Grid)((Border)Content).Child)).Children.Add(notice);
                notice.Hide();

                warning = new WarningControl();
                (((Grid)((Border)Content).Child)).Children.Add(warning);
                warning.Hide();
            };
        }

        protected void showLoading()
        {
            loading.Show();
        }

        protected void showLoading(object message)
        {
            loading.Show(message.ToString());
        }

        protected void hideLoading()
        {
            loading.Hide();
        }

        protected void showNotice(object message)
        {
            notice.Show(message.ToString());
        }

        protected void showWarning(object message)
        {
            warning.Show(message.ToString());
        }        
    }
}
