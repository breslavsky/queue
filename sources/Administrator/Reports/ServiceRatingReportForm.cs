using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator.Reports
{
    public partial class ServiceRatingReportForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ClientService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region fields

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;

        #endregion fields

        public ServiceRatingReportForm()
            : base()
        {
            InitializeComponent();

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            int currentYear = ServerDateTime.Today.Year;
            for (int year = currentYear - 5; year <= currentYear; year++)
            {
                startYearComboBox.Items.Add(year);
            }
            startYearComboBox.SelectedIndex = startYearComboBox.Items.Count - 1;
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

        private static void ResetCheckedNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (node.Checked)
                {
                    node.Checked = false;
                }

                ResetCheckedNodes(node.Nodes);
            }
        }

        private async void createReportButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    createReportButton.Enabled = false;

                    var report = await taskPool.AddTask(channel.Service.GetServiceRatingReport(await GetReportSettings()));
                    var path = Path.GetTempPath() + Path.GetRandomFileName() + ".xls";

                    var file = File.OpenWrite(path);
                    file.Write(report, 0, report.Length);
                    file.Close();

                    Process.Start(path);
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
                    createReportButton.Enabled = true;
                }
            }
        }

        private async Task<ServiceRatingReportSettings> GetReportSettings()
        {
            ServiceRatingReportSettings settings;
            switch ((ReportDetailLevel)detailLevelTabControl.SelectedIndex)
            {
                case ReportDetailLevel.Year:
                    settings = new ServiceRatingReportSettings()
                    {
                        StartYear = (int)startYearComboBox.SelectedItem,
                        FinishYear = (int)finishYearComboBox.SelectedItem,
                    };
                    break;

                case ReportDetailLevel.Month:
                    settings = new ServiceRatingReportSettings()
                    {
                        StartYear = startMonthPicker.Value.Year,
                        StartMonth = startMonthPicker.Value.Month,
                        FinishYear = finishMonthPicker.Value.Year,
                        FinishMonth = finishMonthPicker.Value.Month
                    };
                    break;

                case ReportDetailLevel.Day:
                    settings = new ServiceRatingReportSettings()
                    {
                        StartYear = startDatePicker.Value.Year,
                        StartMonth = startDatePicker.Value.Month,
                        StartDay = startDatePicker.Value.Day,
                        FinishYear = finishDatePicker.Value.Year,
                        FinishMonth = finishDatePicker.Value.Month,
                        FinishDay = finishDatePicker.Value.Day
                    };
                    break;

                default:
                    throw new ApplicationException("Неверный тип детализации");
            }

            settings.DetailLevel = (ReportDetailLevel)detailLevelTabControl.SelectedIndex;
            settings.Services = isFullCheckBox.Checked ? new Guid[0] : await GetSelectedServices();
            settings.IsServiceTypes = isServiceTypesCheckBox.Checked;

            return settings;
        }

        private async Task<Guid[]> GetSelectedServices()
        {
            return await GetSelectedServices(servicesTreeView.Nodes);
        }

        private async Task<Guid[]> GetSelectedServices(TreeNodeCollection nodes)
        {
            List<Guid> servicesIds = new List<Guid>();
            foreach (TreeNode node in nodes)
            {
                if (node is ServiceTreeNode)
                {
                    if (node.Checked)
                    {
                        servicesIds.Add((node as ServiceTreeNode).Service.Id);
                    }
                }
                else
                {
                    var groupNode = (ServiceGroupTreeNode)node;
                    if (groupNode.IsLoaded)
                    {
                        servicesIds.AddRange(await GetSelectedServices(node.Nodes));
                    }
                    else if (groupNode.Checked)
                    {
                        servicesIds.AddRange(await groupNode.GetAllServices());
                    }
                }
            }
            return servicesIds.ToArray();
        }

        private void isFullCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            servicesTreeView.Enabled = !isFullCheckBox.Checked;
            if (!isFullCheckBox.Checked)
            {
                ResetCheckedNodes(servicesTreeView.Nodes);
            }
        }

        private async void LoadServiceGroup(TreeNodeCollection nodes)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    var serviceGroups = await taskPool.AddTask(channel.Service.GetRootServiceGroups());

                    foreach (var g in serviceGroups)
                    {
                        nodes.Add(new ServiceGroupTreeNode(this, g));
                    }

                    var services = await taskPool.AddTask(channel.Service.GetRootServices());

                    foreach (var s in services)
                    {
                        nodes.Add(new ServiceTreeNode(s));
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

        private void ServiceRatingReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private void ServiceRatingReportForm_Load(object sender, EventArgs e)
        {
            LoadServiceGroup(servicesTreeView.Nodes);

            var currentDate = ServerDateTime.Today;
            finishMonthPicker.Value = currentDate;
            finishDatePicker.Value = currentDate;
        }

        private void servicesTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            servicesTreeView.AfterCheck -= servicesTreeView_AfterCheck;

            if (e.Node is ServiceGroupTreeNode)
            {
                ((ServiceGroupTreeNode)e.Node).SetChecked(e.Node.Checked);
            }
            else
            {
                ((ServiceTreeNode)e.Node).SetChecked(e.Node.Checked);
            }

            servicesTreeView.AfterCheck += servicesTreeView_AfterCheck;
        }

        private async void servicesTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            var expandedNode = e.Node as ServiceGroupTreeNode;

            if (expandedNode == null)
            {
                return;
            }

            var serviceGroup = expandedNode.Group;
            if (!expandedNode.IsLoaded)
            {
                await expandedNode.Load();
            }
        }

        private void startYearComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var startYear = (int)startYearComboBox.SelectedItem;
            var currentYear = ServerDateTime.Today.Year;
            finishYearComboBox.Items.Clear();
            for (var year = startYear; year <= currentYear; year++)
            {
                finishYearComboBox.Items.Add(year);
            }
            finishYearComboBox.SelectedIndex = finishYearComboBox.Items.Count - 1;
            finishYearComboBox.Enabled = finishYearComboBox.Items.Count > 1;
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private class ServiceGroupTreeNode : TreeNode
        {
            private readonly ServiceRatingReportForm form;

            public ServiceGroupTreeNode(ServiceRatingReportForm form, ServiceGroup group)
            {
                this.form = form;

                Group = group;
                Text = group.ToString();
                Checked = group.IsActive;

                Nodes.Add(new TreeNode("загрузка..."));
                IsLoaded = false;
            }

            public ServiceGroup Group { get; private set; }

            public bool IsLoaded { get; private set; }

            internal void AdjustCheckState()
            {
                bool isChecked = true;

                foreach (TreeNode node in Nodes)
                {
                    if (!node.Checked)
                    {
                        isChecked = false;
                        break;
                    }
                }

                if (isChecked != Checked)
                {
                    Checked = isChecked;
                    if (Parent is ServiceGroupTreeNode)
                    {
                        (Parent as ServiceGroupTreeNode).AdjustCheckState();
                    }
                }
            }

            internal async Task<Guid[]> GetAllServices()
            {
                var result = new List<Guid>();

                using (var channel = form.channelManager.CreateChannel())
                {
                    try
                    {
                        result.AddRange(await GetGroupServices(channel, Group));
                    }
                    catch (Exception) { }
                }

                return result.ToArray();
            }

            internal async Task Load()
            {
                if (IsLoaded)
                {
                    return;
                }

                using (var channel = form.channelManager.CreateChannel())
                {
                    try
                    {
                        var serviceGroups = await form.taskPool.AddTask(channel.Service.GetServiceGroups(Group.Id));

                        foreach (var g in serviceGroups)
                        {
                            Nodes.Add(new ServiceGroupTreeNode(form, g));
                        }

                        var services = await form.taskPool.AddTask(channel.Service.GetServices(Group.Id));

                        foreach (var s in services)
                        {
                            Nodes.Add(new ServiceTreeNode(s));
                        }

                        Nodes.RemoveAt(0);
                        IsLoaded = true;

                        SetChecked(Checked);
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

            internal void SetChecked(bool value)
            {
                foreach (TreeNode node in Nodes)
                {
                    node.Checked = value;
                }
            }

            private async Task<Guid[]> GetGroupServices(Channel<IServerTcpService> channel, ServiceGroup group)
            {
                var result = new List<Guid>();

                result.AddRange((await form.taskPool.AddTask(channel.Service.GetServices(Group.Id))).Select(s => s.Id));

                foreach (var child in await form.taskPool.AddTask(channel.Service.GetServiceGroups(Group.Id)))
                {
                    result.AddRange(await GetGroupServices(channel, child));
                }

                return result.ToArray();
            }
        }

        private class ServiceTreeNode : TreeNode
        {
            public ServiceTreeNode(Service service)
                : base()
            {
                Service = service;

                Text = service.ToString();
            }

            public Service Service { get; private set; }

            internal void SetChecked(bool p)
            {
                if (Parent is ServiceGroupTreeNode)
                {
                    ((ServiceGroupTreeNode)Parent).AdjustCheckState();
                }
            }
        }
    }
}