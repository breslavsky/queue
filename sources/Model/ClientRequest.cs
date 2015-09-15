using Junte.Data.Common;
using Junte.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System;
using System.Collections.Generic;

namespace Queue.Model
{
    [Class(Table = "client_request", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class ClientRequest : IdentifiedEntity
    {
        #region fields

        private TimeSpan requestTime;

        #endregion fields

        public ClientRequest()
        {
            CreateDate = DateTime.Now;
        }

        #region properties

        [Property]
        public virtual DateTime CreateDate { get; set; }

        [Property(Index = "TypeIndex")]
        public virtual ClientRequestType Type { get; set; }

        [Property]
        public virtual int Number { get; set; }

        [ManyToOne(ClassType = typeof(Client), Column = "ClientId", ForeignKey = "ClientRequestToClientReference")]
        public virtual Client Client { get; set; }

        [NotNull(Message = "Для заявки не указана услуга")]
        [ManyToOne(ClassType = typeof(Service), Column = "ServiceId", ForeignKey = "ClientRequestToServiceReference")]
        public virtual Service Service { get; set; }

        [Property(Index = "ServiceTypeIndex")]
        public virtual ServiceType ServiceType { get; set; }

        [ManyToOne(ClassType = typeof(ServiceStep), Column = "StepId", ForeignKey = "ClientRequestToServiceStepReference")]
        public virtual ServiceStep ServiceStep { get; set; }

        [ManyToOne(ClassType = typeof(Operator), Column = "OperatorId", ForeignKey = "ClientRequestToOperatorReference")]
        public virtual Operator Operator { get; set; }

        [Property]
        public virtual float Productivity { get; set; }

        [Property]
        public virtual int ClientRecalls { get; set; }

        [Property]
        public virtual bool IsPriority { get; set; }

        [Property(Index = "RequestDateIndex")]
        public virtual DateTime RequestDate { get; set; }

        [Property]
        public virtual TimeSpan RequestTime
        {
            get { return requestTime; }
            set { WaitingStartTime = requestTime = value; }
        }

        [Min(Value = 1, Message = "Количество объектов не может быть менее 1")]
        [Property]
        public virtual int Subjects { get; set; }

        [Property]
        public virtual int Rating { get; set; }

        [Property]
        public virtual TimeSpan ClientInterval { get; set; }

        [Property]
        public virtual TimeSpan WaitingStartTime { get; set; }

        [Property]
        public virtual TimeSpan CallingStartTime { get; set; }

        [Property]
        public virtual TimeSpan CallingLastTime { get; set; }

        [Property]
        public virtual TimeSpan CallingFinishTime { get; set; }

        [Property]
        public virtual TimeSpan RenderStartTime { get; set; }

        [Property]
        public virtual TimeSpan RenderFinishTime { get; set; }

        [Property(Index = "StateIndex")]
        public virtual ClientRequestState State { get; set; }

        [Property]
        public virtual bool IsClosed { get; set; }

        [Property]
        public virtual int Version { get; set; }

        public virtual bool IsEditable
        {
            get { return State == ClientRequestState.Waiting || State == ClientRequestState.Postponed; }
        }

        public virtual bool IsRestorable
        {
            get
            {
                return State == ClientRequestState.Absence
                    || State == ClientRequestState.Canceled;
            }
        }

        public bool InWorking
        {
            get
            {
                return State == ClientRequestState.Calling
                    || State == ClientRequestState.Rendering;
            }
        }

        #endregion properties

        public virtual void Open()
        {
            if (!IsClosed)
            {
                throw new JunteException("Запрос уже открыт");
            }

            switch (State)
            {
                case ClientRequestState.Absence:
                case ClientRequestState.Canceled:
                    WaitingStartTime = DateTime.Now.TimeOfDay;
                    State = ClientRequestState.Waiting;
                    IsClosed = false;
                    break;

                default:
                    throw new JunteException("Нельзя открыть запрос клиента находящегося в данном статусе");
            }
        }

        public virtual void Calling(Operator queueOperator)
        {
            Operator = queueOperator;
            CallingStartTime = DateTime.Now.TimeOfDay;
            CallingLastTime = CallingStartTime;
            State = ClientRequestState.Calling;
        }

        public virtual void Absence()
        {
            CallingFinishTime = DateTime.Now.TimeOfDay;
            Close(ClientRequestState.Absence);
        }

        public virtual void Rendering(TimeSpan clientInterval)
        {
            ClientInterval = clientInterval;
            CallingFinishTime = RenderStartTime = DateTime.Now.TimeOfDay;
            State = ClientRequestState.Rendering;
        }

        public virtual void Rendered()
        {
            RenderFinishTime = DateTime.Now.TimeOfDay;

            Productivity = (float)ClientInterval.Ticks / (RenderFinishTime - RenderStartTime).Ticks * 100;

            Close(ClientRequestState.Rendered);
        }

        public virtual void FirstStep(ISession session)
        {
            ServiceStep = Service.GetFirstStep(session);
        }

        public virtual bool NextStep(ISession session)
        {
            if (ServiceStep != null)
            {
                var nextStep = session.CreateCriteria<ServiceStep>()
                    .Add(Restrictions.Eq("Service", Service))
                    .Add(Restrictions.Gt("SortId", ServiceStep.SortId))
                    .AddOrder(Order.Asc("SortId"))
                    .SetMaxResults(1)
                    .UniqueResult<ServiceStep>();
                if (nextStep != null)
                {
                    ServiceStep = nextStep;
                    return true;
                }
            }

            return false;
        }

        public virtual void Return()
        {
            switch (State)
            {
                case ClientRequestState.Calling:
                case ClientRequestState.Rendering:
                    Operator = null;
                    State = ClientRequestState.Waiting;
                    break;

                default:
                    throw new JunteException("Нельзя вернуть запрос клиента находящийся в данном статусе");
            }
        }

        public virtual void Cancel()
        {
            switch (State)
            {
                case ClientRequestState.Waiting:
                case ClientRequestState.Postponed:
                    Close(ClientRequestState.Canceled);
                    break;

                default:
                    //TODO: JunteException is bad!
                    throw new JunteException("Нельзя отменить запрос клиента находящийся в данном статусе");
            }
        }

        public virtual void Close(ClientRequestState state)
        {
            if (IsClosed)
            {
                throw new JunteException("Запрос уже закрыт");
            }

            switch (State)
            {
                case ClientRequestState.Waiting:
                case ClientRequestState.Calling:
                case ClientRequestState.Postponed:
                case ClientRequestState.Rendering:
                    State = state;
                    IsClosed = true;
                    break;

                default:
                    throw new JunteException("Нельзя закрыть запрос клиента находящегося в данном статусе");
            }
        }

        public virtual void Restore()
        {
            switch (State)
            {
                case ClientRequestState.Absence:
                case ClientRequestState.Canceled:
                    Open();
                    Operator = null;
                    break;

                default:
                    throw new JunteException("Нельзя восстановить запрос клиента находящегося в данном статусе");
            }
        }

        public virtual void Postpone(TimeSpan postponeTime)
        {
            if (IsClosed)
            {
                throw new JunteException("Невозможно отложить закрытый запрос клиента");
            }

            switch (State)
            {
                case ClientRequestState.Waiting:
                case ClientRequestState.Calling:
                case ClientRequestState.Postponed:
                case ClientRequestState.Rendering:
                    State = ClientRequestState.Postponed;
                    RequestTime = WaitingStartTime
                        = DateTime.Now.TimeOfDay.Add(postponeTime);
                    break;

                default:
                    throw new JunteException("Нельзя откладывать запрос клиента находящийся в данном статусе");
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} - {2}", Number, Client != null ? Client.ToString() : string.Empty, Service);
        }
    }
}