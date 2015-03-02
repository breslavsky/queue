using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.Administrator.Reports
{
    public partial class ServiceRatingReportForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        public ServiceRatingReportForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
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

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void ServiceRatingReportForm_Load(object sender, EventArgs e)
        {
            LoadServiceGroup(servicesTreeView.Nodes);

            DateTime currentDate = ServerDateTime.Today;
            finishMonthPicker.Value = currentDate;
            finishDatePicker.Value = currentDate;
        }

        private void startYearComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int startYear = (int)startYearComboBox.SelectedItem;
            int currentYear = ServerDateTime.Today.Year;
            finishYearComboBox.Items.Clear();
            for (int year = startYear; year <= currentYear; year++)
            {
                finishYearComboBox.Items.Add(year);
            }
            finishYearComboBox.SelectedIndex = finishYearComboBox.Items.Count - 1;
            finishYearComboBox.Enabled = finishYearComboBox.Items.Count > 1;
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

        private void servicesTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            servicesTreeView.AfterCheck -= servicesTreeView_AfterCheck;

            if (e.Node is ServiceGroupTreeNode)
            {
                (e.Node as ServiceGroupTreeNode).SetChecked(e.Node.Checked);
            }
            else
            {
                (e.Node as ServiceTreeNode).SetChecked(e.Node.Checked);
            }

            servicesTreeView.AfterCheck += servicesTreeView_AfterCheck;
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
                if (!node.Checked)
                {
                    continue;
                }

                if (node is ServiceTreeNode)
                {
                    servicesIds.Add((node as ServiceTreeNode).Service.Id);
                }
                else
                {
                    ServiceGroupTreeNode groupNode = node as ServiceGroupTreeNode;

                    servicesIds.AddRange(groupNode.IsLoaded ?
                        await GetSelectedServices(node.Nodes) :
                        await groupNode.GetAllServices());
                }
            }
            return servicesIds.ToArray();
        }

        private void isFullCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            servicesTreeView.Enabled = !isFullCheckBox.Checked;
        }

        private async void createReportButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    createReportButton.Enabled = false;

                    ServiceRatingReportSettings settings;
                    DateTime startDate, finishDate;

                    ReportDetailLevel detailLevel = (ReportDetailLevel)detailLevelTabControl.SelectedIndex;
                    switch (detailLevel)
                    {
                        case ReportDetailLevel.Year:

                            int startYear = (int)startYearComboBox.SelectedItem;
                            int finishYear = (int)finishYearComboBox.SelectedItem;

                            settings = new ServiceRatingReportSettings()
                            {
                                StartYear = startYear,
                                FinishYear = finishYear,
                                IsServiceTypes = isServiceTypesCheckBox.Checked
                            };
                            break;

                        case ReportDetailLevel.Month:

                            startDate = startMonthPicker.Value;
                            finishDate = finishMonthPicker.Value;

                            settings = new ServiceRatingReportSettings()
                            {
                                StartYear = startDate.Year,
                                StartMonth = startDate.Month,
                                FinishYear = finishDate.Year,
                                FinishMonth = finishDate.Month,
                                IsServiceTypes = isServiceTypesCheckBox.Checked
                            };
                            break;

                        case ReportDetailLevel.Day:

                            startDate = startDatePicker.Value;
                            finishDate = finishDatePicker.Value;

                            settings = new ServiceRatingReportSettings()
                            {
                                StartYear = startDate.Year,
                                StartMonth = startDate.Month,
                                FinishYear = finishDate.Year,
                                FinishMonth = finishDate.Month,
                                StartDay = startDate.Day,
                                FinishDay = finishDate.Day,
                                IsServiceTypes = isServiceTypesCheckBox.Checked
                            };
                            break;

                        default:
                            throw new Exception("Неверный тип детализации");
                    }

                    Guid[] servicesIds = isFullCheckBox.Checked ? new Guid[0] : await GetSelectedServices();

                    byte[] report = await taskPool.AddTask(channel.Service.GetServiceRatingReport(servicesIds, detailLevel, settings));
                    string path = Path.GetTempPath() + Path.GetRandomFileName() + ".xls";

                    FileStream file = File.OpenWrite(path);
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

        private void ServiceRatingReportForm_FormClosing(object sender, FormClosingEventArgs e)
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

        private class ServiceGroupTreeNode : TreeNode
        {
            private ServiceRatingReportForm form;

            public ServiceGroup Group { get; private set; }

            public bool IsLoaded { get; set; }

            public ServiceGroupTreeNode(ServiceRatingReportForm form, ServiceGroup group)
            {
                this.form = form;

                Group = group;
                Text = group.ToString();
                Checked = group.IsActive;

                Nodes.Add(new TreeNode("загрузка..."));
                IsLoaded = false;
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

            internal async Task<Guid[]> GetAllServices()
            {
                List<Guid> result = new List<Guid>();

                using (Channel<IServerTcpService> channel = form.channelManager.CreateChannel())
                {
                    try
                    {
                        result.AddRange(await GetGroupServices(channel, Group));
                    }
                    catch (Exception) { }
                }

                return result.ToArray();
            }

            private async Task<Guid[]> GetGroupServices(Channel<IServerTcpService> channel, ServiceGroup group)
            {
                List<Guid> result = new List<Guid>();

                result.AddRange((await form.taskPool.AddTask(channel.Service.GetServices(Group.Id))).Select(s => s.Id));

                foreach (ServiceGroup child in await form.taskPool.AddTask(channel.Service.GetServiceGroups(Group.Id)))
                {
                    result.AddRange(await GetGroupServices(channel, child));
                }

                return result.ToArray();
            }

            internal void SetChecked(bool value)
            {
                foreach (TreeNode node in Nodes)
                {
                    node.Checked = value;
                }
            }

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
        }

        private class ServiceTreeNode : TreeNode
        {
            public Service Service { get; private set; }

            public ServiceTreeNode(Service service)
                : base()
            {
                Service = service;

                Text = service.ToString();
            }

            internal void SetChecked(bool p)
            {
                if (Parent is ServiceGroupTreeNode)
                {
                    (Parent as ServiceGroupTreeNode).AdjustCheckState();
                }
            }
        }
    }
}