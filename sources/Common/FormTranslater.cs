using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Queue.Common
{
    public class FormTranslater
    {
        private const string ResourcePathTemplate = "{Namespace}.Translate.{Name}";

        private List<DictionaryEntry> defaultTranslation = new List<DictionaryEntry>();

        private Component form;

        public FormTranslater(Component form)
        {
            this.form = form;
        }

        public void Translate()
        {
            Type type = form.GetType();
            string resourcePath = ResourcePathTemplate
                .Replace("{Namespace}", type.Namespace)
                .Replace("{Name}", type.Name);

            IEnumerable<DictionaryEntry> entries;

            try
            {
                entries = new ResourceManager(resourcePath, type.Assembly)
                                    .GetResourceSet(CultureInfo.CurrentCulture, true, true)
                                    .Cast<DictionaryEntry>();
            }
            catch
            {
                entries = defaultTranslation;
            }

            foreach (DictionaryEntry entry in entries)
            {
                object component = form;

                string key = entry.Key.ToString();
                string[] name = key.Split('.');

                if (name.Length > 1)
                {
                    FieldInfo componentInfo = type.GetField(name.First(), BindingFlags.NonPublic | BindingFlags.Instance);
                    if (componentInfo == null)
                    {
                        continue;
                    }

                    component = componentInfo.GetValue(form);
                }

                PropertyInfo propertyInfo = component.GetType().GetProperty(name.Last());
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
    }
}