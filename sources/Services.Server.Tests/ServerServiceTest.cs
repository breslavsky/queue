using Microsoft.VisualStudio.TestTools.UnitTesting;
using Queue.Model;

namespace Queue.Services.Server.Tests
{
    [TestClass]
    public class ServerServiceTest
    {
        [TestMethod]
        public void GetDateTime()
        {
            var c = new ServerServiceReference.ServerHttpServiceClient();
            var d = c.GetDateTime();
            var x = d.TimeOfDay;
        }
    }
}