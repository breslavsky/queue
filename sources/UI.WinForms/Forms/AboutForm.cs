using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class AboutForm : RichForm
    {
        public AboutForm()
            : base()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, System.EventArgs e)
        {
            var info = new List<string>();

            var assembly = Assembly.GetEntryAssembly();

            object[] attributes = assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            if (attributes.Length > 0)
            {
                var a = (AssemblyTitleAttribute)attributes.First();
                info.Add(a.Title);
            }

            var name = assembly.GetName();
            info.Add(name.Version.ToString());

            attributes = assembly.GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length > 0)
            {
                var a = (AssemblyProductAttribute)attributes.First();
                info.Add(a.Product);
            }

            attributes = assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length > 0)
            {
                var a = (AssemblyCopyrightAttribute)attributes.First();
                info.Add(a.Copyright);
            }

            infoTextBox.Lines = info.ToArray();
        }

        private void junteLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.junte.ru/");
        }
    }
}