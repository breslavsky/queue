using AutoMapper;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    public partial class ServerService
    {
        public async Task<DTO.Client> GetClient(Guid clientId)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var client = session.Get<Client>(clientId);
                    if (client == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientId), string.Format("Клиент [{0}] не найден", clientId));
                    }

                    return Mapper.Map<Client, DTO.Client>(client);
                }
            });
        }

        public async Task<DTO.Client> OpenClientSession(Guid sessionId)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var client = session.CreateCriteria<Client>()
                        .Add(Expression.Eq("SessionId", sessionId))
                        .SetMaxResults(1)
                        .UniqueResult<Client>();
                    if (client == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(sessionId), string.Format("Клиент [{0}] не найден", sessionId));
                    }

                    return Mapper.Map<Client, DTO.Client>(client);
                }
            });
        }

        public async Task<DTO.Client> GetClientByIdentity(string identity)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator);

                if (string.IsNullOrWhiteSpace(identity))
                {
                    throw new FaultException("Идентификация не может быть пустой");
                }

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var client = session.CreateCriteria<Client>()
                        .Add(Expression.Eq("Identity", identity))
                        .SetMaxResults(1)
                        .UniqueResult<Client>();
                    if (client == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(identity), string.Format("Клиент [{0}] не найден", identity));
                    }

                    return Mapper.Map<Client, DTO.Client>(client);
                }
            });
        }

        public async Task<DTO.Client[]> FindClients(int startIndex, int maxResults, string filter)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                {
                    var criteria = session.CreateCriteria<Client>()
                        .AddOrder(Order.Desc("RegisterDate"))
                        .SetFirstResult(startIndex)
                        .SetMaxResults(maxResults);

                    if (filter.Length > 0)
                    {
                        criteria.Add(
                            Expression.Disjunction()
                                .Add(Expression.InsensitiveLike("Surname", filter, MatchMode.Anywhere))
                                .Add(Expression.InsensitiveLike("Name", filter, MatchMode.Anywhere))
                                .Add(Expression.InsensitiveLike("Patronymic", filter, MatchMode.Anywhere))
                                .Add(Expression.InsensitiveLike("Email", filter, MatchMode.Anywhere))
                                .Add(Expression.InsensitiveLike("Mobile", filter, MatchMode.Anywhere))
                        );
                    }

                    var clients = criteria.List<Client>();

                    return Mapper.Map<IList<Client>, DTO.Client[]>(clients);
                }
            });
        }

        public async Task<DTO.Client> AddClient(string surname, string name, string patronymic, string email, string mobile, string identity, string password)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    if (!string.IsNullOrWhiteSpace(email))
                    {
                        int count = session.CreateCriteria<Client>()
                            .Add(Expression.Eq("Email", email))
                            .SetProjection(Projections.RowCount())
                            .UniqueResult<int>();

                        if (count > 0)
                        {
                            throw new FaultException("Клиент с таким электронным адресом уже существует");
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(identity))
                    {
                        int count = session.CreateCriteria<Client>()
                            .Add(Expression.Eq("Identity", identity))
                            .SetProjection(Projections.RowCount())
                            .UniqueResult<int>();

                        if (count > 0)
                        {
                            throw new FaultException("Клиент с данной идентификацией уже существует");
                        }
                    }

                    Client client = null;

                    if (string.IsNullOrWhiteSpace(email) && string.IsNullOrWhiteSpace(identity))
                    {
                        client = session.CreateCriteria<Client>()
                            .Add(new Conjunction()
                                .Add(Expression.Eq("Surname", surname))
                                .Add(Expression.Eq("Name", name))
                                .Add(Expression.Eq("Patronymic", patronymic)))
                            .SetMaxResults(1)
                            .UniqueResult<Client>();
                    }

                    if (client == null)
                    {
                        client = new Client()
                        {
                            Surname = surname,
                            Name = name,
                            Patronymic = patronymic,
                            Email = email,
                            Mobile = mobile,
                            Identity = identity
                        };

                        if (!string.IsNullOrWhiteSpace(password))
                        {
                            try
                            {
                                client.SetPassword(password);
                            }
                            catch (Exception exception)
                            {
                                throw new FaultException(exception.Message);
                            }
                        }

                        var errors = client.Validate();
                        if (errors.Length > 0)
                        {
                            throw new FaultException(errors.First().Message);
                        }

                        session.Save(client);
                        transaction.Commit();

                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            var сonfig = session.Get<SMTPConfig>(ConfigType.SMTP);
                            if (сonfig == null)
                            {
                                throw new SystemException();
                            }

                            string template = @"Вы успешно зарегистрированы, Ваш пароль {Password}";

                            string text = template
                                .Replace("{Password}", password);

                            using (var smtpClient = new SmtpClient(сonfig.Server, сonfig.Port))
                            {
                                try
                                {
                                    smtpClient.UseDefaultCredentials = false;
                                    smtpClient.Credentials = new NetworkCredential(сonfig.User, сonfig.Password);

                                    var message = new MailMessage(сonfig.From, email)
                                    {
                                        Subject = "Вы зарегистрированы",
                                        Body = text
                                    };
                                    smtpClient.Send(message);
                                }
                                catch (Exception exception)
                                {
                                    logger.Error(exception);
                                }
                            }
                        }
                    }

                    return Mapper.Map<Client, DTO.Client>(client);
                }
            });
        }

        public async Task<DTO.Client> ClientLogin(string email, string password)
        {
            return await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new FaultException("Электронный адрес не может быть пустым");
                }

                try
                {
                    new MailAddress(email);
                }
                catch
                {
                    throw new FaultException("Указан не верный электронный адрес");
                }

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var client = session.CreateCriteria<Client>()
                        .Add(Expression.Eq("Email", email))
                        .SetMaxResults(1)
                        .UniqueResult<Client>();
                    if (client == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(email), string.Format("Клиент [{0}] не найден", email));
                    }

                    if (!client.CheckPassword(password))
                    {
                        throw new FaultException("Не верный пароль");
                    }

                    return Mapper.Map<Client, DTO.Client>(client);
                }
            });
        }

        public async Task ClientRestorePassword(string email)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    throw new FaultException("Электронный адрес не может быть пустым");
                }

                try
                {
                    new MailAddress(email);
                }
                catch
                {
                    throw new FaultException("Указан не верный электронный адрес");
                }

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var client = session.CreateCriteria<Client>()
                        .Add(Expression.Eq("Email", email))
                        .SetMaxResults(1)
                        .UniqueResult<Client>();
                    if (client == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(email), string.Format("Клиент [{0}] не найден", email));
                    }

                    SMTPConfig сonfig = session.Get<SMTPConfig>(ConfigType.SMTP);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    var password = new Random().Next(1000000, 9999999).ToString();
                    client.SetPassword(password);

                    session.Save(client);
                    transaction.Commit();

                    var template = @"Ваш пароль {Password}";

                    var text = template
                        .Replace("{Password}", password);

                    using (var smtpClient = new SmtpClient(сonfig.Server, сonfig.Port))
                    {
                        try
                        {
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.Credentials = new NetworkCredential(сonfig.User, сonfig.Password);

                            var message = new MailMessage(сonfig.From, email)
                            {
                                Subject = "Ваш пароль",
                                Body = text
                            };
                            smtpClient.Send(message);
                        }
                        catch (Exception exception)
                        {
                            logger.Error(exception);
                        }
                    }
                }
            });
        }

        public async Task<DTO.Client> EditClient(DTO.Client source)
        {
            return await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientId = source.Id;

                    var client = session.Get<Client>(clientId);
                    if (client == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientId), string.Format("Клиент [{0}] не найден", clientId));
                    }

                    client.Surname = source.Surname;
                    client.Name = source.Name;
                    client.Patronymic = source.Patronymic;
                    client.Email = source.Email;
                    client.Mobile = source.Mobile;

                    var errors = client.Validate();
                    if (errors.Length > 0)
                    {
                        throw new FaultException(errors.First().Message);
                    }

                    session.Save(client);

                    var queuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = queuePlan.WriteLock())
                    {
                        transaction.Commit();

                        queuePlan.Put(client);
                    }

                    return Mapper.Map<Client, DTO.Client>(client);
                }
            });
        }

        public async Task ChangeClientPassword(Guid clientId, string password)
        {
            await Task.Run(() =>
            {
                checkPermission(UserRole.Administrator | UserRole.Manager);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var client = session.Get<Client>(clientId);
                    if (client == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientId), string.Format("Клиент [{0}] не найден", clientId));
                    }

                    try
                    {
                        client.SetPassword(password);
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }

                    session.Save(client);
                    transaction.Commit();
                }
            });
        }

        public async Task DeleteClient(Guid clientId)
        {
            await Task.Run(() =>
            {
                checkPermission(UserRole.Manager | UserRole.Administrator);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var client = session.Get<Client>(clientId);
                    if (client == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientId), string.Format("Клиент [{0}] не найден", clientId));
                    }

                    session.Delete(client);
                    transaction.Commit();
                }
            });
        }
    }
}