using Junte.Data.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public static class Bindings
    {
        private const int MaxReceivedMessageSize = 1024 * 1024 * 50;

        public static NetNamedPipeBinding NetNamedPipeBinding
        {
            get
            {
                return new NetNamedPipeBinding(NetNamedPipeSecurityMode.None)
                {
                    TransferMode = TransferMode.Buffered,
                    MaxReceivedMessageSize = MaxReceivedMessageSize,
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
                    MaxReceivedMessageSize = MaxReceivedMessageSize,
                    CloseTimeout = TimeSpan.MaxValue
                };
            }
        }

        public static BasicHttpBinding BasicHttpBinding
        {
            get
            {
                return new BasicHttpBinding()
                {
                    TransferMode = TransferMode.Buffered,
                    MaxReceivedMessageSize = MaxReceivedMessageSize,
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
                    MaxReceivedMessageSize = MaxReceivedMessageSize,
                    CloseTimeout = TimeSpan.MaxValue
                };
            }
        }
    }
}