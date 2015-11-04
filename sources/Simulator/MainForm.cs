using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Simulator
{
    public partial class SimulatorForm : DependencyForm
    {
        public SimulatorForm()
            : base()
        {
            InitializeComponent();
        }

        public bool IsLogout { get; private set; }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            IsLogout = true;
            Close();
        }

        private void MainForm_Load(object sender, EventArgs eventArgs)
        {
            Text += string.Format(" ({0})", Assembly.GetEntryAssembly().GetName().Version);
        }

        private void ShowForm<T>(Func<Form> create)
        {
            var form = MdiChildren.FirstOrDefault(f => f.GetType() == typeof(T));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = create();
            form.MdiParent = this;

            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void clientRequestsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ClientRequstsForm>(() => new ClientRequstsForm());
        }

        private void opeatorsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<OperatorsForm>(() => new OperatorsForm());
        }
    }
}