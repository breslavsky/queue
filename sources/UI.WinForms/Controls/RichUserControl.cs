using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;
using System.Resources;
using System.Globalization;

namespace Queue.UI.WinForms
{
    public partial class RichUserControl : UserControl
    {
        private const string ResourcePathTemplate = "{Namespace}.Translate.{Name}";
        private IList<DictionaryEntry> defaultTranslation = new List<DictionaryEntry>();

        public RichUserControl()
        {
            Load += RichUserControl_Load;
        }

        public IEnumerable<Control> ControlList(Control control = null)
        {
            var controls = (control != null ? control.Controls : Controls).Cast<Control>();

            return controls.SelectMany(c => ControlList(c))
                .Concat(controls);
        }

        internal void Translate()
        {
            var type = this.GetType();
            var resourcePath = ResourcePathTemplate
                .Replace("{Namespace}", type.Namespace)
                .Replace("{Name}", type.Name);

            IEnumerable<DictionaryEntry> entries;

            try
            {
                entries = new ResourceManager(resourcePath, type.Assembly)
                    .GetResourceSet(CultureInfo.CurrentCulture, true, true).Cast<DictionaryEntry>();
            }
            catch
            {
                entries = defaultTranslation;
            }

            foreach (DictionaryEntry entry in entries)
            {
                object component = this;

                string key = entry.Key.ToString();
                string[] name = key.Split('.');

                if (name.Length > 1)
                {
                    var componentInfo = GetType().GetField(name.First(), BindingFlags.NonPublic | BindingFlags.Instance);
                    if (componentInfo == null)
                    {
                        continue;
                    }

                    component = componentInfo.GetValue(this);
                }

                var propertyInfo = component.GetType().GetProperty(name.Last());
                if (propertyInfo != null)
                {
                    string value = entry.Value.ToString();

                    if (!defaultTranslation.Any(x => x.Key.Equals(key)))
                    {
                        defaultTranslation.Add(new DictionaryEntry(key, propertyInfo.GetValue(component)));
                    }

                    propertyInfo.SetValue(component, value);
                }
            }
        }

        private void RichUserControl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            Translate();
        }
    }
}
