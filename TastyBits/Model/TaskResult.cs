using System.Net;

namespace TastyBits.Model
{
    public class TaskResult
    {
        public bool HasError { get; set; }
        public string ErrorDesc { get; set; }
        public object Result { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
