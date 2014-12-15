using Translation = Queue.Model.Common.Translation;

namespace Queue.Model.Common
{
    public enum AdministratorPermissions
    {
        Settings,
        Clients,
        ClientsRequests,
        Users,
        DefaultSchedule,
        Workplaces,
        Services,
        CurrentSchedule,
        QueueMonitor,
        Reports
    }
}