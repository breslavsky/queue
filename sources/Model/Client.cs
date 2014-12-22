using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Queue.Model
{
    [Class(Table = "client", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class Client : IdentifiedEntity
    {
        public Client()
        {
            RegisterDate = DateTime.Now;
        }

        #region properties

        [Property]
        public virtual DateTime RegisterDate { get; set; }

        [Property]
        public virtual Guid SessionId { get; set; }

        [NotNullNotEmpty(Message = "Фамилия не указана")]
        [Property]
        public virtual string Surname { get; set; }

        [Property]
        public virtual string Name { get; set; }

        [Property]
        public virtual string Patronymic { get; set; }

        [Property]
        public virtual string Email { get; set; }

        [Property]
        public virtual string Mobile { get; set; }

        [Property(Column = "_Identity")]
        public virtual string Identity { get; set; }

        [Property]
        public virtual string Password { get; set; }

        [Property]
        public virtual bool VIP { get; set; }

        #endregion properties

        public virtual void SetPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                throw new Exception("Пароль не может быть пустым");
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