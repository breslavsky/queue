using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum AdministratorPermissions : long
    {
        Config = 1,
        Clients = 2,
        ClientsRequests = 4,
        Users = 8,
        DefaultSchedule = 16,
        Workplaces = 32,
        Services = 64,
        CurrentSchedule = 128,
        QueuePlan = 256,
        Reports = 512,
        Offices = 1024
    }
}