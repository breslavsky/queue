using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum CouponSection : long
    {
        RequestDate = 1,
        RequestTime = 2,
        Objects = 4,
        Position = 8,
        WaitingTime = 16,
        Client = 32,
        Workplaces = 64,
        Service = 128
    }
}