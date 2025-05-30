using DataAccess;
using DataDomain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DeepQueryUnitTest;

// unit test still not working due to the async
public class DeepQueryTest : IClassFixture<DatabaseFixture<ApplicationContext>>
{
    private readonly DatabaseFixture<ApplicationContext> _databaseFixture;

    public DeepQueryTest(DatabaseFixture<ApplicationContext> databaseFixture)
    {
        _databaseFixture = databaseFixture;
    }


    //private readonly DatabaseFixture<PassportContext> databaseFixture = new DatabaseFixture<PassportContext>();

    [Fact]
    public async Task Test1()
    {
        var s = typeof(IAsyncQueryProvider).GetMethod("ExecuteAsync");

        var context = GetContext();
        var dataService = new DataService();

        var include = "Posts.User.Name";
        var query = "Posts.User.Name";

        var list = await context.Blogs.Where(x=>x.Id == 1).ToListAsync();

        list = await Query<Blog>(context, include, query, new List<string>() { "joe" });

        

        //var result1 = await Query<Blog>(context, "Posts.User.Name", "Posts.User.Name", new List<string>() { "joe" });

        list = null;

    }

    public Task<List<T>> Query<T>(DbContext context, string include, string queryStr, List<string> values)
       where T : class
    {
        var dbSet = context.Set<T>();
        //var query = dbSet.AsQueryable();


        int lastIndex = include.LastIndexOf('.');
        var q = dbSet.Include(include.Substring(0, lastIndex));

        q = q.WhereNavigationPropertyIn(queryStr, values);

        return q.ToListAsync();
        /*
        query = query
            .Include(include.Substring(0, lastIndex))
            .WhereNavigationPropertyIn(queryStr, values);

        return query.ToListAsync();*/
    }


    private ApplicationContext GetContext()
        => _databaseFixture
            .WithDbSet(TestData.Blogs, x => x.Blogs)
            .WithDbSet(TestData.Posts, x => x.Posts)
            .WithDbSet(TestData.Users, x => x.Users)
            .GetMockObject()
            ;
}