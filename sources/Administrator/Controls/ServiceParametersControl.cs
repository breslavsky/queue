using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class ServiceParametersControl : DependencyUserControl
    {
        #region dependency

        [Dependency]
        [ReadOnly(true)]
        [Browsable(false)]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region fields

        private readonly TaskPool taskPool;
        private Service service;

        #endregion fields

        #region properties

        public Service Service
        {
            get
            {
                return service;
            }
            set
            {
                parametersGridView.Rows.Clear();

                service = value;
                if (service != null)
                {
                    Invoke(new MethodInvoker(async () =>
                    {
                        using (var channel = ChannelManager.CreateChannel())
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

        #endregion properties

        public ServiceParametersControl()
        {
            InitializeComponent();

            if (designtime)
            {
                return;
            }

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            parameterTypeControl.Initialize<ServiceParameterType>();
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
                    using (var f = new EditServiceParameterNumberForm(service.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            if (row == null)
                            {
                                row = parametersGridView.Rows[parametersGridView.Rows.Add()];
                            }
                            RenderParametersGridViewRow(row, f.ServiceParameterNumber);
                            f.Close();
                        };

                        f.ShowDialog();
                    }
                    break;

                case ServiceParameterType.Text:
                    using (var f = new EditServiceParameterTextForm(service.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            if (row == null)
                            {
                                row = parametersGridView.Rows[parametersGridView.Rows.Add()];
                            }
                            RenderParametersGridViewRow(row, f.ServiceParameterText);
                            f.Close();
                        };

                        f.ShowDialog();
                    }
                    break;

                case ServiceParameterType.Options:
                    using (var f = new EditServiceParameterOptionsForm(service.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            if (row == null)
                            {
                                row = parametersGridView.Rows[parametersGridView.Rows.Add()];
                            }
                            RenderParametersGridViewRow(row, f.ServiceParameterOptions);
                            f.Close();
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
                    using (var f = new EditServiceParameterNumberForm(null, parameter.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            RenderParametersGridViewRow(row, f.ServiceParameterNumber);
                            f.Close();
                        };

                        f.ShowDialog();
                    }
                }
                else if (parameter is ServiceParameterText)
                {
                    using (var f = new EditServiceParameterTextForm(null, parameter.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            RenderParametersGridViewRow(row, f.ServiceParameterText);
                            f.Close();
                        };

                        f.ShowDialog();
                    }
                }
                else if (parameter is ServiceParameterOptions)
                {
                    using (var f = new EditServiceParameterOptionsForm(null, parameter.Id))
                    {
                        f.Saved += (s, eventArgs) =>
                        {
                            RenderParametersGridViewRow(row, f.ServiceParameterOptions);
                            f.Close();
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

                using (var channel = ChannelManager.CreateChannel())
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