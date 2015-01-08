﻿using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Contracts
{
    [ServiceContract]
    public interface IServerService
    {
        [OperationContract]
        Task<DateTime> GetDateTime();

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
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Client> ClientLogin(string email, string password);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task ClientRestorePassword(string email);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Client> EditClient(Client client);

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
        Task<ClientRequest[]> FindClientRequests(int startIndex, int maxResults, ClientRequestFilter filter);

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
        Task<ClientRequest> EditClientRequest(ClientRequest source);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ClientRequestCoupon> GetClientRequestCoupon(Guid clientRequestId);

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
        Task<ClientRequestPlan[]> GetOperatorClientRequestPlans();

        //TODO: shit!
        [OperationContract]
        Task<Dictionary<Operator, ClientRequestPlan>> GetCurrentClientRequestPlans();

        [OperationContract]
        Task<ClientRequestPlan> GetCurrentClientRequestPlan();

        [OperationContract]
        Task<ClientRequest> EditCurrentClientRequest(ClientRequest source);

        [OperationContract]
        Task UpdateCurrentClientRequest(ClientRequestState state);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task PostponeCurrentClientRequest(TimeSpan postponeTime);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task ReturnCurrentClientRequest();

        [OperationContract]
        Task CallCurrentClient();

        [OperationContract]
        Task<QueuePlan> GetQueuePlan(DateTime planDate);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceFreeTime> GetServiceFreeTime(Guid serviceId, DateTime planDate, ClientRequestType queueType);

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
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceGroup> EditServiceGroup(ServiceGroup serviceGroup);

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
        Task<IdentifiedEntityLink[]> GetUserLinks(UserRole userRole);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<User> UserLogin(Guid userId, string password);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Administrator> EditAdministrator(Administrator administrator);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Operator> EditOperator(Operator user);

        [OperationContract]
        Task DeleteUser(Guid userId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task ChangeUserPassword(Guid userId, string password);

        [OperationContract]
        Task<Office[]> GetOffices();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Office> GetOffice(Guid officeId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Office> EditOffice(Office office);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteOffice(Guid officeId);

        [OperationContract]
        Task<IdentifiedEntityLink[]> GetWorkplacesLinks();

        [OperationContract]
        Task<Workplace[]> GetWorkplaces();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Workplace> GetWorkplace(Guid workplaceId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Workplace> EditWorkplace(Workplace workplace);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteWorkplace(Guid workplaceId);

        [OperationContract]
        Task<Operator[]> GetWorkplaceOperators(Guid workplaceId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<IdentifiedEntityLink[]> GetServiceLinks();

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
        Task<Service[]> FindServices(string filter, int startIndex, int maxResults);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Service> EditService(Service service);

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
        Task<IdentifiedEntityLink[]> GetServiceStepLinks(Guid serviceId);

        [OperationContract]
        Task<ServiceStep[]> GetServiceSteps(Guid serviceId);

        [OperationContract]
        Task<ServiceStep> GetServiceStep(Guid serviceStepId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceStep> EditServiceStep(ServiceStep serviceStep);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteServiceStep(Guid serviceStepId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<bool> ServiceStepUp(Guid serviceStepId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<bool> ServiceStepDown(Guid serviceStepId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<DTO.ServiceRendering[]> GetServiceRenderings(Guid scheduleId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<DTO.ServiceRendering> GetServiceRendering(Guid serviceRenderingId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceRendering> EditServiceRendering(ServiceRendering serviceRendering);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteServiceRendering(Guid serviceRenderingId);

        //[OperationContract]
        //[FaultContract(typeof(ObjectNotFoundFault))]
        //Task<Schedule> GetServiceCurrentSchedule(Guid serviceId, DateTime planDate);

        [OperationContract]
        Task<Schedule> GetDefaultWeekdaySchedule(DayOfWeek dayOfWeek);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Schedule> GetDefaultExceptionSchedule(DateTime scheduleDate);

        [OperationContract]
        Task<Schedule> AddDefaultExceptionSchedule(DateTime scheduleDate);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Schedule> GetServiceWeekdaySchedule(Guid serviceId, DayOfWeek dayOfWeek);

        [OperationContract]
        Task<Schedule> AddServiceWeekdaySchedule(Guid serviceId, DayOfWeek dayOfWeek);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Schedule> GetServiceExceptionSchedule(Guid serviceId, DateTime scheduleDate);

        [OperationContract]
        Task<Schedule> AddServiceExceptionSchedule(Guid serviceId, DateTime scheduleDate);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Schedule> GetSchedule(Guid scheduleId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Schedule> EditSchedule(Schedule schedule);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteSchedule(Guid scheduleId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceParameter> GetServiceParameter(Guid serviceParameterId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceParameter[]> GetServiceParameters(Guid serviceId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceParameterNumber> EditServiceParameterNumber(ServiceParameterNumber parameter);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceParameterText> EditServiceParameterText(ServiceParameterText parameter);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<ServiceParameterOptions> EditServiceParameterOptions(ServiceParameterOptions parameter);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteServiceParameter(Guid serviceParameterId);

        [OperationContract]
        Task<byte[]> GetServiceRatingReport(Guid[] services, ReportDetailLevel detailLavel, ServiceRatingReportSettings settings);

        [OperationContract]
        Task<byte[]> GetOperatorRatingReport(Guid[] operators, ReportDetailLevel detailLavel, OperatorRatingReportSettings settings);

        [OperationContract]
        Task<byte[]> GetExceptionScheduleReport(DateTime fromDate);

        [OperationContract]
        Task<byte[]> GetClientRequestReport(Guid clientRequestId);

        [OperationContract]
        Task<DefaultConfig> GetDefaultConfig();

        [OperationContract]
        Task<DefaultConfig> EditDefaultConfig(DefaultConfig config);

        [OperationContract]
        Task<DesignConfig> GetDesignConfig();

        [OperationContract]
        Task<DesignConfig> EditDesignConfig(DesignConfig config);

        [OperationContract]
        Task<CouponConfig> GetCouponConfig();

        [OperationContract]
        Task<CouponConfig> EditCouponConfig(CouponConfig config);

        [OperationContract]
        Task<SMTPConfig> GetSMTPConfig();

        [OperationContract]
        Task<SMTPConfig> EditSMTPConfig(SMTPConfig config);

        [OperationContract]
        Task<PortalConfig> GetPortalConfig();

        [OperationContract]
        Task<PortalConfig> EditPortalConfig(PortalConfig config);

        [OperationContract]
        Task<MediaConfig> GetMediaConfig();

        [OperationContract]
        Task<MediaConfig> EditMediaConfig(MediaConfig config);

        [OperationContract]
        Task<MediaConfigFile[]> GetMediaConfigFiles();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<MediaConfigFile> GetMediaConfigFile(Guid mediaConfigFileId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<MediaConfigFile> EditMediaConfigFile(MediaConfigFile config);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteMediaConfigFile(Guid mediaConfigFileId);

        [OperationContract]
        Task<TerminalConfig> GetTerminalConfig();

        [OperationContract]
        Task<TerminalConfig> EditTerminalConfig(TerminalConfig config);

        [OperationContract]
        Task<NotificationConfig> GetNotificationConfig();

        [OperationContract]
        Task<NotificationConfig> EditNotificationConfig(NotificationConfig config);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<QueuePlanMetric> GetQueuePlanMetric(int year, int month, int day, int hours, int minutes, int seconds);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<QueuePlanServiceMetric> GetQueuePlanServiceMetric(int year, int month, int day, int hours, int minutes, int seconds, Guid serviceId);
    }
}