using Microsoft.VisualStudio.TestTools.UnitTesting;
using Queue.Services.DTO;
using Queue.UI.Common;
using System;
using System.Printing;

namespace UnitTest
{
    [TestClass]
    public class XPSDocumentTest
    {
        [TestMethod]
        public void CouponXPS()
        {
            ClientRequestCoupon data = new ClientRequestCoupon()
                {
                    Client = new Client()
                    {
                        Name = "Вася"
                    },
                    CreateDate = DateTime.Now,
                    IsToday = true,
                    Number = 10,
                    Position = 15,
                    QueueName = "super",
                    RequestDate = DateTime.Now,
                    RequestTime = DateTime.Now.TimeOfDay,
                    Service = new Service()
                    {
                        Name = "super service"
                    },
                    Subjects = 10,
                    Workplaces = new Workplace[] { new Workplace()
                    {
                        Comment  = "test"
                    }}
                };

            string xpsFile = XPSGenerator.FromXaml(Queue.Resources.Templates.ClientRequestCoupon, data);
            try
            {
                PrintQueue defaultPrintQueue = LocalPrintServer.GetDefaultPrintQueue();
                defaultPrintQueue.AddJob("test", xpsFile, false);
            }
            catch
            {
            }
        }
    }
}