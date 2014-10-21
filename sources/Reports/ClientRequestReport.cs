using NHibernate;
using NHibernate.Criterion;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Queue.Model;
using Queue.Services.Common;
using System;
using System.IO;
using System.ServiceModel;

namespace Queue.Reports
{
    public class ClientRequestReport : BaseReport
    {
        private readonly Guid clientRequestId;

        public ClientRequestReport(Guid clientRequestId)
        {
            this.clientRequestId = clientRequestId;
        }

        public override HSSFWorkbook Generate()
        {
            HSSFWorkbook workbook = new HSSFWorkbook(new MemoryStream(Templates.ClientRequest));
            ISheet worksheet = workbook.GetSheetAt(0);

            IDataFormat format = workbook.CreateDataFormat();
            ICellStyle boldCellStyle = CreateCellBoldStyle(workbook);

            IRow row;
            ICell cell;
            int rowIndex = worksheet.LastRowNum + 1;

            DateTime dateTitle = DateTime.MinValue;

            using (ISession session = SessionProvider.OpenSession())
            {
                ClientRequest clientRequest = session.Get<ClientRequest>(clientRequestId);
                if (clientRequest == null)
                {
                    throw new FaultException<ObjectNotFoundFault>(
                        new ObjectNotFoundFault(clientRequestId), string.Format("Запрос [{0}] не найден", clientRequestId));
                }

                row = worksheet.CreateRow(0);
                row.RowStyle = boldCellStyle;

                cell = row.CreateCell(0);
                cell.SetCellValue(clientRequest.ToString());
                cell.CellStyle = boldCellStyle;

                foreach (ClientRequestEvent e in session.CreateCriteria<ClientRequestEvent>()
                    .Add(Restrictions.Eq("ClientRequest", clientRequest))
                    .AddOrder(Order.Asc("CreateDate"))
                    .List<ClientRequestEvent>())
                {
                    row = worksheet.CreateRow(rowIndex++);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(e.CreateDate.ToString());
                    cell = row.CreateCell(1);
                    cell.SetCellValue(e.Message);
                };
            }

            return workbook;
        }
    }
}