using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CodeSnippetsReflection
{
    /// <summary>
    /// This Model holders all the required payload data
    /// From Url, request payload body, all headers and the HTTP Method
    /// </summary>
    internal class RequestPayloadModel
    {
        public string Url { get; set; }
        public string Body { get; set; }
        public bool IsBeta { get; set; }
        public List<string> Headers { get; set; }
        public HttpMethod HttpMethod { get; set; }
    }
}
