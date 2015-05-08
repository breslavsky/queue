using Junte.Data.NHibernate;
using Junte.Translation;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;

namespace Queue.Model
{
    [Class(Table = "service_rendering", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class ServiceRendering : IdentifiedEntity
    {
        #region properties

        [NotNull(Message = "Для обслуживания услуги не указано расписание")]
        [ManyToOne(1, ClassType = typeof(Schedule), Column = "ScheduleId", ForeignKey = "ServiceRenderingToScheduleReference")]
        public virtual Schedule Schedule { get; set; }

        [NotNull(Message = "Для обслуживания услуги не указан оператор")]
        [ManyToOne(ClassType = typeof(Operator), Column = "OperatorId", ForeignKey = "ServiceRenderingToOperatorReference")]
        public virtual Operator Operator { get; set; }

        [Property]
        public virtual ServiceRenderingMode Mode { get; set; }

        [ManyToOne(ClassType = typeof(ServiceStep), Column = "ServiceStepId", ForeignKey = "ServiceRenderingToServiceStepReference")]
        public virtual ServiceStep ServiceStep { get; set; }

        [Property]
        public virtual int Priority { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Operator, Translater.Enum(Mode));
        }
    }
}