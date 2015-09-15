using Junte.Configuration;
using Microsoft.Practices.Unity;
using Queue.Common.Settings;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.Operator
{
    public partial class SettingsForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public ConfigurationManager Configuration { get; set; }

        #endregion dependency

        public SettingsForm()
            : base()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            Configuration.Save();
            DialogResult = DialogResult.OK;
        }
    }
}