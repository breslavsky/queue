﻿using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Contracts
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IServerCallback))]
    public interface IServerTcpService : IServerService
    {
        [OperationContract]
        bool IsSubscribed(ServerServiceEventType eventType);

        [OperationContract]
        void Subscribe(ServerServiceEventType eventType, ServerSubscribtionArgs args = null);

        [OperationContract]
        void UnSubscribe(ServerServiceEventType eventType);
    }
}