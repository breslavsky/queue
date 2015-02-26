using System.Collections.Generic;

namespace Queue.Terminal
{
    public class EarlyRequestHour
    {
        private List<int> minutes = new List<int>();

        public int Hour { get; set; }

        public List<int> Minutes
        {
            get { return minutes; }
            set { minutes = value; }
        }
    }
}