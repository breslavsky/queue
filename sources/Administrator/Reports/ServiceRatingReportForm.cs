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

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();

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

        private void ServiceRatingReportForm_Load(object sender, EventArgs e)
        {
            loadServiceGroup(servicesTreeView.Nodes);

            DateTime currentDate = ServerDateTime.Today;
            finishMonthPicker.Value = currentDate;
            finishDatePicker.Value = currentDate;
            targetDatePicker.Value = currentDate;
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

        private void servicesTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode checkedNode = e.Node;
            if (checkedNode != null)
            {
                if (typeof(ServiceGroup).IsInstanceOfType(checkedNode.Tag))
                {
                    foreach (TreeNode node in checkedNode.Nodes)
                    {
                        node.Checked = checkedNode.Checked;
                    }
                }
            }
        }

        private List<Guid> getSelectedServices()
        {
            return getSelectedServices(servicesTreeView.Nodes);
        }

        private List<Guid> getSelectedServices(TreeNodeCollection nodes)
        {
            List<Guid> servicesIds = new List<Guid>();
            foreach (TreeNode node in nodes)
            {
                if (node.Checked && typeof(Service).IsInstanceOfType(node.Tag))
                {
                    servicesIds.Add(((Service)node.Tag).Id);
                }

                servicesIds.AddRange(getSelectedServices(node.Nodes));
            }
            return servicesIds;
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

                        case ReportDetailLevel.Hour:

                            settings = new ServiceRatingReportSettings()
                            {
                                TargetDate = targetDatePicker.Value,
                                IsServiceTypes = isServiceTypesCheckBox.Checked
                            };
                            break;

                        default:
                            throw new Exception("Не верный тип детализации");
                    }

                    List<Guid> servicesIds = isFullCheckBox.Checked
                        ? new List<Guid>()
                        : getSelectedServices();

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    byte[] report = await taskPool.AddTask(channel.Service.GetServiceRatingReport(servicesIds.ToArray(), detailLevel, settings));
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
    }
}