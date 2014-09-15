using Queue.Services.DTO;

namespace Queue.Hub
{
    public class ControllerEventArgs
    {
        public ClientRequest[] CallingClientRequests { get; set; }
    }

    public class Controller
    {
        private static object locker = new object();

        private Workplace[] workplaces;

        private ClientRequest[] callingClientRequests;

        public Controller()
        {
            workplaces = new Workplace[] { };
            callingClientRequests = new ClientRequest[] { };
        }

        public delegate void ControllerEventHandler(object sender, ControllerEventArgs e);

        public event ControllerEventHandler OnCallingClientRequestsChanged;

        public Workplace[] Workplaces
        {
            get { return workplaces; }
            set
            {
                lock (locker)
                {
                    workplaces = value;
                }
            }
        }

        public ClientRequest[] CallingClientRequests
        {
            get { return callingClientRequests; }
            set
            {
                lock (locker)
                {
                    callingClientRequests = value;

                    if (OnCallingClientRequestsChanged != null)
                    {
                        OnCallingClientRequestsChanged(this, new ControllerEventArgs()
                        {
                            CallingClientRequests = callingClientRequests
                        });
                    }
                }
            }
        }
    }
}