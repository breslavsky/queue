using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class EditAdditionalServiceForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region events

        public event EventHandler<EventArgs> Saved;

        #endregion events

        #region fields

        private readonly Guid additionalServiceId;
        private readonly TaskPool taskPool;
        private AdditionalService additionalService;

        #endregion fields

        #region properties

        public AdditionalService AdditionalService
        {
            get { return additionalService; }
            private set
            {
                additionalService = value;
                additionalServiceBindingSource.DataSource = additionalService;
            }
        }

        #endregion properties

        public EditAdditionalServiceForm(Guid? additionalServiceId = null)
            : base()
        {
            InitializeComponent();

            this.additionalServiceId = additionalServiceId.HasValue ?
                additionalServiceId.Value : Guid.Empty;

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void EditAdditionalServiceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            taskPool.Cancel();

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

        private async void EditAdditionalServiceForm_Load(object sender, EventArgs e)
        {
            if (additionalServiceId != Guid.Empty)
            {
                Enabled = false;

                using (Channel<IServerTcpService> channel = ChannelManager.CreateChannel())
                {
                    try
                    {
                        AdditionalService = await taskPool.AddTask(channel.Service.GetAdditionalService(additionalServiceId));

                        Enabled = true;
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
            else
            {
                AdditionalService = new AdditionalService()
                {
                    Name = "Новая дополнительная услуга"
                };
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (Channel<IServerTcpService> channel = ChannelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    AdditionalService = await taskPool.AddTask(channel.Service.EditAdditionalService(additionalService));

                    if (Saved != null)
                    {
                        Saved(this, EventArgs.Empty);
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
                    saveButton.Enabled = true;
                }
            }
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