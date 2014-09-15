using Queue.Model.Common;
using Queue.Operator.Silverlight.QueueRemoteService;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Windows;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Operator.Silverlight
{
    public delegate void LoginPageEventHandler(object sender, EventArgs e);

    public partial class LoginPage : Queue.UI.Silverlight.RichPage, IServerServiceCallback
    {
        private UserRole userRole;

        public LoginPage(UserRole userRole)
            : base()
        {
            InitializeComponent();

            this.userRole = userRole;
        }

        public event LoginPageEventHandler OnLogined
        {
            add { loginedHandler += value; }
            remove { loginedHandler -= value; }
        }

        private event LoginPageEventHandler loginedHandler;

        public DuplexChannelBuilder<IServerService> ChannelBuilder { get; private set; }

        public string Endpoint
        {
            get
            {
                return endpointTextBox.Text;
            }
            set
            {
                endpointTextBox.Text = value;
            }
        }

        public Guid UserId { get; set; }

        public string Password
        {
            get
            {
                return passwordBox.Password;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    passwordBox.Password = value;
                }
            }
        }

        public bool IsRemember
        {
            get
            {
                return (bool)rememberCheckBox.IsChecked;
            }
            set
            {
                rememberCheckBox.IsChecked = value;
            }
        }

        public User User { get; private set; }

        private void LoginPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsRemember)
            {
                connect();
            }
        }

        private void connect()
        {
            CustomBinding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(),
                new TcpTransportBindingElement()
                {
                    MaxReceivedMessageSize = DataLength._10M
                });
            EndpointAddress endpoint = new EndpointAddress(new Uri(Endpoint));

            ChannelBuilder = new DuplexChannelBuilder<IServerService>(this, binding, endpoint);

            Channel<IServerService> channel = ChannelBuilder.CreateChannel();
            try
            {
                showLoading();

                channel.Service.BeginGetUserList(userRole, (getUsersListResult) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            operatorsComboBox.ItemsSource = channel.Service.EndGetUserList(getUsersListResult);
                            operatorsComboBox.SelectedIndex = 0;

                            if (UserId != Guid.Empty)
                            {
                                operatorsComboBox.SelectedValue = UserId;
                            }

                            passwordBox.Focus();

                            if (IsRemember)
                            {
                                login();
                            }
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
        }

        private void login()
        {
            object selectedOperator = operatorsComboBox.SelectedValue;
            if (selectedOperator != null)
            {
                Guid operatorId = (Guid)selectedOperator;
                string password = passwordBox.Password;

                Channel<IServerService> channel = ChannelBuilder.CreateChannel();
                try
                {
                    showLoading();

                    channel.Service.BeginUserLogin(operatorId, password, (userLoginResult) =>
                    {
                        Dispatcher.BeginInvoke(() =>
                        {
                            try
                            {
                                User = channel.Service.EndUserLogin(userLoginResult);

                                Endpoint = endpointTextBox.Text;
                                UserId = (Guid)operatorsComboBox.SelectedValue;

                                if (loginedHandler != null)
                                {
                                    loginedHandler(this, new EventArgs());
                                }
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
            }
        }

        private void connectButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            connect();
        }

        private void loginButton_Click(object sender, RoutedEventArgs eventArgs)
        {
            login();
        }

        #region callbacks

        public void CallClient(ClientRequest clientRequest)
        {
        }

        public void ClientRequestUpdated(ClientRequest clientRequest)
        {
        }

        public void CurrentClientRequestUpdated(ClientRequest clientRequest, QueueOperator queueOperator)
        {
        }

        public void OperatorPlanMetricsUpdated(OperatorPlanMetrics operatorPlanMetrics)
        {
        }

        public void ConfigUpdated(Config config)
        {
        }

        public void Event(Event queueEvent)
        {
        }

        #endregion callbacks
    }
}