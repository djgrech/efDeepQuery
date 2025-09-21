using GraphQL.Types;
using GraphQLSample.Models;

namespace GraphQLSample.Schema;

public class HelloWorldType : ObjectGraphType<HelloWorld>
{
    public HelloWorldType()
    {
        Name = "HelloWorld";
        Field(x => x.Message).Description("A welcome message.");
    }
}
