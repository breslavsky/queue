﻿using MahApps.Metro.Controls;
using Queue.UI.WPF.Types;
using System;
using System.Windows.Controls;

namespace Queue.UI.WPF
{
    public abstract class RichWindow : MetroWindow, IMainWindow
    {
        protected abstract Panel RootElement { get; }

        private LoadingControl loading;
        private UserControl activeMessageBox;

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
    }
}