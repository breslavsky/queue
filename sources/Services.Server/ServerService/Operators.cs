using AutoMapper;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using System;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    public partial class ServerService
    {
        public async Task<DTO.Operator> EditOperator(DTO.Operator source)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var operatorId = source.Id;

                    var queueOperator = session.Get<Operator>(operatorId);
                    if (queueOperator == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(operatorId), string.Format("Оператор [{0}] не найден", operatorId));
                    }

                    var workplaceId = source.Workplace.Id;

                    var workplace = session.Get<Workplace>(workplaceId);
                    if (workplace == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(workplaceId), string.Format("Рабочее место [{0}] не найдено", workplaceId));
                    }

                    queueOperator.Workplace = workplace;
                    queueOperator.IsInterruption = source.IsInterruption;
                    queueOperator.InterruptionStartTime = source.InterruptionStartTime;
                    queueOperator.InterruptionFinishTime = source.InterruptionFinishTime;

                    var errors = queueOperator.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(queueOperator);
                    transaction.Commit();

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        todayQueuePlan.Put(queueOperator);
                    }

                    return Mapper.Map<Operator, DTO.Operator>(queueOperator);
                }
            });
        }
    }
}