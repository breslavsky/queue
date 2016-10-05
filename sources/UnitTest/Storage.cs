using Junte.Data.NHibernate;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using NHibernate.Criterion;
using Queue.Model;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Queue.UnitTest
{
    public enum TestEnum
    {
        STATE2,
        STATE1
    }

    public class TestObject : IdentifiedEntity
    {
        private int x;

        public string Name { get; set; }

        public int Age { get; set; }

        public TestObject FirstChild { get; set; }

        public IList<TestObject> Childs { get; set; }

        public TestEnum State { get; set; }

        public void SetId(Guid id)
        {
            Id = id;
            x = id.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }

    [TestClass]
    public class Storage
    {
        private SessionProvider sessionProvider;

        [TestInitialize]
        public void Init()
        {
            DatabaseSettings settings = new DatabaseSettings()
            {
                Integrated = true,
                Type = DatabaseType.MsSql,
                Server = @"(local)\SQLEXPRESS",
                Name = "queue"
            };

            UnityContainer container = new UnityContainer();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
            sessionProvider = new SessionProvider(new string[] { "Queue.Data.Model" }, settings);
            container.RegisterInstance<SessionProvider>(sessionProvider);
        }

        [TestMethod]
        public void BigPlan()
        {
            using (ISession session = sessionProvider.OpenSession())
            {
                EntityStorage storage = new EntityStorage();

                Schedule defaultSchedule = null;

                var planDate = DateTime.Today;

                var defaultWeekdaySchedule = session.CreateCriteria<DefaultWeekdaySchedule>()
                    .Add(Expression.Eq("DayOfWeek", planDate.DayOfWeek))
                    .UniqueResult<DefaultWeekdaySchedule>();
                if (defaultWeekdaySchedule != null)
                {
                    defaultSchedule = defaultWeekdaySchedule;
                }

                var defaultExceptionSchedule = session.CreateCriteria<DefaultExceptionSchedule>()
                    .Add(Expression.Eq("ScheduleDate", planDate))
                    .UniqueResult<DefaultExceptionSchedule>();
                if (defaultExceptionSchedule != null)
                {
                    defaultSchedule = defaultExceptionSchedule;
                }

                if (defaultSchedule == null)
                {
                    throw new Exception("Раписание по умолчанию не определено");
                }

                defaultSchedule = storage.Put(defaultSchedule);

                var operators = session.CreateCriteria<Operator>()
                    .AddOrder(Order.Asc("Surname"))
                    .AddOrder(Order.Asc("Name"))
                    .AddOrder(Order.Asc("Patronymic"))
                    .List<Operator>();

                foreach (var o in operators)
                {
                    storage.Put(o);
                }

                var clientRequests = session.CreateCriteria<ClientRequest>()
                  .Add(Restrictions.Eq("RequestDate", DateTime.Today))
                  .SetMaxResults(5)
                  .AddOrder(Order.Asc("Number"))
                  .List<ClientRequest>();

                foreach (var r in clientRequests)
                {
                    storage.Put(r);
                }
            }
        }

        [TestMethod]
        public void Simple()
        {
            var storage = new EntityStorage();

            var guid1 = Guid.NewGuid();

            var obj1 = new TestObject()
            {
                Name = "Anton",
                Age = 27
            };
            obj1.SetId(guid1);
            obj1.State = TestEnum.STATE1;

            var x1 = storage.Put(obj1);

            var guid2 = Guid.NewGuid();

            var obj2 = new TestObject()
            {
                Name = "Vadim",
                FirstChild = obj1,
                Age = 55
            };
            obj2.SetId(guid2);
            obj2.Childs = new TestObject[] { obj2 };

            var x2 = storage.Put(obj2);

            var guid3 = Guid.NewGuid();

            var obj3 = new TestObject()
            {
                Name = "Irina",
                FirstChild = obj1,
                Age = 56
            };
            obj3.Childs = new List<TestObject> { };

            for (var i = 0; i < 1000; i++)
            {
                var obj = new TestObject()
                {
                    Name = "demo " + i,
                    FirstChild = obj1,
                    Age = 56
                };
                obj.SetId(Guid.NewGuid());
                obj3.Childs.Add(obj);
            }

            obj3.FirstChild = obj3;
            obj3.SetId(guid3);

            storage.Put(obj3);

            var obj4 = storage.Get(obj1);
            obj4.Age = 28;

            /*var test1 = new List<TestObject>() { obj1, obj2, obj3 };

            var storage2 = new MemoryStorage();

            storage2.Put(test1);
            obj1.Name = "Denis";
            obj1.Age = 40;
            storage2.Put(obj1);
            obj3.Name = "Ira Super";
            storage2.Put(obj3);

            var xxx1 = storage2.Get(obj3);
            xxx1.Name = "1";*/

            IList a = new string[] { };

            //int count = storage.cache.Count;
        }
    }
}