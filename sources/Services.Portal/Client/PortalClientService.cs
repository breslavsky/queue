using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Portal;
using Queue.Services.DTO;
using Queue.UI.Common;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;

namespace Queue.Services.Portal
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class PortalClientService : PortalService, IPortalClientService
    {
        #region dependency

        [Dependency]
        public ITemplateManager TemplateManager { get; set; }

        #endregion dependency

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private Client currentClient;
        private Guid sessionId;

        public PortalClientService()
            : base()
        {
            try
            {
                sessionId = Guid.Parse(request.Headers[ExtendHttpHeaders.Session]);

                using (var channel = ChannelManager.CreateChannel())
                {
                    try
                    {
                        currentClient = channel.Service.OpenClientSession(sessionId).Result;
                    }
                    catch (FaultException exception)
                    {
                        throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.Warn(exception);
            }
        }

        public async Task<ClientRequest> AddRequest(string serviceId, string requestDate, string requestTime, string subjects)
        {
            checkLogin();

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    return await channel.Service.AddEarlyClientRequest(currentClient.Id, Guid.Parse(serviceId), DateTime.Parse(requestDate), TimeSpan.Parse(requestTime), new Dictionary<Guid, object>() { }, int.Parse(subjects));
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
                catch (Exception exception)
                {
                    logger.Error(exception);
                    throw exception;
                }
            }
        }

        public async Task<ClientRequest> CancelRequest(string requestId)
        {
            checkLogin();

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    return await channel.Service.CancelClientRequest(Guid.Parse(requestId));
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public void checkLogin()
        {
            if (currentClient == null)
            {
                throw new WebFaultException<string>("Клиент не авторизован", HttpStatusCode.BadRequest);
            }
        }

        public async Task<bool> CheckPIN(string email, string PIN)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    await channel.Service.CheckPIN(email, int.Parse(PIN));
                    return true;
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<Client> EditProfile(string surname, string name, string patronymic, string mobile)
        {
            checkLogin();

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    currentClient.Surname = surname;
                    currentClient.Name = name;
                    currentClient.Patronymic = patronymic;
                    currentClient.Mobile = mobile;

                    currentClient = await channel.Service.EditClient(currentClient);
                    return currentClient;
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public Client GetProfile()
        {
            checkLogin();

            return currentClient;
        }

        public async Task<Stream> GetRequestCoupon(string requestId)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                string xpsFile = string.Empty;

                var coupon = await channel.Service.GetClientRequestCoupon(Guid.Parse(requestId));

                var thread = new Thread(new ThreadStart(() =>
                {
                    var template = TemplateManager.GetTemplate(Templates.Coupon);
                    xpsFile = XPSUtils.WriteXaml(template, coupon);
                }));

                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();

                response.ContentType = "application/vnd.ms-xpsdocument";
                return File.OpenRead(xpsFile);
            }
        }

        public async Task<ClientRequest[]> GetRequests()
        {
            checkLogin();

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    return await channel.Service.FindClientRequests(0, 20, new ClientRequestFilter()
                    {
                        IsClient = true,
                        ClientId = currentClient.Id
                    });
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<ServiceGroup[]> GetRootServiceGroups()
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                return await channel.Service.GetRootServiceGroups();
            }
        }

        public async Task<Service[]> GetRootServices()
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                return await channel.Service.GetRootServices();
            }
        }

        public async Task<ServiceFreeTime> GetServiceFreeTime(string planDate, string queueType, string serviceId)
        {
            try
            {
                var targetDate = DateTime.Parse(planDate);
                if (targetDate.Date == DateTime.Today)
                {
                    using (var channel = ChannelManager.CreateChannel())
                    {
                        var portalConfig = await channel.Service.GetPortalConfig();
                        if (!portalConfig.CurrentDayRecording)
                        {
                            throw new FaultException("Запись на текущий день на портале запрещена");
                        }
                    }
                }

                using (var channel = ChannelManager.CreateChannel())
                {
                    return await channel.Service.GetServiceFreeTime(Guid.Parse(serviceId), targetDate, (ClientRequestType)int.Parse(queueType));
                }
            }
            catch (FaultException exception)
            {
                throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                throw exception;
            }
        }

        public async Task<ServiceGroup[]> GetServiceGroups(string serviceGroupId)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                return await channel.Service.GetServiceGroups(Guid.Parse(serviceGroupId));
            }
        }

        public async Task<Service[]> GetServices(string serviceGroupId)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                return await channel.Service.GetServices(Guid.Parse(serviceGroupId));
            }
        }

        public override Stream Index()
        {
            return GetContent("client\\index.html");
        }

        public async Task<Client> Login(string email, string password)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    return await channel.Service.ClientLogin(email, password);
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<Client> Register(string email, string PIN, string surname, string name, string patronymic, string mobile)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    await channel.Service.CheckPIN(email, int.Parse(PIN));
                    return await channel.Service.EditClient(new Client()
                    {
                        Surname = surname,
                        Name = name,
                        Patronymic = patronymic,
                        Email = email,
                        Mobile = mobile,
                        Password = PIN
                    });
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<bool> RestorePassword(string email)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    await channel.Service.ClientRestorePassword(email);
                    return true;
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<bool> SendPINToEmail(string email)
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    await channel.Service.SendPINToEmail(email);
                    return true;
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<Stream> ULogin(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var body = reader.ReadToEnd();

                var parameters = HttpUtility.ParseQueryString(body);
                var token = parameters["token"];

                var url = string.Format("http://ulogin.ru/token.php?token={0}&host={1}", token, request.Headers[HttpRequestHeader.Host]);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var responseStream = new StreamReader(httpWebRequest.GetResponse().GetResponseStream(), Encoding.UTF8);

                var serializer = new JavaScriptSerializer();
                var userInfo = serializer.Deserialize<Dictionary<string, string>>(responseStream.ReadToEnd());

                var identity = userInfo["identity"];

                Func<Guid, Stream> result = (sessionId) =>
                {
                    response.StatusCode = HttpStatusCode.Redirect;
                    response.Headers[ExtendHttpHeaders.Location] = string.Format("/client/?SessionId={0}", sessionId);
                    return new MemoryStream(Encoding.UTF8.GetBytes("Переадресация..."));
                };

                using (var channel = ChannelManager.CreateChannel())
                {
                    try
                    {
                        var client = await channel.Service.GetClientByIdentity(identity);
                        return result(client.SessionId);
                    }
                    catch (FaultException<ObjectNotFoundFault>)
                    {
                        // go to next
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }

                    try
                    {
                        var client = await channel.Service.EditClient(new Client()
                        {
                            Surname = userInfo["last_name"],
                            Name = userInfo["first_name"]
                        });

                        return result(client.SessionId);
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                }
            }
        }

        public Task<Client> EditProfile(string email, string surname, string name, string patronymic, string mobile)
        {
            throw new NotImplementedException();
        }
    }
}