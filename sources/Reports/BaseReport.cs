using Junte.Data.NHibernate;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Queue.Reports
{
    public abstract class BaseReport
    {
        #region dependency

        [Dependency]
        public SessionProvider SessionProvider { get; set; }

        #endregion dependency

        public BaseReport()
        {
            ServiceLocator.Current.GetInstance<UnityContainer>()
                .BuildUp(this.GetType(), this);
        }

        public HSSFWorkbook Generate()
        {
            var wk = InternalGenerate();
            var sheet = wk.GetSheetAt(0);
            wk.SetPrintArea(0, 0, sheet.GetRow(1).LastCellNum, 0, sheet.LastRowNum);

            return wk;
        }

        protected abstract HSSFWorkbook InternalGenerate();

        protected ICellStyle CreateCellBoldStyle(IWorkbook workBook)
        {
            var boldCellStyle = workBook.CreateCellStyle();

            var font = workBook.CreateFont();
            font.Boldweight = 1000;
            boldCellStyle.SetFont(font);

            return boldCellStyle;
        }
    }
}