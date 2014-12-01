using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceModel;
using DTO = Queue.Services.DTO;

namespace Queue.Administrator.Reports
{
    public partial class OperatorRatingReportForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private DTO.User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        public OperatorRatingReportForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, DTO.User currentUser)
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

        private void OperatorRatingReportForm_Load(object sender, System.EventArgs e)
        {
            LoadOperators();

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

        private async void LoadOperators()
        {
            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    await channel.Service.OpenUserSession(currentUser.SessionId);

                    foreach (DTO.Operator op in await channel.Service.GetOperators())
                    {
                        operatorsListBox.Items.Add(op, true);
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

        private void isFullCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            operatorsListBox.Enabled = !isFullCheckBox.Checked;
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

        private async void createReportButton_Click(object sender, EventArgs e)
        {
            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    createReportButton.Enabled = false;

                    OperatorRatingReportSettings settings = GetReportSettings();
                    ReportDetailLevel detailLevel = (ReportDetailLevel)detailLevelTabControl.SelectedIndex;

                    Guid[] operators = isFullCheckBox.Checked ? new Guid[0] : GetSelectedOperators();

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    byte[] report = await taskPool.AddTask(channel.Service.GetOperatorRatingReport(operators, detailLevel, settings));
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

        private OperatorRatingReportSettings GetReportSettings()
        {
            switch ((ReportDetailLevel)detailLevelTabControl.SelectedIndex)
            {
                case ReportDetailLevel.Year:
                    return new OperatorRatingReportSettings()
                    {
                        StartYear = (int)startYearComboBox.SelectedItem,
                        FinishYear = (int)finishYearComboBox.SelectedItem,
                    };

                case ReportDetailLevel.Month:
                    return new OperatorRatingReportSettings()
                    {
                        StartYear = startMonthPicker.Value.Year,
                        StartMonth = startMonthPicker.Value.Month,
                        FinishYear = finishMonthPicker.Value.Year,
                        FinishMonth = finishMonthPicker.Value.Month
                    };

                case ReportDetailLevel.Day:
                    return new OperatorRatingReportSettings()
                    {
                        StartYear = startDatePicker.Value.Year,
                        StartMonth = startDatePicker.Value.Month,
                        StartDay = startDatePicker.Value.Day,
                        FinishYear = finishDatePicker.Value.Year,
                        FinishMonth = finishDatePicker.Value.Month,
                        FinishDay = finishDatePicker.Value.Day
                    };

                default:
                    throw new ApplicationException("Неверный тип детализации");
            }
        }

        private Guid[] GetSelectedOperators()
        {
            return operatorsListBox.CheckedItems
                                        .Cast<DTO.Operator>()
                                        .Select(o => o.Id)
                                        .ToArray();
        }
    }
}