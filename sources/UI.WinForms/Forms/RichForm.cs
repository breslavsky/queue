using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public class RichForm : Junte.UI.WinForms.RichForm
    {
        public RichForm()
        {
            Load += RichForm_Load;
        }

        public IEnumerable<Control> ControlList(Control control = null)
        {
            var controls = (control != null ? control.Controls : Controls).Cast<Control>();

            return controls.SelectMany(c => ControlList(c))
                .Concat(controls);
        }

        private void RichForm_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            var type = this.GetType();
            var resourcePath = string.Format("{0}.Translate.{1}", type.Namespace, type.Name);

            try
            {
                var entries = new ResourceManager(resourcePath, type.Assembly)
                    .GetResourceSet(CultureInfo.CurrentCulture, true, true);

                var text = entries.GetString("Text");
                if (text != null)
                {
                    Text = text;
                }

                foreach (DictionaryEntry entry in entries)
                {
                    string[] name = entry.Key.ToString().Split('.');
                    if (name.Length > 1)
                    {
                        var componentInfo = GetType().GetField(name.First(), BindingFlags.NonPublic | BindingFlags.Instance);
                        if (componentInfo != null)
                        {
                            var component = componentInfo.GetValue(this);

                            var propertyInfo = component.GetType().GetProperty(name.Skip(1).First());
                            if (propertyInfo != null)
                            {
                                string value = entry.Value.ToString();
                                propertyInfo.SetValue(component, value);
                            }
                        }
                    }
                }
            }
            catch { }
        }
    }
}