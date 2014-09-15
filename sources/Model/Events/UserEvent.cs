using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;

namespace Queue.Model
{
    [JoinedSubclass(Table = "user_event", ExtendsType = typeof(Event), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "EventId", ForeignKey = "UserEventToEventReference")]
    public class UserEvent : Event
    {
        #region properties

        [NotNull(Message = "Для события не указан пользователь")]
        [ManyToOne(0, ClassType = typeof(User), Column = "UserId", ForeignKey = "UserEventToUserReference")]
        [Key(1, Column = "UserId", OnDelete = OnDelete.Cascade)]
        public virtual User User { get; set; }

        #endregion properties
    }
}