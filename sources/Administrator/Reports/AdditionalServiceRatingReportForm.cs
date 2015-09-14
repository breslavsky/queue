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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator.Reports
{
    public partial class AdditionalServiceRatingReportForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ServerService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region fields

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;

        #endregion fields

        public AdditionalServiceRatingReportForm()
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

        private void AdditionalServiceRatingReportForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private void AdditionalServiceRatingReportForm_Load(object sender, EventArgs e)
        {
            LoadAdditionalServices();

            DateTime currentDate = ServerDateTime.Today;
            finishMonthPicker.Value = currentDate;
            finishDatePicker.Value = currentDate;

            int currentYear = ServerDateTime.Today.Year;
            for (int year = currentYear - 5; year <= currentYear; year++)
            {
                startYearComboBox.Items.Add(year);
            }
            startYearComboBox.SelectedIndex = startYearComboBox.Items.Count - 1;

            isFullCheckBox.Checked = true;
        }

        private async void createReportButton_Click(object sender, EventArgs e)
        {
            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    createReportButton.Enabled = false;

                    byte[] report = await taskPool.AddTask(channel.Service.GetAdditinalServicesRatingReport(GetReportSettings()));
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

        private AdditionalServicesRatingReportSettings GetReportSettings()
        {
            AdditionalServicesRatingReportSettings settings = null;
            switch ((ReportDetailLevel)detailLevelTabControl.SelectedIndex)
            {
                case ReportDetailLevel.Year:
                    settings = new AdditionalServicesRatingReportSettings()
                    {
                        StartYear = (int)startYearComboBox.SelectedItem,
                        FinishYear = (int)finishYearComboBox.SelectedItem,
                    };
                    break;

                case ReportDetailLevel.Month:
                    settings = new AdditionalServicesRatingReportSettings()
                    {
                        StartYear = startMonthPicker.Value.Year,
                        StartMonth = startMonthPicker.Value.Month,
                        FinishYear = finishMonthPicker.Value.Year,
                        FinishMonth = finishMonthPicker.Value.Month
                    };
                    break;

                case ReportDetailLevel.Day:
                    settings = new AdditionalServicesRatingReportSettings()
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
            settings.Services = isFullCheckBox.Checked ? new Guid[0] : GetSelectedAdditionalServices();

            return settings;
        }

        private Guid[] GetSelectedAdditionalServices()
        {
            return additionalServicesListBox.CheckedItems
                                            .Cast<IdentifiedEntityLink>()
                                            .Select(o => o.Id)
                                            .ToArray();
        }

        private void isFullCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            additionalServicesListBox.Enabled = !isFullCheckBox.Checked;
        }

        private async void LoadAdditionalServices()
        {
            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    foreach (IdentifiedEntity op in await channel.Service.GetAdditionalServiceLinks())
                    {
                        additionalServicesListBox.Items.Add(op, true);
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

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }
    }
}