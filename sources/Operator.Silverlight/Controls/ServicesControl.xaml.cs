using Queue.Operator.Silverlight.QueueRemoteService;
using Queue.Services.DTO;
using Queue.UI.Silverlight;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using QueueOperator = Queue.Services.DTO.Operator;

namespace Queue.Operator.Silverlight
{
    public class ServicesControlEventArgs
    {
        public Service Service;
    }

    public delegate void ServicesControlEventHandler(object sender, ServicesControlEventArgs e);

    public partial class ServicesControl : RichControl
    {
        public abstract class TreeNode
        {
            public virtual string Name { get { return string.Empty; } }

            public virtual ObservableCollection<TreeNode> Children
            {
                get
                {
                    return new ObservableCollection<TreeNode>();
                }
            }
        }

        public class ServiceGroupNode : TreeNode
        {
            public ServiceGroup ServiceGroup { get; set; }

            public override string Name
            {
                get
                {
                    return ServiceGroup.ToString();
                }
            }

            private Func<ServiceGroup, ObservableCollection<TreeNode>> loader;

            public override ObservableCollection<TreeNode> Children
            {
                get { return loader(ServiceGroup); }
            }

            public ServiceGroupNode(Func<ServiceGroup, ObservableCollection<TreeNode>> loader)
            {
                this.loader = loader;
            }
        }

        public class ServiceNode : TreeNode
        {
            public Service Service { get; set; }

            public override string Name
            {
                get { return Service.ToString(); }
            }
        }

        public event ServicesControlEventHandler OnServiceSelected
        {
            add { serviceSelectedHandler += value; }
            remove { serviceSelectedHandler -= value; }
        }

        private event ServicesControlEventHandler serviceSelectedHandler;

        public event ServicesControlEventHandler OnClose
        {
            add { closeHandler += value; }
            remove { closeHandler -= value; }
        }

        private event ServicesControlEventHandler closeHandler;

        private DuplexChannelBuilder<IServerService> channelBuilder;
        private QueueOperator currentOperator;

        public ServicesControl(DuplexChannelBuilder<IServerService> channelBuilder, QueueOperator currentOperator)
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentOperator = currentOperator;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Channel<IServerService> channel = channelBuilder.CreateChannel();

            AsyncCallback onEndGetRootServiceGroups = (r) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        var serviceGroups = channel.Service.EndGetRootServiceGroups(r);

                        var treeNodes = new ObservableCollection<TreeNode>();
                        foreach (var g in serviceGroups)
                        {
                            treeNodes.Add(new ServiceGroupNode(loadServiceGroup)
                            {
                                ServiceGroup = g
                            });
                        }
                        servicesTreeView.DataContext = treeNodes;
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
                        channel.Service.BeginGetRootServiceGroups(onEndGetRootServiceGroups, null);
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

        private ObservableCollection<TreeNode> loadServiceGroup(ServiceGroup serviceGroup)
        {
            var nodes = new ObservableCollection<TreeNode>();

            Dispatcher.BeginInvoke(() =>
            {
                Channel<IServerService> channel = channelBuilder.CreateChannel();

                AsyncCallback onEndGetServiceGroups = (r) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            var serviceGroups = channel.Service.EndGetServiceGroups(r);

                            foreach (var g in serviceGroups)
                            {
                                nodes.Add(new ServiceGroupNode(loadServiceGroup)
                                {
                                    ServiceGroup = g
                                });
                            }

                            channel.Service.BeginGetServices(serviceGroup.Id, (getServicesResult) =>
                            {
                                Dispatcher.BeginInvoke(() =>
                                {
                                    try
                                    {
                                        var services = channel.Service.EndGetServices(getServicesResult);

                                        foreach (var s in services)
                                        {
                                            nodes.Add(new ServiceNode()
                                            {
                                                Service = s
                                            });
                                        }
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
                    });
                };

                AsyncCallback onEndOpenUserSession = (r) =>
                {
                    Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            channel.Service.EndOpenUserSession(r);
                            channel.Service.BeginGetServiceGroups(serviceGroup.Id, onEndGetServiceGroups, null);
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
            });

            return nodes;
        }

        private void servicesTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            object selected = e.NewValue;
            if (selected is ServiceNode && serviceSelectedHandler != null)
            {
                serviceSelectedHandler(this, new ServicesControlEventArgs()
                {
                    Service = ((ServiceNode)selected).Service
                });
            }
        }

        private void hideButton_Click(object sender, RoutedEventArgs e)
        {
            if (closeHandler != null)
            {
                closeHandler(this, new ServicesControlEventArgs());
            }
        }
    }
}