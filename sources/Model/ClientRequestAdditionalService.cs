using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;

namespace Queue.Model
{
    [Class(Table = "client_request_additional_service", DynamicUpdate = true, Lazy = false)]
    public class ClientRequestAdditionalService : IdentifiedEntity
    {
        #region properties

        [NotNull(Message = "Дополнительная услуга не указана")]
        [ManyToOne(ClassType = typeof(AdditionalService), Column = "AdditionalServiceId", ForeignKey = "ClientRequestAdditionalServiceToAdditionalServiceReference")]
        public virtual AdditionalService AdditionalService { get; set; }

        [Property]
        public virtual float Quantity { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", AdditionalService, Quantity, AdditionalService.Measure);
        }
    }
}