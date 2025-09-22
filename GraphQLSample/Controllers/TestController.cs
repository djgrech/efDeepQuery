using Microsoft.AspNetCore.Mvc;

namespace GraphQLSample.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public ActionResult Get()
    {
        return Ok("hello world - gql - 1.0.0");
    }
}

