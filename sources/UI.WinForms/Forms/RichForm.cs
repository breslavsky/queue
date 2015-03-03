using Queue.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public class RichForm : Junte.UI.WinForms.RichForm
    {
        private FormTranslater translater;

        public RichForm()
        {
            if (!DesignMode)
            {
                UpdateIcon();
                translater = new FormTranslater(this);
            }
        }

        private void UpdateIcon()
        {
            try
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
            }
            catch { }
        }

        internal void Translate()
        {
            translater.Translate();

            foreach (Control c in ControlList())
            {
                if (c is RichUserControl)
                {
                    (c as RichUserControl).Translate();
                }
            }
        }

        private IEnumerable<Control> ControlList(Control control = null)
        {
            var controls = (control != null ? control.Controls : Controls).Cast<Control>();

            return controls.SelectMany(c => ControlList(c))
                            .Concat(controls);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!DesignMode)
            {
                Translate();
            }

            base.OnLoad(e);
        }
    }
}