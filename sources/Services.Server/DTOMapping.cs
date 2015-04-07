using AutoMapper;
using Junte.Data.NHibernate;
using Queue.Model;

namespace Queue.Services.Server
{
    public class ParticularDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<IdentifiedEntity, DTO.IdentifiedEntityLink>()
                .ForMember(d => d.Presentation, o => o.MapFrom(s => s.ToString()));
        }
    }

    public class ConfigDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<MediaConfigFile, DTO.MediaConfigFile>();

            Mapper.CreateMap<Config, DTO.Config>()
                 .Include<DefaultConfig, DTO.DefaultConfig>()
                 .Include<DesignConfig, DTO.DesignConfig>()
                 .Include<CouponConfig, DTO.CouponConfig>()
                 .Include<SMTPConfig, DTO.SMTPConfig>()
                 .Include<TerminalConfig, DTO.TerminalConfig>()
                 .Include<PortalConfig, DTO.PortalConfig>()
                 .Include<MediaConfig, DTO.MediaConfig>()
                 .Include<NotificationConfig, DTO.NotificationConfig>();

            Mapper.CreateMap<DefaultConfig, DTO.DefaultConfig>();
            Mapper.CreateMap<DesignConfig, DTO.DesignConfig>();
            Mapper.CreateMap<CouponConfig, DTO.CouponConfig>();
            Mapper.CreateMap<SMTPConfig, DTO.SMTPConfig>();
            Mapper.CreateMap<TerminalConfig, DTO.TerminalConfig>();
            Mapper.CreateMap<PortalConfig, DTO.PortalConfig>();
            Mapper.CreateMap<MediaConfig, DTO.MediaConfig>();
            Mapper.CreateMap<NotificationConfig, DTO.NotificationConfig>();
        }
    }

    public class WorkplaceDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Workplace, DTO.Workplace>();
        }
    }

    public class ClientDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Client, DTO.Client>();
        }
    }

    public class UserDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.AddProfile(new WorkplaceDTOProfile());

            Mapper.CreateMap<User, DTO.User>()
                .Include<Administrator, DTO.Administrator>()
                .Include<Operator, DTO.Operator>();

            Mapper.CreateMap<Administrator, DTO.Administrator>();
            Mapper.CreateMap<Operator, DTO.Operator>();
            Mapper.CreateMap<OperatorInterruption, DTO.OperatorInterruption>();
        }
    }

    public class OfficeDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Office, DTO.Office>();
        }
    }

    public class ServiceDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.AddProfile(new UserDTOProfile());

            Mapper.CreateMap<ServiceGroup, DTO.ServiceGroup>();
            Mapper.CreateMap<Service, DTO.Service>();
            Mapper.CreateMap<AdditionalService, DTO.AdditionalService>();

            Mapper.CreateMap<ServiceStep, DTO.ServiceStep>();
            Mapper.CreateMap<ServiceRendering, DTO.ServiceRendering>();
            Mapper.CreateMap<ServiceParameter, DTO.ServiceParameter>()
                 .Include<ServiceParameterNumber, DTO.ServiceParameterNumber>()
                 .Include<ServiceParameterText, DTO.ServiceParameterText>()
                 .Include<ServiceParameterOptions, DTO.ServiceParameterOptions>();

            Mapper.CreateMap<ServiceParameterNumber, DTO.ServiceParameterNumber>();
            Mapper.CreateMap<ServiceParameterText, DTO.ServiceParameterText>();
            Mapper.CreateMap<ServiceParameterOptions, DTO.ServiceParameterOptions>();
            Mapper.CreateMap<Schedule, DTO.Schedule>()
                 .Include<DefaultWeekdaySchedule, DTO.DefaultWeekdaySchedule>()
                 .Include<DefaultExceptionSchedule, DTO.DefaultExceptionSchedule>()
                 .Include<ServiceSchedule, DTO.ServiceSchedule>()
                 .Include<ServiceWeekdaySchedule, DTO.ServiceWeekdaySchedule>()
                 .Include<ServiceExceptionSchedule, DTO.ServiceExceptionSchedule>();

            Mapper.CreateMap<DefaultWeekdaySchedule, DTO.DefaultWeekdaySchedule>();
            Mapper.CreateMap<DefaultExceptionSchedule, DTO.DefaultExceptionSchedule>();
            Mapper.CreateMap<ServiceSchedule, DTO.ServiceSchedule>();
            Mapper.CreateMap<ServiceWeekdaySchedule, DTO.ServiceWeekdaySchedule>();
            Mapper.CreateMap<ServiceExceptionSchedule, DTO.ServiceExceptionSchedule>();
        }
    }

    public class MetricDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.AddProfile(new ServiceDTOProfile());

            Mapper.CreateMap<Metric, DTO.Metric>()
                 .Include<QueuePlanMetric, DTO.QueuePlanMetric>()
                 .Include<QueuePlanServiceMetric, DTO.QueuePlanServiceMetric>();

            Mapper.CreateMap<QueuePlanMetric, DTO.QueuePlanMetric>();
            Mapper.CreateMap<QueuePlanServiceMetric, DTO.QueuePlanServiceMetric>();
        }
    }

    public class ClientRequestDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.AddProfile(new UserDTOProfile());
            Mapper.AddProfile(new ServiceDTOProfile());
            Mapper.CreateMap<ClientRequest, DTO.ClientRequest>();
            Mapper.CreateMap<ClientRequestParameter, DTO.ClientRequestParameter>();
            Mapper.CreateMap<ClientRequestPlan, DTO.ClientRequestPlan>();
        }
    }

    public class EventDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.AddProfile(new UserDTOProfile());
            Mapper.AddProfile(new ServiceDTOProfile());

            Mapper.CreateMap<Event, DTO.Event>()
                 .Include<UserEvent, DTO.UserEvent>()
                 .Include<ClientRequestEvent, DTO.ClientRequestEvent>();

            Mapper.CreateMap<UserEvent, DTO.UserEvent>();
            Mapper.CreateMap<ClientRequestEvent, DTO.ClientRequestEvent>();
        }
    }

    public class QueuePlanDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<QueuePlan, DTO.QueuePlan>();
            Mapper.CreateMap<ClientRequest, DTO.QueuePlan.ClientRequest>()
                .ForMember(x => x.Color, o => o.Ignore());

            Mapper.CreateMap<Operator, DTO.QueuePlan.Operator>();
            Mapper.CreateMap<ClientRequestPlan, DTO.QueuePlan.ClientRequestPlan>();
            Mapper.CreateMap<OperatorPlan, DTO.QueuePlan.OperatorPlan>();
            Mapper.CreateMap<OperatorPlanMetrics, DTO.OperatorPlanMetrics>();
            Mapper.CreateMap<NotDistributedClientRequest, DTO.QueuePlan.NotDistributedClientRequest>();
            Mapper.CreateMap<ServiceFreeTime, DTO.ServiceFreeTime>();
        }
    }

    public class FullDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.AddProfile(new ParticularDTOProfile());
            Mapper.AddProfile(new ConfigDTOProfile());
            Mapper.AddProfile(new UserDTOProfile());
            Mapper.AddProfile(new ClientDTOProfile());
            Mapper.AddProfile(new OfficeDTOProfile());
            Mapper.AddProfile(new ServiceDTOProfile());
            Mapper.AddProfile(new MetricDTOProfile());
            Mapper.AddProfile(new ClientRequestDTOProfile());
            Mapper.AddProfile(new EventDTOProfile());
            Mapper.AddProfile(new QueuePlanDTOProfile());

            Mapper.AssertConfigurationIsValid();
        }
    }
}