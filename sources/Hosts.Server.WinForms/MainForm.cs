using Junte.Data.NHibernate;
using Junte.UI.WinForms.NHibernate;
using Queue.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

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
        }

        private void databaseButton_Click(object sender, EventArgs eventArgs)
        {
            using (var loginForm = new LoginForm(settings.Database ?? new DatabaseSettings()))
            {
                loginForm.OnLogin += (s, e) =>
                {
                    settings.Database = e.Settings;
                    loginForm.DialogResult = DialogResult.OK;                    
                };

                if (loginForm.ShowDialog() != DialogResult.OK)
                {
                    return;
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
    }
}