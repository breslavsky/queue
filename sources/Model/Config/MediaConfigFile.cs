using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using System;

namespace Queue.Model
{
    [Class(Table = "media_config_file", DynamicUpdate = true, Lazy = false)]
    public class MediaConfigFile : IdentifiedEntity
    {
        #region properties

        [Length(Min = 1, Max = 255, Message = "Название файла должно быть от 1 до 255 символов")]
        [Property]
        public virtual string Name { get; set; }

        #endregion properties

        public override string ToString()
        {
            return Name;
        }
    }
}