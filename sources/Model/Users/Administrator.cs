using NHibernate.Mapping.Attributes;
using Queue.Model.Common;

namespace Queue.Model
{
    [Subclass(ExtendsType = typeof(User), DiscriminatorValueObject = UserRole.Administrator, Lazy = false, DynamicUpdate = true)]
    public class Administrator : User
    {
    }
}