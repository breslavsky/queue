using System;

namespace Queue.UI.WPF
{
    //TODO: create configuration
    public class UserLoginSettings
    {
        public string Endpoint { get; set; }

        public Guid UserId { get; set; }

        public string Password { get; set; }

        public bool IsRemember { get; set; }

        public string Accent { get; set; }
    }
}