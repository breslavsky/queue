using NHibernate.Mapping.Attributes;
using Queue.Model.Common;

namespace Queue.Model
{
    [Subclass(ExtendsType = typeof(User), DiscriminatorValueObject = UserRole.Manager, Lazy = false, DynamicUpdate = true)]
    public class Manager : User
    {

    }
}