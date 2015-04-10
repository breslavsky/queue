using Junte.Data.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using Queue.Model;
using System;
using System.Collections.Generic;

namespace Queue.UnitTest
{
    [TestClass]
    public class AdditionalServicesGenerator
    {
        private SessionProvider sessionProvider;

        [TestInitialize]
        public void Initialize()
        {
            DatabaseSettings settings = new DatabaseSettings()
            {
                Integrated = true,
                Type = DatabaseType.MsSql,
                Server = @"(local)\SQLEXPRESS",
                Name = "queue"
            };

            sessionProvider = new SessionProvider(new string[] { "Queue.Model" }, settings);
        }

        [TestMethod]
        public void Generate()
        {
            Random rand = new Random();
            using (ISession session = sessionProvider.OpenSession())
            {
                IList<ClientRequest> requests = session.QueryOver<ClientRequest>().List();
                IList<AdditionalService> addServices = session.QueryOver<AdditionalService>().List();

                foreach (ClientRequest req in requests)
                {
                    foreach (AdditionalService service in addServices)
                    {
                        ClientRequestAdditionalService newService = new ClientRequestAdditionalService()
                        {
                            AdditionalService = service,
                            ClientRequest = req,
                            Operator = req.Operator,
                            Quantity = rand.Next(50)
                        };
                        session.Save(newService);
                    }
                }

                session.Flush();
            }
        }
    }
}