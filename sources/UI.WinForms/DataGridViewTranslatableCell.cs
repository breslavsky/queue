using Junte.Translation;
using System.ComponentModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public class DataGridViewTranslatableCell : DataGridViewTextBoxCell
    {
        protected override object GetFormattedValue(object value,
                                                    int rowIndex,
                                                    ref DataGridViewCellStyle cellStyle,
                                                    TypeConverter valueTypeConverter,
                                                    TypeConverter formattedValueTypeConverter,
                                                    DataGridViewDataErrorContexts context)
        {
            return Translater.Enum(value);
        }
    }
}