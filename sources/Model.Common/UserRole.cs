using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum UserRole
    {
        Administrator = 1,
        Manager = 2,
        Operator = 4,
        All = Operator | Manager | Administrator
    }
}