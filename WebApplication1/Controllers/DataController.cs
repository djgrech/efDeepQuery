using DataAccess;
using DataDomain.Order;
using EFDeepQueryDynamicLinq;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataController(
    ILogger<DataController> logger,
    IEFFilterTranslator translator,
    ApplicationContext context
) : ControllerBase
{
    [HttpPost]
    public List<Order> Get(Request request)
    {
        var query = context.Set<Order>().AsQueryable();
        query = translator.BuildQuery(query, request.Filter, request.Sort);
        return [.. query];
    }
}

public class Request
{
    public FilterGroup Filter { get; set; }
    public SortInput? Sort { get; set; }
}

