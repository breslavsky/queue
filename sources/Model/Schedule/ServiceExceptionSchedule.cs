using NHibernate.Mapping.Attributes;

namespace Queue.Model
{
    [JoinedSubclass(Table = "service_exception_schedule", ExtendsType = typeof(Schedule), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ScheduleId", ForeignKey = "ServiceExceptionScheduleToScheduleReference")]
    public class ServiceExceptionSchedule : ExceptionSchedule
    {
        #region properties

        [ManyToOne(ClassType = typeof(Service), Column = "ServiceId", ForeignKey = "ServiceExceptionScheduleToServiceReference")]
        public virtual Service Service { get; set; }

        #endregion properties
    }
}