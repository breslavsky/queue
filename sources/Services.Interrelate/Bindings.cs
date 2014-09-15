using Junte.Data.Common;
using Queue.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services.Interrelate
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