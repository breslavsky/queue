using Queue.Model.Common;
using Queue.Services.DTO;

namespace Queue.Services.Contracts.Server
{
    public class QueuePlanSubscribtionArgs
    {
        public Operator[] Operators;
        public ConfigType[] ConfigTypes;
    }
}