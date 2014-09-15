using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class LinqTest
    {
        [TestMethod]
        public void OrderBy()
        {
            var list = new[] { new { x = false, y = 1, z = 1 }, new { x = true, y = 1, z = 6 }, new { x = true, y = 1, z = 4 } };

            var result = list.OrderByDescending(i => i.x).ThenBy(i => i.y).ThenBy(i => i.z);

            int m = result.Count();
        }
    }
}