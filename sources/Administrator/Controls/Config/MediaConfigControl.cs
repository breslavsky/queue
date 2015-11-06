using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class MediaConfigControl : DependencyUserControl
    {
        #region dependency

        [Dependency]
        [ReadOnly(true)]
        [Browsable(false)]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region fields

        private readonly TaskPool taskPool;
        private MediaConfig config;

        #endregion fields

        #region properties

        public MediaConfig Config
        {
            get
            {
                return config;
            }
            private set
            {
                Invoke(new MethodInvoker(async () =>
                {
                    config = value;
                    if (config != null)
                    {
                        serviceUrlTextBox.Text = config.ServiceUrl;
                        tickerTextBox.Text = config.Ticker;
                        tickerSpeedTrackBar.Value = config.TickerSpeed;

                        mediaConfigFilesGridView.Rows.Clear();

                        using (var channel = ChannelManager.CreateChannel())
                        {
                            try
                            {
                                foreach (var f in await taskPool.AddTask(channel.Service.GetMediaConfigFiles()))
                                {
                                    var row = mediaConfigFilesGridView.Rows[mediaConfigFilesGridView.Rows.Add()];
                                    MediaConfigFilesGridViewRow(row, f);
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
                        }
                    }
                }));
            }
        }

        #endregion properties

        public MediaConfigControl()
        {
            InitializeComponent();

            config = new MediaConfig();

            if (designtime)
            {
                return;
            }

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void addMediaConfigFileButton_Click(object sender, EventArgs e)
        {
            using (var f = new EditMediaConfigFileForm())
            {
                DataGridViewRow row = null;

                f.Saved += (s, eventArgs) =>
                {
                    if (row == null)
                    {
                        row = mediaConfigFilesGridView.Rows[mediaConfigFilesGridView.Rows.Add()];
                    }
                    MediaConfigFilesGridViewRow(row, f.MediaConfigFile);
                    f.Close();
                };

                f.ShowDialog();
            }
        }

        private async void MediaConfigControl_Load(object sender, EventArgs e)
        {
            if (designtime)
            {
                return;
            }
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    Config = await taskPool.AddTask(channel.Service.GetMediaConfig());
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

        private void mediaConfigFilesGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex,
                columnIndex = e.ColumnIndex;

            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var row = mediaConfigFilesGridView.Rows[rowIndex];
                MediaConfigFile mediaConfigFile = row.Tag as MediaConfigFile;

                using (var f = new EditMediaConfigFileForm(mediaConfigFile.Id))
                {
                    f.Saved += (s, eventArgs) =>
                    {
                        MediaConfigFilesGridViewRow(row, f.MediaConfigFile);
                        f.Close();
                    };

                    f.ShowDialog();
                }
            }
        }

        private async void mediaConfigFilesGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить файл?",
                                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MediaConfigFile mediaConfigFile = e.Row.Tag as MediaConfigFile;

                using (var channel = ChannelManager.CreateChannel())
                {
                    try
                    {
                        await taskPool.AddTask(channel.Service.DeleteMediaConfigFile(mediaConfigFile.Id));
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
                e.Cancel = true;
            }
        }

        private void MediaConfigFilesGridViewRow(DataGridViewRow row, MediaConfigFile mediaConfigFile)
        {
            row.Cells["nameColumn"].Value = mediaConfigFile.Name;
            row.Tag = mediaConfigFile;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    config = await taskPool.AddTask(channel.Service.EditMediaConfig(config));
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

        private void serviceUrlTextBox_Leave(object sender, EventArgs e)
        {
            config.ServiceUrl = serviceUrlTextBox.Text;
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void tickerSpeedTrackBar_Leave(object sender, EventArgs e)
        {
            config.TickerSpeed = tickerSpeedTrackBar.Value;
        }

        private void tickerTextBox_Leave(object sender, EventArgs e)
        {
            config.Ticker = tickerTextBox.Text;
        }
    }
}