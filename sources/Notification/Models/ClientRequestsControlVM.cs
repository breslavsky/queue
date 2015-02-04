using Junte.UI.WPF;
using NLog;
using Queue.Model.Common;
using Queue.Notification.UserControls;
using Queue.Services.DTO;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Queue.Notification.Models
{
    public class ClientRequestsControlVM : ObservableObject, IDisposable
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const int DefaultClientRequestsLength = 6;

        private bool disposed = false;

        private TimeSpan ClientRequestTimeout;

        private object updateLock;

        private Grid clientRequestsGrid;
        private List<UIElement> controls;
        private List<ClientRequestWrap> requests;
        private DispatcherTimer timer;

        public int ClientRequestsLength { get; set; }

        public ClientRequestsControlVM()
        {
            ClientRequestTimeout = TimeSpan.FromMinutes(20);
            ClientRequestsLength = DefaultClientRequestsLength;

            updateLock = new object();

            controls = new List<UIElement>();
            requests = new List<ClientRequestWrap>();

            timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        public void SetClientRequestsGrid(Grid clientRequestsGrid)
        {
            this.clientRequestsGrid = clientRequestsGrid;
        }

        public void AddToClientRequests(ClientRequest request)
        {
            lock (updateLock)
            {
                ClientRequestWrap wrap = requests.SingleOrDefault(r => r.Request.Equals(request));

                bool isImportantState = (request.State == ClientRequestState.Calling)
                    || (request.State == ClientRequestState.Absence);

                if (!isImportantState && (wrap == null))
                {
                    return;
                }

                if (isImportantState)
                {
                    if (wrap != null)
                    {
                        requests.Remove(wrap);
                    }

                    requests.Insert(0, new ClientRequestWrap()
                    {
                        Request = request,
                        Added = DateTime.Now
                    });
                }
                else
                {
                    wrap.Request = request;
                    wrap.Added = DateTime.Now;
                }

                if (requests.Count > ClientRequestsLength)
                {
                    requests.RemoveRange(ClientRequestsLength, requests.Count - ClientRequestsLength);
                }

                clientRequestsGrid.Dispatcher.Invoke(UpdateCallingClientRequests);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lock (updateLock)
            {
                DateTime now = DateTime.Now;
                if (requests.RemoveAll(r => r.Request.IsClosed && (now - r.Added) > ClientRequestTimeout) > 0)
                {
                    UpdateCallingClientRequests();
                }
            }
        }

        private void UpdateCallingClientRequests()
        {
            ClearState();

            int row = 1;

            foreach (ClientRequestWrap req in requests)
            {
                try
                {
                    controls.Add(CreateTextBox(req.Request.Number.ToString(), 0, row));

                    ClientRequestStateUserControl ctrl = new ClientRequestStateUserControl(req.Request);
                    ctrl.SetValue(Grid.ColumnProperty, 2);
                    ctrl.SetValue(Grid.RowProperty, row);
                    controls.Add(ctrl);

                    row++;
                }
                catch (Exception e)
                {
                    logger.Error(e);
                }
            }

            for (int i = 0; i < row; i++)
            {
                clientRequestsGrid.RowDefinitions.Add(new RowDefinition());
            }

            foreach (UIElement c in controls)
            {
                clientRequestsGrid.Children.Add(c);
            }
        }

        private void ClearState()
        {
            foreach (UIElement c in controls)
            {
                clientRequestsGrid.Children.Remove(c);
            }
            controls.Clear();

            clientRequestsGrid.RowDefinitions.Clear();
        }

        private TextBlock CreateTextBox(string text, int col, int row, string color = null)
        {
            TextBlock result = new TextBlock()
            {
                Text = text
            };

            if (color != null)
            {
                result.Foreground = color.GetBrushForColor();
            }

            result.SetValue(Grid.ColumnProperty, col);
            result.SetValue(Grid.RowProperty, row);
            return result;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                timer.Stop();
            }

            disposed = true;
        }

        ~ClientRequestsControlVM()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}