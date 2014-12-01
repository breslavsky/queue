using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Handlers;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class MediaConfigControl : UserControl
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private MediaConfig config;

        #endregion fields

        #region properties

        public MediaConfig Config
        {
            set
            {
                config = value;
                if (config != null)
                {
                    serviceUrlTextBox.Text = config.ServiceUrl;
                    tickerTextBox.Text = config.Ticker;
                    tickerSpeedTrackBar.Value = config.TickerSpeed;

                    foreach (var f in config.MediaFiles)
                    {
                        int index = mediaFilesGridView.Rows.Add();
                        var row = mediaFilesGridView.Rows[index];

                        row.Cells["nameColumn"].Value = f.Name;
                        row.Tag = f;
                    }
                }
            }
        }

        #endregion properties

        public MediaConfigControl()
        {
            InitializeComponent();
        }

        public void Initialize(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private void serviceUrlTextBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.ServiceUrl = serviceUrlTextBox.Text;
            }
        }

        private void tickerTextBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.Ticker = tickerTextBox.Text;
            }
        }

        private void tickerSpeedTrackBar_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.TickerSpeed = tickerSpeedTrackBar.Value;
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    await taskPool.AddTask(channel.Service.EditMediaConfig(config));
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

        private async void mediaFilesGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = e.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = mediaFilesGridView.Rows[rowIndex];
                    var mediaConfigFile = (MediaConfigFile)row.Tag;

                    string name = row.Cells["nameColumn"].Value as string;
                    if (name != null)
                    {
                        mediaConfigFile.Name = name;
                    }

                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                            await taskPool.AddTask(channel.Service.EditMediaConfigFile(mediaConfigFile));
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

        private async void mediaFilesGridView_CellClick(object sender, DataGridViewCellEventArgs eventArgs)
        {
            int rowIndex = eventArgs.RowIndex;
            if (rowIndex >= 0)
            {
                int columnIndex = eventArgs.ColumnIndex;
                if (columnIndex >= 0)
                {
                    var row = mediaFilesGridView.Rows[rowIndex];
                    var cell = row.Cells[columnIndex];

                    MediaConfigFile mediaConfigFile = (MediaConfigFile)row.Tag;

                    switch (cell.OwningColumn.Name)
                    {
                        case "uploadColumn":
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

                                        Uri uri = new Uri(string.Format("{0}/media-config/files/{1}/upload", serviceUrlTextBox.Text, mediaConfigFile.Id));

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
                            break;

                        case "deleteColumn":

                            if (MessageBox.Show("Вы действительно хотите удалить файл?",
                                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                            {
                                using (var channel = channelManager.CreateChannel())
                                {
                                    try
                                    {
                                        await channel.Service.OpenUserSession(currentUser.SessionId);
                                        await channel.Service.DeleteMediaConfigFile(mediaConfigFile.Id);

                                        mediaFilesGridView.Rows.Remove(row);
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
                            break;
                    }
                }
            }
        }

        private async void addMediaFileButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    addMediaFileButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    MediaConfigFile mediaConfigFile = await taskPool.AddTask(channel.Service.AddMediaConfigFile());

                    int index = mediaFilesGridView.Rows.Add();
                    var row = mediaFilesGridView.Rows[index];

                    row.Cells["nameColumn"].Value = mediaConfigFile.Name;
                    row.Tag = mediaConfigFile;
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
                    addMediaFileButton.Enabled = true;
                }
            }
        }
    }
}