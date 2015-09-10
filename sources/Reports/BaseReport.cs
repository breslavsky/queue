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

        public abstract HSSFWorkbook Generate();

        protected ICellStyle CreateCellBoldStyle(IWorkbook workBook)
        {
            ICellStyle boldCellStyle = workBook.CreateCellStyle();

            IFont font = workBook.CreateFont();
            font.Boldweight = 1000;
            boldCellStyle.SetFont(font);

            return boldCellStyle;
        }
    }
}