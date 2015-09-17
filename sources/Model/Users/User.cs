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
        public const int LostTimeout = 3600;
        public const int GoneTimeout = 60;
        public const int HeartbitTimeout = 20;

        public User()
        {
            CreateDate = DateTime.Now;
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
        [NotNullNotEmpty(Message = "Фамилия пользователя не указана")]
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

        [Property]
        public virtual bool IsActive { get; set; }

        public virtual bool Online
        {
            get { return Heartbeat > DateTime.Now - TimeSpan.FromSeconds(HeartbitTimeout); }
        }

        public virtual bool HasGone
        {
            get { return Heartbeat < DateTime.Now - TimeSpan.FromSeconds(GoneTimeout); }
        }

        public virtual bool HasLost
        {
            get { return Heartbeat < DateTime.Now - TimeSpan.FromSeconds(LostTimeout); }
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