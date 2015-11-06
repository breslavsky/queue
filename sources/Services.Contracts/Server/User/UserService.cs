namespace Queue.Services.Contracts.Server
{
    public class UserService : ClientService<IUserTcpService>
    {
        public UserService(string endpoint)
            : base(endpoint, ServicesPaths.User)
        {
        }
    }
}