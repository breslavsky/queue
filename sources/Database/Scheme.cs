using System;
using System.Collections.Generic;

namespace Queue.Database
{
    public static class SchemeState
    {
        private const string SqlSeparator = "-- SEPARATOR";

        public static string[] Constraints
        {
            get
            {
                return Scheme.constraints.Split(new string[] { SqlSeparator },
                    StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public static Dictionary<int, string> Patches = new Dictionary<int, string>()
        {
            {1, Scheme._001},
            {2, Scheme._002},
            {3, Scheme._003},
            {4, Scheme._004},
            {5, Scheme._005},
            {6, Scheme._006},
            {7, Scheme._007},
            {8, Scheme._008},
            {9, Scheme._009},
            {10, Scheme._010},
            {11, Scheme._011},
            {12, Scheme._012}
        };
    }
}