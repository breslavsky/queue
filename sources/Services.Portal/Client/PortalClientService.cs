using Junte.WCF.Common;
using log4net;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using System.Xml;
using XamlReader = System.Windows.Markup.XamlReader;

namespace Queue.Services.Portal
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true, UseSynchronizationContext = false)]
    public class PortalClientService : PortalService, IPortalClientService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PortalClientService));

        private Guid sessionId;
        private Client currentClient;

        public PortalClientService(DuplexChannelBuilder<IServerTcpService> channelBuilder, Administrator currentUser)
            : base(channelBuilder, currentUser)
        {
            try
            {
                sessionId = Guid.Parse(Request.Headers[ExtendHttpHeaders.SESSION]);

                using (Channel<IServerTcpService> channel = ChannelBuilder.CreateChannel())
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

        public override Stream Index()
        {
            return GetContent("client\\index.html");
        }

        public void checkLogin()
        {
            if (currentClient == null)
            {
                throw new WebFaultException<string>("Клиент не авторизован", HttpStatusCode.BadRequest);
            }
        }

        public async Task<Client> Login(string email, string password)
        {
            using (var channel = ChannelBuilder.CreateChannel())
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

        public async Task<Stream> ULogin(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var body = reader.ReadToEnd();

                var parameters = HttpUtility.ParseQueryString(body);
                var token = parameters["token"];

                var referer = Request.Headers[HttpRequestHeader.Referer];
                var uri = new System.Uri(referer);
                var query = HttpUtility.ParseQueryString(uri.Query);

                var q = query["q"];

                query = HttpUtility.ParseQueryString(q);

                var url = string.Format("http://ulogin.ru/token.php?token={0}&host={1}", token, query["host"]);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                var responseStream = new StreamReader(httpWebRequest.GetResponse().GetResponseStream(), Encoding.UTF8);

                var serializer = new JavaScriptSerializer();
                var userInfo = serializer.Deserialize<Dictionary<string, string>>(responseStream.ReadToEnd());

                var identity = userInfo["identity"];

                Func<Guid, Stream> result = (sessionId) =>
                {
                    Response.StatusCode = HttpStatusCode.Redirect;
                    Response.Headers[ExtendHttpHeaders.LOCATION] = string.Format("/client/?SessionId={0}", sessionId);
                    return new MemoryStream(Encoding.UTF8.GetBytes("Переадресация..."));
                };

                using (var channel = ChannelBuilder.CreateChannel())
                {
                    try
                    {
                        await channel.Service.OpenUserSession(CurrentAdministrator.SessionId);
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
                        await channel.Service.OpenUserSession(CurrentAdministrator.SessionId);
                        var client = await channel.Service.AddClient(userInfo["last_name"], userInfo["first_name"], string.Empty, string.Empty, string.Empty, identity, string.Empty);
                        return result(client.SessionId);
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                }
            }
        }

        public async Task<bool> RestorePassword(string email)
        {
            using (var channel = ChannelBuilder.CreateChannel())
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

        public Client GetProfile()
        {
            checkLogin();

            return currentClient;
        }

        public async Task<Client> EditProfile(string email, string surname, string name, string patronymic, string mobile)
        {
            checkLogin();

            using (var channel = ChannelBuilder.CreateChannel())
            {
                try
                {
                    currentClient.Email = email;
                    currentClient.Surname = surname;
                    currentClient.Name = name;
                    currentClient.Patronymic = patronymic;
                    currentClient.Mobile = mobile;

                    await channel.Service.OpenUserSession(CurrentAdministrator.SessionId);
                    currentClient = await channel.Service.EditClient(currentClient);
                    return currentClient;
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<ClientRequest[]> GetRequests()
        {
            checkLogin();

            using (var channel = ChannelBuilder.CreateChannel())
            {
                try
                {
                    await channel.Service.OpenUserSession(CurrentAdministrator.SessionId);
                    return await channel.Service.FindClientRequests(0, int.MaxValue, new ClientRequestFilter() { ClientId = currentClient.Id });
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<ClientRequest> CancelRequest(string requestId)
        {
            checkLogin();

            using (var channel = ChannelBuilder.CreateChannel())
            {
                try
                {
                    await channel.Service.OpenUserSession(CurrentAdministrator.SessionId);
                    return await channel.Service.CancelClientRequest(Guid.Parse(requestId));
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<bool> SendPINToEmail(string email)
        {
            using (var channel = ChannelBuilder.CreateChannel())
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

        public async Task<bool> CheckPIN(string email, string PIN)
        {
            using (var channel = ChannelBuilder.CreateChannel())
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

        public async Task<Client> Register(string email, string PIN, string surname, string name, string patronymic, string mobile)
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                try
                {
                    await channel.Service.CheckPIN(email, int.Parse(PIN));
                    return await channel.Service.AddClient(surname, name, patronymic, email, mobile, string.Empty, PIN);
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<ServiceGroup[]> GetRootServiceGroups()
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                return await channel.Service.GetRootServiceGroups();
            }
        }

        public async Task<ServiceGroup[]> GetServiceGroups(string serviceGroupId)
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                return await channel.Service.GetServiceGroups(Guid.Parse(serviceGroupId));
            }
        }

        public async Task<Service[]> GetRootServices()
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                return await channel.Service.GetRootServices();
            }
        }

        public async Task<Service[]> GetServices(string serviceGroupId)
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                return await channel.Service.GetServices(Guid.Parse(serviceGroupId));
            }
        }

        public async Task<Stream> GetRequestCoupon(string requestId)
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                var xpsFile = Path.GetTempFileName() + ".xps";

                var coupon = await channel.Service.GetClientRequestCoupon(Guid.Parse(requestId));
                var thread = new Thread(new ThreadStart(() =>
                {
                    var xmlReader = new XmlTextReader(new StringReader(coupon));
                    var grid = (Grid)XamlReader.Load(xmlReader);

                    using (var container = Package.Open(xpsFile, FileMode.Create))
                    {
                        using (var document = new XpsDocument(container, CompressionOption.SuperFast))
                        {
                            var fixedPage = new FixedPage();
                            fixedPage.Children.Add(grid);

                            var pageConent = new PageContent();
                            ((IAddChild)pageConent).AddChild(fixedPage);

                            var fixedDocument = new FixedDocument();
                            fixedDocument.Pages.Add(pageConent);

                            var xpsDocumentWriter = XpsDocument.CreateXpsDocumentWriter(document);
                            xpsDocumentWriter.Write(fixedDocument);
                        }
                    }
                }));
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
                thread.Join();

                Response.ContentType = "application/vnd.ms-xpsdocument";
                return File.OpenRead(xpsFile);
            }
        }

        public async Task<ServiceFreeTime> GetServiceFreeTime(string planDate, string queueType, string serviceId)
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                try
                {
                    var targetDate = DateTime.Parse(planDate);
                    if (targetDate.Date == DateTime.Today)
                    {
                        await channel.Service.OpenUserSession(CurrentAdministrator.SessionId);
                        var portalConfig = await channel.Service.GetPortalConfig();
                        if (!portalConfig.CurrentDayRecording)
                        {
                            throw new FaultException("Запись на текущий день на портале запрещена");
                        }
                    }

                    return await channel.Service.GetFreeTime(Guid.Parse(serviceId), targetDate, (ClientRequestType)int.Parse(queueType));
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

        public async Task<ClientRequest> AddRequest(string serviceId, string requestDate, string requestTime, string subjects)
        {
            checkLogin();

            using (var channel = ChannelBuilder.CreateChannel())
            {
                try
                {
                    await channel.Service.OpenUserSession(CurrentAdministrator.SessionId);
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
    }
}