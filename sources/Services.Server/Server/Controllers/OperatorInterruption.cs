using AutoMapper;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    public partial class ServerService
    {
        public async Task<DTO.OperatorInterruption[]> GetOperatorInterruptions(DTO.OperatorInterruptionFilter filter)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Users);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var criteria = session.CreateCriteria<OperatorInterruption>()
                        .AddOrder(Order.Asc("DayOfWeek"));

                    if (filter.IsOperator)
                    {
                        criteria.Add(Restrictions.Eq("Operator.Id", filter.OperatorId));
                    }

                    var interruptions = criteria.List<OperatorInterruption>();

                    return Mapper.Map<IList<OperatorInterruption>, DTO.OperatorInterruption[]>(interruptions);
                }
            });
        }

        public async Task<DTO.OperatorInterruption> GetOperatorInterruption(Guid operatorInterruptionId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var interruption = session.Get<OperatorInterruption>(operatorInterruptionId);
                    if (interruption == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(operatorInterruptionId),
                            string.Format("Перерыв оператора [{0}] не найден", operatorInterruptionId));
                    }

                    return Mapper.Map<OperatorInterruption, DTO.OperatorInterruption>(interruption);
                }
            });
        }

        public async Task<DTO.OperatorInterruption> EditOperatorInterruption(DTO.OperatorInterruption source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Users);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    OperatorInterruption interruption;

                    if (!source.Empty())
                    {
                        var operatorInterruptionId = source.Id;
                        interruption = session.Get<OperatorInterruption>(operatorInterruptionId);
                        if (interruption == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(operatorInterruptionId),
                                string.Format("Перерыв оператора [{0}] не найден", operatorInterruptionId));
                        }
                    }
                    else
                    {
                        interruption = new OperatorInterruption();
                    }

                    interruption.Type = source.Type;
                    interruption.TargetDate = source.TargetDate.Date;
                    interruption.DayOfWeek = source.DayOfWeek;
                    interruption.StartTime = source.StartTime;
                    interruption.FinishTime = source.FinishTime;
                    interruption.ServiceRenderingMode = source.ServiceRenderingMode;
                    interruption.WeekFold = source.WeekFold;

                    if (source.Operator != null)
                    {
                        Guid operatorId = source.Operator.Id;

                        Operator queueOperator = session.Get<Operator>(operatorId);
                        if (queueOperator == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(operatorId),
                                string.Format("Оператор [{0}] не найден", operatorId));
                        }

                        interruption.Operator = queueOperator;
                    }
                    else
                    {
                        interruption.Operator = null;
                    }

                    var errors = interruption.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(interruption);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Flush(QueuePlanFlushMode.OperatorInterruptions);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }

                    return Mapper.Map<OperatorInterruption, DTO.OperatorInterruption>(interruption);
                }
            });
        }

        public async Task DeleteOperatorInterruption(Guid operatorInterruptionId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Users);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var interruption = session.Get<OperatorInterruption>(operatorInterruptionId);
                    if (interruption == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(operatorInterruptionId),
                            string.Format("Перерыв оператора [{0}] не найден", operatorInterruptionId));
                    }

                    session.Delete(interruption);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Flush(QueuePlanFlushMode.OperatorInterruptions);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }
                }
            });
        }
    }
}