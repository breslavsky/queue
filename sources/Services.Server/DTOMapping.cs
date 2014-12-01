using AutoMapper;
using Queue.Model;
using System.Drawing;

namespace Queue.Services.Server
{
    public class ColorTypeConverter : ITypeConverter<Color, string>
    {
        public string Convert(ResolutionContext context)
        {
            return ColorTranslator.ToHtml((Color)context.SourceValue);
        }
    }

    public class ParticularDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Color, string>().ConvertUsing(new ColorTypeConverter());

            //ConfigurationIsValid
            Mapper.AssertConfigurationIsValid();
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

            //ConfigurationIsValid
            Mapper.AssertConfigurationIsValid();
        }
    }

    public class WorkplaceDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Workplace, DTO.Workplace>();

            //ConfigurationIsValid
            Mapper.AssertConfigurationIsValid();
        }
    }

    public class ClientDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Client, DTO.Client>();
            Mapper.CreateMap<Client, DTO.IdentifiedEntityLink<DTO.Client>>()
                .ForMember(dest => dest.Presentation, map => map.MapFrom(src => src.ToString()));

            //ConfigurationIsValid
            Mapper.AssertConfigurationIsValid();
        }
    }

    public class UserDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.AddProfile(new WorkplaceDTOProfile());

            Mapper.CreateMap<User, DTO.User>()
                .Include<Administrator, DTO.Administrator>()
                .Include<Manager, DTO.Manager>()
                .Include<Operator, DTO.Operator>();

            Mapper.CreateMap<User, DTO.IdentifiedEntityLink<DTO.User>>()
                .ForMember(dest => dest.Presentation, map => map.MapFrom(src => src.ToString()));

            Mapper.CreateMap<Administrator, DTO.Administrator>();
            Mapper.CreateMap<Manager, DTO.Manager>();
            Mapper.CreateMap<Operator, DTO.Operator>();
            Mapper.CreateMap<Operator, DTO.IdentifiedEntityLink<DTO.Operator>>()
                .ForMember(dest => dest.Presentation, map => map.MapFrom(src => src.ToString()));

            //ConfigurationIsValid
            Mapper.AssertConfigurationIsValid();
        }
    }

    public class OfficeDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Office, DTO.Office>();

            //ConfigurationIsValid
            Mapper.AssertConfigurationIsValid();
        }
    }

    public class ServiceDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.AddProfile(new UserDTOProfile());

            Mapper.CreateMap<ServiceGroup, DTO.ServiceGroup>();
            Mapper.CreateMap<ServiceGroup, DTO.IdentifiedEntityLink<DTO.ServiceGroup>>()
                .ForMember(dest => dest.Presentation, map => map.MapFrom(src => src.ToString()));

            Mapper.CreateMap<Service, DTO.Service>();

            Mapper.CreateMap<Service, DTO.IdentifiedEntityLink<DTO.Service>>()
                .ForMember(d => d.Presentation, map => map.MapFrom(s => s.ToString()));

            Mapper.CreateMap<ServiceStep, DTO.ServiceStep>();
            Mapper.CreateMap<ServiceRendering, DTO.ServiceRendering>();
            Mapper.CreateMap<ServiceFreeTime, DTO.ServiceFreeTime>();

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

            //ConfigurationIsValid
            Mapper.AssertConfigurationIsValid();
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

            //ConfigurationIsValid
            Mapper.AssertConfigurationIsValid();
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

            //ConfigurationIsValid
            Mapper.AssertConfigurationIsValid();
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

            //ConfigurationIsValid
            Mapper.AssertConfigurationIsValid();
        }
    }

    public class QueuePlanDTOProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<QueuePlan, DTO.QueuePlan>();
            Mapper.CreateMap<ClientRequest, DTO.QueuePlan.ClientRequest>();
            Mapper.CreateMap<Operator, DTO.QueuePlan.Operator>();
            Mapper.CreateMap<ClientRequestPlan, DTO.QueuePlan.ClientRequestPlan>();
            Mapper.CreateMap<OperatorPlan, DTO.QueuePlan.OperatorPlan>();
            Mapper.CreateMap<OperatorPlanMetrics, DTO.OperatorPlanMetrics>();
            Mapper.CreateMap<NotDistributedClientRequest, DTO.QueuePlan.NotDistributedClientRequest>();

            //ConfigurationIsValid
            Mapper.AssertConfigurationIsValid();
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
        }
    }
}