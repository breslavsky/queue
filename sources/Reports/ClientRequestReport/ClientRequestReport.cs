using NHibernate.Transform;
using NPOI.HSSF.UserModel;
using Queue.Model;
using Queue.Services.Common;
using System;
using System.IO;
using System.Linq;
using System.ServiceModel;

namespace Queue.Reports.ClientRequestReport
{
    public class ClientRequestReport : BaseReport
    {
        private readonly Guid clientRequestId;

        protected override int ColumnCount { get { return 2; } }

        public ClientRequestReport(Guid clientRequestId)
            : base()
        {
            this.clientRequestId = clientRequestId;
        }

        protected override HSSFWorkbook InternalGenerate()
        {
            var data = GetData();
            var workbook = new HSSFWorkbook(new MemoryStream(Templates.ClientRequest));
            var worksheet = workbook.GetSheetAt(0);

            styles = new StandardCellStyles(workbook);

            int rowIndex = worksheet.LastRowNum + 1;
            var row = worksheet.CreateRow(0);

            WriteCell(row, 0, c => c.SetCellValue(data.Title), styles[StandardCellStyles.BoldStyle]);

            foreach (var item in data.Items)
            {
                row = worksheet.CreateRow(rowIndex++);

                WriteCell(row, 0, c => c.SetCellValue(item.CreateDate.ToString()));
                WriteCell(row, 1, c => c.SetCellValue(item.Message));
            };

            return workbook;
        }

        private ReportData GetData()
        {
            using (var session = SessionProvider.OpenSession())
            {
                var clientRequest = session.Get<ClientRequest>(clientRequestId);
                if (clientRequest == null)
                {
                    throw new FaultException<ObjectNotFoundFault>(
                        new ObjectNotFoundFault(clientRequestId), String.Format("Запрос [{0}] не найден", clientRequestId));
                }

                var result = new ReportData();
                result.Title = clientRequest.ToString();

                ClientRequestEventReportItem dto = null;
                result.Items = session.QueryOver<ClientRequestEvent>()
                                        .Where(e => e.ClientRequest == clientRequest)
                                        .OrderBy(e => e.CreateDate).Asc
                                        .SelectList(list => list
                                           .Select(p => p.CreateDate).WithAlias(() => dto.CreateDate)
                                           .Select(p => p.Message).WithAlias(() => dto.Message))
                                        .TransformUsing(Transformers.AliasToBean<ClientRequestEventReportItem>())
                                        .List<ClientRequestEventReportItem>()
                                        .ToArray();
                return result;
            }
        }

        private class ReportData
        {
            public string Title { get; set; }

            public ClientRequestEventReportItem[] Items;
        }

        private class ClientRequestEventReportItem
        {
            public DateTime CreateDate { get; set; }

            public string Message { get; set; }
        }
    }
}