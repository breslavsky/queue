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

namespace Queue.WinForms
{
    public class ServerSettings : ConfigurationSection
    {
        [ConfigurationProperty("services")]
        public ServicesConfig Services
        {
            get { return (ServicesConfig)this["services"]; }
            set { this["services"] = value; }
        }
    }

    public partial class Form1 : Form
    {
        private Server server;

        private static Properties.Settings settings = Properties.Settings.Default;

        public Form1()
        {
            InitializeComponent();

            ServerSettings settings = ConfigurationManager.GetSection("server") as ServerSettings;

            //server = new Server(new ServerSettings());
            int c = 1 + 3;
        }
    }
}