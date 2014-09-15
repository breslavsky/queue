using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using System;

namespace Queue.Model
{
    [Class(Table = "office", DynamicUpdate = true, Lazy = false)]
    public class Office : IdentifiedEntity
    {
        //TODO:   Endpoint = "net.tcp://queue:4505"; вынести в константы
        public Office()
        {
            Name = "Новый филиал";
            Endpoint = "net.tcp://queue:4505";
            SessionId = Guid.Empty;
        }

        #region properties

        [Length(Min = 1, Max = 255, Message = "Поле (название филиала) должно быть больше 1 и менее 255 символов")]
        [Property]
        public virtual string Name { get; set; }

        [Property]
        public virtual string Endpoint { get; set; }

        [Property]
        public virtual Guid SessionId { get; set; }

        #endregion properties

        public override string ToString()
        {
            return Name;
        }
    }
}