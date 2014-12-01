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
        public async Task<DTO.User> OpenUserSession(Guid sessionId)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
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
                        var todayQueuePlan = queueInstance.TodayQueuePlan;
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
                checkPermission(UserRole.All);

                using (var session = sessionProvider.OpenSession())
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
                            var todayQueuePlan = queueInstance.TodayQueuePlan;
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
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
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

        public async Task<DTO.Operator[]> GetOperators()
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var users = session.CreateCriteria<Operator>()
                        .AddOrder(Order.Asc("Surname"))
                        .AddOrder(Order.Asc("Name"))
                        .AddOrder(Order.Asc("Patronymic"))
                        .List<Operator>();

                    return Mapper.Map<IList<Operator>, DTO.Operator[]>(users);
                }
            });
        }

        public async Task<DTO.User> GetUser(Guid userId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var user = session.Get<User>(userId);
                    if (user == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(userId), string.Format("Пользователь [{0}] не найден", userId));
                    }

                    if (user is Administrator || user is Manager)
                    {
                        checkPermission(UserRole.Administrator);
                    }

                    return Mapper.Map<User, DTO.User>(user);
                }
            });
        }

        public async Task<DTO.UserLink[]> GetUserLinks(UserRole userRole)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Type userType;
                    switch (userRole)
                    {
                        case UserRole.Administrator:
                            userType = typeof(Administrator);
                            break;

                        case UserRole.Manager:
                            userType = typeof(Manager);
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
                        .List<User>();

                    return Mapper.Map<IList<User>, DTO.UserLink[]>(users);
                }
            });
        }

        public async Task<DTO.User> UserLogin(Guid userId, string password)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var user = session.Get<User>(userId);
                    if (user == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(userId), string.Format("Пользователь [{0}] не найден", userId));
                    }

                    if (!user.CheckPassword(password))
                    {
                        throw new FaultException("Не верный пароль");
                    }

                    var hasGone = user.HasGone;

                    user.SessionId = Guid.NewGuid();
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
                        var todayQueuePlan = queueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.WriteLock())
                        {
                            todayQueuePlan.Put(user);

                            if (hasGone)
                            {
                                todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                            }
                        }
                    }

                    queueInstance.Event(queueEvent);

                    currentUser = user;

                    return Mapper.Map<User, DTO.User>(user);
                }
            });
        }

        public async Task ChangeUserPassword(Guid userId, string password)
        {
            await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    User user = session.Get<User>(userId);
                    if (user == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(userId), string.Format("Пользователь [{0}] не найден", userId));
                    }

                    if (user is Administrator)
                    {
                        checkPermission(UserRole.Administrator);
                    }

                    if (user is Manager || user is Operator)
                    {
                        checkPermission(UserRole.Administrator | UserRole.Manager);
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

        public async Task<DTO.User> AddUser(UserRole role)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    User user;

                    switch (role)
                    {
                        case UserRole.Administrator:
                            user = new Administrator();
                            break;

                        case UserRole.Manager:
                            user = new Manager();
                            break;

                        case UserRole.Operator:
                            var workplace = session.CreateCriteria<Workplace>()
                                .AddOrder(Order.Asc("Number"))
                                .SetMaxResults(1)
                                .UniqueResult<Workplace>();
                            if (workplace == null)
                            {
                                throw new FaultException("Добавьте хотя бы одно рабочее место в справочник");
                            }

                            user = new Operator()
                            {
                                Workplace = workplace
                            };
                            break;

                        default:
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(role), "Указанная роль пользователя не найдена");
                    }

                    session.Save(user);
                    transaction.Commit();

                    return Mapper.Map<User, DTO.User>(user);
                }
            });
        }

        public async Task<DTO.User> EditUser(DTO.User source)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var userId = source.Id;

                    var user = session.Get<User>(userId);
                    if (user == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(userId), string.Format("Пользователь [{0}] не найден", userId));
                    }

                    user.Surname = source.Surname;
                    user.Name = source.Name;
                    user.Patronymic = source.Patronymic;
                    user.Email = source.Email;
                    user.Mobile = source.Mobile;

                    var errors = user.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(user);

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(user);
                    }

                    return Mapper.Map<User, DTO.User>(user);
                }
            });
        }

        public async Task DeleteUser(Guid userId)
        {
            await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
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