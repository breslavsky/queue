using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class SelectServiceControl : DependencyUserControl
    {
        #region dependency

        [Dependency]
        public User CurrentUser { get; set; }

        [Dependency]
        public ServerService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region events

        public event EventHandler<EventArgs> Selected;

        #endregion events

        #region fields

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;

        #endregion fields

        #region properties

        public Service SelectedService { get; private set; }

        #endregion properties

        public SelectServiceControl()
        {
            InitializeComponent();

            if (designtime)
            {
                return;
            }

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private async void LoadServiceGroup(TreeNodeCollection nodes, ServiceGroup serviceGroup = null)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    var serviceGroups = serviceGroup != null
                        ? await taskPool.AddTask(channel.Service.GetServiceGroups(serviceGroup.Id))
                        : await taskPool.AddTask(channel.Service.GetRootServiceGroups());

                    foreach (var g in serviceGroups)
                    {
                        var node = new TreeNode()
                        {
                            Text = g.ToString(),
                            Checked = g.IsActive,
                            Tag = g
                        };

                        node.Nodes.Add(new TreeNode("загрузка...") { Tag = g });
                        nodes.Add(node);
                    }

                    var services = serviceGroup != null
                        ? await taskPool.AddTask(channel.Service.GetServices(serviceGroup.Id))
                        : await taskPool.AddTask(channel.Service.GetRootServices());

                    foreach (var s in services)
                    {
                        var node = new TreeNode()
                        {
                            Text = s.ToString(),
                            Tag = s
                        };
                        nodes.Add(node);
                    }

                    if (serviceGroup != null)
                    {
                        nodes.RemoveAt(0);
                    }
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException ex)
                {
                    UIHelper.Warning(ex.Reason.ToString());
                }
                catch (Exception ex)
                {
                    UIHelper.Warning(ex.Message);
                }
            }
        }

        private void SelectServiceControl_Load(object sender, EventArgs e)
        {
            LoadServiceGroup(treeView.Nodes);
        }

        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var expandedNode = e.Node;

            var serviceGroup = expandedNode.Tag as ServiceGroup;
            if (serviceGroup != null)
            {
                if (expandedNode.Nodes.Cast<TreeNode>()
                    .Any(x => x.Tag.Equals(serviceGroup)))
                {
                    LoadServiceGroup(expandedNode.Nodes, serviceGroup);
                }
            }
        }

        private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Tag is ServiceGroup)
            {
                if (!e.Node.IsExpanded)
                {
                    e.Node.Expand();
                }
            }
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectedService = treeView.SelectedNode.Tag as Service;

            Selected(this, new EventArgs());
        }
    }
}