using GraphQL.Types;

namespace GraphQLSample.Schema.Inputs;

public class HelloInputType : InputObjectGraphType<HelloParam>
{
    public HelloInputType()
    {
        Name = "MyInput123";
        Field<StringGraphType, string>("name");
    }
}

public class HelloParam
{
    public string Name { get; set; }
}
