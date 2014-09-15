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

namespace Queue.Administrator
{
    public partial class HtmlEditorForm : Queue.UI.WinForms.RichForm
    {
        private const string HIGHLIGHTING_STYLE = "HTML";

        public string HTML
        {
            get { return htmlEditorControl.Text; }
            set { htmlEditorControl.Text = value; }
        }

        public HtmlEditorForm()
        {
            InitializeComponent();

            htmlEditorControl.SetHighlighting(HIGHLIGHTING_STYLE);

            okCancelPanel.OnOk += Save;
            okCancelPanel.OnCancel += Cancel;
        }

        private void Save()
        {
            DialogResult = DialogResult.OK;
        }

        private void Cancel()
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
