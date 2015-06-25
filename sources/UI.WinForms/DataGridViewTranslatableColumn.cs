using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public class DataGridViewTranslatableColumn : DataGridViewColumn
    {
        public DataGridViewTranslatableColumn()
        {
            this.CellTemplate = new DataGridViewTranslatableCell();
            this.ReadOnly = true;
        }
    }
}