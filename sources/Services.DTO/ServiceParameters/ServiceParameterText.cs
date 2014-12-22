using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    public class ServiceParameterText : ServiceParameter
    {
        public int MinLength { get; set; }

        public int MaxLength { get; set; }
    }
}