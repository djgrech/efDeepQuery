using DataAccess;
using DataDomain.Order;
using EFDeepQueryDynamicLinq;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {
        private readonly ILogger<DataController> _logger;
        private readonly IEFFilterTranslator translator;
        private readonly ApplicationContext context;

        public DataController(ILogger<DataController> logger, IEFFilterTranslator translator, ApplicationContext context)
        {
            _logger = logger;
            this.translator = translator;
            this.context = context;
        }

        [HttpPost]
        public List<Order> Get(Request request)
        {
            var query = context.Set<Order>().AsQueryable();
            query = translator.BuildQuery(query, request.Filter, request.Sort);
            var list = query.ToList();

            return list;

            return [.. query];
        }
    }
}


public class Request
{
    public FilterGroup Filter { get; set; }
    public SortInput? Sort { get; set; }
}

