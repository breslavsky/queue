﻿using Junte.Data.NHibernate;
using Microsoft.Practices.Unity;
using NHibernate.Criterion;
using NLog;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public partial class StandardServerService : DependencyService
    {
        #region dependency

        [Dependency]
        public SessionProvider SessionProvider { get; set; }

        [Dependency]
        public QueueInstance QueueInstance { get; set; }

        #endregion dependency

        #region fields

        protected readonly Logger logger = LogManager.GetCurrentClassLogger();
        protected readonly Guid sessionId;
        protected readonly IContextChannel channel;
        protected User currentUser;

        #endregion fields

        public StandardServerService()
            : base()
        {
            try
            {
                var operationContext = OperationContext.Current;
                if (operationContext != null)
                {
                    sessionId = Guid.Parse(operationContext.IncomingMessageHeaders
                        .GetHeader<string>("SessionId", string.Empty));
                }
            }
            catch { }

            try
            {
                var webOperationContext = WebOperationContext.Current;
                if (webOperationContext != null && sessionId == Guid.Empty)
                {
                    sessionId = Guid.Parse(webOperationContext.IncomingRequest
                        .Headers[ExtendHttpHeaders.Session]);
                }
            }
            catch { }

            if (sessionId != Guid.Empty)
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    currentUser = session.CreateCriteria<User>()
                        .Add(Restrictions.Eq("SessionId", sessionId))
                        .SetMaxResults(1)
                        .UniqueResult<User>();
                }
            }

            logger.Debug("Создан новый экземпляр службы [{0}]", sessionId);

            channel = OperationContext.Current.Channel;
            channel.Faulted += channel_Faulted;
            channel.Closing += channel_Closing;
        }

        public async Task<string> Echo(string message)
        {
            return await Task.Run(() => message);
        }

        public async Task Heartbeat()
        {
            await Task.Run(() => DateTime.Now);
        }

        protected void CheckPermission(UserRole role, Enum permissions = null)
        {
            if (currentUser == null)
            {
                throw new FaultException("Пользователь не авторизован в системе");
            }

            if (currentUser is Administrator
                && role.HasFlag(UserRole.Administrator))
            {
                var administrator = currentUser as Administrator;
                if (permissions != null && !administrator.Permissions.HasFlag(permissions))
                {
                    throw new FaultException("Недостаточно прав для доступа");
                }

                return;
            }

            if (currentUser is Operator
                && role.HasFlag(UserRole.Operator))
            {
                return;
            }

            throw new FaultException("Ошибка прав доступа");
        }

        private void channel_Closing(object sender, EventArgs e)
        {
            logger.Info("Канал службы закрывается [{0}]", sessionId);
        }

        private void channel_Faulted(object sender, EventArgs e)
        {
            logger.Info("В канале службы произошла ошибка [{0}]", sessionId);
        }
    }
}