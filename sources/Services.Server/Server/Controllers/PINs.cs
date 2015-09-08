using Data.Model.Common;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    public partial class ServerService
    {
        public async Task SendPINToEmail(string email)
        {
            await Task.Run(() =>
            {
                try
                {
                    new MailAddress(email);
                }
                catch
                {
                    throw new FaultException("Неверный электронный адрес");
                }

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    int count = session.CreateCriteria<Client>()
                        .Add(Expression.Eq("Email", email))
                        .SetProjection(Projections.RowCount())
                        .UniqueResult<int>();

                    if (count > 0)
                    {
                        throw new FaultException("Клиент с таким электронным адресом уже существует");
                    }

                    SMTPConfig сonfig = session.Get<SMTPConfig>(ConfigType.SMTP);
                    if (сonfig == null)
                    {
                        throw new SystemException();
                    }

                    string template = @"Ваш PIN-код {PIN}";

                    string text = template
                        .Replace("{PIN}", PINUtils.Create(email).ToString());

                    using (SmtpClient smtpClient = new SmtpClient(сonfig.Server, сonfig.Port))
                    {
                        try
                        {
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.EnableSsl = сonfig.EnableSsl;
                            smtpClient.Credentials = new NetworkCredential(сonfig.User, сonfig.Password);

                            MailMessage message = new MailMessage(сonfig.From, email)
                            {
                                Subject = "Ваш PIN-код",
                                Body = text
                            };
                            smtpClient.Send(message);
                        }
                        catch (Exception exception)
                        {
                            throw new FaultException(exception.Message);
                        }
                    }
                }
            });
        }

        public async Task CheckPIN(string email, int source)
        {
            await Task.Run(() =>
            {
                try
                {
                    new MailAddress(email);
                }
                catch
                {
                    throw new FaultException("Неверный электронный адрес");
                }

                if (!PINUtils.Check(email, source))
                {
                    throw new FaultException("Указан неверный PIN-код");
                }
            });
        }
    }
}