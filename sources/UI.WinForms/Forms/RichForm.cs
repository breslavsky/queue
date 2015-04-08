using System;
using System.Reflection;

namespace Queue.UI.WinForms
{
    public class RichForm : Junte.UI.WinForms.RichForm, ITranslatableControl
    {
        private ControlTranslater translater;

        public RichForm()
        {
            if (!DesignMode)
            {
                UpdateIcon();
                translater = new ControlTranslater(this);
            }
        }

        private void UpdateIcon()
        {
            try
            {
                //TODO: адаптировать под размер
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