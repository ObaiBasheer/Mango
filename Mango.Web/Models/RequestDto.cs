using static Mango.Web.Utility.SD;

namespace Mango.Web.Models
{
    public class RequestDto
    {
        public MethodType MethodType { get; set; } = MethodType.GET;
        public string? URL { get; set; }
        public object? Data { get; set; }
        public string? AccessToken { get; set; }

        public ContentType ContentType { get; set; } = ContentType.Json;
    }
}
