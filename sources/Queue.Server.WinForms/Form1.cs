using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.WinForms
{
    public partial class Form1 : Form
    {
        private Server server;

        private static Properties.Settings settings = Properties.Settings.Default;

        public Form1()
        {
            InitializeComponent();

            server = new Server(settings.Server);
        }
    }
}