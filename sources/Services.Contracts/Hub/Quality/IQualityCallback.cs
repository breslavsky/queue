using System.ServiceModel;

namespace Queue.Services.Contracts.Hub
{
    public interface IQualityCallback
    {
        [OperationContract(IsOneWay = true)]
        void Accepted(byte rating);
    }
}