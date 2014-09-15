using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class ServicesForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        public ServicesForm(DuplexChannelBuilder<IServerService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private void ServicesForm_Load(object sender, EventArgs e)
        {
            loadServiceGroup(servicesTreeView.Nodes);
        }

        private async void loadServiceGroup(TreeNodeCollection nodes, ServiceGroup serviceGroup = null)
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

        private async void addServiceGroupMenuItem_Click(object sender, EventArgs e)
        {
            ServiceGroup selectedServiceGroup = null;

            var selectedNode = servicesTreeView.SelectedNode;
            if (selectedNode != null)
            {
                selectedServiceGroup = selectedNode.Tag as ServiceGroup;
            }

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    addServiceGroupMenuItem.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    var serviceGroup = selectedServiceGroup != null
                        ? await taskPool.AddTask(channel.Service.AddServiceGroup(selectedServiceGroup.Id))
                        : await taskPool.AddTask(channel.Service.AddRootServiceGroup());

                    var treeNode = new TreeNode(serviceGroup.ToString());
                    treeNode.Tag = serviceGroup;

                    if (selectedNode != null)
                    {
                        selectedNode.Nodes.Add(treeNode);
                        selectedNode.Expand();
                    }
                    else
                    {
                        servicesTreeView.Nodes.Add(treeNode);
                    }

                    using (var f = new ServiceGroupEditForm(channelBuilder, currentUser, serviceGroup))
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            treeNode.Text = f.ServiceGroup.ToString();
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
                    addServiceGroupMenuItem.Enabled = true;
                }
            }
        }

        private async void addServiceMenuItem_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    addServiceMenuItem.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));

                    Service service;

                    var selectedNode = servicesTreeView.SelectedNode;
                    if (selectedNode != null)
                    {
                        var selectedServiceGroup = selectedNode.Tag as ServiceGroup;
                        var selectedServiceGroupId = selectedServiceGroup.Id;

                        service = await taskPool.AddTask(channel.Service.AddService(selectedServiceGroupId));
                    }
                    else
                    {
                        service = await taskPool.AddTask(channel.Service.AddRootService());
                    }

                    var treeNode = new TreeNode(service.ToString());
                    treeNode.Tag = service;

                    if (selectedNode != null)
                    {
                        selectedNode.Nodes.Add(treeNode);
                        selectedNode.Expand();
                    }
                    else
                    {
                        servicesTreeView.Nodes.Add(treeNode);
                    }

                    using (var f = new ServiceEditForm(channelBuilder, currentUser, service))
                    {
                        if (f.ShowDialog() == DialogResult.OK)
                        {
                            treeNode.Text = f.Service.ToString();
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
                    addServiceMenuItem.Enabled = true;
                }
            }
        }

        private void editServiceGroupMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = servicesTreeView.SelectedNode;
            if (selectedNode != null)
            {
                var serviceGroup = selectedNode.Tag as ServiceGroup;

                using (var f = new ServiceGroupEditForm(channelBuilder, currentUser, serviceGroup))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        selectedNode.Text = f.ServiceGroup.ToString();
                    }
                }
            }
        }

        private void editServiceMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var selectedNode = servicesTreeView.SelectedNode;
            if (selectedNode != null)
            {
                var service = selectedNode.Tag as Service;

                using (var f = new ServiceEditForm(channelBuilder, currentUser, service))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        selectedNode.Text = f.Service.ToString();
                    }
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
                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            deleteServiceGroupMenuItem.Enabled = false;

                            await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
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
                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            deleteServiceMenuItem.Enabled = false;

                            await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
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
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        buttonUp.Enabled = false;

                        await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));

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
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        buttonDown.Enabled = false;

                        await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));

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

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));

                    if (typeof(ServiceGroup).IsInstanceOfType(checkedNode.Tag))
                    {
                        var serviceGroup = (ServiceGroup)checkedNode.Tag;
                        if (checkedNode.Checked)
                        {
                            await taskPool.AddTask(channel.Service.ServiceGroupActivate(serviceGroup.Id));
                        }
                        else
                        {
                            await taskPool.AddTask(channel.Service.ServiceGroupDeactivate(serviceGroup.Id));
                        }
                    }

                    if (typeof(Service).IsInstanceOfType(checkedNode.Tag))
                    {
                        Service service = (Service)checkedNode.Tag;
                        await taskPool.AddTask(channel.Service.ChangeServiceActivity(service.Id, checkedNode.Checked));
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
                            using (var channel = channelManager.CreateChannel())
                            {
                                try
                                {
                                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
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
                        using (var channel = channelManager.CreateChannel())
                        {
                            try
                            {
                                await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
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
                            using (var channel = channelManager.CreateChannel())
                            {
                                try
                                {
                                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
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
                if (channelManager != null)
                {
                    channelManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}