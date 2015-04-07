using NHibernate.Mapping.Attributes;
using Queue.Model.Common;

namespace Queue.Model
{
    [Subclass(ExtendsType = typeof(User), DiscriminatorValueObject = UserRole.Operator, Lazy = false, DynamicUpdate = true)]
    public class Operator : User
    {
        #region properties

        [ManyToOne(ClassType = typeof(Workplace), Column = "WorkplaceId", ForeignKey = "OperatorToWorkplaceReference")]
        public virtual Workplace Workplace { get; set; }

        #endregion properties
    }
}