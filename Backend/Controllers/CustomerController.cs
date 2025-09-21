using DataService.DTOs;
using DataService.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController(
    ILogger<CustomerController> logger,
    ICustomerService customerService
) : ControllerBase
{
    [HttpPost]
    public async Task<List<CustomerDTO>> Get(Request request)
    {
        var result = await customerService.GetFilteredData(request.Filter, request.Sort);

        return [.. result];
    }

    [HttpPost("ids")]
    public async Task<List<CustomerDTO>> GetByIds(HashSet<int> ids)
    {
        var result = await customerService.GetByIds(ids);
        return [.. result];
    }
}

