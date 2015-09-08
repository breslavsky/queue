using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Drawing;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class EditServiceGroupForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ClientService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region fields

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly Guid parenGrouptId;
        private readonly Guid serviceGroupId;
        private readonly TaskPool taskPool;
        private ServiceGroup parentGroup;
        private ServiceGroup serviceGroup;

        #endregion fields

        public EditServiceGroupForm(Guid? parentId = null, Guid? serviceGroupId = null)
            : base()
        {
            InitializeComponent();

            this.parenGrouptId = parentId.HasValue
                ? parentId.Value : Guid.Empty;
            this.serviceGroupId = serviceGroupId.HasValue
                ? serviceGroupId.Value : Guid.Empty;

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        public event EventHandler<EventArgs> Saved;

        public ServiceGroup ServiceGroup
        {
            get { return serviceGroup; }
            private set
            {
                serviceGroup = value;

                codeTextBox.Text = serviceGroup.Code;
                nameTextBox.Text = serviceGroup.Name;
                commentTextBox.Text = serviceGroup.Comment;
                descriptionTextBox.Text = serviceGroup.Description;
                columnsUpDown.Value = serviceGroup.Columns;
                rowsUpDown.Value = serviceGroup.Rows;
                if (!string.IsNullOrWhiteSpace(serviceGroup.Color))
                {
                    colorButton.BackColor = ColorTranslator.FromHtml(serviceGroup.Color);
                }
            }
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

        private void colorButton_Click(object sender, EventArgs e)
        {
            using (var d = new ColorDialog())
            {
                if (d.ShowDialog() == DialogResult.OK)
                {
                    colorButton.BackColor = d.Color;
                }
            }
        }

        private void descriptionTextBox_Click(object sender, EventArgs e)
        {
            using (var f = new HtmlEditorForm())
            {
                f.HTML = descriptionTextBox.Text;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    descriptionTextBox.Text = f.HTML;
                }
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    ServiceGroup = await taskPool.AddTask(channel.Service.EditServiceGroup(serviceGroup));

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

        private async void ServiceGroupEdit_Load(object sender, EventArgs e)
        {
            Enabled = false;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    if (parenGrouptId != Guid.Empty)
                    {
                        parentGroup = await taskPool.AddTask(channel.Service.GetServiceGroup(parenGrouptId));
                    }

                    if (serviceGroupId != Guid.Empty)
                    {
                        ServiceGroup = await taskPool.AddTask(channel.Service.GetServiceGroup(serviceGroupId));
                    }
                    else
                    {
                        ServiceGroup = new ServiceGroup()
                        {
                            IsActive = true,
                            ParentGroup = parentGroup,
                            Code = "0.0",
                            Name = "Новая группа услуг",
                            Columns = 2,
                            Rows = 5
                        };
                    }

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

        private void ServiceGroupEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        #region bindings

        private void codeTextBox_Leave(object sender, EventArgs e)
        {
            serviceGroup.Code = codeTextBox.Text;
        }

        private void colorButton_Leave(object sender, EventArgs e)
        {
            serviceGroup.Color = ColorTranslator.ToHtml(colorButton.BackColor);
        }

        private void columnsUpDown_Leave(object sender, EventArgs e)
        {
            serviceGroup.Columns = (int)columnsUpDown.Value;
        }

        private void commentTextBox_Leave(object sender, EventArgs e)
        {
            serviceGroup.Comment = commentTextBox.Text;
        }

        private void descriptionTextBox_Leave(object sender, EventArgs e)
        {
            serviceGroup.Description = descriptionTextBox.Text;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            serviceGroup.Name = nameTextBox.Text;
        }

        private void rowsUpDown_Leave(object sender, EventArgs e)
        {
            serviceGroup.Rows = (int)rowsUpDown.Value;
        }

        #endregion bindings
    }
}