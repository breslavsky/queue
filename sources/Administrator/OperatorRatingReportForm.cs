using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Common;
using Queue.Services.Contracts;
using System;
using System.ServiceModel;
using DTO = Queue.Services.DTO;

namespace Queue.Administrator
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
            targetDatePicker.Value = currentDate;

            int currentYear = ServerDateTime.Today.Year;
            for (int year = currentYear - 5; year <= currentYear; year++)
            {
                startYearComboBox.Items.Add(year);
            }
            startYearComboBox.SelectedIndex = startYearComboBox.Items.Count - 1;
        }

        private async void LoadOperators()
        {
            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    await channel.Service.OpenUserSession(currentUser.SessionId);
                    DTO.Operator[] operators = await channel.Service.GetOperators();
                    int y = 9;
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
}