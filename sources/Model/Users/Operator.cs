using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using Queue.Model.Common;
using System;
using System.Collections.Generic;

namespace Queue.Model
{
    [Subclass(ExtendsType = typeof(User), DiscriminatorValueObject = UserRole.Operator, Lazy = false, DynamicUpdate = true)]
    public class Operator : User
    {
        #region properties

        [ManyToOne(ClassType = typeof(Workplace), Column = "WorkplaceId", ForeignKey = "OperatorToWorkplaceReference")]
        public virtual Workplace Workplace { get; set; }

        [Property]
        public virtual bool IsInterruption { get; set; }

        [Property]
        public virtual TimeSpan InterruptionStartTime { get; set; }

        [Property]
        public virtual TimeSpan InterruptionFinishTime { get; set; }

        #endregion properties

        public override ValidationError[] Validate()
        {
            var errors = base.Validate();

            var result = new List<ValidationError>(errors);

            if (IsInterruption)
            {
                if (InterruptionStartTime > InterruptionFinishTime)
                {
                    result.Add(new ValidationError("Время начала перерыва должно быть меньше времени окончания перерыва", "InterruptionStartTime"));
                }
            }

            return result.ToArray();
        }
    }
}