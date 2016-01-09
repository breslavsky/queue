using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Drawing;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class EditLifeSituationForm : DependencyForm
    {
        #region events

        public event EventHandler<EventArgs> Saved;

        #endregion events

        #region dependency

        [Dependency]
        public ChannelManager<ILifeSituationTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region fields

        private const byte fontSizeConverter = 100;

        private readonly TaskPool taskPool;
        private readonly Guid groupId;
        private readonly Guid lifeSituationId;
        private LifeSituationGroup group;
        private LifeSituation lifeSituation;

        #endregion fields

        #region properties

        public LifeSituation LifeSituation
        {
            get { return lifeSituation; }
            private set
            {
                lifeSituation = value;

                codeTextBox.Text = lifeSituation.Code;
                nameTextBox.Text = lifeSituation.Name;
                commentTextBox.Text = lifeSituation.Comment;

                var service = lifeSituation.Service;
                if (service != null)
                {
                    serviceTextBlock.Text = service.ToString();
                }

                if (!string.IsNullOrWhiteSpace(lifeSituation.Color))
                {
                    colorButton.BackColor = ColorTranslator.FromHtml(lifeSituation.Color);
                }
                fontSizeTrackBar.Value = (int)(lifeSituation.FontSize * fontSizeConverter);
            }
        }

        #endregion properties

        public EditLifeSituationForm(Guid? groupId = null, Guid? lifeSituationId = null)

            : base()
        {
            InitializeComponent();

            this.groupId = groupId.HasValue
                ? groupId.Value : Guid.Empty;
            this.lifeSituationId = lifeSituationId.HasValue
                ? lifeSituationId.Value : Guid.Empty;

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

        private void EditLifeSituationForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void EditLifeSituationForm_Load(object sender, EventArgs e)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    Enabled = false;

                    if (groupId != Guid.Empty)
                    {
                        group = await taskPool.AddTask(channel.Service.GetGroup(groupId));
                    }

                    if (lifeSituationId != Guid.Empty)
                    {
                        LifeSituation = await taskPool.AddTask(channel.Service.GetLifeSituation(lifeSituationId));
                    }
                    else
                    {
                        LifeSituation = new LifeSituation()
                        {
                            IsActive = true,
                            LifeSituationGroup = group,
                            Code = "0.0",
                            Name = "Новая жизненная ситуация",
                            Color = "#FFFFFF",
                            FontSize = 1
                        };
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

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    LifeSituation = await taskPool.AddTask(channel.Service.EditLifeSituation(lifeSituation));
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

        #region bindings

        private void codeTextBox_Leave(object sender, EventArgs e)
        {
            lifeSituation.Code = codeTextBox.Text;
        }

        private void commentTextBox_Leave(object sender, EventArgs e)
        {
            lifeSituation.Comment = commentTextBox.Text;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            lifeSituation.Name = nameTextBox.Text;
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            using (var d = new ColorDialog())
            {
                if (d.ShowDialog() == DialogResult.OK)
                {
                    colorButton.BackColor = d.Color;
                }
            }
        }

        private void colorButton_Leave(object sender, EventArgs e)
        {
            lifeSituation.Color = ColorTranslator.ToHtml(colorButton.BackColor);
        }

        private void fontSizeTrackBar_Leave(object sender, EventArgs e)
        {
            lifeSituation.FontSize = (float)fontSizeTrackBar.Value / fontSizeConverter;
        }

        private void fontSizeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            fontSizeValueLabel.Text = fontSizeTrackBar.Value.ToString();
        }

        #endregion bindings

        private void serviceChangeLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (var f = new SelectServiceForm())
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    lifeSituation.Service = f.Service;
                    serviceTextBlock.Text = lifeSituation.Service.ToString();
                }
            }
        }
    }
}