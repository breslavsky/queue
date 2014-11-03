using NHibernate;
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

        public ExceptionScheduleReport(DateTime fromDate)
        {
            this.fromDate = fromDate;
        }

        public override HSSFWorkbook Generate()
        {
            HSSFWorkbook workbook = new HSSFWorkbook(new MemoryStream(Templates.ExceptionSchedule));

            AddDataToReport(workbook.GetSheetAt(0), GetDefaultExceptionScheduleData());
            AddDataToReport(workbook.GetSheetAt(1), GetServiceExceptionScheduleData(), false);

            return workbook;
        }

        private DefaultExceptionScheduleReportData[] GetDefaultExceptionScheduleData()
        {
            using (ISession session = SessionProvider.OpenSession())
            {
                return session.QueryOver<DefaultExceptionSchedule>()
                    .Where(_s => _s.ScheduleDate >= fromDate)
                    .OrderBy(s => s.ScheduleDate).Asc
                    .List()
                    .Select(s_ => new DefaultExceptionScheduleReportData(s_))
                    .ToArray();
            }
        }

        private ServiceExceptionScheduleReportData[] GetServiceExceptionScheduleData()
        {
            using (ISession session = SessionProvider.OpenSession())
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
            int rowIndex = worksheet.LastRowNum + 1;

            ICellStyle boldCellStyle = CreateCellBoldStyle(worksheet.Workbook);

            var dateTitle = DateTime.MinValue;

            IRow row;
            ICell cell;

            foreach (DefaultExceptionScheduleReportData item in items)
            {
                if (isDefault)
                {
                    row = worksheet.CreateRow(rowIndex++);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(item.ScheduleDate.ToShortDateString());
                }
                else
                {
                    if (dateTitle != item.ScheduleDate)
                    {
                        dateTitle = item.ScheduleDate;

                        row = worksheet.CreateRow(rowIndex++);
                        row.RowStyle = boldCellStyle;

                        cell = row.CreateCell(0);
                        cell.SetCellValue(dateTitle.ToShortDateString());
                        cell.CellStyle = boldCellStyle;
                    }

                    row = worksheet.CreateRow(rowIndex++);
                    cell = row.CreateCell(0);
                    cell.SetCellValue((item as ServiceExceptionScheduleReportData).Service);
                }

                cell = row.CreateCell(1);
                cell.SetCellValue(item.IsWorked ? "Да" : "Нет");

                if (item.IsWorked)
                {
                    cell = row.CreateCell(2);
                    cell.SetCellValue(item.StartTime.ToString());

                    cell = row.CreateCell(3);
                    cell.SetCellValue(item.FinishTime.ToString());

                    if (item.IsInterruption)
                    {
                        cell = row.CreateCell(4);
                        cell.SetCellValue(item.InterruptionStartTime.ToString());

                        cell = row.CreateCell(5);
                        cell.SetCellValue(item.InterruptionFinishTime.ToString());
                    }

                    cell = row.CreateCell(6);
                    cell.SetCellValue(item.ClientInterval.Minutes);

                    cell = row.CreateCell(7);
                    cell.SetCellValue(item.Intersection.Minutes);

                    cell = row.CreateCell(8);
                    cell.SetCellValue(item.MaxClientRequests);
                }
            };
        }

        private class DefaultExceptionScheduleReportData
        {
            public DefaultExceptionScheduleReportData(ExceptionSchedule source)
            {
                ScheduleDate = source.ScheduleDate;
                IsWorked = source.IsWorked;
                StartTime = source.StartTime;
                FinishTime = source.FinishTime;
                IsInterruption = source.IsInterruption;
                InterruptionStartTime = source.InterruptionStartTime;
                InterruptionFinishTime = source.InterruptionFinishTime;
                ClientInterval = source.ClientInterval;
                Intersection = source.Intersection;
                MaxClientRequests = source.MaxClientRequests;
            }

            public DateTime ScheduleDate { get; set; }

            public bool IsWorked { get; set; }

            public TimeSpan StartTime { get; set; }

            public TimeSpan FinishTime { get; set; }

            public bool IsInterruption { get; set; }

            public TimeSpan InterruptionStartTime { get; set; }

            public TimeSpan InterruptionFinishTime { get; set; }

            public TimeSpan ClientInterval { get; set; }

            public TimeSpan Intersection { get; set; }

            public int MaxClientRequests { get; set; }
        }

        private class ServiceExceptionScheduleReportData : DefaultExceptionScheduleReportData
        {
            public ServiceExceptionScheduleReportData(ServiceExceptionSchedule source)
                : base(source)
            {
                Service = source.Service.ToString();
            }

            public string Service { get; set; }
        }
    }
}