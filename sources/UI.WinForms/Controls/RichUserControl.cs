using Queue.UI.Common;
using System;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class RichUserControl : UserControl, ITranslatable
    {
        private ControlTranslater translater;

        public RichUserControl()
        {
            if (!DesignMode)
            {
                translater = new ControlTranslater(this);
            }
        }

        public virtual void Translate()
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