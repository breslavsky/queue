using AutoMapper;
using Junte.Data.NHibernate;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public class UserService : StandardServerService, IUserService
    {
        public UserService()
            : base()
        {
        }

        public async Task<DTO.User> OpenUserSession(Guid sessionId)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var user = session.CreateCriteria<User>()
                        .Add(Expression.Eq("SessionId", sessionId))
                        .SetMaxResults(1)
                        .UniqueResult<User>();
                    if (user == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(sessionId), "Идентификатор сессии пользователя не найден в базе данных либо устарел. Произведите повторный вход в систему.");
                    }

                    var hasGone = user.HasGone;

                    user.Heartbeat = DateTime.Now;
                    session.Save(user);

                    transaction.Commit();

                    if (user is Operator)
                    {
                        var todayQueuePlan = QueueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.WriteLock())
                        {
                            todayQueuePlan.Put(user);

                            if (hasGone)
                            {
                                todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                            }
                        }
                    }

                    currentUser = user;

                    return Mapper.Map<User, DTO.User>(user);
                }
            });
        }

        public async Task UserHeartbeat()
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.All);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    lock (currentUser)
                    {
                        var user = session.Get<User>(currentUser.GetId());

                        var hasGone = user.HasGone;

                        user.Heartbeat = DateTime.Now;
                        session.Save(user);

                        transaction.Commit();

                        if (user is Operator)
                        {
                            var todayQueuePlan = QueueInstance.TodayQueuePlan;
                            using (var locker = todayQueuePlan.WriteLock())
                            {
                                todayQueuePlan.Put(user);

                                if (hasGone)
                                {
                                    todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                                }
                            }
                        }

                        currentUser = user;
                    }
                }
            });
        }

        public async Task<DTO.User[]> GetUsers()
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Users);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var users = session.CreateCriteria<User>()
                        .AddOrder(Order.Asc("Surname"))
                        .AddOrder(Order.Asc("Name"))
                        .AddOrder(Order.Asc("Patronymic"))
                        .List<User>();

                    return Mapper.Map<IList<User>, DTO.User[]>(users);
                }
            });
        }

        public async Task<DTO.User> GetUser(Guid userId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Users);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var user = session.Get<User>(userId);
                    if (user == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(userId), string.Format("Пользователь [{0}] не найден", userId));
                    }

                    return Mapper.Map<User, DTO.User>(user);
                }
            });
        }

        public async Task<DTO.IdentifiedEntityLink[]> GetUserLinks(UserRole userRole)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Type userType;
                    switch (userRole)
                    {
                        case UserRole.Administrator:
                            userType = typeof(Administrator);
                            break;

                        case UserRole.Operator:
                            userType = typeof(Operator);
                            break;

                        default:
                            throw new FaultException("Указанная роль пользователя не найдена");
                    }

                    var users = session.CreateCriteria(userType)
                        .AddOrder(Order.Asc("Surname"))
                        .AddOrder(Order.Asc("Name"))
                        .AddOrder(Order.Asc("Patronymic"))
                        .List<IdentifiedEntity>();
                    return Mapper.Map<IList<IdentifiedEntity>, DTO.IdentifiedEntityLink[]>(users);
                }
            });
        }

        public async Task<DTO.IdentifiedEntityLink[]> GetRedirectOperatorsLinks()
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                var queueOperator = (Operator)currentUser;

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var operators = session.CreateCriteria<Operator>()
                        .AddOrder(Order.Asc("Surname"))
                        .AddOrder(Order.Asc("Name"))
                        .AddOrder(Order.Asc("Patronymic"))
                        .Add(Restrictions.Not(Restrictions.Eq("Id", queueOperator.Id)))
                        .Add(Restrictions.Gt("Heartbeat", DateTime.Now - TimeSpan.FromSeconds(User.GoneTimeout)))
                        .List<IdentifiedEntity>();
                    return Mapper.Map<IList<IdentifiedEntity>, DTO.IdentifiedEntityLink[]>(operators);
                }
            });
        }

        public async Task<DTO.User> UserLogin(Guid userId, string password)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var user = session.Get<User>(userId);
                    if (user == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(userId),
                            string.Format("Пользователь [{0}] не найден", userId));
                    }

                    if (!user.CheckPassword(password))
                    {
                        throw new FaultException("Неверный пароль");
                    }

                    var hasGone = user.HasGone;

                    if (user.HasLost)
                    {
                        user.SessionId = Guid.NewGuid();
                    }
                    user.Heartbeat = DateTime.Now;
                    session.Save(user);

                    var queueEvent = new UserEvent()
                    {
                        User = user,
                        Message = string.Format("Пользователь вошел в систему [{0}]", user)
                    };
                    session.Save(queueEvent);

                    transaction.Commit();

                    if (user is Operator)
                    {
                        var todayQueuePlan = QueueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.WriteLock())
                        {
                            todayQueuePlan.Put(user);

                            if (hasGone)
                            {
                                todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                            }
                        }
                    }

                    QueueInstance.Event(queueEvent);

                    currentUser = user;

                    return Mapper.Map<User, DTO.User>(user);
                }
            });
        }

        public async Task ChangeUserPassword(Guid userId, string password)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.All);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    User user = session.Get<User>(userId);
                    if (user == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(userId),
                            string.Format("Пользователь [{0}] не найден", userId));
                    }

                    if (!user.Equals(currentUser))
                    {
                        CheckPermission(UserRole.Administrator, AdministratorPermissions.Users);
                    }

                    try
                    {
                        user.SetPassword(password);
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }

                    session.Save(user);
                    transaction.Commit();
                }
            });
        }

        public async Task<DTO.Administrator> EditAdministrator(DTO.Administrator source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Users);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Administrator administrator;

                    if (!source.Empty())
                    {
                        var administratorId = source.Id;
                        administrator = session.Get<Administrator>(administratorId);
                        if (administrator == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(administratorId),
                                string.Format("Администратор [{0}] не найден", administratorId));
                        }
                    }
                    else
                    {
                        administrator = new Administrator();
                    }

                    administrator.Surname = source.Surname;
                    administrator.Name = source.Name;
                    administrator.Patronymic = source.Patronymic;
                    administrator.Email = source.Email;
                    administrator.Mobile = source.Mobile;
                    administrator.IsActive = source.IsActive;
                    administrator.Permissions = source.Permissions;

                    var errors = administrator.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(administrator);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(administrator);
                    }

                    return Mapper.Map<Administrator, DTO.Administrator>(administrator);
                }
            });
        }

        public async Task<DTO.Operator> EditOperator(DTO.Operator source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Users);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Operator queueOperator;

                    if (!source.Empty())
                    {
                        var operatorId = source.Id;
                        queueOperator = session.Get<Operator>(operatorId);
                        if (queueOperator == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(operatorId),
                                string.Format("Оператор [{0}] не найден", operatorId));
                        }
                    }
                    else
                    {
                        queueOperator = new Operator();
                    }

                    queueOperator.Surname = source.Surname;
                    queueOperator.Name = source.Name;
                    queueOperator.Patronymic = source.Patronymic;
                    queueOperator.Email = source.Email;
                    queueOperator.Mobile = source.Mobile;
                    queueOperator.IsActive = source.IsActive;
                    queueOperator.Identity = source.Identity;

                    if (source.Workplace != null)
                    {
                        Guid workplaceId = source.Workplace.Id;

                        Workplace workplace = session.Get<Workplace>(workplaceId);
                        if (workplace == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(workplaceId),
                                string.Format("Рабочее место [{0}] не найдено", workplaceId));
                        }

                        queueOperator.Workplace = workplace;
                    }
                    else
                    {
                        queueOperator.Workplace = null;
                    }

                    var errors = queueOperator.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(queueOperator);

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(queueOperator);
                    }

                    return Mapper.Map<Operator, DTO.Operator>(queueOperator);
                }
            });
        }

        public async Task DeleteUser(Guid userId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.Users);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var user = session.Get<User>(userId);
                    if (user == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(userId), string.Format("Пользователь [{0}] не найден", userId));
                    }

                    if (currentUser.Equals(user))
                    {
                        throw new FaultException("Не возможно удалить самого себя");
                    }

                    if (typeof(Administrator).IsInstanceOfType(user))
                    {
                        int count = session.CreateCriteria<Administrator>().
                            SetProjection(Projections.Count(Projections.Id())).
                            UniqueResult<int>();

                        if (count <= 1)
                        {
                            throw new FaultException("В системе должно оставаться не менее 1 администратора");
                        }
                    }

                    session.Delete(user);
                    transaction.Commit();
                }
            });
        }
    }
}