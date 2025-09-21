using GraphQL;
using GraphQL.DataLoader;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using GraphQL.Utilities;
using GraphQLSample.Requests.GraphQL;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

//using GraphQL.NewtonsoftJson;


namespace GraphQLSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GraphQLController : ControllerBase
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly ISchema _schema;
        private readonly ILogger<GraphQLController> _logger;
        private readonly IGraphQLTextSerializer _serializer;
        private readonly IServiceProvider _services;

        public GraphQLController(
            IDocumentExecuter documentExecuter,
            ISchema schema,
            ILogger<GraphQLController> logger,
            IGraphQLTextSerializer serializer,
            IServiceProvider services)
        {
            _documentExecuter = documentExecuter;
            _schema = schema;
            _logger = logger;
            _serializer = serializer;
            _services = services;
        }

        [HttpPost("test")]
        public IActionResult Test([FromBody] GqlRequest request)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GqlRequest request)
        {
            if (request == null || !ModelState.IsValid)
                return BadRequest();

            var listener = _services.GetRequiredService<DataLoaderDocumentListener>();

            Dictionary<string, object> variablesDict = new Dictionary<string, object>();

            if (request.Variables.HasValue && request.Variables.Value.ValueKind == JsonValueKind.Object)
            {
                variablesDict = (Dictionary<string, object>)ConvertJsonElement(request.Variables.Value);
            }



            var result = await _documentExecuter.ExecuteAsync(opts =>
            {
                opts.Schema = _schema;
                opts.Query = request.Query;
                opts.OperationName = request.OperationName;
                opts.Variables = variablesDict != null ? new Inputs(variablesDict) : null; //.ToInputs();//  request.Variables?.ToInputs();
                opts.ThrowOnUnhandledException = false;
                opts.RequestServices = HttpContext.RequestServices;
                opts.Listeners.Add(listener);

                /*if (!string.IsNullOrEmpty(vars))
                    opts.Inputs = vars.ToInputs();*/
            });

            if (result.Errors?.Any() == true)
            {
                return BadRequest(result.Errors);
            }

            // Use GraphQL's built-in serializer to handle ExecutionResult
            var json = _serializer.Serialize(result);

            // Return serialized JSON manually
            return new ContentResult
            {
                Content = json,
                ContentType = "application/json",
                StatusCode = result.Errors?.Count > 0 ? 400 : 200
            };
        }

        private object ConvertJsonElement(JsonElement element)
        {
            switch (element.ValueKind)
            {
                case JsonValueKind.Object:
                    var dict = new Dictionary<string, object>();
                    foreach (var prop in element.EnumerateObject())
                        dict[prop.Name] = ConvertJsonElement(prop.Value);
                    return dict;
                case JsonValueKind.Array:
                    var list = new List<object>();
                    foreach (var item in element.EnumerateArray())
                        list.Add(ConvertJsonElement(item));
                    return list;
                case JsonValueKind.String:
                    return element.GetString()!;
                case JsonValueKind.Number:
                    if (element.TryGetInt32(out int i)) return i;
                    if (element.TryGetInt64(out long l)) return l;
                    if (element.TryGetDouble(out double d)) return d;
                    return element.GetDecimal();
                case JsonValueKind.True: return true;
                case JsonValueKind.False: return false;
                case JsonValueKind.Null: return null!;
                default: throw new NotSupportedException($"Unsupported JsonValueKind {element.ValueKind}");
            }
        }
    }
}

