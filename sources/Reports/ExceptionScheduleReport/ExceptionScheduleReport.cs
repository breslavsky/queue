using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Queue.Model;
using System;
using System.IO;
using System.Linq;

namespace Queue.Reports.ExceptionScheduleReport
{
    public class ExceptionScheduleReport : BaseReport
    {
        private readonly DateTime fromDate;

        protected override int ColumnCount { get { return 7; } }

        public ExceptionScheduleReport(DateTime fromDate)
        {
            this.fromDate = fromDate;
        }

        protected override HSSFWorkbook InternalGenerate()
        {
            var workbook = new HSSFWorkbook(new MemoryStream(Templates.ExceptionSchedule));

            styles = new StandardCellStyles(workbook);

            AddDataToReport(workbook.GetSheetAt(0), GetDefaultExceptionScheduleData());
            AddDataToReport(workbook.GetSheetAt(1), GetServiceExceptionScheduleData(), false);

            return workbook;
        }

        private DefaultExceptionScheduleReportData[] GetDefaultExceptionScheduleData()
        {
            using (var session = SessionProvider.OpenSession())
            {
                return session.QueryOver<DefaultExceptionSchedule>()
                    .Where(s => s.ScheduleDate >= fromDate)
                    .OrderBy(s => s.ScheduleDate).Asc
                    .List()
                    .Select(s => new DefaultExceptionScheduleReportData(s, s.ScheduleDate))
                    .ToArray();
            }
        }

        private ServiceExceptionScheduleReportData[] GetServiceExceptionScheduleData()
        {
            using (var session = SessionProvider.OpenSession())
            {
                return session.QueryOver<ServiceExceptionSchedule>()
                    .Where(s => s.ScheduleDate >= fromDate)
                    .OrderBy(s => s.ScheduleDate).Asc
                    .List()
                    .Select(s => new ServiceExceptionScheduleReportData(s))
                    .ToArray();
            }
        }

        private void AddDataToReport(ISheet worksheet, DefaultExceptionScheduleReportData[] items, bool isDefault = true)
        {
            var rowIndex = worksheet.LastRowNum + 1;

            var dateTitle = DateTime.MinValue;

            foreach (DefaultExceptionScheduleReportData item in items)
            {
                IRow row;

                if (isDefault)
                {
                    row = worksheet.CreateRow(rowIndex++);

                    WriteCell(row, 0, c => c.SetCellValue(item.ScheduleDate.ToShortDateString()));
                }
                else
                {
                    if (dateTitle != item.ScheduleDate)
                    {
                        dateTitle = item.ScheduleDate;

                        row = worksheet.CreateRow(rowIndex++);

                        WriteCell(row, 0, c => c.SetCellValue(dateTitle.ToShortDateString()), styles[StandardCellStyles.BoldStyle]);
                    }

                    row = worksheet.CreateRow(rowIndex++);

                    WriteCell(row, 0, c => c.SetCellValue((item as ServiceExceptionScheduleReportData).Service));
                }

                WriteCell(row, 1, c => c.SetCellValue(item.IsWorked ? "Да" : "Нет"));

                if (item.IsWorked)
                {
                    WriteCell(row, 2, c => c.SetCellValue(item.StartTime.ToString()));
                    WriteCell(row, 3, c => c.SetCellValue(item.FinishTime.ToString()));
                    WriteCell(row, 4, c => c.SetCellValue(item.ClientInterval.Minutes));
                    WriteCell(row, 5, c => c.SetCellValue(item.Intersection.Minutes));
                    WriteCell(row, 6, c => c.SetCellValue(item.MaxClientRequests));
                }
            };
        }

        private class DefaultExceptionScheduleReportData
        {
            public DefaultExceptionScheduleReportData(Schedule source, DateTime scheduleDate)
            {
                ScheduleDate = scheduleDate;
                IsWorked = source.IsWorked;
                StartTime = source.StartTime;
                FinishTime = source.FinishTime;
                ClientInterval = source.LiveClientInterval;
                Intersection = source.Intersection;
                MaxClientRequests = source.MaxClientRequests;
            }

            public DateTime ScheduleDate { get; set; }

            public bool IsWorked { get; set; }

            public TimeSpan StartTime { get; set; }

            public TimeSpan FinishTime { get; set; }

            public TimeSpan ClientInterval { get; set; }

            public TimeSpan Intersection { get; set; }

            public int MaxClientRequests { get; set; }
        }

        private class ServiceExceptionScheduleReportData : DefaultExceptionScheduleReportData
        {
            public ServiceExceptionScheduleReportData(ServiceExceptionSchedule source)
                : base(source, source.ScheduleDate)
            {
                Service = source.Service.ToString();
            }

            public string Service { get; set; }
        }
    }
}