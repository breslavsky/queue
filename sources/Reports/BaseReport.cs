using Junte.Data.NHibernate;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;

namespace Queue.Reports
{
    public abstract class BaseReport : IQueueReport
    {
        protected StandardCellStyles styles;

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

        protected ICell WriteCell(IRow row, int column, Action<ICell> setValue, ICellStyle style = null)
        {
            var cell = row.CreateCell(column);
            setValue(cell);
            if (style != null)
            {
                cell.CellStyle = style;
            }

            return cell;
        }
    }
}