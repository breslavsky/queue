using System;

namespace Queue.Operator.Silverlight
{
    public class Settings
    {
        public string Endpoint { get; set; }

        public Guid UserId { get; set; }

        public string Password { get; set; }

        public bool IsRemember { get; set; }

        public void Save()
        {
            IsolatedStorage<Settings>.Put(this);
        }
    }
}