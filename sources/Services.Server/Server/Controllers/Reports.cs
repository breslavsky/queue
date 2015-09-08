using NPOI.HSSF.UserModel;
using Queue.Model.Common;
using Queue.Reports;
using Queue.Reports.AdditionalServicesRatingReport;
using Queue.Reports.ClientRequestReport;
using Queue.Reports.ExceptionScheduleReport;
using Queue.Reports.OperatorRatingReport;
using Queue.Reports.ServiceRatingReport;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    public partial class ServerService
    {
        public async Task<byte[]> GetServiceRatingReport(ServiceRatingReportSettings settings)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Reports);
                return GenerateReport(new ServiceRatingReport(settings));
            });
        }

        public async Task<byte[]> GetOperatorRatingReport(OperatorRatingReportSettings settings)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Reports);
                return GenerateReport(new OperatorRatingReport(settings));
            });
        }

        public async Task<byte[]> GetAdditinalServicesRatingReport(AdditionalServicesRatingReportSettings settings)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Reports);
                return GenerateReport(new AdditionalServiceRatingReport(settings));
            });
        }

        public async Task<byte[]> GetExceptionScheduleReport(DateTime from)
        {
            return await Task.Run(() => GenerateReport(new ExceptionScheduleReport(from)));
        }

        public async Task<byte[]> GetClientRequestReport(Guid reqId)
        {
            return await Task.Run(() => GenerateReport(new ClientRequestReport(reqId)));
        }

        private byte[] GenerateReport(BaseReport report)
        {
            HSSFWorkbook workbook = report.Generate();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.Write(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}