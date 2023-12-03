using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Project.Core.Entities.Helper
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }

    public class ApiResponse<T> : ApiResponse
    {
        public T? body { get; set; }
    }
}
