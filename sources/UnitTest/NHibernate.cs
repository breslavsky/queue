using Junte.Data.NHibernate;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using Queue.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Queue.UnitTest
{
    [TestClass]
    public class NHibernate
    {
        private SessionProvider sessionProvider;
        private EntityStorage storage;

        [TestInitialize]
        public void Initialize()
        {
            DatabaseSettings settings = new DatabaseSettings()
            {
                Integrated = true,
                Type = DatabaseType.MsSql,
                Server = "localhost",
                Name = "queue"
            };

            sessionProvider = new SessionProvider(new string[] { "Queue.Model" }, settings);

            storage = new EntityStorage();
        }

        [TestMethod]
        public void Mistery()
        {
            User user;

            using (var session = sessionProvider.OpenSession())
            using (var t = session.BeginTransaction())
            {
                var u = new User()
                {
                    Name = "test"
                };

                session.Save(u);
                t.Commit();

                Debug.WriteLine(u.Id);

                user = storage.Put(u);
            }

            using (var session = sessionProvider.OpenSession())
            using (var t = session.BeginTransaction())
            {
                var u = session.Merge(user);
                session.Delete(u);

                t.Commit();
            }

            using (var session = sessionProvider.OpenSession())
            using (var t = session.BeginTransaction())
            {
                var u = session.Merge(user);
                Debug.WriteLine(u.Id);
                u.Name = "deleted";
                session.Save(u);
                Debug.WriteLine(u.Id);

                t.Commit();
            }
        }
    }
}