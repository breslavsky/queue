﻿using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class CurrentScheduleForm : Queue.UI.WinForms.RichForm
    {
        #region filelds

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        #endregion filelds

        #region properties

        private Service SelectedService { get { return selectServiceControl.SelectedService; } }

        #endregion properties

        public CurrentScheduleForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();

            currentScheduleControl.Initialize(channelBuilder, currentUser);
            selectServiceControl.Initialize(channelBuilder, currentUser);
        }

        private async void selectServiceControl_ServiceSelected(object sender, EventArgs e)
        {
            if (SelectedService != null)
            {
                currentSchedulePanel.Enabled = true;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        await channel.Service.OpenUserSession(currentUser.SessionId);
                        currentScheduleControl.Schedule = await taskPool.AddTask(channel.Service.GetServiceExceptionSchedule(SelectedService.Id, ServerDateTime.Today));

                        currentScheduleCheckBox.Checked = true;
                    }
                    catch (OperationCanceledException) { }
                    catch (CommunicationObjectAbortedException) { }
                    catch (ObjectDisposedException) { }
                    catch (InvalidOperationException) { }
                    catch (FaultException<ObjectNotFoundFault>)
                    {
                        currentScheduleCheckBox.Checked = false;
                        currentScheduleControl.Schedule = null;
                    }
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
                currentSchedulePanel.Enabled = false;
            }
        }

        private async void currentScheduleCheckBox_Click(object sender, EventArgs e)
        {
            if (SelectedService != null)
            {
                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        currentScheduleCheckBox.Enabled = false;

                        var scheduleDate = ServerDateTime.Today;

                        await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));

                        if (currentScheduleCheckBox.Checked)
                        {
                            currentScheduleControl.Schedule = await taskPool.AddTask(channel.Service.AddServiceExceptionSchedule(SelectedService.Id, scheduleDate));
                        }
                        else
                        {
                            var schedule = currentScheduleControl.Schedule;
                            if (schedule != null)
                            {
                                await taskPool.AddTask(channel.Service.DeleteSchedule(schedule.Id));
                                currentScheduleControl.Schedule = null;
                            }
                        }
                    }
                    catch (OperationCanceledException) { }
                    catch (CommunicationObjectAbortedException) { }
                    catch (ObjectDisposedException) { }
                    catch (InvalidOperationException) { }
                    catch (FaultException<ObjectNotFoundFault>)
                    {
                        // nothing
                    }
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
                        currentScheduleCheckBox.Enabled = true;
                    }
                }
            }
        }

        private void currentScheduleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            currentScheduleControl.Enabled = currentScheduleCheckBox.Checked;
        }

        private void ServicesForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }
    }
}