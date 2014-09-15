using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Queue.Model
{
    [Subclass(ExtendsType = typeof(ServiceParameter), DiscriminatorValueObject = ServiceParameterType.Options, Lazy = false, DynamicUpdate = true)]
    public class ServiceParameterOptions : ServiceParameter
    {
        #region properties

        [Property]
        public virtual string Options { get; set; }

        [Property]
        public virtual bool IsMultiple { get; set; }

        #endregion properties

        public override ClientRequestParameter Compile(object value)
        {
            ClientRequestParameter compiled = new ClientRequestParameter()
            {
                Name = Name
            };

            string[] options = Regex.Split(Options, "\r\n");
            string[] selects;
            if (IsMultiple)
            {
                selects = (string[])value;
                if (IsRequire && selects.Length < 1)
                {
                    throw new Exception(string.Format("Поля [{0}] обязательно для заполнения", Name));
                }
                foreach (string select in selects)
                {
                    if (!options.Contains(select))
                    {
                        throw new Exception(string.Format("Вариант выбора [{0}] для поля [{1}] не найден", select, Name));
                    }
                }
                compiled.Value = string.Join("; ", selects);
            }
            else
            {
                if (IsRequire && value == null)
                {
                    throw new Exception(string.Format("Поле [{0}] обязательно для заполнения", Name));
                }
                string select = (string)value;
                if (!options.Contains(select))
                {
                    throw new Exception(string.Format("Вариант выбора [{0}] для поля [{1}] не найден", select, Name));
                }
                compiled.Value = select;
            }

            return compiled;
        }

        public override ValidationError[] Validate()
        {
            var errors = base.Validate();

            var result = new List<ValidationError>(errors);

            if (string.IsNullOrWhiteSpace(Options))
            {
                result.Add(new ValidationError("Не указаны варианты выбора"));
            }

            return result.ToArray();
        }
    }
}