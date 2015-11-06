using Junte.Parallel;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class SelectServiceForm : DependencyForm
    {
        public Service Service
        {
            get { return serviceControl.SelectedService; }
        }

        public SelectServiceForm()
            : base()
        {
            InitializeComponent();
        }

        private void serviceControl_Selected(object sender, EventArgs e)
        {
            if (Service != null)
            {
                DialogResult = DialogResult.OK;
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
    }
}