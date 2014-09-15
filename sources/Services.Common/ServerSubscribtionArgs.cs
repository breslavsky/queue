using Queue.Model.Common;
using Queue.Services.DTO;

namespace Queue.Services.Common
{
    public class ServerSubscribtionArgs
    {
        public Operator[] Operators;
        public ConfigType[] ConfigTypes;
    }
}