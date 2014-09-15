using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class ServiceGroupEditForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        private ServiceGroup serviceGroup;

        public ServiceGroup ServiceGroup
        {
            get { return serviceGroup; }
        }

        public ServiceGroupEditForm(DuplexChannelBuilder<IServerService> channelBuilder, User currentUser, ServiceGroup serviceGroup)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.serviceGroup = serviceGroup;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private void ServiceGroupEdit_Load(object sender, EventArgs e)
        {
            codeTextBox.Text = ServiceGroup.Code;
            nameTextBox.Text = ServiceGroup.Name;
            commentTextBox.Text = ServiceGroup.Comment;
            descriptionTextBox.Text = ServiceGroup.Description;
            columnsUpDown.Value = ServiceGroup.Columns;
            rowsUpDown.Value = ServiceGroup.Rows;
            if (!string.IsNullOrWhiteSpace(ServiceGroup.Color))
            {
                colorPanel.BackColor = ColorTranslator.FromHtml(ServiceGroup.Color);
            }

            //TODO: create!
            byte[] icon = new byte[] { };
            if (icon.Length > 0)
            {
                iconImageBox.Image = Image.FromStream(new MemoryStream(icon));
            }
        }

        private void iconImageBox_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog()
            {
                InitialDirectory = Application.StartupPath
            };
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = fileDialog.FileName.ToString();
                iconImageBox.Image = Image.FromStream(new MemoryStream(File.ReadAllBytes(filePath)));
            }
        }

        private void codeTextBox_Leave(object sender, EventArgs e)
        {
            serviceGroup.Code = codeTextBox.Text;
        }

        private void colorPanel_Leave(object sender, EventArgs e)
        {
            serviceGroup.Color = ColorTranslator.ToHtml(colorPanel.BackColor);
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            var d = new ColorDialog();
            if (d.ShowDialog() == DialogResult.OK)
            {
                colorPanel.BackColor = d.Color;
            }
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            serviceGroup.Name = nameTextBox.Text;
        }

        private void commentTextBox_Leave(object sender, EventArgs e)
        {
            serviceGroup.Comment = commentTextBox.Text;
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

        private void descriptionTextBox_Leave(object sender, EventArgs e)
        {
            serviceGroup.Description = descriptionTextBox.Text;
        }

        private void columnsUpDown_Leave(object sender, EventArgs e)
        {
            serviceGroup.Columns = (int)columnsUpDown.Value;
        }

        private void rowsUpDown_Leave(object sender, EventArgs e)
        {
            serviceGroup.Rows = (int)rowsUpDown.Value;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            var memoryStream = new MemoryStream();
            if (iconImageBox.Image != null)
            {
                iconImageBox.Image.Save(memoryStream, ImageFormat.Png);
            }

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    serviceGroup = await taskPool.AddTask(channel.Service.EditServiceGroup(serviceGroup));

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

        private void ServiceGroupEditForm_FormClosing(object sender, FormClosingEventArgs e)
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