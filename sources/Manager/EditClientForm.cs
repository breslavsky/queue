using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Manager
{
    public partial class EditClientForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private Client client;

        public Client Client
        {
            get { return client; }
        }

        public EditClientForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Client client)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.client = client;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private void EditClientForm_Load(object sender, EventArgs e)
        {
            surnameTextBox.Text = client.Surname;
            nameTextBox.Text = client.Name;
            patronymicTextBox.Text = client.Patronymic;
            emailTextBox.Text = client.Email;
            mobileTextBox.Text = client.Mobile;
        }

        private void surnameTextBox_Leave(object sender, EventArgs e)
        {
            client.Surname = surnameTextBox.Text;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            client.Name = nameTextBox.Text;
        }

        private void patronymicTextBox_Leave(object sender, EventArgs e)
        {
            client.Patronymic = patronymicTextBox.Text;
        }

        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            client.Email = emailTextBox.Text;
        }

        private void mobileTextBox_Leave(object sender, EventArgs e)
        {
            client.Mobile = mobileTextBox.Text;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    client = await taskPool.AddTask(channel.Service.EditClient(client));

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

                            await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                            await taskPool.AddTask(channel.Service.ChangeClientPassword(client.Id, f.Password));
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

        private void EditClientRequestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }
    }
}