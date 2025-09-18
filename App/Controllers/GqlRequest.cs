
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace GraphQLSample.Requests.GraphQL
{
    public class GqlRequest
    {
        public string Query { get; set; }
        public string OperationName { get; set; }
        public Dictionary<string, object>? Variables { get; set; }
    }
}
