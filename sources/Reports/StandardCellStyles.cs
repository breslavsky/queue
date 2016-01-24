using NPOI.SS.UserModel;
using System.Collections.Generic;

namespace Queue.Reports
{
    public class StandardCellStyles
    {
        public const int BoldStyle = 0;
        public const int BorderedStyle = 1;

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
            this[BorderedStyle] = CreateBorderedStyle(workbook);
        }

        private ICellStyle CreateBoldStyle(IWorkbook workbook)
        {
            var style = workbook.CreateCellStyle();

            var font = workbook.CreateFont();
            font.Boldweight = 1000;
            style.SetFont(font);
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;

            return style;
        }

        private ICellStyle CreateBorderedStyle(IWorkbook workbook)
        {
            var style = workbook.CreateCellStyle();

            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;

            return style;
        }
    }
}