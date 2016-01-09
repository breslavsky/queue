using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class LifeSituationsForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public ChannelManager<ILifeSituationTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region fields

        private readonly TaskPool taskPool;

        #endregion fields

        public LifeSituationsForm()
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

        private void LifeSituationsForm_Load(object sender, EventArgs e)
        {
            loadGroup(treeView.Nodes);
        }

        private void LifeSituationsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void loadGroup(TreeNodeCollection nodes, LifeSituationGroup group = null)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    var serviceGroups = group != null
                        ? await taskPool.AddTask(channel.Service.GetGroups(group.Id))
                        : await taskPool.AddTask(channel.Service.GetRootGroups());

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
                        ? await taskPool.AddTask(channel.Service.GetLifeSituations(group.Id))
                        : await taskPool.AddTask(channel.Service.GetRootLifeSituations());

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

                        if (selectedNode.Tag is LifeSituationGroup)
                        {
                            var group = selectedNode.Tag as LifeSituationGroup;
                            isUp = await taskPool.AddTask(channel.Service.GroupUp(group.Id));
                        }

                        if (selectedNode.Tag is LifeSituation)
                        {
                            var lifeSituation = selectedNode.Tag as LifeSituation;
                            isUp = await taskPool.AddTask(channel.Service.LifeSituationUp(lifeSituation.Id));
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

                        if (selectedNode.Tag is LifeSituationGroup)
                        {
                            var group = selectedNode.Tag as LifeSituationGroup;
                            isDown = await taskPool.AddTask(channel.Service.GroupDown(group.Id));
                        }

                        if (selectedNode.Tag is LifeSituation)
                        {
                            var lifeSituation = selectedNode.Tag as LifeSituation;
                            isDown = await taskPool.AddTask(channel.Service.LifeSituationDown(lifeSituation.Id));
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

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            TreeNode selectedNode = treeView.SelectedNode;

            addGroupMenuItem.Visible = false;
            addLifeSituationMenuItem.Visible = false;
            editGroupMenuItem.Visible = false;
            editLifeSituationMenuItem.Visible = false;
            deleteGroupMenuItem.Visible = false;
            deleteLifeSituationMenuItem.Visible = false;

            if (selectedNode != null)
            {
                if (selectedNode.Tag is LifeSituationGroup)
                {
                    addGroupMenuItem.Visible = true;
                    addLifeSituationMenuItem.Visible = true;
                    editGroupMenuItem.Visible = true;
                    deleteGroupMenuItem.Visible = true;
                }

                if (selectedNode.Tag is LifeSituation)
                {
                    editLifeSituationMenuItem.Visible = true;
                    deleteLifeSituationMenuItem.Visible = true;
                }
            }
            else
            {
                addGroupMenuItem.Visible = true;
                addLifeSituationMenuItem.Visible = true;
            }
        }

        private void addGroupMenuItem_Click(object sender, EventArgs e)
        {
            Guid? parentGroupId = null;

            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                parentGroupId = (selectedNode.Tag as LifeSituationGroup).Id;
            }

            using (var f = new EditLifeSituationGroupForm(parentGroupId))
            {
                TreeNode treeNode = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (treeNode == null)
                    {
                        treeNode = new TreeNode();
                        treeNode.Tag = f.Group;
                        treeNode.Checked = f.Group.IsActive;

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

                    treeNode.Text = f.Group.ToString();
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private void addLifeSituationMenuItem_Click(object sender, EventArgs e)
        {
            Guid? groupId = null;

            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                groupId = (selectedNode.Tag as LifeSituationGroup).Id;
            }

            using (var f = new EditLifeSituationForm(groupId))
            {
                TreeNode treeNode = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (treeNode == null)
                    {
                        treeNode = new TreeNode();
                        treeNode.Tag = f.LifeSituation;
                        treeNode.Checked = f.LifeSituation.IsActive;

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

                    treeNode.Text = f.LifeSituation.ToString();
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private void editGroupMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                var group = selectedNode.Tag as LifeSituationGroup;
                using (var f = new EditLifeSituationGroupForm(null, group.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        selectedNode.Text = f.Group.ToString();
                        f.Close();
                    };

                    f.ShowDialog();
                }
            }
        }

        private void editLifeSituationMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                var lifeSituation = selectedNode.Tag as LifeSituation;
                using (var f = new EditLifeSituationForm(null, lifeSituation.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        selectedNode.Text = f.LifeSituation.ToString();
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
                var group = selectedNode.Tag as LifeSituationGroup;
                if (MessageBox.Show(string.Format("Вы действительно хотите удалить [{0}] ?", group),
                    "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (var channel = ChannelManager.CreateChannel())
                    {
                        try
                        {
                            deleteGroupMenuItem.Enabled = false;

                            await taskPool.AddTask(channel.Service.DeleteGroup(group.Id));

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

        private async void deleteLifeSituationMenuItem_Click(object sender, EventArgs e)
        {
            var selectedNode = treeView.SelectedNode;
            if (selectedNode != null)
            {
                var lifeSituation = selectedNode.Tag as LifeSituation;
                if (MessageBox.Show(string.Format("Вы действительно хотите удалить [{0}] ?", lifeSituation),
                    "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    using (var channel = ChannelManager.CreateChannel())
                    {
                        try
                        {
                            deleteLifeSituationMenuItem.Enabled = false;

                            await taskPool.AddTask(channel.Service.DeleteLifeSituation(lifeSituation.Id));

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
                            deleteLifeSituationMenuItem.Enabled = true;
                        }
                    }
                }
            }
        }

        #endregion context menu

        #region tree view

        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var expandedNode = e.Node;
            if (expandedNode != null && expandedNode.Tag is LifeSituationGroup)
            {
                var group = expandedNode.Tag as LifeSituationGroup;

                if (expandedNode.Nodes.Cast<TreeNode>()
                    .Any(x => x.Tag.Equals(group)))
                {
                    loadGroup(expandedNode.Nodes, group);
                }
            }
        }

        private async void treeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            var checkedNode = e.Node;

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    if (checkedNode.Tag is LifeSituationGroup)
                    {
                        var group = checkedNode.Tag as LifeSituationGroup;
                        group.IsActive = checkedNode.Checked;
                        await taskPool.AddTask(channel.Service.EditGroup(group));
                    }

                    if (checkedNode.Tag is LifeSituation)
                    {
                        var lifeSituation = checkedNode.Tag as LifeSituation;
                        lifeSituation.IsActive = checkedNode.Checked;
                        await taskPool.AddTask(channel.Service.EditLifeSituation(lifeSituation));
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
            treeView.SelectedNode = e.Item as TreeNode;
            treeView.DoDragDrop(e.Item, DragDropEffects.All);
        }

        private async void treeView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                var draggingNode = e.Data.GetData(typeof(TreeNode)) as TreeNode;
                var hoveringNode = treeView.GetNodeAt(treeView.PointToClient(new Point(e.X, e.Y)));

                if (draggingNode.Tag is LifeSituationGroup)
                {
                    var group = draggingNode.Tag as LifeSituationGroup;

                    if (hoveringNode != null)
                    {
                        if (hoveringNode.Tag is LifeSituationGroup)
                        {
                            var targetGroup = hoveringNode.Tag as LifeSituationGroup;

                            using (var channel = ChannelManager.CreateChannel())
                            {
                                try
                                {
                                    await taskPool.AddTask(channel.Service.MoveGroup(group.Id, targetGroup.Id));
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
                    else
                    {
                        using (var channel = ChannelManager.CreateChannel())
                        {
                            try
                            {
                                await taskPool.AddTask(channel.Service.MoveGroupToRoot(group.Id));
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

                if (draggingNode.Tag is LifeSituation)
                {
                    var lifeSituation = draggingNode.Tag as LifeSituation;

                    if (hoveringNode != null)
                    {
                        if (hoveringNode.Tag is ServiceGroup)
                        {
                            var group = hoveringNode.Tag as LifeSituationGroup;

                            using (var channel = ChannelManager.CreateChannel())
                            {
                                try
                                {
                                    await taskPool.AddTask(channel.Service.MoveLifeSituation(lifeSituation.Id, group.Id));
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
            var hoveringNode = treeView.GetNodeAt(treeView.PointToClient(new Point(e.X, e.Y))) as TreeNode;
            if (draggingNode != hoveringNode)
            {
                if (draggingNode.Tag is LifeSituationGroup)
                {
                    if (hoveringNode != null)
                    {
                        if (hoveringNode.Tag is LifeSituationGroup)
                        {
                            e.Effect = DragDropEffects.Move;
                        }

                        if (hoveringNode.Tag is LifeSituation)
                        {
                            e.Effect = DragDropEffects.None;
                        }
                    }
                    else
                    {
                        e.Effect = DragDropEffects.Move;
                    }
                }

                if (draggingNode.Tag is LifeSituation)
                {
                    if (hoveringNode != null)
                    {
                        if (hoveringNode.Tag is LifeSituationGroup)
                        {
                            e.Effect = DragDropEffects.Move;
                        }

                        if (hoveringNode.Tag is LifeSituation)
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