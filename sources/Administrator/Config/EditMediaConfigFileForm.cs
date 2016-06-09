using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.ServiceModel;
using System.Threading;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class EditMediaConfigFileForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region events

        public event EventHandler<EventArgs> Saved;

        #endregion events

        #region fields

        private readonly Guid mediaConfigFileId;
        private readonly TaskPool taskPool;
        private MediaConfig mediaConfig;
        private MediaConfigFile mediaConfigFile;

        #endregion fields

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

        public EditMediaConfigFileForm(Guid? mediaConfigFileId = null)
            : base()
        {
            InitializeComponent();

            this.mediaConfigFileId = mediaConfigFileId.HasValue
                ? mediaConfigFileId.Value : Guid.Empty;

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
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
                if (ChannelManager != null)
                {
                    ChannelManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private async void EditMediaConfigFileForm_Load(object sender, EventArgs e)
        {
            Enabled = false;

            using (var channel = ChannelManager.CreateChannel())
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
            using (var channel = ChannelManager.CreateChannel())
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

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private async void uploadButton_Click(object sender, EventArgs eventArgs)
        {
            if (selectMediaFileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = selectMediaFileDialog.FileName;

                using (var channel = ChannelManager.CreateChannel())
                {
                    try
                    {
                        if (MediaConfigFile.Id == Guid.Empty)
                        {
                            MediaConfigFile = await taskPool.AddTask(channel.Service.EditMediaConfigFile(MediaConfigFile));
                        }

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