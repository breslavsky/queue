using System;
using System.Collections.Generic;

namespace Queue.Database
{
    public class Scheme
    {
        public string[] Constraints
        {
            get
            {
                return SchemePatches.constraint.Split(new string[] { "-- SEPARATOR" },
                    StringSplitOptions.RemoveEmptyEntries);
            }
        }

        public static Dictionary<int, string> Patches = new Dictionary<int, string>()
        {
            {1, SchemePatches._001},
            {2, SchemePatches._002},
            {3, SchemePatches._003},
            {4, SchemePatches._004},
            {5, SchemePatches._005},
            {6, SchemePatches._006}
        };
    }
}