using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class EditClientForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private Client client;
        private Guid clientId;
        private User currentUser;
        private TaskPool taskPool;

        public EditClientForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid clientId)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.clientId = clientId;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
        }

        public Client Client
        {
            get { return client; }
            private set
            {
                client = value;

                surnameTextBox.Text = client.Surname;
                nameTextBox.Text = client.Name;
                patronymicTextBox.Text = client.Patronymic;
                emailTextBox.Text = client.Email;
                mobileTextBox.Text = client.Mobile;
            }
        }

        private async void EditClientForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    Client = await taskPool.AddTask(channel.Service.GetClient(clientId));
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

        private void EditClientRequestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }

        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            Client.Email = emailTextBox.Text;
        }

        private void mobileTextBox_Leave(object sender, EventArgs e)
        {
            Client.Mobile = mobileTextBox.Text;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            Client.Name = nameTextBox.Text;
        }

        private async void passwordButton_Click(object sender, EventArgs e)
        {
            using (var f = new PasswordForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    using (var channel = channelManager.CreateChannel())
                    {
                        try
                        {
                            passwordButton.Enabled = false;

                            await taskPool.AddTask(channel.Service.ChangeClientPassword(Client.Id, f.Password));
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
                            passwordButton.Enabled = true;
                        }
                    }
                }
            }
        }

        private void patronymicTextBox_Leave(object sender, EventArgs e)
        {
            Client.Patronymic = patronymicTextBox.Text;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    Client = await taskPool.AddTask(channel.Service.EditClient(Client));

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

        private void surnameTextBox_Leave(object sender, EventArgs e)
        {
            Client.Surname = surnameTextBox.Text;
        }
    }
}