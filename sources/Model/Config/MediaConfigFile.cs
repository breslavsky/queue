using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using System;

namespace Queue.Model
{
    [Class(Table = "media_config_file", DynamicUpdate = true, Lazy = false)]
    public class MediaConfigFile : IdentifiedEntity
    {
        public MediaConfigFile()
        {
            Name = "Новый медиа-файл";
            SortId = DateTime.Now.Ticks;
        }

        #region properties

        [ManyToOne(ClassType = typeof(MediaConfig), Column = "MediaConfigId", ForeignKey = "MediaConfigFileToMediaConfigReference")]
        public virtual MediaConfig MediaConfig { get; set; }

        [Property]
        public virtual DateTime CreateDate { get; set; }

        [Length(Min = 1, Max = 255, Message = "Название файла должно быть от 1 до 255 символов")]
        [Property]
        public virtual string Name { get; set; }

        [Property]
        public virtual bool IsActive { get; set; }

        [Property]
        public virtual long SortId { get; set; }

        #endregion properties

        public override string ToString()
        {
            return Name;
        }
    }
}