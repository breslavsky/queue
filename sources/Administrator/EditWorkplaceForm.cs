using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class EditWorkplaceForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        public EditWorkplaceForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid? workplaceId = null)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.workplaceId = workplaceId.HasValue
                ? workplaceId.Value : Guid.Empty;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();

            InitializeComponent();

            typeControl.Initialize<WorkplaceType>();
            modificatorControl.Initialize<WorkplaceModificator>();
        }

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

        private Workplace workplace { get; set; }

        private Guid workplaceId { get; set; }

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
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        Workplace = await taskPool.AddTask(channel.Service.GetWorkplace(workplaceId));
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
                Workplace = new Workplace();
            }
        }

        #region bindings

        private void commentTextBox_Leave(object sender, EventArgs e)
        {
            Workplace.Comment = commentTextBox.Text;
        }

        private void displayUpDown_Leave(object sender, EventArgs e)
        {
            Workplace.Display = (byte)displayUpDown.Value;
        }

        private void modificatorControl_Leave(object sender, EventArgs e)
        {
            Workplace.Modificator = modificatorControl.Selected<WorkplaceModificator>();
        }

        private void numberUpDown_Leave(object sender, EventArgs e)
        {
            Workplace.Number = (int)numberUpDown.Value;
        }

        private void segmentsUpDown_Leave(object sender, EventArgs e)
        {
            Workplace.Segments = (byte)segmentsUpDown.Value;
        }

        private void typeControl_Leave(object sender, EventArgs e)
        {
            Workplace.Type = typeControl.Selected<WorkplaceType>();
        }

        #endregion bindings

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.EditWorkplace(workplace));

                    DialogResult = DialogResult.OK;
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