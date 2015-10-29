using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class ServicesForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region fields

        private readonly TaskPool taskPool;

        #endregion fields

        public ServicesForm()
            : base()
        {
            InitializeComponent();

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

        private void ServicesForm_Load(object sender, EventArgs e)
        {
            loadServiceGroup(servicesTreeView.Nodes);
        }

        private async void loadServiceGroup(TreeNodeCollection nodes, ServiceGroup serviceGroup = null)
        {
            using (var channel = ChannelManager.CreateChannel())
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
                            Checked = s.IsActive,
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
                catch (FaultException exception)
                {
                    UIHelper.Warning(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(exception.Message);
                }
            }
        }

        private void servicesTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var expandedNode = e.Node;
            if (expandedNode != null && expandedNode.Tag is ServiceGroup)
            {
                var serviceGroup = (ServiceGroup)expandedNode.Tag;

                if (expandedNode.Nodes.Cast<TreeNode>()
                    .Any(x => x.Tag.Equals(serviceGroup)))
                {
                    loadServiceGroup(expandedNode.Nodes, serviceGroup);
                }
            }
        }

        private void addServiceGroupMenuItem_Click(object sender, EventArgs e)
        {
            Guid? parentGroupId = null;

            var selectedNode = servicesTreeView.SelectedNode;
            if (selectedNode != null)
            {
                parentGroupId = (selectedNode.Tag as ServiceGroup).Id;
            }

            using (var f = new EditServiceGroupForm())
            {
                TreeNode treeNode = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (treeNode == null)
                    {
                        treeNode = new TreeNode();
                        treeNode.Tag = f.ServiceGroup;
                        treeNode.Checked = f.ServiceGroup.IsActive;

                        if (selectedNode != null)
                        {
                            selectedNode.Nodes.Add(treeNode);
                            selectedNode.Expand();
                        }
                        else
                        {
                            servicesTreeView.Nodes.Add(treeNode);
                        }

                        servicesTreeView.SelectedNode = treeNode;
                    }

                    treeNode.Text = f.ServiceGroup.ToString();
                    f.Close();
                };

                f.ShowDialog();

                ServiceGroup serviceGroup = f.ServiceGroup;
            }
        }

        private void addServiceMenuItem_Click(object sender, EventArgs e)
        {
            Guid? serviceGroupId = null;

            var selectedNode = servicesTreeView.SelectedNode;
            if (selectedNode != null)
            {
                serviceGroupId = (selectedNode.Tag as ServiceGroup).Id;
            }

            using (var f = new EditServiceForm())
            {
                TreeNode treeNode = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (treeNode == null)
                    {
                        treeNode = new TreeNode();
                        treeNode.Tag = f.Service;
                        treeNode.Checked = f.Service.IsActive;

                        if (selectedNode != null)
                        {
                            selectedNode.Nodes.Add(treeNode);
                            selectedNode.Expand();
                        }
                        else
                        {
                            servicesTreeView.Nodes.Add(treeNode);
                        }

                        servicesTreeView.SelectedNode = treeNode;
                    }

                    treeNode.Text = f.Service.ToString();
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private void editServiceGroupMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = servicesTreeView.SelectedNode;
            if (selectedNode != null)
            {
                var serviceGroup = selectedNode.Tag as ServiceGroup;

                using (var f = new EditServiceGroupForm(null, serviceGroup.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        selectedNode.Text = f.ServiceGroup.ToString();
                        f.Close();
                    };

                    f.ShowDialog();
                }
            }
        }

        private void editServiceMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = servicesTreeView.SelectedNode;
            if (selectedNode != null)
            {
                var service = selectedNode.Tag as Service;

                using (var f = new EditServiceForm(null, service.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        selectedNode.Text = f.Service.ToString();
                        f.Close();
                    };

                    f.ShowDialog();
                }
            }
        }

        private async void deleteServiceGroupMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = servicesTreeView.SelectedNode;
            if (selectedNode != null)
            {
                var selectedServiceGroup = selectedNode.Tag as ServiceGroup;
                if (MessageBox.Show(string.Format("Вы действительно хотите удалить [{0}] ?", selectedServiceGroup),
                    "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (var channel = ChannelManager.CreateChannel())
                    {
                        try
                        {
                            deleteServiceGroupMenuItem.Enabled = false;

                            await taskPool.AddTask(channel.Service.DeleteServiceGroup(selectedServiceGroup.Id));

                            selectedNode.Nodes.Remove(selectedNode);
                        }
                        catch (OperationCanceledException) { }
                        catch (CommunicationObjectAbortedException) { }
                        catch (ObjectDisposedException) { }
                        catch (InvalidOperationException) { }
                        catch (FaultException exception)
                        {
                            UIHelper.Warning(exception.Reason.ToString());
                        }
                        catch (Exception exception)
                        {
                            UIHelper.Warning(exception.Message);
                        }
                        finally
                        {
                            deleteServiceGroupMenuItem.Enabled = true;
                        }
                    }
                }
            }
        }

        private async void deleteServiceMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = servicesTreeView.SelectedNode;
            if (selectedNode != null)
            {
                var selectedService = selectedNode.Tag as Service;

                if (MessageBox.Show(string.Format("Вы действительно хотите удалить [{0}] ?", selectedService),
                    "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (var channel = ChannelManager.CreateChannel())
                    {
                        try
                        {
                            deleteServiceMenuItem.Enabled = false;

                            await taskPool.AddTask(channel.Service.DeleteService(selectedService.Id));

                            selectedNode.Nodes.Remove(selectedNode);
                        }
                        catch (OperationCanceledException) { }
                        catch (CommunicationObjectAbortedException) { }
                        catch (ObjectDisposedException) { }
                        catch (InvalidOperationException) { }
                        catch (FaultException exception)
                        {
                            UIHelper.Warning(exception.Reason.ToString());
                        }
                        catch (Exception exception)
                        {
                            UIHelper.Warning(exception.Message);
                        }
                        finally
                        {
                            deleteServiceMenuItem.Enabled = true;
                        }
                    }
                }
            }
        }

        private async void buttonUp_Click(object sender, EventArgs e)
        {
            var selectedNode = servicesTreeView.SelectedNode;
            if (selectedNode != null)
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    try
                    {
                        buttonUp.Enabled = false;

                        bool isUp = false;

                        if (typeof(ServiceGroup).IsInstanceOfType(selectedNode.Tag))
                        {
                            var serviceGroup = (ServiceGroup)selectedNode.Tag;
                            isUp = await taskPool.AddTask(channel.Service.ServiceGroupUp(serviceGroup.Id));
                        }

                        if (typeof(Service).IsInstanceOfType(selectedNode.Tag))
                        {
                            var service = (Service)selectedNode.Tag;
                            isUp = await taskPool.AddTask(channel.Service.ServiceUp(service.Id));
                        }

                        if (isUp)
                        {
                            var nodes = selectedNode.Parent != null ? selectedNode.Parent.Nodes : servicesTreeView.Nodes;

                            var previosNode = selectedNode.PrevNode;
                            if (previosNode != null)
                            {
                                int indexSelectedNode = selectedNode.Index;
                                int indexPreviousNode = previosNode.Index;

                                nodes.RemoveAt(indexSelectedNode);
                                nodes.Insert(indexPreviousNode, selectedNode);

                                servicesTreeView.SelectedNode = selectedNode;
                            }
                        }
                    }
                    catch (OperationCanceledException) { }
                    catch (CommunicationObjectAbortedException) { }
                    catch (ObjectDisposedException) { }
                    catch (InvalidOperationException) { }
                    catch (FaultException exception)
                    {
                        UIHelper.Warning(exception.Reason.ToString());
                    }
                    catch (Exception exception)
                    {
                        UIHelper.Warning(exception.Message);
                    }
                    finally
                    {
                        buttonUp.Enabled = true;
                    }
                }
            }
        }

        private async void buttonDown_Click(object sender, EventArgs e)
        {
            var selectedNode = servicesTreeView.SelectedNode;
            if (selectedNode != null)
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    try
                    {
                        buttonDown.Enabled = false;

                        bool isDown = false;

                        if (typeof(ServiceGroup).IsInstanceOfType(selectedNode.Tag))
                        {
                            var serviceGroup = (ServiceGroup)selectedNode.Tag;
                            isDown = await taskPool.AddTask(channel.Service.ServiceGroupDown(serviceGroup.Id));
                        }

                        if (typeof(Service).IsInstanceOfType(selectedNode.Tag))
                        {
                            var service = (Service)selectedNode.Tag;
                            isDown = await taskPool.AddTask(channel.Service.ServiceDown(service.Id));
                        }

                        if (isDown)
                        {
                            var nodes = selectedNode.Parent != null ? selectedNode.Parent.Nodes : servicesTreeView.Nodes;

                            var nextNode = selectedNode.NextNode;
                            if (nextNode != null)
                            {
                                int indexSelectedNode = selectedNode.Index;
                                int indexNextNode = nextNode.Index;

                                nodes.RemoveAt(indexSelectedNode);
                                nodes.Insert(indexNextNode, selectedNode);

                                servicesTreeView.SelectedNode = selectedNode;
                            }
                        }
                    }
                    catch (OperationCanceledException) { }
                    catch (CommunicationObjectAbortedException) { }
                    catch (ObjectDisposedException) { }
                    catch (InvalidOperationException) { }
                    catch (FaultException exception)
                    {
                        UIHelper.Warning(exception.Reason.ToString());
                    }
                    catch (Exception exception)
                    {
                        UIHelper.Warning(exception.Message);
                    }
                    finally
                    {
                        buttonDown.Enabled = true;
                    }
                }
            }
        }

        private async void serviceGroupsTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var checkedNode = e.Node;

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    if (typeof(ServiceGroup).IsInstanceOfType(checkedNode.Tag))
                    {
                        ServiceGroup serviceGroup = checkedNode.Tag as ServiceGroup;
                        serviceGroup.IsActive = checkedNode.Checked;
                        await taskPool.AddTask(channel.Service.EditServiceGroup(serviceGroup));
                    }

                    if (typeof(Service).IsInstanceOfType(checkedNode.Tag))
                    {
                        Service service = checkedNode.Tag as Service;
                        service.IsActive = checkedNode.Checked;
                        await taskPool.AddTask(channel.Service.EditService(service));
                    }
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException exception)
                {
                    UIHelper.Warning(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(exception.Message);
                }
            }
        }

        private void serviceGroupsTreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                servicesTreeView.SelectedNode = e.Node;
            }
        }

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TreeNode selectedNode = servicesTreeView.SelectedNode;

            addServiceGroupMenuItem.Visible = false;
            addServiceMenuItem.Visible = false;
            editServiceGroupMenuItem.Visible = false;
            editServiceMenuItem.Visible = false;
            deleteServiceGroupMenuItem.Visible = false;
            deleteServiceMenuItem.Visible = false;

            if (selectedNode != null)
            {
                if (typeof(ServiceGroup).IsInstanceOfType(selectedNode.Tag))
                {
                    addServiceGroupMenuItem.Visible = true;
                    addServiceMenuItem.Visible = true;
                    editServiceGroupMenuItem.Visible = true;
                    deleteServiceGroupMenuItem.Visible = true;
                }

                if (typeof(Service).IsInstanceOfType(selectedNode.Tag))
                {
                    editServiceMenuItem.Visible = true;
                    deleteServiceMenuItem.Visible = true;
                }
            }
            else
            {
                addServiceGroupMenuItem.Visible = true;
                addServiceMenuItem.Visible = true;
            }
        }

        private void serviceGroupsTreeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (servicesTreeView.GetNodeAt(e.X, e.Y) == null)
            {
                servicesTreeView.SelectedNode = null;
            }
        }

        private void serviceGroupsTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            servicesTreeView.SelectedNode = (TreeNode)e.Item;
            servicesTreeView.DoDragDrop(e.Item, DragDropEffects.All);
        }

        private async void serviceGroupsTreeView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                var draggingNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
                var hoveringNode = servicesTreeView.GetNodeAt(servicesTreeView.PointToClient(new Point(e.X, e.Y)));

                if (typeof(ServiceGroup).IsInstanceOfType(draggingNode.Tag))
                {
                    var sourceServiceGroup = (ServiceGroup)draggingNode.Tag;

                    if (hoveringNode != null)
                    {
                        var targetServiceGroup = (ServiceGroup)hoveringNode.Tag;

                        if (typeof(ServiceGroup).IsInstanceOfType(hoveringNode.Tag))
                        {
                            using (var channel = ChannelManager.CreateChannel())
                            {
                                try
                                {
                                    await taskPool.AddTask(channel.Service.MoveServiceGroup(sourceServiceGroup.Id, targetServiceGroup.Id));
                                }
                                catch (OperationCanceledException) { }
                                catch (CommunicationObjectAbortedException) { }
                                catch (ObjectDisposedException) { }
                                catch (InvalidOperationException) { }
                                catch (FaultException exception)
                                {
                                    UIHelper.Warning(exception.Reason.ToString());
                                }
                                catch (Exception exception)
                                {
                                    UIHelper.Warning(exception.Message);
                                }
                            }
                        }

                        if (typeof(Service).IsInstanceOfType(hoveringNode.Tag))
                        {
                            return;
                        }
                    }
                    else
                    {
                        using (var channel = ChannelManager.CreateChannel())
                        {
                            try
                            {
                                await taskPool.AddTask(channel.Service.MoveServiceGroupToRoot(sourceServiceGroup.Id));
                            }
                            catch (OperationCanceledException) { }
                            catch (CommunicationObjectAbortedException) { }
                            catch (ObjectDisposedException) { }
                            catch (InvalidOperationException) { }
                            catch (FaultException exception)
                            {
                                UIHelper.Warning(exception.Reason.ToString());
                            }
                            catch (Exception exception)
                            {
                                UIHelper.Warning(exception.Message);
                            }
                        }
                    }
                }

                if (typeof(Service).IsInstanceOfType(draggingNode.Tag))
                {
                    Service service = (Service)draggingNode.Tag;

                    if (hoveringNode != null)
                    {
                        var serviceGroup = (ServiceGroup)hoveringNode.Tag;

                        if (typeof(ServiceGroup).IsInstanceOfType(hoveringNode.Tag))
                        {
                            using (var channel = ChannelManager.CreateChannel())
                            {
                                try
                                {
                                    await taskPool.AddTask(channel.Service.MoveService(service.Id, serviceGroup.Id));
                                }
                                catch (OperationCanceledException) { }
                                catch (CommunicationObjectAbortedException) { }
                                catch (ObjectDisposedException) { }
                                catch (InvalidOperationException) { }
                                catch (FaultException exception)
                                {
                                    UIHelper.Warning(exception.Reason.ToString());
                                }
                                catch (Exception exception)
                                {
                                    UIHelper.Warning(exception.Message);
                                }
                            }
                        }

                        if (typeof(Service).IsInstanceOfType(hoveringNode.Tag))
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                if (hoveringNode != null)
                {
                    draggingNode.Remove();
                    hoveringNode.Nodes.Add(draggingNode);
                }
                else
                {
                    draggingNode.Remove();
                    servicesTreeView.Nodes.Add(draggingNode);
                }
            }
        }

        private void serviceGroupsTreeView_DragOver(object sender, DragEventArgs e)
        {
            TreeNode draggingNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            TreeNode hoveringNode = servicesTreeView.GetNodeAt(servicesTreeView.PointToClient(new Point(e.X, e.Y)));

            if (draggingNode != hoveringNode)
            {
                if (typeof(ServiceGroup).IsInstanceOfType(draggingNode.Tag))
                {
                    if (hoveringNode != null)
                    {
                        if (typeof(ServiceGroup).IsInstanceOfType(hoveringNode.Tag))
                        {
                            e.Effect = DragDropEffects.Move;
                        }

                        if (typeof(Service).IsInstanceOfType(hoveringNode.Tag))
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
                    else
                    {
                        e.Effect = DragDropEffects.Move;
                    }
                }

                if (typeof(Service).IsInstanceOfType(draggingNode.Tag))
                {
                    if (hoveringNode != null)
                    {
                        if (typeof(ServiceGroup).IsInstanceOfType(hoveringNode.Tag))
                        {
                            e.Effect = DragDropEffects.Move;
                        }

                        if (typeof(Service).IsInstanceOfType(hoveringNode.Tag))
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
                    else
                    {
                        e.Effect = DragDropEffects.None;
                    }
                }
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ServicesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (taskPool != null)
                {
                    taskPool.Dispose();
                }
                if (ChannelManager != null)
                {
                    ChannelManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}