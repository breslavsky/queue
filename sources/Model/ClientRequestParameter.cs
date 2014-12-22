using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;

namespace Queue.Model
{
    [Class(Table = "client_request_parameter", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class ClientRequestParameter : IdentifiedEntity
    {
        #region properties

        [NotNull(Message = "Для параметра не указана заявка клиента")]
        [ManyToOne(ClassType = typeof(ClientRequest), Column = "ClientRequestId", ForeignKey = "ClientRequestParameterToClientRequestReference")]
        public virtual ClientRequest ClientRequest { get; set; }

        [NotNullNotEmpty(Message = "Название параметра не может быть пустым")]
        [Property]
        public virtual string Name { get; set; }

        [NotNullNotEmpty(Message = "Значение параметра не может быть пустым")]
        [Property]
        public virtual string Value { get; set; }

        #endregion properties
    }
}