using Junte.Data.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public static class Bindings
    {
        public static NetNamedPipeBinding NetNamedPipeBinding
        {
            get
            {
                return new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
                {
                    TransferMode = TransferMode.Buffered,
                    MaxReceivedMessageSize = DataLength._150M,
                    CloseTimeout = TimeSpan.MaxValue
                };
            }
        }

        public static NetTcpBinding NetTcpBinding
        {
            get
            {
                return new NetTcpBinding(SecurityMode.None)
                {
                    TransferMode = TransferMode.Buffered,
                    MaxReceivedMessageSize = DataLength._150M,
                    CloseTimeout = TimeSpan.MaxValue
                };
            }
        }

        public static WebHttpBinding WebHttpBinding
        {
            get
            {
                return new WebHttpBinding(WebHttpSecurityMode.None)
                {
                    TransferMode = TransferMode.Streamed,
                    MaxReceivedMessageSize = DataLength._150M,
                    CloseTimeout = TimeSpan.MaxValue
                };
            }
        }
    }
}