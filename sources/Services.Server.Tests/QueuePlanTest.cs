using Microsoft.VisualStudio.TestTools.UnitTesting;
using Queue.Model;

namespace Queue.Services.Server.Tests
{
    [TestClass]
    public class QueuePlanTest
    {
        private QueuePlan plan;

        [TestInitialize]
        public void Initialize()
        {
            plan = new QueuePlan();
        }

        [TestMethod]
        public void AddClientRequest()
        {
            ClientRequest req = new ClientRequest();
            plan.AddClientRequest(req);
        }

        [TestMethod]
        public void GetScheduleNullService()
        {
            plan.GetServiceSchedule(null);
        }

        [TestMethod]
        public void GetScheduleEmptyService()
        {
            plan.GetServiceSchedule(new Service());
        }
    }
}