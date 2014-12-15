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
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private Service service;

        private BindingList<ServiceParameter> parameters;

        #endregion fields

        #region properties

        public Service Service
        {
            set
            {
                service = value;
                if (service != null)
                {
                    Invoke(new MethodInvoker(async () =>
                    {
                        using (var channel = channelManager.CreateChannel())
                        {
                            try
                            {
                                parameters = new BindingList<ServiceParameter>(await taskPool.AddTask(channel.Service.GetServiceParameters(service.Id)));
                                listBox.DataSource = parameters;
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

            parameterTypeComboBox.Items.AddRange(EnumItem<ServiceParameterType>.GetItems());
        }

        public void Initialize(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
        }

        private async void addButton_Click(object sender, EventArgs e)
        {
            var serviceParameterType = (ServiceParameterType)parameterTypeComboBox.SelectedValue;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    addButton.Enabled = false;
                    //TODO: create!
                    //var parameter = await taskPool.AddTask(channel.Service.AddServiceParameter(service.Id, serviceParameterType));
                    //parameters.Add(parameter);
                    //listBox.SelectedItem = parameter;
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
                    addButton.Enabled = true;
                }
            }
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var parameter = listBox.SelectedItem as ServiceParameter;
            if (parameter != null)
            {
                parameterMinLengthUpDown.Enabled = false;
                parameterMaxLengthUpDown.Enabled = false;
                parameterOptionsTextBox.Enabled = false;
                parameterIsMultipleCheckBox.Enabled = false;

                parameterNameTextBox.Text = parameter.Name;
                parameterToolTipTextBox.Text = parameter.ToolTip;
                parameterIsRequireCheckBox.Checked = parameter.IsRequire;

                if (parameter is ServiceParameterText)
                {
                    var parameterText = parameter as ServiceParameterText;

                    parameterMinLengthUpDown.Value = parameterText.MinLength;
                    parameterMaxLengthUpDown.Value = parameterText.MaxLength;

                    parameterMinLengthUpDown.Enabled = true;
                    parameterMaxLengthUpDown.Enabled = true;
                }
                else

                    if (parameter is ServiceParameterOptions)
                    {
                        var parameterOptions = parameter as ServiceParameterOptions;

                        parameterOptionsTextBox.Text = parameterOptions.Options;
                        parameterIsMultipleCheckBox.Checked = parameterOptions.IsMultiple;

                        parameterOptionsTextBox.Enabled = true;
                        parameterIsMultipleCheckBox.Enabled = true;
                    }

                parameterGroupBox.Enabled = true;
            }
            else
            {
                parameterGroupBox.Enabled = false;
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            var parameter = listBox.SelectedItem as ServiceParameter;
            if (parameter != null)
            {
                var parameterId = parameter.Id;

                string name = parameterNameTextBox.Text;
                bool isRequire = parameterIsRequireCheckBox.Checked;
                string toolTip = parameterToolTipTextBox.Text;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        saveButton.Enabled = false;

                        if (typeof(ServiceParameterNumber).IsInstanceOfType(parameter))
                        {
                            //parameter = await taskPool.AddTask(channel.Service.EditNumberServiceParameter(parameterId, name, toolTip, isRequire));
                        }

                        if (typeof(ServiceParameterText).IsInstanceOfType(parameter))
                        {
                            int minLength = (int)parameterMinLengthUpDown.Value;
                            int maxLength = (int)parameterMaxLengthUpDown.Value;

                            //parameter = await taskPool.AddTask(channel.Service.EditTextServiceParameter(parameterId, name, toolTip, isRequire, minLength, maxLength));
                        }

                        if (typeof(ServiceParameterOptions).IsInstanceOfType(parameter))
                        {
                            string options = parameterOptionsTextBox.Text;
                            bool isMultiple = parameterIsMultipleCheckBox.Checked;

                            //parameter = await taskPool.AddTask(channel.Service.EditOptionsServiceParameter(parameterId, name, toolTip, isRequire, options, isMultiple));
                        }

                        listBox.Items[listBox.SelectedIndex] = parameter;
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
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            var parameter = listBox.SelectedItem as ServiceParameter;
            if (parameter != null)
            {
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        deleteButton.Enabled = false;

                        await taskPool.AddTask(taskPool.AddTask(channel.Service.DeleteServiceParameter(parameter.Id)));

                        parameters.Remove(parameter);
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
                        deleteButton.Enabled = true;
                    }
                }
            }
        }
    }
}