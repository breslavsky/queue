using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System;
using System.Collections.Generic;

namespace Queue.Model
{
    [Subclass(ExtendsType = typeof(ServiceParameter),
        DiscriminatorValueObject = ServiceParameterType.Text,
        Lazy = false,
        DynamicUpdate = true)]
    public class ServiceParameterText : ServiceParameter
    {
        public ServiceParameterText()
            : base()
        {
            MinLength = 5;
            MaxLength = 20;
        }

        #region properties

        [Min(Value = 1, Message = "Минимальная длина поля должна быть больше нуля")]
        [Property]
        public virtual int MinLength { get; set; }

        [Min(Value = 1, Message = "Максимальная длина поля должна быть больше нуля")]
        [Property]
        public virtual int MaxLength { get; set; }

        #endregion properties

        public override ClientRequestParameter Compile(object value)
        {
            ClientRequestParameter compiled = new ClientRequestParameter()
            {
                Name = Name
            };

            string text = (string)value;
            if (IsRequire && text.Length < 1)
            {
                throw new Exception(string.Format("Поле [{0}] обязательно для заполнения", Name));
            }
            if (text.Length < MinLength)
            {
                throw new Exception(string.Format("Длина поля [{0}] должна быть не менее {1} символов", Name, MinLength));
            }
            if (text.Length > MaxLength)
            {
                throw new Exception(string.Format("Длина поля [{0}] должна быть не более {1} символов", Name, MaxLength));
            }
            compiled.Value = text;

            return compiled;
        }

        public override ValidationError[] Validate()
        {
            var errors = base.Validate();

            var result = new List<ValidationError>(errors);

            if (MinLength > MaxLength)
            {
                result.Add(new ValidationError("Минимальная длина поля не пожет быть больше максимальной"));
            }

            return result.ToArray();
        }
    }
}