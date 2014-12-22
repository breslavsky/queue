using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum UserRole
    {
        Administrator = 1,
        Operator = 2,
        All = Operator | Administrator
    }
}