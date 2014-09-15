using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Queue.Model
{
    [Class(Table = "user", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class User : IdentifiedEntity
    {
        private const int GoneTimeout = 60;

        private const int HeartbitTimeout = 20;

        // TODO: хм... не очень хорошо. это может хорошо работать, если мы создаем нового пользователя через UI (хотя должна происходить инициализация таких свойств),
        // а если nhibernate? поля же потом перезатрутся - лишние действия...
        // можно сделать метот статический, что то типа - createDefault
        public User()
        {
            CreateDate = DateTime.Now;
            SessionId = Guid.NewGuid();
            Heartbeat = DateTime.Now;
            Surname = "Новый пользователь";
        }

        #region properties

        [Property]
        public virtual DateTime CreateDate { get; set; }

        [Property]
        public virtual Guid SessionId { get; set; }

        [Property]
        public virtual DateTime Heartbeat { get; set; }

        [Discriminator(Column = "Role", Force = true)]
        public virtual UserRole Role { get; set; }

        [Property]
        [NotNullNotEmpty(Message = "Не указано поле (фамилия)")]
        public virtual string Surname { get; set; }

        [Property]
        public virtual string Name { get; set; }

        [Property]
        public virtual string Patronymic { get; set; }

        [Property]
        public virtual string Email { get; set; }

        [Property]
        public virtual string Mobile { get; set; }

        [Property]
        public virtual string Password { get; set; }

        public virtual bool Online
        {
            get { return DateTime.Now - Heartbeat < TimeSpan.FromSeconds(HeartbitTimeout); }
        }

        public virtual bool HasGone
        {
            get { return DateTime.Now - Heartbeat > TimeSpan.FromSeconds(GoneTimeout); }
        }

        #endregion properties

        public virtual void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                Password = string.Empty;
                return;
            }

            MD5 Md5 = new MD5CryptoServiceProvider();
            byte[] originalBytes = UTF8Encoding.Default.GetBytes(password);
            byte[] encodedBytes = Md5.ComputeHash(originalBytes);

            Password = BitConverter.ToString(encodedBytes);
        }

        public virtual bool CheckPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                return true;
            }

            MD5 Md5 = new MD5CryptoServiceProvider();
            byte[] originalBytes = UTF8Encoding.Default.GetBytes(password);
            byte[] encodedBytes = Md5.ComputeHash(originalBytes);

            return BitConverter.ToString(encodedBytes) == Password;
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Surname, Name).Trim();
        }
    }
}