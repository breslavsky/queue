using NHibernate.Mapping.Attributes;
using Queue.Model.Common;

namespace Queue.Model
{
    [Subclass(ExtendsType = typeof(ServiceParameter),
        DiscriminatorValueObject = ServiceParameterType.Number,
        Lazy = false,
        DynamicUpdate = true)]
    public class ServiceParameterNumber : ServiceParameter
    {
        public override ClientRequestParameter Compile(object value)
        {
            return new ClientRequestParameter()
            {
                Name = Name,
                Value = value.ToString()
            };
        }
    }
}