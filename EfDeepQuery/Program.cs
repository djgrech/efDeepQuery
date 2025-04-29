using DataAccess;
using DataDomain;
using EfDeepQuery;
using Microsoft.EntityFrameworkCore;

var context = new ApplicationContext();

var result1 = await Query<Blog>(context, "Posts.User.Name", "Posts.User.Name", new List<string>() { "joe" });

var result2 = await Query<Blog>(context, "Posts.User.Name", "Name", new List<string>() { "blog1" });

result2 = null;
/*
var query = context.Blogs.AsQueryable();

var result = context.Blogs
    .Include(x => x.Posts)
        .ThenInclude(x => x.User)
    .WhereNavigationPropertyIn("Posts.User.Name", new List<string> { "joe", "mary" })
    .FirstOrDefault();

result = null;
*/

Task<List<T>> Query<T>(DbContext context, string include, string queryStr, List<string> values)
    where T : class
{
    var dbSet = context.Set<T>();
    var query = dbSet.AsQueryable();

    int lastIndex = include.LastIndexOf('.');
    query = query.Include(include.Substring(0, lastIndex)).WhereNavigationPropertyIn(queryStr, values);

    return query.ToListAsync();
}

