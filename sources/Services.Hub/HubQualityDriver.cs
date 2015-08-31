using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services.Hub
{
    public class HubQualityDriverArgs
    {
        public int Rating;
    }

    public interface IHubQualityDriver
    {
    }

    public class HubQualityDriver : HubDriver
    {
        public event EventHandler<HubQualityDriverArgs> Answered;

        public void Working(int deviceId)
        {
        }

        public void Sleep(int deviceId)
        {
        }
    }
}