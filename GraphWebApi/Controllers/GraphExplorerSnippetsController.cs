using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CodeSnippetsReflection;
using System.Net.Http;
using GraphWebApi.Models;
using Microsoft.Extensions.Options;

namespace GraphWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphExplorerSnippetsController : ControllerBase
    {
        private GraphVersion GraphVersion { get; set; }
        public GraphExplorerSnippetsController(IOptions<GraphVersion> settings)
        {
            this.GraphVersion = settings.Value;
        }

        // GET api/graphexplorersnippets/GET,/me/
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Get(string arg)
        {
            if (arg != String.Empty && arg != null)
            {
                SnippetsGeneratorCSharp code = new SnippetsGeneratorCSharp();
                string snippet = code.GenerateCode(arg);

                return new OkObjectResult(new CodeSnippetResult { Code = snippet, StatusCode = true, Message = "Success" , Language="C#"});
            }
            else
            {
                string result = "No Results! The Service expects atleast a HTTP Method and Graph Resource." + Environment.NewLine +
                        "You can also add OData parameters which are optional." + Environment.NewLine + "An example of a paramater expect would be [GET,/me/events]";

                return new OkObjectResult(new CodeSnippetResult { Code = "", StatusCode = false, Message = result, Language="C#" });
            }
        }

         //POST api/graphexplorersnippets
        [HttpPost]
        public void Post([FromBody] string value)
        {
            StreamContent content = new StreamContent(ControllerContext.HttpContext.Request.Body);
            Task<HttpRequestMessage> httpRequestMessage = content.ReadAsHttpRequestMessageAsync();
            HttpRequestMessage result = httpRequestMessage.Result;


         

            bool IsBeta = (bool)result.Properties["IsBeta"];

            string serviceRoot;

            if(IsBeta)
            {
                serviceRoot = GraphVersion.graphBeta;
            }
            else
            {
                serviceRoot = GraphVersion.graphV1;
            }   

            SnippetsGenerator snippetGenerator = new SnippetsGenerator(result, serviceRoot);          
        }
    }
}
