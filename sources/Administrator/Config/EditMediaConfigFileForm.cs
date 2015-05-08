using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class EditMediaConfigFileForm : RichForm
    {
        public event EventHandler<EventArgs> Saved;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private MediaConfig mediaConfig;
        private Guid mediaConfigFileId;
        private MediaConfigFile mediaConfigFile;
        private TaskPool taskPool;

        #region properties

        public MediaConfigFile MediaConfigFile
        {
            get { return mediaConfigFile; }
            private set
            {
                mediaConfigFile = value;

                nameTextBox.Text = mediaConfigFile.Name;
            }
        }

        #endregion properties

        public EditMediaConfigFileForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid? mediaConfigFileId = null)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.mediaConfigFileId = mediaConfigFileId.HasValue
                ? mediaConfigFileId.Value : Guid.Empty;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
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

        private async void EditMediaConfigFileForm_Load(object sender, EventArgs e)
        {
            Enabled = false;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    mediaConfig = await taskPool.AddTask(channel.Service.GetMediaConfig());

                    if (mediaConfigFileId != Guid.Empty)
                    {
                        MediaConfigFile = await taskPool.AddTask(channel.Service.GetMediaConfigFile(mediaConfigFileId));
                    }
                    else
                    {
                        MediaConfigFile = new MediaConfigFile();
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

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            mediaConfigFile.Name = nameTextBox.Text;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    MediaConfigFile = await taskPool.AddTask(channel.Service.EditMediaConfigFile(mediaConfigFile));

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

        private async void uploadButton_Click(object sender, EventArgs eventArgs)
        {
            if (selectMediaFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = selectMediaFileDialog.FileName;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        CancellationTokenSource uploadOperation = new CancellationTokenSource();

                        var progress = new ProgressMessageHandler();
                        progress.HttpSendProgress += new EventHandler<HttpProgressEventArgs>((s, e) =>
                        {
                            try
                            {
                                Invoke(new MethodInvoker(() =>
                                {
                                    uploadMediaFileProgressBar.Value = e.ProgressPercentage;
                                }));
                            }
                            catch (ObjectDisposedException)
                            {
                                uploadOperation.Cancel();
                            }
                        });

                        Uri uri = new Uri(string.Format("{0}/media-config/files/{1}/upload", mediaConfig.ServiceUrl, mediaConfigFile.Id));

                        var message = new HttpRequestMessage()
                        {
                            Method = HttpMethod.Post,
                            Content = new StreamContent(File.OpenRead(selectMediaFileDialog.FileName)),
                            RequestUri = uri
                        };

                        var client = HttpClientFactory.Create(progress);

                        await taskPool.AddTask(client.SendAsync(message, uploadOperation.Token));
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
}