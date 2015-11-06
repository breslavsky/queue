using MahApps.Metro.Controls;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public abstract class RichWindow : MetroWindow
    {
        protected abstract Panel RootElement { get; }

        private LoadingControl loading;
        private UserControl activeMessageBox;

        public RichWindow()
            : base()
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(GetType(), this);
        }

        #region IMainWindow

        public LoadingControl ShowLoading()
        {
            if (loading != null)
            {
                return loading;
            }

            Invoke(() =>
            {
                var control = new LoadingControl();
                AttachControl(control);
                loading = control;
            });

            return loading;
        }

        public void HideLoading()
        {
            if (loading == null)
            {
                return;
            }

            DetachControl(loading);

            loading = null;
        }

        private void HideActiveMessageBox()
        {
            if (activeMessageBox != null)
            {
                HideMessageBox(activeMessageBox);
            }
        }

        public NoticeControl Notice(object message, Action callback = null)
        {
            return ShowMessageBox(() => new NoticeControl(message.ToString(), callback));
        }

        public WarningControl Warning(object message, Action callback = null)
        {
            return ShowMessageBox(() => new WarningControl(message.ToString(), callback));
        }

        public T ShowMessageBox<T>(Func<T> ctor) where T : UserControl
        {
            HideActiveMessageBox();

            T box = null;
            Invoke(() =>
            {
                box = ctor();
                AttachControl(box);
            });

            activeMessageBox = box;

            return box;
        }

        public void HideMessageBox(UserControl control)
        {
            DetachControl(control);
            activeMessageBox = null;
        }

        public void AttachControl(UserControl control)
        {
            Invoke(() => RootElement.Children.Add(control));
        }

        public void DetachControl(UserControl control)
        {
            Invoke(() => RootElement.Children.Remove(control));
        }

        public void Invoke(Action action)
        {
            if (Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Dispatcher.Invoke(action);
            }
        }

        #endregion IMainWindow

        public void MakeFullScreen()
        {
            WindowState = WindowState.Maximized;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;

            ShowCloseButton = false;
            ShowMaxRestoreButton = false;
            ShowMinButton = false;
            ShowTitleBar = false;

            UseNoneWindowStyle = true;
            TitlebarHeight = 0;
        }
    }
}