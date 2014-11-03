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
        private ServerInstance server;

        public MainForm()
        {
            InitializeComponent();

            ServerSettings settings = ConfigurationManager.GetSection("server") as ServerSettings;

            server = new ServerInstance(settings);
            
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            server.Start();
            startButton.Enabled = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Stop();
        }
    }
}