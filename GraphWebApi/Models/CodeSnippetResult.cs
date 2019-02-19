using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphWebApi.Models
{
    public class CodeSnippetResult
    {
        public string Code { get; set; }
        public bool StatusCode { get; set; }
        public string Message { get; set; }
        public string Language { get; set; }
    }
}
