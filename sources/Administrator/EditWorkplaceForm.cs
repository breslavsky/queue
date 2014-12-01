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
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private Guid workplaceId { get; set; }

        private Workplace workplace { get; set; }

        public Workplace Workplace
        {
            get { return workplace; }
            private set
            {
                workplace = value;

                typeComboBox.SelectedItem = new EnumItem<WorkplaceType>(workplace.Type);
                numberUpDown.Value = workplace.Number;
                modificatorСomboBox.SelectedItem = new EnumItem<WorkplaceModificator>(workplace.Modificator);
                commentTextBox.Text = workplace.Comment;
                displayUpDown.Value = workplace.Display;
                segmentsUpDown.Value = workplace.Segments;
            }
        }

        public EditWorkplaceForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid workplaceId)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.workplaceId = workplaceId;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();

            InitializeComponent();

            typeComboBox.Items.AddRange(EnumItem<WorkplaceType>.GetItems());
            modificatorСomboBox.Items.AddRange(EnumItem<WorkplaceModificator>.GetItems());
        }

        private async void EditWorkplaceForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
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

        #region bindings

        private void typeComboBox_Leave(object sender, EventArgs e)
        {
            var selectedItem = typeComboBox.SelectedItem as EnumItem<WorkplaceType>;
            if (selectedItem != null)
            {
                Workplace.Type = selectedItem.Value;
            }
        }

        private void numberUpDown_Leave(object sender, EventArgs e)
        {
            Workplace.Number = (int)numberUpDown.Value;
        }

        private void modificatorСomboBox_Leave(object sender, EventArgs e)
        {
            var selectedItem = modificatorСomboBox.SelectedItem as EnumItem<WorkplaceModificator>;
            if (selectedItem != null)
            {
                Workplace.Modificator = selectedItem.Value;
            }
        }

        private void commentTextBox_Leave(object sender, EventArgs e)
        {
            Workplace.Comment = commentTextBox.Text;
        }

        private void displayUpDown_Leave(object sender, EventArgs e)
        {
            Workplace.Display = (byte)displayUpDown.Value;
        }

        private void segmentsUpDown_Leave(object sender, EventArgs e)
        {
            Workplace.Segments = (byte)segmentsUpDown.Value;
        }

        #endregion bindings

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    Workplace = await taskPool.AddTask(channel.Service.EditWorkplace(Workplace));

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

        private void EditWorkplaceForm_FormClosing(object sender, FormClosingEventArgs e)
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
    }
}