using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Configuration;

//ODataUriParser and Dependancies
using Microsoft.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.UriParser;


namespace CodeSnippetsReflection
{
    /// <summary>
    /// Snippets Generator Class with all the logic for code generation
    /// </summary>
    public class SnippetsGenerator
    {
        private HttpRequestMessage httpRequestMessage;
        private string serviceRoot;

        public SnippetsGenerator(HttpRequestMessage httpRequestMessage, string serviceRoot)
        {
            this.httpRequestMessage = httpRequestMessage;
            this.serviceRoot = serviceRoot;
        }

        /// <summary>
        /// Formulates the requested Graph snippets and returns it as string
        /// </summary>
        /// <returns></returns>
        public string GenerateCsharpSnippet()
        {
            //get all the HttpRequestMessage to our RequestPayloadModel
            RequestPayloadModel requestPayloadModel;

            // TODO
            // Assign all properties from the above model all their respective 
            // values

            /*** Sample Data for Test Purposes **/

            requestPayloadModel = new RequestPayloadModel()
            {
                Url = @"https://graph.microsoft.com/v1.0/me/messages?$select=subject,IsRead,sender,toRecipients&$filter=IsRead eq false&$skip=10",
                Headers = new List<string>()
                {
                    "Content-Type: application/json"
                },
                Body = string.Empty,
                HttpMethod = HttpMethod.Get
            };

            Uri serviceRootV1 = new Uri(serviceRoot);
            Uri fullUriV1 = new Uri(requestPayloadModel.Url);
            IEdmModel iedmModel = CsdlReader.Parse(XmlReader.Create(serviceRootV1 + "/$metadata"));
            /*** End of sample data for test purposes **/

            ODataUriParser parser = new ODataUriParser(iedmModel, serviceRootV1, fullUriV1);
            ODataUri odatauri = parser.ParseUri();

            StringBuilder snippet = new StringBuilder();

            //Fomulate all resources path
            snippet = GenerateResourcesPath(odatauri);

            /********************************/
            /**Formulate the Query options**/
            /*******************************/

            if (odatauri.Filter != null)
            {
                snippet.Append(FilterExpression(odatauri).ToString());
            }
           
            if (odatauri.SelectAndExpand != null)
            {
                snippet.Append(SelectExpression(odatauri).ToString());
            }

            if (odatauri.Search != null)
            {
                snippet.Append(SearchExpression(odatauri).ToString());
            }



            snippet.Append(".GetAsync();");
            return snippet.ToString();
        }

        /// <summary>
        /// Formulates the resources part of the generated snippets
        /// </summary>
        /// <param name="odatauri"></param>
        /// <returns></returns>
        private StringBuilder GenerateResourcesPath(ODataUri odatauri)
        {
            StringBuilder resourcesPath = new StringBuilder();
            resourcesPath.Append("GraphServiceClient graphClient = new GraphServiceClient();\n");
            resourcesPath.Append("graphClient");

            // lets append all resources
            foreach (var item in odatauri.Path)
            {
                resourcesPath.Append("." + UppercaseFirstLetter(item.Identifier));
            }

            resourcesPath.Append(".Request()");
            return resourcesPath;
        }


        #region OData Query Options Expresssions

        /// <summary>
        /// Formulates Select query options and its respective parameters
        /// </summary>
        /// <param name="odatauri"></param>
        /// <returns></returns>
        private StringBuilder SelectExpression(ODataUri odatauri)
        {
            StringBuilder selectExpression = new StringBuilder();

            selectExpression.Append(".Select(\"");
            var pathSelectedItems = odatauri.SelectAndExpand.SelectedItems;

            foreach (PathSelectItem item in pathSelectedItems)
            {
                foreach (var si in item.SelectedPath)
                {
                    selectExpression.Append(si.Identifier + ",");
                }
            }
            selectExpression.Remove(selectExpression.Length - 1, 1);
            selectExpression.Append("\")");

            return selectExpression;
        }       

        /// <summary>
        /// Formulates Filter query options and its respective parameters
        /// </summary>
        /// <param name="odatauri"></param>
        /// <returns></returns>
        private StringBuilder FilterExpression(ODataUri odatauri)
        {
            StringBuilder filterExpression = new StringBuilder();

            //TODO


            return filterExpression;
        }

        /// <summary>
        /// Formulates Orderby query options and its respective parameters
        /// </summary>
        /// <param name="odatauri"></param>
        /// <returns></returns>
        private StringBuilder OrderbyExpression(ODataUri odatauri)
        {
            StringBuilder orderbyExpression = new StringBuilder();

            //TODO
            return orderbyExpression;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="odatauri"></param>
        /// <returns></returns>
        private StringBuilder SearchExpression(ODataUri odatauri)
        {
            StringBuilder searchExpression = new StringBuilder();

            //TODO
            return searchExpression;
        }

        /// <summary>
        /// Formulates Skip query options and its respective value
        /// </summary>
        /// <param name="odatauri"></param>
        /// <returns></returns>
        private StringBuilder SkipExpression(ODataUri odatauri)
        {
            StringBuilder skipExpression = new StringBuilder();
            skipExpression.Append($".Skip({odatauri.Skip})");

            return skipExpression;
        }

        /// <summary>
        /// Formulates SkipToken query options and its respective value
        /// </summary>
        /// <param name="odatauri"></param>
        /// <returns></returns>
        private StringBuilder SkipTokenExpression(ODataUri odatauri)
        {
            StringBuilder skipTokenExpression = new StringBuilder();
            skipTokenExpression.Append($".SkipToken({odatauri.SkipToken})");

            return skipTokenExpression;
        }


        /// <summary>
        /// Formulates Top query options and its respective value
        /// </summary>
        /// <param name="odatauri"></param>
        /// <returns></returns>
        private StringBuilder TopExpression(ODataUri odatauri)
        {
            StringBuilder topExpression = new StringBuilder();
            topExpression.Append($".Top({odatauri.Top})");

            return topExpression;
        }
              

        #endregion


        private static string UppercaseFirstLetter(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }
}
