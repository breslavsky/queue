using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class EditWorkplaceForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ClientService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region events

        public event EventHandler<EventArgs> Saved;

        #endregion events

        #region fields

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;
        private readonly Guid workplaceId;
        private Workplace workplace;

        #endregion fields

        #region properties

        public Workplace Workplace
        {
            get { return workplace; }
            private set
            {
                workplace = value;

                typeControl.Select<WorkplaceType>(workplace.Type);
                numberUpDown.Value = workplace.Number;
                modificatorControl.Select<WorkplaceModificator>(workplace.Modificator);
                commentTextBox.Text = workplace.Comment;
                displayUpDown.Value = workplace.Display;
                segmentsUpDown.Value = workplace.Segments;
            }
        }

        #endregion properties

        public EditWorkplaceForm(Guid? workplaceId = null)
        {
            InitializeComponent();

            this.workplaceId = workplaceId.HasValue
                ? workplaceId.Value : Guid.Empty;

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            typeControl.Initialize<WorkplaceType>();
            modificatorControl.Initialize<WorkplaceModificator>();
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
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

        private void EditWorkplaceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void EditWorkplaceForm_Load(object sender, EventArgs e)
        {
            if (workplaceId != Guid.Empty)
            {
                Enabled = false;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        Workplace = await taskPool.AddTask(channel.Service.GetWorkplace(workplaceId));

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
                Workplace = new Workplace()
                {
                    Number = 1
                };
            }
        }

        #region bindings

        private void commentTextBox_Leave(object sender, EventArgs e)
        {
            workplace.Comment = commentTextBox.Text;
        }

        private void displayUpDown_Leave(object sender, EventArgs e)
        {
            workplace.Display = (byte)displayUpDown.Value;
        }

        private void modificatorControl_Leave(object sender, EventArgs e)
        {
            workplace.Modificator = modificatorControl.Selected<WorkplaceModificator>();
        }

        private void numberUpDown_Leave(object sender, EventArgs e)
        {
            workplace.Number = (int)numberUpDown.Value;
        }

        private void segmentsUpDown_Leave(object sender, EventArgs e)
        {
            workplace.Segments = (byte)segmentsUpDown.Value;
        }

        private void typeControl_Leave(object sender, EventArgs e)
        {
            workplace.Type = typeControl.Selected<WorkplaceType>();
        }

        #endregion bindings

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    Workplace = await taskPool.AddTask(channel.Service.EditWorkplace(workplace));

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
    }
}