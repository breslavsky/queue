using System;

namespace Queue.Hub.Svetovod
{
    public interface ISvetovodDisplayConnection : IDisposable
    {
        void ShowText(byte sysnum, string text);

        void Clear(byte sysnum);

        void ShowLines(byte sysnum, ushort[][] lines);
    }
}