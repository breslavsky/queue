using Queue.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public class ControlTranslater
    {
        private readonly List<DictionaryEntry> defaultTranslation = new List<DictionaryEntry>();
        private readonly Control control;

        public ControlTranslater(Control control)
        {
            this.control = control;
        }

        public void Translate()
        {
            Translater translater = new Translater(control.GetType());

            DictionaryEntry[] entries = translater.GetStrings();
            if (entries.Length == 0)
            {
                entries = defaultTranslation.ToArray();
            }

            foreach (DictionaryEntry entry in entries)
            {
                object ctrl = GetControlForTranslation(entry);
                if (ctrl != null)
                {
                    TranslateControl(ctrl, entry);
                }
            }

            foreach (Control ctrl in control.Controls)
            {
                if (ctrl is ITranslatableControl)
                {
                    (ctrl as ITranslatableControl).Translate();
                }
            }
        }

        private object GetControlForTranslation(DictionaryEntry entry)
        {
            object ctrl = control;
            string[] name = entry.Key.ToString().Split('.');

            if (name.Length > 1)
            {
                FieldInfo controlInfo = control.GetType().GetField(name.First(), BindingFlags.NonPublic | BindingFlags.Instance);
                if (controlInfo == null)
                {
                    return null;
                }

                ctrl = controlInfo.GetValue(control);
            }

            return ctrl;
        }

        private void TranslateControl(object ctrl, DictionaryEntry entry)
        {
            string key = entry.Key.ToString();
            string[] name = key.Split('.');

            PropertyInfo propertyInfo = ctrl.GetType().GetProperty(name.Last());
            if (propertyInfo == null)
            {
                return;
            }

            if (!defaultTranslation.Any(x => x.Key.Equals(key)))
            {
                defaultTranslation.Add(new DictionaryEntry(key, propertyInfo.GetValue(ctrl)));
            }

            propertyInfo.SetValue(ctrl, entry.Value.ToString());
        }
    }
}