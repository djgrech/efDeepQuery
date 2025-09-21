using DataService.DTOs;
using DataService.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(
    ILogger<OrderController> logger,
    IOrderService orderService
) : ControllerBase
{
    [HttpPost]
    public async Task<List<OrderDTO>> Get(Request request)
    {
        var result = await orderService.GetFilteredData(request.Filter, request.Sort);

        return [.. result];
    }

    [HttpPost("ids")]
    public async Task<List<OrderDTO>> GetByIds(HashSet<int> ids)
    {
        var result = await orderService.GetByIds(ids);
        return [.. result];
    }


    [HttpPost("byCustomerIds")]
    public async Task<List<OrderDTO>> GeByCustomerIds(HashSet<int> ids)
    {
        return [.. await orderService.GetByCustomerIds(ids)];
    }
}
