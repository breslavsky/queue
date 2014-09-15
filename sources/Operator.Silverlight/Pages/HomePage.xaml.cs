using Queue.Model.Common;
using Queue.Operator.Silverlight.QueueRemoteService;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using QueueOperator = Queue.Services.DTO.Operator;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Operator.Silverlight
{
    public delegate void HomePageEventHandler(object sender, EventArgs e);

    public partial class HomePage : Queue.UI.Silverlight.RichPage, IServerServiceCallback
    {
        private DuplexChannelBuilder<IServerService> channelBuilder;

        private QueueOperator currentOperator;

        private Settings settings;

        private Channel<IServerService> callbackChannel;

        private DateTime currentDateTime;

        private DispatcherTimer pingTimer;

        private int PING_INTERVAL = 10000;

        private ClientRequest currentClientRequest;

        public HomePage(DuplexChannelBuilder<IServerService> channelBuilder, QueueOperator currentOperator)
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentOperator = currentOperator;

            callbackChannel = channelBuilder.CreateChannel(this);

            settings = IsolatedStorage<Settings>.Get();

            step = 0;

            titleTextBlock.Text = string.Format("{0} | {1}", currentOperator, currentOperator.Workplace);

            pingTimer = new DispatcherTimer();
            pingTimer.Tick += (s, e) =>
            {
                pingTimer.Stop();

                var interval = TimeSpan.FromMilliseconds(PING_INTERVAL);
                if (pingTimer.Interval < interval)
                {
                    pingTimer.Interval = interval;
                }

                Dispatcher.BeginInvoke(() =>
                {
                    serverStateBorder.Background = new SolidColorBrush(Colors.Yellow);

                    Action ping = () =>
                    {
                        try
                        {
                            callbackChannel.Service.BeginUserHeartbeat((userHeartbeatResult) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    try
                                    {
                                        callbackChannel.Service.EndUserHeartbeat(userHeartbeatResult);

                                        callbackChannel.Service.BeginGetDateTime((getDateTimeResult) =>
                                        {
                                            Dispatcher.BeginInvoke(() =>
                                            {
                                                try
                                                {
                                                    currentDateTime = callbackChannel.Service.EndGetDateTime(getDateTimeResult);
                                                    currentDateTimeTextBlock.Text = currentDateTime.ToLongTimeString();

                                                    serverStateBorder.Background = new SolidColorBrush(Colors.Green);
                                                }
                                                catch
                                                {
                                                    // exception
                                                }
                                            });
                                        }, null);
                                    }
                                    catch
                                    {
                                        serverStateBorder.Background = new SolidColorBrush(Colors.Red);

                                        callbackChannel.Dispose();
                                        callbackChannel = channelBuilder.CreateChannel(this);
                                    }
                                    finally
                                    {
                                        pingTimer.Start();
                                    }
                                });
                            }, null);
                        }
                        catch
                        {
                            serverStateBorder.Background = new SolidColorBrush(Colors.Red);

                            callbackChannel.Dispose();
                            callbackChannel = channelBuilder.CreateChannel(this);
                        }
                    };

                    if (!callbackChannel.IsConnected)
                    {
                        try
                        {
                            callbackChannel.Service.BeginOpenUserSession(currentOperator.SessionId, (openUserSessionResult) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    try
                                    {
                                        callbackChannel.Service.EndOpenUserSession(openUserSessionResult);
                                        callbackChannel.Service.BeginSubscribe(ServerServiceEventType.CurrentClientRequestUpdated,
                                            new ServerSubscribtionArgs() { Operator = currentOperator }, (subscribeResult) =>
                                        {
                                            Dispatcher.BeginInvoke(() =>
                                            {
                                                try
                                                {
                                                    callbackChannel.Service.EndSubscribe(subscribeResult);
                                                    callbackChannel.Service.BeginGetCurrentClientRequest((getCurrentClientRequestResult) =>
                                                    {
                                                        Dispatcher.BeginInvoke(() =>
                                                        {
                                                            try
                                                            {
                                                                CurrentClientRequest = callbackChannel.Service.EndGetCurrentClientRequest(getCurrentClientRequestResult);
                                                                ping();
                                                            }
                                                            catch
                                                            {
                                                                // exception
                                                            }
                                                        });
                                                    }, null);
                                                }
                                                catch
                                                {
                                                    // exception
                                                }
                                            });
                                        }, null);
                                    }
                                    catch
                                    {
                                        // exception
                                    }
                                });
                            }, null);
                        }
                        catch
                        {
                            // exception
                        }
                    }
                    else
                    {
                        ping();
                    }
                });
            };
        }

        public event HomePageEventHandler OnLogout
        {
            add { logoutHandler += value; }
            remove { logoutHandler -= value; }
        }

        private event HomePageEventHandler logoutHandler;

        public ClientRequest CurrentClientRequest
        {
            get
            {
                return currentClientRequest;
            }
            private set
            {
                if (value != null)
                {
                    if (currentClientRequest == null || !currentClientRequest.Equals(value) || value.IsRecent(currentClientRequest))
                    {
                        currentClientRequest = value;

                        numberTextBox.Text = currentClientRequest.Number.ToString();
                        isPriorityCheckBox.IsChecked = currentClientRequest.IsPriority;
                        requestTimeTextBox.Text = currentClientRequest.RequestTime.ToString("hh\\:mm\\:ss");
                        var translation1 = Translation.ClientRequestType.ResourceManager;
                        typeTextBoxTextBox.Text = translation1.GetString(currentClientRequest.Type.ToString());

                        var client = currentClientRequest.Client;
                        clientTextBox.Text = client.ToString();

                        var service = currentClientRequest.Service;
                        serviceTextBox.Text = service.ToString();

                        serviceTypesComboBox.ItemsSource = null;

                        if (service.Type != ServiceType.None)
                        {
                            var translation = Translation.ServiceType.ResourceManager;

                            var serviceTypes = new Dictionary<ServiceType, string>()
                            {
                                {ServiceType.None, string.Empty}
                            };
                            foreach (ServiceType type in Enum.GetValues(typeof(ServiceType)))
                            {
                                if (type != ServiceType.None && service.Type.HasFlag(type))
                                {
                                    serviceTypes.Add(type, translation.GetString(type.ToString()));
                                }
                            }
                            if (serviceTypes.Count > 0)
                            {
                                serviceTypesComboBox.ItemsSource = serviceTypes;
                                serviceTypesComboBox.SelectedValue = currentClientRequest.ServiceType;
                            }
                        }

                        var translation2 = Translation.ClientRequestState.ResourceManager;
                        stateTextBox.Text = translation2.GetString(currentClientRequest.State.ToString());

                        versionTextBlock.Text = string.Format("[{0}]", currentClientRequest.Version.ToString());

                        switch (currentClientRequest.State)
                        {
                            case ClientRequestState.Waiting:
                            case ClientRequestState.Postponed:
                                step = 1;
                                break;

                            case ClientRequestState.Calling:
                                step = 2;
                                break;

                            case ClientRequestState.Rendering:
                                step = 3;
                                break;
                        }
                    }

                    switch (currentClientRequest.State)
                    {
                        case ClientRequestState.Waiting:
                        case ClientRequestState.Postponed:

                            if (currentClientRequest.RequestTime.Add(TimeSpan.FromSeconds(20)) <= currentDateTime.TimeOfDay)
                            {
                                var channel = channelBuilder.CreateChannel();

                                AsyncCallback onEndCallCurrentClient = (r) =>
                                {
                                    Dispatcher.BeginInvoke(() =>
                                    {
                                        try
                                        {
                                            channel.Service.EndCallCurrentClient(r);
                                        }
                                        catch (Exception exception)
                                        {
                                            MessageBox.Show(exception.Message);
                                        }
                                        finally
                                        {
                                            channel.Dispose();
                                        }
                                    });
                                };

                                AsyncCallback onBeginUpdateCurrentClientRequest = (r) =>
                                {
                                    Dispatcher.BeginInvoke(() =>
                                    {
                                        try
                                        {
                                            channel.Service.EndUpdateCurrentClientRequest(r);
                                            channel.Service.BeginCallCurrentClient(onEndCallCurrentClient, null);
                                        }
                                        catch
                                        {
                                            //logger
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

                                            channel.Service.BeginUpdateCurrentClientRequest(ClientRequestState.Calling, onBeginUpdateCurrentClientRequest, null);
                                        }
                                        catch (Exception exception)
                                        {
                                            MessageBox.Show(exception.Message);
                                        }
                                    });
                                };

                                Dispatcher.BeginInvoke(() =>
                                {
                                    try
                                    {
                                        channel.Service.BeginOpenUserSession(currentOperator.SessionId, onEndOpenUserSession, null);
                                    }
                                    catch (Exception exception)
                                    {
                                        MessageBox.Show(exception.Message);
                                    }
                                });
                            }
                            break;
                    }
                }
                else
                {
                    currentClientRequest = null;

                    numberTextBox.Text = string.Empty;
                    requestTimeTextBox.Text = string.Empty;
                    typeTextBoxTextBox.Text = string.Empty;
                    clientTextBox.Text = string.Empty;
                    serviceTextBox.Text = string.Empty;
                    serviceTypesComboBox.ItemsSource = null;
                    stateTextBox.Text = string.Empty;

                    versionTextBlock.Text = string.Empty;

                    step = 0;
                }
            }
        }

        private int step
        {
            set
            {
                step1Grid.Visibility = Visibility.Collapsed;
                step2Grid.Visibility = Visibility.Collapsed;
                step3Grid.Visibility = Visibility.Collapsed;

                serviceTypesComboBox.IsEnabled = false;
                serviceChangeLink.IsEnabled = false;

                switch (value)
                {
                    case 0:
                        break;

                    case 1:
                        step1Grid.Visibility = Visibility.Visible;
                        break;

                    case 2:
                        step2Grid.Visibility = Visibility.Visible;
                        break;

                    case 3:
                        step3Grid.Visibility = Visibility.Visible;
                        serviceChangeLink.IsEnabled = true;
                        if (serviceTypesComboBox.ItemsSource != null)
                        {
                            serviceTypesComboBox.IsEnabled = true;
                        }
                        break;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs eventArgs)
        {
            pingTimer.Start();
        }

        private void serviceTypesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object selectedValue = serviceTypesComboBox.SelectedValue;
            if (selectedValue != null)
            {
                var serviceType = (ServiceType)selectedValue;
                if (serviceType != ServiceType.None && serviceType != CurrentClientRequest.ServiceType)
                {
                    serviceTypesComboBox.IsEnabled = false;
                    Task.Factory.StartNew(() =>
                    {
                        Thread.Sleep(2000);
                        Dispatcher.BeginInvoke(() => serviceTypesComboBox.IsEnabled = true);
                    });

                    var channel = channelBuilder.CreateChannel();
                    try
                    {
                        showLoading();

                        channel.Service.BeginOpenUserSession(currentOperator.SessionId, (openUserSessionResult) =>
                        {
                            Dispatcher.BeginInvoke(() =>
                            {
                                try
                                {
                                    channel.Service.EndOpenUserSession(openUserSessionResult);
                                    channel.Service.BeginChangeCurrentClientRequestServiceType(serviceType, (changeCurrentClientRequestServiceTypeResult) =>
                                    {
                                        Dispatcher.BeginInvoke(() =>
                                        {
                                            try
                                            {
                                                CurrentClientRequest = channel.Service.EndChangeCurrentClientRequestServiceType(changeCurrentClientRequestServiceTypeResult);
                                            }
                                            catch (Exception exception)
                                            {
                                                MessageBox.Show(exception.Message);
                                            }
                                            finally
                                            {
                                                channel.Dispose();
                                                loading.Hide();
                                            }
                                        });
                                    }, null);
                                }
                                catch (Exception exception)
                                {
                                    MessageBox.Show(exception.Message);
                                }
                            });
                        }, null);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(exception.Message);
                    }
                }
            }
        }

        private void serviceChangeLink_Click(object sender, RoutedEventArgs eventArgs)
        {
            var servicesControl = new ServicesControl(channelBuilder, currentOperator);
            servicesControl.OnServiceSelected += (s, e) =>
            {
                var service = e.Service;

                var channel = channelBuilder.CreateChannel();
                try
                {
                    showLoading();

                    channel.Service.BeginOpenUserSession(currentOperator.SessionId, (openUserSessionResult) =>
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            try
                            {
                                serviceChangeLink.IsEnabled = false;
                                Task.Factory.StartNew(() =>
                                {
                                    Thread.Sleep(2000);
                                    Dispatcher.BeginInvoke(() => serviceChangeLink.IsEnabled = true);
                                });

                                channel.Service.EndOpenUserSession(openUserSessionResult);
                                channel.Service.BeginChangeCurrentClientRequestService(service.Id, (changeCurrentClientRequestServiceResult) =>
                                {
                                    Dispatcher.BeginInvoke(() =>
                                    {
                                        try
                                        {
                                            CurrentClientRequest = channel.Service.EndChangeCurrentClientRequestService(changeCurrentClientRequestServiceResult);
                                            mainGrid.Children.Remove(servicesControl);
                                        }
                                        catch (Exception exception)
                                        {
                                            MessageBox.Show(exception.Message);
                                        }
                                        finally
                                        {
                                            channel.Dispose();
                                            loading.Hide();
                                        }
                                    });
                                }, null);
                            }
                            catch (Exception exception)
                            {
                                MessageBox.Show(exception.Message);
                            }
                        });
                    }, null);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            };
            servicesControl.OnClose += (s, e) =>
            {
                mainGrid.Children.Remove(servicesControl);
            };
            mainGrid.Children.Add(servicesControl);
        }

        private void clientRequestsButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            var clientRequestsControl = new ClientRequestsControl(channelBuilder, currentOperator);
            clientRequestsControl.OnClose += (s, e) =>
            {
                mainGrid.Children.Remove(clientRequestsControl);
            };
            mainGrid.Children.Add(clientRequestsControl);
        }

        private void logoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (logoutHandler != null)
            {
                logoutHandler(this, new EventArgs());
            }
        }

        #region step 1

        private void callingButton_Click(object sender, RoutedEventArgs e)
        {
            callingButton.IsEnabled = false;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.BeginInvoke(() => callingButton.IsEnabled = true);
            });

            var channel = channelBuilder.CreateChannel();
            try
            {
                showLoading();

                channel.Service.BeginOpenUserSession(currentOperator.SessionId, (openUserSessionResult) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            channel.Service.EndOpenUserSession(openUserSessionResult);
                            channel.Service.BeginUpdateCurrentClientRequest(ClientRequestState.Calling, (updateCurrentClientRequestResult) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    try
                                    {
                                        channel.Service.EndUpdateCurrentClientRequest(updateCurrentClientRequestResult);
                                        channel.Service.BeginCallCurrentClient((callCurrentClientResult) =>
                                        {
                                            Dispatcher.BeginInvoke(() =>
                                            {
                                                try
                                                {
                                                    channel.Service.EndCallCurrentClient(callCurrentClientResult);
                                                }
                                                catch (Exception exception)
                                                {
                                                    MessageBox.Show(exception.Message);
                                                }
                                                finally
                                                {
                                                    channel.Dispose();
                                                    loading.Hide();
                                                }
                                            });
                                        }, null);
                                    }
                                    catch (FaultException exception)
                                    {
                                        MessageBox.Show(exception.Message);
                                    }
                                });
                            }, null);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                    });
                }, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion step 1

        #region step 2

        private void recallingButton_Click(object sender, RoutedEventArgs e)
        {
            recallingButton.IsEnabled = false;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.BeginInvoke(() => recallingButton.IsEnabled = true);
            });

            var channel = channelBuilder.CreateChannel();
            try
            {
                showLoading();

                channel.Service.BeginOpenUserSession(currentOperator.SessionId, (openUserSessionResult) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            channel.Service.EndOpenUserSession(openUserSessionResult);
                            channel.Service.BeginCallCurrentClient((callCurrentClientResult) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    try
                                    {
                                        channel.Service.EndCallCurrentClient(callCurrentClientResult);
                                    }
                                    catch (Exception exception)
                                    {
                                        MessageBox.Show(exception.Message);
                                    }
                                    finally
                                    {
                                        channel.Dispose();
                                        loading.Hide();
                                    }
                                });
                            }, null);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                    });
                }, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void renderingButton_Click(object sender, RoutedEventArgs e)
        {
            renderingButton.IsEnabled = false;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.BeginInvoke(() => renderingButton.IsEnabled = true);
            });

            var channel = channelBuilder.CreateChannel();
            try
            {
                showLoading();

                channel.Service.BeginOpenUserSession(currentOperator.SessionId, (openUserSessionResult) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            channel.Service.EndOpenUserSession(openUserSessionResult);
                            channel.Service.BeginUpdateCurrentClientRequest(ClientRequestState.Rendering, (updateCurrentClientRequestResult) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    try
                                    {
                                        channel.Service.EndUpdateCurrentClientRequest(updateCurrentClientRequestResult);
                                    }
                                    catch (Exception exception)
                                    {
                                        MessageBox.Show(exception.Message);
                                    }
                                    finally
                                    {
                                        channel.Dispose();
                                        loading.Hide();
                                    }
                                });
                            }, null);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                    });
                }, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void absenceButton_Click(object sender, RoutedEventArgs e)
        {
            absenceButton.IsEnabled = false;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.BeginInvoke(() => absenceButton.IsEnabled = true);
            });

            var channel = channelBuilder.CreateChannel();
            try
            {
                showLoading();

                channel.Service.BeginOpenUserSession(currentOperator.SessionId, (openUserSessionResult) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            channel.Service.EndOpenUserSession(openUserSessionResult);
                            channel.Service.BeginUpdateCurrentClientRequest(ClientRequestState.Absence, (updateCurrentClientRequestResult) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    try
                                    {
                                        channel.Service.EndUpdateCurrentClientRequest(updateCurrentClientRequestResult);
                                    }
                                    catch (Exception exception)
                                    {
                                        MessageBox.Show(exception.Message);
                                    }
                                    finally
                                    {
                                        channel.Dispose();
                                        loading.Hide();
                                    }
                                });
                            }, null);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                    });
                }, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion step 2

        #region step 3

        private void renderedButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            renderedButton.IsEnabled = false;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.BeginInvoke(() => renderedButton.IsEnabled = true);
            });

            var channel = channelBuilder.CreateChannel();
            try
            {
                showLoading();

                channel.Service.BeginOpenUserSession(currentOperator.SessionId, (openUserSessionResult) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            channel.Service.EndOpenUserSession(openUserSessionResult);
                            channel.Service.BeginUpdateCurrentClientRequest(ClientRequestState.Rendered, (updateCurrentClientRequestResult) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    try
                                    {
                                        channel.Service.EndUpdateCurrentClientRequest(updateCurrentClientRequestResult);
                                    }
                                    catch (Exception exception)
                                    {
                                        MessageBox.Show(exception.Message);
                                    }
                                    finally
                                    {
                                        channel.Dispose();
                                        loading.Hide();
                                    }
                                });
                            }, null);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                    });
                }, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void returnButton_Click(object sender, RoutedEventArgs e)
        {
            returnButton.IsEnabled = false;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.BeginInvoke(() => returnButton.IsEnabled = true);
            });

            var channel = channelBuilder.CreateChannel();
            try
            {
                showLoading();

                channel.Service.BeginOpenUserSession(currentOperator.SessionId, (openUserSessionResult) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            channel.Service.EndOpenUserSession(openUserSessionResult);
                            channel.Service.BeginReturnCurrentClientRequest((returnCurrentClientRequestResult) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    try
                                    {
                                        channel.Service.EndReturnCurrentClientRequest(returnCurrentClientRequestResult);
                                    }
                                    catch (Exception exception)
                                    {
                                        MessageBox.Show(exception.Message);
                                    }
                                    finally
                                    {
                                        channel.Dispose();
                                        loading.Hide();
                                    }
                                });
                            }, null);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                    });
                }, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        private void postponeButton_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan postponeTime;
            try
            {
                postponeTime = TimeSpan.FromMinutes(int.Parse(posponeMinutesTextBox.Text));
            }
            catch
            {
                MessageBox.Show("Время отложенного вызова указано время");
                return;
            }

            postponeButton.IsEnabled = false;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(2000);
                Dispatcher.BeginInvoke(() => postponeButton.IsEnabled = true);
            });

            Channel<IServerService> channel = channelBuilder.CreateChannel();
            try
            {
                showLoading();

                channel.Service.BeginOpenUserSession(currentOperator.SessionId, (openUserSessionResult) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            channel.Service.EndOpenUserSession(openUserSessionResult);
                            channel.Service.BeginPostponeCurrentClientRequest(postponeTime, (postponeCurrentClientResult) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    try
                                    {
                                        channel.Service.EndPostponeCurrentClientRequest(postponeCurrentClientResult);
                                    }
                                    catch (Exception exception)
                                    {
                                        MessageBox.Show(exception.Message);
                                    }
                                    finally
                                    {
                                        channel.Dispose();
                                        loading.Hide();
                                    }
                                });
                            }, null);
                        }
                        catch (Exception exception)
                        {
                            MessageBox.Show(exception.Message);
                        }
                    });
                }, null);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        #endregion step 3

        #region callbacks

        public void CallClient(ClientRequest clientRequest)
        {
        }

        public void ClientRequestUpdated(ClientRequest clientRequest)
        {
        }

        public void CurrentClientRequestUpdated(ClientRequest clientRequest, QueueOperator queueOperator)
        {
            Dispatcher.BeginInvoke(() =>
            {
                CurrentClientRequest = clientRequest;
            });
        }

        public void OperatorPlanMetricsUpdated(OperatorPlanMetrics operatorPlanMetrics)
        {
            //TODO: create
        }

        public void ConfigUpdated(Config config)
        {
        }

        public void Event(Event queueEvent)
        {
        }

        #endregion callbacks

        private void RichPage_Unloaded(object sender, RoutedEventArgs e)
        {
            callbackChannel.Dispose();
        }
    }
}