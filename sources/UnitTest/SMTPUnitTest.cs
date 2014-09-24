using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Queue.Model;
using Queue.Services.Server;
using System;
using System.Net;
using System.Net.Mail;
using DTO = Queue.Services.DTO;

namespace Queue.UnitTest
{
    [TestClass]
    public class SMTPUnitTest
    {

        [TestMethod]
        public void Send()
        {
            using (var smtpClient = new SmtpClient("smtp.yandex.ru", 587))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("mfc-ivanovo@yandex.ru", "tctopol23");

                var message = new MailMessage("mfc-ivanovo@yandex.ru", "anton@breslavsky.ru")
                {
                    Subject = "Вы зарегистрированы",
                    Body = "Привет!"
                };
                smtpClient.Send(message);
            }

            Assert.IsTrue(true);
        }
    }
}