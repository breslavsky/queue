using Junte.Data.NHibernate;
using Junte.UI.WinForms.NHibernate;
using Queue.Server;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace Queue.Hosts.Server.WinForms
{
    public partial class MainForm : Form
    {
        private Configuration configuration;

        private ServerSettings settings;

        private ServerInstance server;

        public MainForm()
        {
            InitializeComponent();

            configuration = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            settings = configuration.GetSection("server") as ServerSettings;

            var tcp = settings.Services.TcpService;

            tcpCheckBox.Checked = tcp.Enabled;
            tcpHostTextBox.Text = tcp.Host;
            tcpPortUpDown.Value = tcp.Port;

            var http = settings.Services.HttpService;

            httpCheckBox.Checked = http.Enabled;
            httpHostTextBox.Text = http.Host;
            httpPortUpDown.Value = http.Port;
        }

        private void databaseButton_Click(object sender, EventArgs eventArgs)
        {
            using (var loginForm = new LoginForm(settings.Database ?? new DatabaseSettings()))
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    settings.Database = loginForm.Settings;
                }
            }
        }

        private void startButton_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                server = new ServerInstance(settings);
                server.Start();
                configuration.Save();
                panel.Enabled = false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server != null)
            {
                server.Stop();
            }
        }

        #region bindings

        private void tcpHostTextBox_Leave(object sender, EventArgs e)
        {
            settings.Services.TcpService.Host = tcpHostTextBox.Text;
        }

        private void tcpPortUpDown_Leave(object sender, EventArgs e)
        {
            settings.Services.TcpService.Port = (int)tcpPortUpDown.Value;
        }

        private void tcpCheckBox_Leave(object sender, EventArgs e)
        {
            settings.Services.TcpService.Enabled = tcpCheckBox.Checked;
        }

        private void tcpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            tcpGroupBox.Enabled = tcpCheckBox.Checked;
        }

        private void httpHostTextBox_Leave(object sender, EventArgs e)
        {
            settings.Services.HttpService.Host = httpHostTextBox.Text;
        }

        private void httpPortUpDown_Leave(object sender, EventArgs e)
        {
            settings.Services.HttpService.Port = (int)httpPortUpDown.Value;
        }

        private void httpCheckBox_Leave(object sender, EventArgs e)
        {
            settings.Services.HttpService.Enabled = httpCheckBox.Checked;
        }

        private void httpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            httpGroupBox.Enabled = httpCheckBox.Checked;
        }

        #endregion bindings
    }
}