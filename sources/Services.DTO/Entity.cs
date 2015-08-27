using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public abstract class Entity : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T field, T value, [CallerMemberName] string property = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(property));
                }
            }
        }

        public void Update(Entity source)
        {
            Type type = GetType();

            if (source.GetType() != type)
            {
                throw new ArgumentException("Аргумент должен быть того же типа");
            }

            TypeMap map = Mapper.FindTypeMapFor(type, type);
            if (map == null)
            {
                Mapper.CreateMap(type, type);
            }

            Mapper.Map(source, this, type, type);
        }
    }
}