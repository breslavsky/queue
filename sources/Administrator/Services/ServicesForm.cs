﻿using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
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
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

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

        #region task pool

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        #endregion task pool

        private void ServicesForm_Load(object sender, EventArgs e)
        {
            loadGroup(treeView.Nodes);
        }

        private void ServicesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void loadGroup(TreeNodeCollection nodes, ServiceGroup group = null)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    var serviceGroups = group != null
                        ? await taskPool.AddTask(channel.Service.GetServiceGroups(group.Id))
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

                    var services = group != null
                        ? await taskPool.AddTask(channel.Service.GetServices(group.Id))
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

                    if (group != null)
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

        private async void buttonUp_Click(object sender, EventArgs e)
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    try
                    {
                        buttonUp.Enabled = false;

                        bool isUp = false;

                        if (selectedNode.Tag is ServiceGroup)
                        {
                            var group = selectedNode.Tag as ServiceGroup;
                            isUp = await taskPool.AddTask(channel.Service.ServiceGroupUp(group.Id));
                        }

                        if (selectedNode.Tag is Service)
                        {
                            var service = selectedNode.Tag as Service;
                            isUp = await taskPool.AddTask(channel.Service.ServiceUp(service.Id));
                        }

                        if (isUp)
                        {
                            var nodes = selectedNode.Parent != null ? selectedNode.Parent.Nodes : treeView.Nodes;

                            var prevNode = selectedNode.PrevNode;
                            if (prevNode != null)
                            {
                                int indexSelectedNode = selectedNode.Index;
                                int indexPreviousNode = prevNode.Index;

                                nodes.RemoveAt(indexSelectedNode);
                                nodes.Insert(indexPreviousNode, selectedNode);

                                treeView.SelectedNode = selectedNode;
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
            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    try
                    {
                        buttonDown.Enabled = false;

                        bool isDown = false;

                        if (selectedNode.Tag is ServiceGroup)
                        {
                            var group = selectedNode.Tag as ServiceGroup;
                            isDown = await taskPool.AddTask(channel.Service.ServiceGroupDown(group.Id));
                        }

                        if (selectedNode.Tag is Service)
                        {
                            var service = selectedNode.Tag as Service;
                            isDown = await taskPool.AddTask(channel.Service.ServiceDown(service.Id));
                        }

                        if (isDown)
                        {
                            var nodes = selectedNode.Parent != null ? selectedNode.Parent.Nodes : treeView.Nodes;

                            var nextNode = selectedNode.NextNode;
                            if (nextNode != null)
                            {
                                int indexSelectedNode = selectedNode.Index;
                                int indexNextNode = nextNode.Index;

                                nodes.RemoveAt(indexSelectedNode);
                                nodes.Insert(indexNextNode, selectedNode);

                                treeView.SelectedNode = selectedNode;
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

        #region context menu

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            addServiceGroupMenuItem.Visible = false;
            addServiceMenuItem.Visible = false;
            editServiceGroupMenuItem.Visible = false;
            editServiceMenuItem.Visible = false;
            deleteGroupMenuItem.Visible = false;
            deleteServiceMenuItem.Visible = false;

            if (selectedNode != null)
            {
                if (selectedNode.Tag is ServiceGroup)
                {
                    addServiceGroupMenuItem.Visible = true;
                    addServiceMenuItem.Visible = true;
                    editServiceGroupMenuItem.Visible = true;
                    deleteGroupMenuItem.Visible = true;
                }

                if (selectedNode.Tag is Service)
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

        private void addGroupMenuItem_Click(object sender, EventArgs e)
        {
            Guid? parentGroupId = null;

            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                parentGroupId = (selectedNode.Tag as ServiceGroup).Id;
            }

            using (var f = new EditServiceGroupForm(parentGroupId))
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
                            treeView.Nodes.Add(treeNode);
                        }

                        treeView.SelectedNode = treeNode;
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
            Guid? groupId = null;

            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                groupId = (selectedNode.Tag as ServiceGroup).Id;
            }

            using (var f = new EditServiceForm(groupId))
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
                            treeView.Nodes.Add(treeNode);
                        }

                        treeView.SelectedNode = treeNode;
                    }

                    treeNode.Text = f.Service.ToString();
                };

                f.ShowDialog();
            }
        }

        private void editServiceGroupMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                var group = selectedNode.Tag as ServiceGroup;

                using (var f = new EditServiceGroupForm(null, group.Id))
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
            var selectedNode = treeView.SelectedNode;
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

        private async void deleteGroupMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                var group = selectedNode.Tag as ServiceGroup;
                if (MessageBox.Show(string.Format("Вы действительно хотите удалить [{0}] ?", group),
                    "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (var channel = ChannelManager.CreateChannel())
                    {
                        try
                        {
                            deleteGroupMenuItem.Enabled = false;

                            await taskPool.AddTask(channel.Service.DeleteServiceGroup(group.Id));

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
                            deleteGroupMenuItem.Enabled = true;
                        }
                    }
                }
            }
        }

        private async void deleteServiceMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                var service = selectedNode.Tag as Service;
                if (MessageBox.Show(string.Format("Вы действительно хотите удалить [{0}] ?", service),
                    "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (var channel = ChannelManager.CreateChannel())
                    {
                        try
                        {
                            deleteServiceMenuItem.Enabled = false;

                            await taskPool.AddTask(channel.Service.DeleteService(service.Id));

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

        #endregion context menu

        #region tree view

        private async void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var checkedNode = e.Node;

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    if (checkedNode.Tag is ServiceGroup)
                    {
                        var group = checkedNode.Tag as ServiceGroup;
                        group.IsActive = checkedNode.Checked;
                        await taskPool.AddTask(channel.Service.EditServiceGroup(group));
                    }

                    if (checkedNode.Tag is Service)
                    {
                        var service = checkedNode.Tag as Service;
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

        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var expandedNode = e.Node;
            if (expandedNode != null && expandedNode.Tag is ServiceGroup)
            {
                var group = expandedNode.Tag as ServiceGroup;

                if (expandedNode.Nodes.Cast<TreeNode>()
                    .Any(x => x.Tag.Equals(group)))
                {
                    loadGroup(expandedNode.Nodes, group);
                }
            }
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView.SelectedNode = e.Node;
            }
        }

        private void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (treeView.GetNodeAt(e.X, e.Y) == null)
            {
                treeView.SelectedNode = null;
            }
        }

        private void treeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            treeView.SelectedNode = (TreeNode)e.Item;
            treeView.DoDragDrop(e.Item, DragDropEffects.All);
        }

        private async void treeView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                var draggingNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;
                var hoveringNode = treeView.GetNodeAt(treeView.PointToClient(new Point(e.X, e.Y)));

                if (draggingNode.Tag is ServiceGroup)
                {
                    var group = draggingNode.Tag as ServiceGroup;

                    if (hoveringNode != null)
                    {
                        if (hoveringNode.Tag is ServiceGroup)
                        {
                            var targetGroup = hoveringNode.Tag as ServiceGroup;

                            using (var channel = ChannelManager.CreateChannel())
                            {
                                try
                                {
                                    await taskPool.AddTask(channel.Service.MoveServiceGroup(group.Id, targetGroup.Id));
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
                                await taskPool.AddTask(channel.Service.MoveServiceGroupToRoot(group.Id));
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

                if (draggingNode.Tag is Service)
                {
                    var service = draggingNode.Tag as Service;

                    if (hoveringNode != null)
                    {
                        var group = hoveringNode.Tag as ServiceGroup;

                        if (hoveringNode.Tag is ServiceGroup)
                        {
                            using (var channel = ChannelManager.CreateChannel())
                            {
                                try
                                {
                                    await taskPool.AddTask(channel.Service.MoveService(service.Id, group.Id));
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
                    treeView.Nodes.Add(draggingNode);
                }
            }
        }

        private void treeView_DragOver(object sender, DragEventArgs e)
        {
            var draggingNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;
            var hoveringNode = treeView.GetNodeAt(treeView.PointToClient(new Point(e.X, e.Y)));

            if (draggingNode != hoveringNode)
            {
                if (draggingNode.Tag is ServiceGroup)
                {
                    if (hoveringNode != null)
                    {
                        if (hoveringNode.Tag is ServiceGroup)
                        {
                            e.Effect = DragDropEffects.Move;
                        }

                        if (hoveringNode.Tag is Service)
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
                    else
                    {
                        e.Effect = DragDropEffects.Move;
                    }
                }

                if (draggingNode.Tag is Service)
                {
                    if (hoveringNode != null)
                    {
                        if (hoveringNode.Tag is ServiceGroup)
                        {
                            e.Effect = DragDropEffects.Move;
                        }

                        if (hoveringNode.Tag is Service)
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

        #endregion tree view

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