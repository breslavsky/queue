﻿using Junte.Data.NHibernate;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using NHibernate.Type;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.ServiceModel;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Reports.ServiceRatingReport
{
    public abstract class BaseDetailedReport<T>
    {
        protected ServiceRatingReportSettings settings;
        protected Guid[] servicesIds;

        protected ISessionProvider SessionProvider
        {
            get { return ServiceLocator.Current.GetInstance<ISessionProvider>(); }
        }

        public BaseDetailedReport(Guid[] servicesIds, ServiceRatingReportSettings settings)
        {
            this.settings = settings;
            this.servicesIds = servicesIds;
        }

        public HSSFWorkbook Generate()
        {
            DateTime startDate = GetStartDate();
            DateTime finishDate = GetFinishDate();

            if (startDate > finishDate)
            {
                throw new FaultException("Начальная дата не может быть больше чем конечная");
            }

            Conjunction conjunction = Expression.Conjunction();
            if (servicesIds.Length > 0)
            {
                conjunction.Add(Expression.In("Service.Id", servicesIds));
            }

            conjunction.Add(Expression.Ge("RequestDate", startDate));
            conjunction.Add(Expression.Le("RequestDate", finishDate));

            ProjectionList projections = GetProjections();

            using (ISession session = SessionProvider.OpenSession())
            {
                T[] data = session.CreateCriteria<ClientRequest>()
                       .Add(conjunction)
                       .SetProjection(projections)
                       .SetResultTransformer(Transformers.AliasToBean(typeof(T)))
                       .List<T>()
                       .ToArray();

                if (data.Length == 0)
                {
                    throw new FaultException("Пустой отчет");
                }

                HSSFWorkbook workbook = new HSSFWorkbook(new MemoryStream(Templates.ServiceRating));
                ISheet worksheet = workbook.GetSheetAt(0);

                IDataFormat format = workbook.CreateDataFormat();
                ICellStyle boldCellStyle = CreateCellBoldStyle(workbook);

                IRow row;
                ICell cell;
                int rowIndex = worksheet.LastRowNum + 1;

                row = worksheet.GetRow(0);
                cell = row.CreateCell(0);
                cell.SetCellValue(string.Format("Период с {0} по {1}", startDate.ToShortDateString(), finishDate.ToShortDateString()));
                cell.CellStyle = boldCellStyle;

                RenderData(worksheet, data);

                return workbook;
            }
        }

        protected abstract DateTime GetStartDate();

        protected abstract DateTime GetFinishDate();

        protected abstract ProjectionList GetProjections();

        protected abstract void RenderData(ISheet worksheet, T[] data);

        protected ProjectionList GetCommonProjections()
        {
            ProjectionList projections = Projections.ProjectionList();

            projections
                .Add(Projections.GroupProperty("Service"))
                .Add(Projections.Property("Service"), "Service")

                .Add(Projections.RowCount(), "Total")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Live),
                    Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Live")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Early),
                    Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Early")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Waiting),
                    Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Waiting")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Absence),
                    Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Absence")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                    Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Rendered")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Canceled),
                    Projections.Constant(1, NHibernateUtil.Int32), Projections.Constant(0, NHibernateUtil.Int32))), "Canceled")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                    Projections.SqlProjection("({alias}.RenderFinishTime - {alias}.RenderStartTime) / Subjects as RenderTime", new string[] { "RenderTime" }, new IType[] { NHibernateUtil.TimeSpan }),
                    Projections.Constant(TimeSpan.Zero, NHibernateUtil.TimeSpan))), "RenderTime")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("State", ClientRequestState.Rendered),
                    Projections.SqlProjection("({alias}.RenderStartTime - {alias}.WaitingStartTime) as WaitingTime", new string[] { "WaitingTime" }, new IType[] { NHibernateUtil.TimeSpan }),
                    Projections.Constant(TimeSpan.Zero, NHibernateUtil.TimeSpan))), "WaitingTime")

                .Add(Projections.Sum("Subjects"), "SubjectsTotal")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Live),
                    Projections.Property("Subjects"), Projections.Constant(0, NHibernateUtil.Int32))), "SubjectsLive")

                .Add(Projections.Sum(Projections.Conditional(Restrictions.Eq("Type", ClientRequestType.Early),
                    Projections.Property("Subjects"), Projections.Constant(0, NHibernateUtil.Int32))), "SubjectsEarly");

            if (settings.IsServiceTypes)
            {
                projections
                    .Add(Projections.GroupProperty("ServiceType"))
                    .Add(Projections.Property("ServiceType"), "ServiceType");
            }

            return projections;
        }

        protected void RenderRating(IRow row, ServiceRating rating)
        {
            ICell cell = row.CreateCell(5);
            cell.SetCellValue(rating.Total);
            cell = row.CreateCell(6);
            cell.SetCellValue(rating.Live);
            cell = row.CreateCell(7);
            cell.SetCellValue(rating.Early);
            cell = row.CreateCell(8);
            cell.SetCellValue(rating.Waiting);
            cell = row.CreateCell(9);
            cell.SetCellValue(rating.Absence);
            cell = row.CreateCell(10);
            cell.SetCellValue(rating.Rendered);
            cell = row.CreateCell(11);
            cell.SetCellValue(rating.Canceled);
            cell = row.CreateCell(12);
            cell.SetCellValue(rating.Rendered != 0 ? Math.Round(rating.RenderTime.TotalMinutes / rating.Rendered) : 0);
            cell = row.CreateCell(13);
            cell.SetCellValue(rating.Rendered != 0 ? Math.Round(rating.WaitingTime.TotalMinutes / rating.Rendered) : 0);
            cell = row.CreateCell(14);
            cell.SetCellValue(rating.SubjectsTotal);
            cell = row.CreateCell(15);
            cell.SetCellValue(rating.SubjectsLive);
            cell = row.CreateCell(16);
            cell.SetCellValue(rating.SubjectsEarly);
        }

        protected ServiceGroupDto GetServicesHierarchy()
        {
            using (ISession session = SessionProvider.OpenSession())
            {
                IList<ServiceDto> rootServices = new List<ServiceDto>();
                IList<ServiceGroupDto> rootGroups = new List<ServiceGroupDto>();

                if (servicesIds.Length == 0)
                {
                    rootServices = GetGroupServices(session, null);
                    rootGroups = GetServicesGroups(session, null);
                }
                else
                {
                    foreach (Guid serviceId in servicesIds)
                    {
                        Service service = session.Get<Service>(serviceId);
                        if (service == null)
                        {
                            continue;
                        }

                        if (service.ServiceGroup == null)
                        {
                            rootServices.Add(new ServiceDto(service));
                        }
                        else
                        {
                            AddServiceToRoot(session, service, rootGroups);
                        }
                    }
                }

                return new ServiceGroupDto()
                    {
                        Id = Guid.Empty,
                        ServicesGroups = rootGroups.ToArray(),
                        Services = rootServices.ToArray()
                    };
            }
        }

        private void AddServiceToRoot(ISession session, Service service, IList<ServiceGroupDto> rootGroups)
        {
            ServiceGroup current = service.ServiceGroup;
            ServiceGroupDto group = null;

            //TODO: no no!
            while (true)
            {
                ServiceGroupDto _group = FindServiceGroup(rootGroups, current.Id);

                if (_group != null)
                {
                    if (group == null)
                    {
                        _group.Services.Add(new ServiceDto(service));
                    }
                    else
                    {
                        _group.ServicesGroups.Add(group);
                    }
                    break;
                }

                if (group == null)
                {
                    group = new ServiceGroupDto(current);
                    group.Services.Add(new ServiceDto(service));
                }
                else
                {
                    ServiceGroupDto tmp = new ServiceGroupDto(current);
                    tmp.ServicesGroups.Add(group);
                    group = tmp;
                }

                if (current.ParentGroup == null)
                {
                    rootGroups.Add(group);
                    break;
                }
                else
                {
                    current = current.ParentGroup;
                }
            }
        }

        private ServiceGroupDto FindServiceGroup(IList<ServiceGroupDto> groups, Guid id)
        {
            foreach (ServiceGroupDto group in groups)
            {
                if (group.Id == id)
                {
                    return group;
                }

                ServiceGroupDto result = FindServiceGroup(group.ServicesGroups, id);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }

        private IList<ServiceGroupDto> GetServicesGroups(ISession session, ServiceGroup parent = null)
        {
            IList<ServiceGroup> groups = session.QueryOver<ServiceGroup>()
                                                .Where(g => g.ParentGroup == parent)
                                                .OrderBy(g => g.SortId).Asc
                                                .List();

            IList<ServiceGroupDto> result = new List<ServiceGroupDto>();
            foreach (ServiceGroup group in groups)
            {
                result.Add(new ServiceGroupDto()
                {
                    Id = group.Id,
                    Name = group.ToString(),
                    Services = GetGroupServices(session, group).ToArray(),
                    ServicesGroups = GetServicesGroups(session, group).ToArray()
                });
            }

            return result;
        }

        private IList<ServiceDto> GetGroupServices(ISession session, ServiceGroup group)
        {
            return session.QueryOver<Service>()
                     .Where(s => s.ServiceGroup == group)
                     .OrderBy(s => s.SortId).Asc
                     .List()
                     .Select(s => new ServiceDto(s))
                     .ToList();
        }

        protected ICellStyle CreateCellBoldStyle(IWorkbook workBook)
        {
            ICellStyle boldCellStyle = workBook.CreateCellStyle();

            IFont font = workBook.CreateFont();
            font.Boldweight = 1000;
            boldCellStyle.SetFont(font);

            return boldCellStyle;
        }

        protected void WriteBoldCell(IRow row, int cellIndex, Action<ICell> setValue)
        {
            ICell cell = row.CreateCell(cellIndex);
            setValue(cell);
            cell.CellStyle = CreateCellBoldStyle(row.Sheet.Workbook);
        }

        protected void WriteServicesRatings(ISheet worksheet, ServiceRating[] ratings, ServiceGroupDto root, ref int rowIndex)
        {
            foreach (ServiceGroupDto subGroup in root.ServicesGroups)
            {
                WriteServiceGroupData(worksheet, ratings, subGroup, ref rowIndex);
            }

            foreach (ServiceDto service in root.Services)
            {
                WriteServiceData(worksheet, ratings, service, ref rowIndex);
            }
        }

        protected void WriteServiceGroupData(ISheet worksheet, ServiceRating[] ratings, ServiceGroupDto group, ref int rowIndex)
        {
            WriteBoldCell(worksheet.CreateRow(rowIndex++), 0, c => c.SetCellValue(group.Name));

            foreach (ServiceGroupDto subGroup in group.ServicesGroups)
            {
                WriteServiceGroupData(worksheet, ratings, subGroup, ref rowIndex);
            }

            foreach (ServiceDto service in group.Services)
            {
                WriteServiceData(worksheet, ratings, service, ref rowIndex);
            }
        }

        protected void WriteServiceData(ISheet worksheet, ServiceRating[] ratings, ServiceDto service, ref int rowIndex)
        {
            IRow row = worksheet.CreateRow(rowIndex++);
            ICell cell = row.CreateCell(4);
            cell.SetCellValue(service.Name);

            if (settings.IsServiceTypes && service.Type != ServiceType.None)
            {
                cell.CellStyle = CreateCellBoldStyle(worksheet.Workbook);

                ResourceManager translation = Translation.ServiceType.ResourceManager;

                foreach (ServiceType serviceType in Enum.GetValues(typeof(ServiceType)))
                {
                    row = worksheet.CreateRow(rowIndex++);
                    cell = row.CreateCell(4);
                    cell.SetCellValue(translation.GetString(serviceType.ToString()));

                    RenderRating(row, ratings.FirstOrDefault(r => r.Service.Id == service.Id && r.ServiceType.Equals(serviceType)) ??
                                        new ServiceRating());
                }
            }
            else
            {
                RenderRating(row, ratings.FirstOrDefault(r => r.Service.Id == service.Id) ??
                                        new ServiceRating());
            }
        }

        protected class ServiceGroupDto
        {
            public Guid Id;
            public string Name;
            public IList<ServiceDto> Services = new List<ServiceDto>();
            public IList<ServiceGroupDto> ServicesGroups = new List<ServiceGroupDto>();

            public ServiceGroupDto()
            {
            }

            public ServiceGroupDto(ServiceGroup source)
            {
                Id = source.Id;
                Name = source.Name;
                Services = new List<ServiceDto>();
                ServicesGroups = new List<ServiceGroupDto>();
            }
        }

        protected class ServiceDto
        {
            public Guid Id;
            public string Name;
            public ServiceType Type;

            public ServiceDto(Service source)
            {
                Id = source.Id;
                Name = source.ToString();
                Type = source.Type;
            }
        }
    }
}