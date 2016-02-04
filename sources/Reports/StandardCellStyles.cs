using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Queue.Reports
{
    public class StandardCellStyles
    {
        public const int BoldStyle = 0;

        private readonly Dictionary<int, ICellStyle> styles = new Dictionary<int, ICellStyle>();

        public ICellStyle this[int i]
        {
            get { return styles[i]; }
            set { styles[i] = value; }
        }

        public StandardCellStyles(IWorkbook workbook)
        {
            InitializeStyles(workbook);
        }

        private void InitializeStyles(IWorkbook workbook)
        {
            this[BoldStyle] = CreateBoldStyle(workbook);
        }

        private ICellStyle CreateBoldStyle(IWorkbook workbook)
        {
            var style = workbook.CreateCellStyle();

            var font = workbook.CreateFont();
            font.Boldweight = 1000;
            style.SetFont(font);

            return style;
        }
    }
}