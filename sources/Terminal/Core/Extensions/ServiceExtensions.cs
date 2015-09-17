using Queue.Services.DTO;
using System;

namespace Queue.Terminal.Extensions
{
    public static class ServiceExtensions
    {
        private const string DefaultServiceColor = "Blue";

        public static string GetColor(this Service service)
        {
            return String.IsNullOrEmpty(service.Color) ?
                                        service.ServiceGroup == null ?
                                                                DefaultServiceColor :
                                                                service.ServiceGroup.Color :
                                        service.Color;
        }
    }
}