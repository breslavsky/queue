using System;
using System.Reflection;

namespace Queue.UI.WinForms
{
    public class RichForm : Junte.UI.WinForms.RichForm, ITranslatable
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

        public void Translate()
        {
            translater.Translate();
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