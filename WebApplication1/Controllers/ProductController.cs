using DataService.DTOs;
using DataService.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(
    ILogger<ProductController> logger,
    IProductService productService
) : ControllerBase
{
    [HttpPost]
    public async Task<List<ProductDTO>> Get(Request request)
    {
        var result = await productService.GetFilteredData(request.Filter, request.Sort);

        return [.. result];
    }

    [HttpPost("ids")]
    public async Task<List<ProductDTO>> GetByIds(HashSet<int> ids)
    {
        var result = await productService.GetByIds(ids);
        return [.. result];
    }
}

