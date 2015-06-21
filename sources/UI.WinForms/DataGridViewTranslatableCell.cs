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
            var val = (string)base.GetFormattedValue(value, rowIndex, ref cellStyle, valueTypeConverter, formattedValueTypeConverter, context);

            //if (value is IConvertible)
            //{
            //    Translater.Enum(Enum.Parse(value.GetType(), value.ToString()), "short");
            //}
            //this.OwningColumn
            return val;
        }
    }
}