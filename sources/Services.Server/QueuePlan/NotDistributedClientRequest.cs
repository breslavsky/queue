using Queue.Model;

namespace Queue.Services.Server
{
    public class NotDistributedClientRequest
    {
        public NotDistributedClientRequest(ClientRequest clientRequest, string reason)
        {
            ClientRequest = clientRequest;
            Reason = reason;
        }

        public ClientRequest ClientRequest { get; private set; }

        public string Reason { get; private set; }
    }
}