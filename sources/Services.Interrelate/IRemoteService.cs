using Queue.Data.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Interrelate
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IRemoteCallback))]
    public interface IRemoteService
    {
        [OperationContract]
        Task<DateTime> GetDateTime();

        [OperationContract]
        bool IsSubscribed(RemoteServiceEventType eventType);

        [OperationContract]
        void Subscribe(RemoteServiceEventType eventType, RemoteSubscribtionArgs args = null);

        [OperationContract]
        void UnSubscribe(RemoteServiceEventType eventType);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Client> GetClient(Guid clientId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Client> OpenClientSession(Guid sessionId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Client> GetClientByIdentity(string identity);

        [OperationContract]
        Task<Client[]> FindClients(int startIndex, int maxResults, string filter);

        [OperationContract]
        Task<Client> AddClient(string surname, string name, string patronymic, string email, string mobile, string identity, string password);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Client> ClientLogin(string email, string password);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task ClientRestorePassword(string email);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Client> EditClient(Guid clientId, string surname, string name, string patronymic, string email, string mobile);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task ChangeClientPassword(Guid clientId, string password);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteClient(Guid clientId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task SendPINToEmail(string email);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task CheckPIN(string email, int source);

        [OperationContract]
        Task<ClientRequest[]> FindClientRequests(int startIndex, int maxResults, ClientRequestFilter filter, string query);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> GetClientRequest(Guid clientRequestId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequestEvent[]> GetClientRequestEvents(Guid clientRequestId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> AddEarlyClientRequest(Guid clientId, Guid serviceId, DateTime requestDate, TimeSpan requestTime, Dictionary<Guid, object> parameters, int subjects);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> AddLiveClientRequest(Guid clientId, Guid serviceId, bool isPriority, Dictionary<Guid, object> parameters, int subjects);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<string> GetClientRequestCoupon(Guid clientRequestId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> ChangeClientRequestPriority(Guid clientRequestId, bool isPriority);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> ChangeClientRequestSubjects(Guid clientRequestId, int subjects);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> ChangeClientRequestService(Guid clientRequestId, Guid serviceId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> ChangeClientRequestOperator(Guid clientRequestId, Guid operatorId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> CancelClientRequest(Guid clientRequestId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> RestoreClientRequest(Guid clientRequestId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> PostponeClientRequest(Guid clientRequestId, TimeSpan postponeTime);

        [OperationContract]
        Task<ClientRequest[]> GetCallingClientRequests();

        [OperationContract]
        Task<ClientRequestPlan[]> GetOperatorClientRequestPlans();

        [OperationContract]
        Task<ClientRequest[]> GetCurrentClientRequests();

        [OperationContract]
        Task<ClientRequest> GetCurrentClientRequest();

        [OperationContract]
        Task UpdateCurrentClientRequest(ClientRequestState state);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task PostponeCurrentClientRequest(TimeSpan postponeTime);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task ReturnCurrentClientRequest();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> ChangeCurrentClientRequestService(Guid serviceId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequest> ChangeCurrentClientRequestServiceType(ServiceType serviceType);

        [OperationContract]
        Task CallCurrentClient();

        [OperationContract]
        Task<QueuePlan> GetQueuePlan(DateTime planDate);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceFreeTime> GetFreeTime(Guid serviceId, DateTime planDate, ClientRequestType queueType);

        [OperationContract]
        Task RefreshTodayQueuePlan();

        [OperationContract]
        Task<ServiceGroup[]> GetRootServiceGroups();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceGroup[]> GetServiceGroups(Guid parentServiceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceGroup> GetServiceGroup(Guid serviceGroupId);

        [OperationContract]
        Task<ServiceGroup> AddRootServiceGroup();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceGroup> AddServiceGroup(Guid parentServiceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceGroup> EditServiceGroup(Guid serviceGroupId, string code, string name, string comment, string description, string color, byte[] icon);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task MoveServiceGroup(Guid sourceGroupId, Guid targetGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task MoveServiceGroupToRoot(Guid sourceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<bool> ServiceGroupUp(Guid serviceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<bool> ServiceGroupDown(Guid serviceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<bool> ServiceGroupActivate(Guid serviceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<bool> ServiceGroupDeactivate(Guid serviceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteServiceGroup(Guid serviceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<User> OpenUserSession(Guid sessionId);

        [OperationContract]
        Task UserHeartbeat();

        [OperationContract]
        Task<User[]> GetUsers();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<User> GetUser(Guid userId);

        [OperationContract]
        Task<IDictionary<Guid, string>> GetUserList(UserRole userRole);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<User> UserLogin(Guid userId, string password);

        [OperationContract]
        Task<User> AddUser(UserRole role);

        [OperationContract]
        Task<User> EditUser(Guid userId, string surname, string name, string patronymic, string email, string mobile);

        [OperationContract]
        Task DeleteUser(Guid userId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task ChangeUserPassword(Guid userId, string password);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Operator> EditOperator(Guid operatorId, Guid workplaceId, bool isInterruption, TimeSpan interruptionStartTime, TimeSpan interruptionFinishTime);

        [OperationContract]
        Task<Office[]> GetOffices();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Office> GetOffice(Guid zoneId);

        [OperationContract]
        Task<Office> AddOffice();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task EditOffice(Guid zoneId, string name);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task EditOfficeLogin(Guid zoneId, string endpoint, Guid userId, string password);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteOffice(Guid zoneId);

        [OperationContract]
        Task<IDictionary<Guid, string>> GetWorkplacesList();

        [OperationContract]
        Task<Workplace[]> GetWorkplaces();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Workplace> GetWorkplace(Guid workplaceId);

        [OperationContract]
        Task<Workplace> AddWorkplace();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Workplace> EditWorkplace(Guid workplaceId, WorkplaceType type, int number, WorkplaceModificator modificator, string comment, byte display, byte sections);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteWorkplace(Guid workplaceId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<IDictionary<Guid, string>> GetServiceList();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Service[]> GetRootServices();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Service[]> GetServices(Guid serviceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Service> GetService(Guid serviceId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Service[]> FindServices(int startIndex, int maxResults, string filter);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Service> AddRootService();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Service> AddService(Guid serviceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Service> EditService(Guid serviceId, string code, int priority, string name, string comment, string tags, string description, string link, int maxSubjects, int maxEarlyDays, bool clientRequire, ServiceType type, ClientRequestRegistrator liveRegistrator, ClientRequestRegistrator earlyRegistrator);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task MoveService(Guid serviceId, Guid serviceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteService(Guid serviceId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<bool> ServiceUp(Guid serviceId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<bool> ServiceDown(Guid serviceId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task ChangeServiceActivity(Guid serviceId, bool isActive);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Schedule> GetServiceCurrentSchedule(Guid serviceId, DateTime planDate);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceWeekdaySchedule> GetServiceWeekdaySchedule(Guid serviceId, DayOfWeek dayOfWeek);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceWeekdaySchedule> EditServiceWeekdaySchedule(Guid serviceId, DayOfWeek dayOfWeek,
            TimeSpan startTime, TimeSpan finishTime, bool isWorked,
            bool isInterruption, TimeSpan interruptionStartTime, TimeSpan interruptionFinishTime,
            TimeSpan clientInterval, TimeSpan intersection,
            ServiceRenderingMode renderingMode,
            TimeSpan earlyStartTime, TimeSpan earlyFinishTime, int earlyReservation, int maxClientRequests,
            Dictionary<Guid, ServiceRenderingMode> renderings);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteServiceWeekdaySchedule(Guid serviceId, DayOfWeek dayOfWeek);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceExceptionSchedule> GetServiceExceptionSchedule(Guid serviceId, DateTime scheduleDate);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceExceptionSchedule> EditServiceExceptionSchedule(Guid serviceId, DateTime scheduleDate,
            TimeSpan startTime, TimeSpan finishTime, bool isWorked,
            bool isInterruption, TimeSpan interruptionStartTime, TimeSpan interruptionFinishTime,
            TimeSpan clientInterval, TimeSpan intersection,
            ServiceRenderingMode renderingMode,
            TimeSpan earlyStartTime, TimeSpan earlyFinishTime, int earlyReservation, int maxClientRequests,
            Dictionary<Guid, ServiceRenderingMode> renderings);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteServiceExceptionSchedule(Guid serviceId, DateTime scheduleDate);

        [OperationContract]
        Task<DefaultWeekdaySchedule> GetDefaultWeekdaySchedule(DayOfWeek dayOfWeek);

        [OperationContract]
        Task<DefaultWeekdaySchedule> EditDefaultWeekdaySchedule(DayOfWeek dayOfWeek,
            TimeSpan startTime, TimeSpan finishTime, bool isWorked,
            bool isInterruption, TimeSpan interruptionStartTime, TimeSpan interruptionFinishTime,
            TimeSpan clientInterval, TimeSpan intersection,
            ServiceRenderingMode renderingMode,
            TimeSpan earlyStartTime, TimeSpan earlyFinishTime, int earlyReservation, int maxClientRequests,
            Dictionary<Guid, ServiceRenderingMode> renderings);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<DefaultExceptionSchedule> GetDefaultExceptionSchedule(DateTime scheduleDate);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<DefaultExceptionSchedule> EditDefaultExceptionSchedule(DateTime scheduleDate,
            TimeSpan startTime, TimeSpan finishTime, bool isWorked,
            bool isInterruption, TimeSpan interruptionStartTime, TimeSpan interruptionFinishTime,
            TimeSpan clientInterval, TimeSpan intersection,
            ServiceRenderingMode renderingMode,
            TimeSpan earlyStartTime, TimeSpan earlyFinishTime, int earlyReservation, int maxClientRequests,
            Dictionary<Guid, ServiceRenderingMode> renderings);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteDefaultExceptionSchedule(DateTime scheduleDate);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceParameter[]> GetServiceParameters(Guid serviceId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceParameter> AddServiceParameter(Guid serviceId, ServiceParameterType type);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceParameter> EditNumberServiceParameter(Guid parameterId, string name, string tooltip, bool isRequire);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceParameter> EditTextServiceParameter(Guid parameterId, string name, string tooltip, bool isRequire, int minLength, int maxLength);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceParameter> EditOptionsServiceParameter(Guid parameterId, string name, string tooltip, bool isRequire, string options, bool isMultiple);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteServiceParameter(Guid serviceParameterId);

        [OperationContract]
        Task<byte[]> GetServiceRatingReport(Guid[] servicesIds, ServiceRatingReportDetailLavel detailLavel, ServiceRatingReportSettings settings);

        [OperationContract]
        Task<byte[]> GetExceptionScheduleReport(DateTime fromDate);

        [OperationContract]
        Task<byte[]> GetClientRequestReport(Guid clientRequestId);

        [OperationContract]
        Task<DefaultConfig> GetDefaultConfig();

        [OperationContract]
        Task<DefaultConfig> EditDefaultConfig(string queueName, TimeSpan workStartTime, TimeSpan workFinishTime, int maxClientRequests, int maxRenderingTime);

        [OperationContract]
        Task<DesignConfig> GetDesignConfig();

        [OperationContract]
        Task<DesignConfig> EditDesignConfig(byte[] logoSmall);

        [OperationContract]
        Task<CouponConfig> GetCouponConfig();

        [OperationContract]
        Task<string> GetCouponTemplate();

        [OperationContract]
        Task<CouponConfig> EditCouponConfig(string template);

        [OperationContract]
        Task<SMTPConfig> GetSMTPConfig();

        [OperationContract]
        Task<SMTPConfig> EditSMTPConfig(string server, string user, string password, string from);

        [OperationContract]
        Task<PortalConfig> GetPortalConfig();

        [OperationContract]
        Task<PortalConfig> EditPortalConfig(string header, string footer, bool currentDayRecording);

        [OperationContract]
        Task<MediaConfig> GetMediaConfig();

        [OperationContract]
        Task<MediaConfig> EditMediaConfig(string serviceUrl, string creepingLine);

        [OperationContract]
        Task<MediaConfigFile> AddMediaConfigFile();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<MediaConfigFile> EditMediaConfigFile(Guid mediaConfigFileId, string name);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteMediaConfigFile(Guid mediaConfigFileId);

        [OperationContract]
        Task<TerminalConfig> GetTerminalConfig();

        [OperationContract]
        Task<TerminalConfig> EditTerminalConfig(int PIN, bool currentDayRecording, int columns, int rows);

        [OperationContract]
        Task<QueuePlanConfig> GetQueuePlanConfig();

        [OperationContract]
        Task<QueuePlanConfig> EditQueuePlanConfig(int rebuildInterval);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<QueuePlanMetric> GetQueuePlanMetric(int year, int month, int day, int hours, int minutes, int seconds);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<QueuePlanServiceMetric> GetQueuePlanServiceMetric(int year, int month, int day, int hours, int minutes, int seconds, Guid serviceId);
    }
}