using Common;

namespace WebApplication1.Controllers;

public class Request
{
    public FilterGroup? Filter { get; set; }
    public SortInput? Sort { get; set; }
}
