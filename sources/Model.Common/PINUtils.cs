using System;

namespace Data.Model.Common
{
    public static class PINUtils
    {
        public static int Create(string email)
        {
            return Math.Abs(string.Format("{0}/{1}", email, DateTime.Today.ToString()).GetHashCode());
        }

        public static bool Check(string email, int source)
        {
            return Create(email) == source;
        }
    }
}