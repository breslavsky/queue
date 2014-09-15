using Queue.Services.DTO;
using Queue.Operator.Silverlight.QueueRemoteService;
using Queue.UI.Silverlight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Operator.Silverlight
{
    public delegate void ClientRequestsControlEventHandler(object sender, RoutedEventArgs e);

    public partial class ClientRequestsControl : RichControl
    {
        private DuplexChannelBuilder<IServerService> channelBuilder;
        private QueueOperator currentOperator;

        public event ClientRequestsControlEventHandler OnClose
        {
            add { closeHandler += value; }
            remove { closeHandler -= value; }
        }
        event ClientRequestsControlEventHandler closeHandler;

        public ClientRequestsControl(DuplexChannelBuilder<IServerService> channelBuilder, QueueOperator currentOperator)
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentOperator = currentOperator;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Channel<IServerService> channel = channelBuilder.CreateChannel();

            AsyncCallback onEndGetOperatorClientRequestPlans = (r) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        clientRequestPlansDataGrid.ItemsSource = channel.Service.EndGetOperatorClientRequestPlans(r);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                });
            };

            AsyncCallback onEndOpenUserSession = (r) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        channel.Service.EndOpenUserSession(r);
                        channel.Service.BeginGetOperatorClientRequestPlans(onEndGetOperatorClientRequestPlans, null);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                });
            };

            try
            {
                channel.Service.BeginOpenUserSession(currentOperator.SessionId, onEndOpenUserSession, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            if (closeHandler != null)
            {
                closeHandler(this, new RoutedEventArgs());
            }
        }
    }
}
