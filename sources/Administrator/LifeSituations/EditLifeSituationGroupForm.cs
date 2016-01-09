using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Drawing;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class EditLifeSituationGroupForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public ChannelManager<ILifeSituationTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region fields

        private readonly Guid parenGrouptId;
        private readonly Guid groupId;
        private readonly TaskPool taskPool;
        private LifeSituationGroup parentGroup;
        private LifeSituationGroup group;
        private const byte fontSizeConverter = 100;

        #endregion fields

        public EditLifeSituationGroupForm(Guid? parentId = null, Guid? groupId = null)
            : base()
        {
            InitializeComponent();

            this.parenGrouptId = parentId.HasValue
                ? parentId.Value : Guid.Empty;

            this.groupId = groupId.HasValue
                ? groupId.Value : Guid.Empty;

            taskPool = new TaskPool();

            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        public event EventHandler<EventArgs> Saved;

        public LifeSituationGroup Group
        {
            get { return group; }
            private set
            {
                group = value;

                codeTextBox.Text = group.Code;
                nameTextBox.Text = group.Name;
                commentTextBox.Text = group.Comment;
                descriptionTextBox.Text = group.Description;
                columnsUpDown.Value = group.Columns;
                rowsUpDown.Value = group.Rows;
                if (!string.IsNullOrWhiteSpace(group.Color))
                {
                    colorButton.BackColor = ColorTranslator.FromHtml(group.Color);
                }
                fontSizeTrackBar.Value = (int)(group.FontSize * fontSizeConverter);
            }
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

        private void descriptionTextBox_Click(object sender, EventArgs e)
        {
            using (var f = new HtmlEditorForm())
            {
                f.HTML = descriptionTextBox.Text;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    descriptionTextBox.Text = f.HTML;
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

                    group = await taskPool.AddTask(channel.Service.EditGroup(group));
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

        private async void ServiceGroupEdit_Load(object sender, EventArgs e)
        {
            Enabled = false;

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    if (parenGrouptId != Guid.Empty)
                    {
                        parentGroup = await taskPool.AddTask(channel.Service.GetGroup(parenGrouptId));
                    }

                    if (groupId != Guid.Empty)
                    {
                        Group = await taskPool.AddTask(channel.Service.GetGroup(groupId));
                    }
                    else
                    {
                        Group = new LifeSituationGroup()
                        {
                            IsActive = true,
                            ParentGroup = parentGroup,
                            Code = "0.0",
                            Name = "Новая группа жизненной ситуации",
                            Columns = 2,
                            Rows = 5,
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

        private void ServiceGroupEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
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
            group.Code = codeTextBox.Text;
        }

        private void colorButton_Leave(object sender, EventArgs e)
        {
            group.Color = ColorTranslator.ToHtml(colorButton.BackColor);
        }

        private void columnsUpDown_Leave(object sender, EventArgs e)
        {
            group.Columns = (int)columnsUpDown.Value;
        }

        private void commentTextBox_Leave(object sender, EventArgs e)
        {
            group.Comment = commentTextBox.Text;
        }

        private void descriptionTextBox_Leave(object sender, EventArgs e)
        {
            group.Description = descriptionTextBox.Text;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            group.Name = nameTextBox.Text;
        }

        private void rowsUpDown_Leave(object sender, EventArgs e)
        {
            group.Rows = (int)rowsUpDown.Value;
        }

        private void fontSizeTrackBar_Leave(object sender, EventArgs e)
        {
            group.FontSize = (float)fontSizeTrackBar.Value / fontSizeConverter;
        }

        private void fontSizeTrackBar_ValueChanged(object sender, EventArgs e)
        {
            fontSizeValueLabel.Text = fontSizeTrackBar.Value.ToString();
        }

        #endregion bindings
    }
}