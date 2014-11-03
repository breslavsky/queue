using System.Collections.Generic;

namespace Queue.Database
{
    public static class Scheme
    {
        public static Dictionary<int, string> Patches = new Dictionary<int, string>()
        {
            {1, SchemePatch._001},
            {2, SchemePatch._002},
            {3, SchemePatch._003}
        };
    }
}