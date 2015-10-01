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
    public partial class HubSettingsUserControl : DependencyUserControl
    {
        #region dependency

        [Dependency]
        [ReadOnly(true)]
        [Browsable(false)]
        public HubSettings HubQualitySettings { get; set; }

        #endregion dependency

        public HubSettingsUserControl()
            : base()
        {
            InitializeComponent();
        }

        private void HubSettingsUserControl_Load(object sender, EventArgs e)
        {
            if (designtime)
            {
                return;
            }

            enabledCheckBox.Checked = HubQualitySettings.Enabled;
            endpointTextBox.Text = HubQualitySettings.Endpoint;
        }

        private void enabledCheckBox_Leave(object sender, EventArgs e)
        {
            HubQualitySettings.Enabled = enabledCheckBox.Checked;
        }

        private void endpointTextBox_Leave(object sender, EventArgs e)
        {
            HubQualitySettings.Endpoint = endpointTextBox.Text;
        }
    }
}