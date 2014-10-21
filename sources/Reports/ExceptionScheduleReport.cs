using NHibernate;
using NHibernate.Criterion;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Queue.Model;
using System;
using System.IO;

namespace Queue.Reports
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

            IDataFormat format = workbook.CreateDataFormat();
            ICellStyle boldCellStyle = CreateCellBoldStyle(workbook);

            IRow row;
            ICell cell;

            using (ISession session = SessionProvider.OpenSession())
            {
                ISheet worksheet = workbook.GetSheetAt(0);
                int rowIndex = worksheet.LastRowNum + 1;

                foreach (var s in session.CreateCriteria<DefaultExceptionSchedule>()
                    .Add(Restrictions.Ge("ScheduleDate", fromDate))
                    .AddOrder(Order.Asc("ScheduleDate"))
                    .List<DefaultExceptionSchedule>())
                {
                    row = worksheet.CreateRow(rowIndex++);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(s.ScheduleDate.ToShortDateString());

                    cell = row.CreateCell(1);
                    cell.SetCellValue(s.IsWorked ? "Да" : "Нет");

                    if (s.IsWorked)
                    {
                        cell = row.CreateCell(2);
                        cell.SetCellValue(s.StartTime.ToString());

                        cell = row.CreateCell(3);
                        cell.SetCellValue(s.FinishTime.ToString());

                        if (s.IsInterruption)
                        {
                            cell = row.CreateCell(4);
                            cell.SetCellValue(s.InterruptionStartTime.ToString());

                            cell = row.CreateCell(5);
                            cell.SetCellValue(s.InterruptionFinishTime.ToString());
                        }

                        cell = row.CreateCell(6);
                        cell.SetCellValue(s.ClientInterval.Minutes);

                        cell = row.CreateCell(7);
                        cell.SetCellValue(s.Intersection.Minutes);

                        cell = row.CreateCell(8);
                        cell.SetCellValue(s.MaxClientRequests);
                    }
                };

                worksheet = workbook.GetSheetAt(1);
                rowIndex = worksheet.LastRowNum + 1;

                var dateTitle = DateTime.MinValue;

                foreach (var s in session.CreateCriteria<ServiceExceptionSchedule>()
                    .Add(Restrictions.Ge("ScheduleDate", fromDate))
                    .AddOrder(Order.Asc("ScheduleDate"))
                    .List<ServiceExceptionSchedule>())
                {
                    if (dateTitle != s.ScheduleDate)
                    {
                        dateTitle = s.ScheduleDate;

                        row = worksheet.CreateRow(rowIndex++);
                        row.RowStyle = boldCellStyle;

                        cell = row.CreateCell(0);
                        cell.SetCellValue(dateTitle.ToShortDateString());
                        cell.CellStyle = boldCellStyle;
                    }

                    row = worksheet.CreateRow(rowIndex++);
                    cell = row.CreateCell(0);
                    cell.SetCellValue(s.Service.ToString());

                    cell = row.CreateCell(1);
                    cell.SetCellValue(s.IsWorked ? "Да" : "Нет");

                    if (s.IsWorked)
                    {
                        cell = row.CreateCell(2);
                        cell.SetCellValue(s.StartTime.ToString());

                        cell = row.CreateCell(3);
                        cell.SetCellValue(s.FinishTime.ToString());

                        if (s.IsInterruption)
                        {
                            cell = row.CreateCell(4);
                            cell.SetCellValue(s.InterruptionStartTime.ToString());

                            cell = row.CreateCell(5);
                            cell.SetCellValue(s.InterruptionFinishTime.ToString());
                        }

                        cell = row.CreateCell(6);
                        cell.SetCellValue(s.ClientInterval.Minutes);

                        cell = row.CreateCell(7);
                        cell.SetCellValue(s.Intersection.Minutes);

                        cell = row.CreateCell(8);
                        cell.SetCellValue(s.MaxClientRequests);
                    }
                };
            }

            return workbook;
        }
    }
}