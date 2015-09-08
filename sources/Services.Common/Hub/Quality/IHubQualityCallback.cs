using System.ServiceModel;

namespace Queue.Services.Common
{
    public interface IHubQualityCallback
    {
        [OperationContract(IsOneWay = true)]
        void Accepted(byte rating);
    }
}