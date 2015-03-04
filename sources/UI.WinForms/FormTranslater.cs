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
    public class FormTranslater
    {
        private const string ResourcePathTemplate = "{Namespace}.Translate.{Name}";

        private readonly List<DictionaryEntry> defaultTranslation = new List<DictionaryEntry>();
        private readonly Control form;

        public FormTranslater(Control form)
        {
            this.form = form;
        }

        public void Translate()
        {
            foreach (DictionaryEntry entry in GetDictionaryEntries())
            {
                object control = GetControlForTranslation(entry);
                if (control != null)
                {
                    TranslateControl(control, entry);
                }
            }

            foreach (Control control in form.Controls)
            {
                if (control is ITranslatable)
                {
                    (control as ITranslatable).Translate();
                }
            }
        }

        private List<DictionaryEntry> GetDictionaryEntries()
        {
            Type type = form.GetType();
            string resourcePath = ResourcePathTemplate
                .Replace("{Namespace}", type.Namespace)
                .Replace("{Name}", type.Name);

            try
            {
                return new ResourceManager(resourcePath, type.Assembly)
                                            .GetResourceSet(CultureInfo.CurrentCulture, true, true)
                                            .Cast<DictionaryEntry>()
                                            .ToList();
            }
            catch { }

            return defaultTranslation;
        }

        private object GetControlForTranslation(DictionaryEntry entry)
        {
            object control = form;
            string[] name = entry.Key.ToString().Split('.');

            if (name.Length > 1)
            {
                FieldInfo controlInfo = form.GetType().GetField(name.First(), BindingFlags.NonPublic | BindingFlags.Instance);
                if (controlInfo == null)
                {
                    return null;
                }

                control = controlInfo.GetValue(form);
            }

            return control;
        }

        private void TranslateControl(object control, DictionaryEntry entry)
        {
            string key = entry.Key.ToString();
            string[] name = key.Split('.');

            PropertyInfo propertyInfo = control.GetType().GetProperty(name.Last());
            if (propertyInfo == null)
            {
                return;
            }

            if (!defaultTranslation.Any(x => x.Key.Equals(key)))
            {
                defaultTranslation.Add(new DictionaryEntry(key, propertyInfo.GetValue(control)));
            }

            propertyInfo.SetValue(control, entry.Value.ToString());
        }
    }
}