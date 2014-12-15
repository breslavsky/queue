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

namespace Queue.Administrator
{
    public partial class MediaConfigControl : UserControl
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private MediaConfig config;
        private User currentUser;
        private TaskPool taskPool;

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
                        var row = mediaConfigFilesGridView.Rows[mediaConfigFilesGridView.Rows.Add()];
                        MediaConfigFilesGridViewRow(row, f);
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

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
        }

        private void addMediaConfigFileButton_Click(object sender, EventArgs e)
        {
            using (var f = new EditMediaConfigFileForm(channelBuilder, currentUser))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var row = mediaConfigFilesGridView.Rows[mediaConfigFilesGridView.Rows.Add()];
                    MediaConfigFilesGridViewRow(row, f.MediaConfigFile);
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

                using (var f = new EditMediaConfigFileForm(channelBuilder, currentUser, mediaConfigFile.Id))
                {
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        MediaConfigFilesGridViewRow(row, f.MediaConfigFile);
                    }
                }
            }
        }

        private async void mediaConfigFilesGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить файл?",
                                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MediaConfigFile mediaConfigFile = e.Row.Tag as MediaConfigFile;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await channel.Service.DeleteMediaConfigFile(mediaConfigFile.Id);
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
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

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

        private void serviceUrlTextBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.ServiceUrl = serviceUrlTextBox.Text;
            }
        }

        private void tickerSpeedTrackBar_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.TickerSpeed = tickerSpeedTrackBar.Value;
            }
        }

        private void tickerTextBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.Ticker = tickerTextBox.Text;
            }
        }
    }
}