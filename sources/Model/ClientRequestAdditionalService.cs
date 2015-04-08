using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;

namespace Queue.Model
{
    [Class(Table = "client_request_additional_service", DynamicUpdate = true, Lazy = false)]
    public class ClientRequestAdditionalService : IdentifiedEntity
    {
        #region properties

        [NotNull(Message = "Запрос клиента не указан")]
        [ManyToOne(ClassType = typeof(ClientRequest), Column = "ClientRequestId", ForeignKey = "ClientRequestAdditionalServiceToClientRequestReference")]
        public virtual ClientRequest ClientRequest { get; set; }

        [NotNull(Message = "Оператор не указан")]
        [ManyToOne(ClassType = typeof(Operator), Column = "OperatorId", ForeignKey = "ClientRequestAdditionalServiceToOperatorReference")]
        public virtual Operator Operator { get; set; }

        [NotNull(Message = "Дополнительная услуга не указана")]
        [ManyToOne(ClassType = typeof(AdditionalService), Column = "AdditionalServiceId", ForeignKey = "ClientRequestAdditionalServiceToAdditionalServiceReference")]
        public virtual AdditionalService AdditionalService { get; set; }

        [Min(Value = 1, Message = "Количество не указано")]
        [Property]
        public virtual float Quantity { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", AdditionalService, Quantity, AdditionalService.Measure);
        }
    }
}