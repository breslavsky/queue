using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;

namespace Queue.Model
{
    [JoinedSubclass(Table = "config_terminal", ExtendsType = typeof(Config), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ConfigId", ForeignKey = "TerminalConfigToConfigReference")]
    public class TerminalConfig : Config
    {
        public TerminalConfig()
        {
            Type = ConfigType.Terminal;
            Columns = 2;
            Rows = 5;
        }

        #region properties

        [Property]
        public virtual int PIN { get; set; }

        [Property]
        public virtual bool CurrentDayRecording { get; set; }

        [Range(Min = 1, Max = 10, Message = "Количество колонок должно быть от 1 до 10")]
        [Property]
        public virtual int Columns { get; set; }

        [Range(Min = 1, Max = 15, Message = "Количество строк должно быть от 1 до 10")]
        [Property]
        public virtual int Rows { get; set; }

        #endregion properties
    }
}