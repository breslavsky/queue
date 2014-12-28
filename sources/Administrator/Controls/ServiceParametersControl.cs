using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class ServiceParametersControl : UserControl
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private Service service;
        private TaskPool taskPool;

        public Service Service
        {
            set
            {
                parametersGridView.Rows.Clear();

                service = value;
                if (service != null)
                {
                    Invoke(new MethodInvoker(async () =>
                    {
                        using (var channel = channelManager.CreateChannel())
                        {
                            try
                            {
                                foreach (var p in await taskPool.AddTask(channel.Service.GetServiceParameters(service.Id)))
                                {
                                    RenderParametersGridViewRow(parametersGridView.Rows[parametersGridView.Rows.Add()], p);
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
                    }));
                }
            }
        }

        #endregion fields

        public ServiceParametersControl()
        {
            InitializeComponent();

            parameterTypeControl.Initialize<ServiceParameterType>();
        }

        public void Initialize(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

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

        private void addButton_Click(object sender, EventArgs e)
        {
            DataGridViewRow row = null;

            switch (parameterTypeControl.Selected<ServiceParameterType>())
            {
                case ServiceParameterType.Number:
                    using (var f = new EditServiceParameterNumberForm(channelBuilder, currentUser, service.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            if (row == null)
                            {
                                row = parametersGridView.Rows[parametersGridView.Rows.Add()];
                            }
                            RenderParametersGridViewRow(row, f.ServiceParameterNumber);
                        };

                        f.ShowDialog();
                    }
                    break;

                case ServiceParameterType.Text:
                    using (var f = new EditServiceParameterTextForm(channelBuilder, currentUser, service.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            if (row == null)
                            {
                                row = parametersGridView.Rows[parametersGridView.Rows.Add()];
                            }
                            RenderParametersGridViewRow(row, f.ServiceParameterText);
                        };

                        f.ShowDialog();
                    }
                    break;

                case ServiceParameterType.Options:
                    using (var f = new EditServiceParameterOptionsForm(channelBuilder, currentUser, service.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            if (row == null)
                            {
                                row = parametersGridView.Rows[parametersGridView.Rows.Add()];
                            }
                            RenderParametersGridViewRow(row, f.ServiceParameterOptions);
                        };

                        f.ShowDialog();
                    }
                    break;
            }
        }

        private void parametersGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int rowIndex = e.RowIndex,
                columnIndex = e.ColumnIndex;

            if (rowIndex >= 0 && columnIndex >= 0)
            {
                var row = parametersGridView.Rows[rowIndex];
                ServiceParameter parameter = row.Tag as ServiceParameter;

                if (parameter is ServiceParameterNumber)
                {
                    using (var f = new EditServiceParameterNumberForm(channelBuilder, currentUser, null, parameter.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            RenderParametersGridViewRow(row, f.ServiceParameterNumber);
                        };

                        f.ShowDialog();
                    }
                }
                else if (parameter is ServiceParameterText)
                {
                    using (var f = new EditServiceParameterTextForm(channelBuilder, currentUser, null, parameter.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            RenderParametersGridViewRow(row, f.ServiceParameterText);
                        };

                        f.ShowDialog();
                    }
                }
                else if (parameter is ServiceParameterOptions)
                {
                    using (var f = new EditServiceParameterOptionsForm(channelBuilder, currentUser, null, parameter.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            RenderParametersGridViewRow(row, f.ServiceParameterOptions);
                        };

                        f.ShowDialog();
                    }
                }
            }
        }

        private async void parametersGridView_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Вы действительно хотите удалить параметр?",
                "Подтвердите удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                ServiceParameter parameter = e.Row.Tag as ServiceParameter;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await taskPool.AddTask(channel.Service.DeleteServiceParameter(parameter.Id));
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

        private void RenderParametersGridViewRow(DataGridViewRow row, ServiceParameter parameter)
        {
            row.Cells["nameColumn"].Value = parameter.Name;
            row.Cells["toolTipColumn"].Value = parameter.ToolTip;
            row.Cells["isRequireColumn"].Value = parameter.IsRequire;
            row.Tag = parameter;
        }
    }
}