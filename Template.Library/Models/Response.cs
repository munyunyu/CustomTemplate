using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Enums;

namespace Template.Library.Models
{
    public class Response<T>
    {
        public Status Code { get; set; }
        public string? Message { get; set; }
        public T? Payload { get; set; }
    }
}
