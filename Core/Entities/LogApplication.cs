using System.Net;

namespace Core.Entities
{
    public class LogApplication : BaseEntity
    {
        public string detail { get; set; }
        public string title { get; set; }
        public HttpStatusCode status { get; set; }
        public DateTime logDate { get; set; }
    }
}
