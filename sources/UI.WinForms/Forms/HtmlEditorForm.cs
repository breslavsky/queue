using Junte.UI.WinForms;
using System;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class HtmlEditorForm : RichForm
    {
        private const string HighlightStyle = "HTML";

        public string HTML
        {
            get { return htmlEditorControl.Text; }
            set { htmlEditorControl.Text = value; }
        }

        public HtmlEditorForm()
        {
            InitializeComponent();

            htmlEditorControl.SetHighlighting(HighlightStyle);

            okCancelPanel.OnOk += okCancelPanel_OnOk;
            okCancelPanel.OnCancel += okCancelPanel_OnCancel;
        }

        private void okCancelPanel_OnOk(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void okCancelPanel_OnCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}