using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;

namespace Queue.Model
{
    [JoinedSubclass(Table = "client_request_event", ExtendsType = typeof(Event), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "EventId", ForeignKey = "ClientRequestEventToEventReference")]
    public class ClientRequestEvent : Event
    {
        #region properties

        [NotNull(Message = "Для события не указана заявка клиента")]
        [ManyToOne(ClassType = typeof(ClientRequest), Column = "ClientRequestId", ForeignKey = "ClientRequestEventToClientRequestReference")]
        public virtual ClientRequest ClientRequest { get; set; }

        #endregion properties
    }
}