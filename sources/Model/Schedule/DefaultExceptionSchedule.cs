using NHibernate.Mapping.Attributes;

namespace Queue.Model
{
    [JoinedSubclass(Table = "default_exception_schedule",
                    ExtendsType = typeof(Schedule),
                    Lazy = false,
                    DynamicUpdate = true)]
    [Key(Column = "ScheduleId", ForeignKey = "DefaultExceptionScheduleToScheduleReference")]
    public class DefaultExceptionSchedule : ExceptionSchedule
    {
    }
}