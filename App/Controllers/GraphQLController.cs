using GraphQL;
using GraphQL.DataLoader;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using GraphQL.Utilities;
using Microsoft.AspNetCore.Mvc;
using GraphQLSample.Requests.GraphQL;

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

            var result = await _documentExecuter.ExecuteAsync(opts =>
            {
                opts.Schema = _schema;
                opts.Query = request.Query;
                opts.OperationName = request.OperationName;
                opts.Variables = request.Variables?.ToInputs();
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
    }
}

