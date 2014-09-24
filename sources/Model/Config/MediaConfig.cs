using Junte.Data.Common;
using NHibernate.Mapping.Attributes;
using Queue.Model.Common;
using System.Collections.Generic;

namespace Queue.Model
{
    [JoinedSubclass(Table = "config_media", ExtendsType = typeof(Config), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ConfigId", ForeignKey = "MediaConfigToConfigReference")]
    public class MediaConfig : Config
    {
        #region fields

        private IList<MediaConfigFile> mediaFiles = new List<MediaConfigFile>();

        #endregion fields

        public MediaConfig()
        {
            Type = ConfigType.Media;
        }

        #region properties

        [Property]
        public virtual string ServiceUrl { get; set; }

        [Property(Length = DataLength._500K)]
        public virtual string Ticker { get; set; }

        [Property]
        public virtual int TickerSpeed { get; set; }

        #endregion properties

        #region collection

        [Bag(0, Inverse = true, Lazy = CollectionLazy.False)]
        [Key(1, Column = "MediaConfigId", OnDelete = OnDelete.Cascade)]
        [OneToMany(2, ClassType = typeof(MediaConfigFile))]
        public virtual IList<MediaConfigFile> MediaFiles
        {
            get { return mediaFiles; }
            set { mediaFiles = value; }
        }

        #endregion collection
    }
}