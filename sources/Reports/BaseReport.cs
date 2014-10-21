using Junte.Data.NHibernate;
using Microsoft.Practices.ServiceLocation;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace Queue.Reports
{
    public abstract class BaseReport
    {
        protected ISessionProvider SessionProvider
        {
            get { return ServiceLocator.Current.GetInstance<ISessionProvider>(); }
        }

        public abstract HSSFWorkbook Generate();

        protected ICellStyle CreateCellBoldStyle(HSSFWorkbook workBook)
        {
            ICellStyle boldCellStyle = workBook.CreateCellStyle();

            IFont font = workBook.CreateFont();
            font.Boldweight = 1000;
            boldCellStyle.SetFont(font);

            return boldCellStyle;
        }
    }
}