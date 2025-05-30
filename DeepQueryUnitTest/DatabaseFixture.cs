using DataDomain;
using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MockQueryable.FakeItEasy;
using Moq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace DeepQueryUnitTest;

public class DatabaseFixture<TContext>
    where TContext : DbContext
{
    private readonly Mock<TContext> MockContext = new();

    public DatabaseFixture<TContext> WithDbSet<T>(IEnumerable<T> data, Expression<Func<TContext, DbSet<T>>> exp)
        where T : class
    {
        //var mockDbSet = GetMockedDbSet(data);

        var mockDbSet = data.AsQueryable().BuildMockDbSet();
        MockContext.Setup(c => c.Set<T>()).Returns(mockDbSet);
        MockContext.Setup(exp).Returns(mockDbSet);

        return this;
    }

    public DatabaseFixture<TContext> WithDbSet1<T>(IEnumerable<T> data, Expression<Func<TContext, DbSet<T>>> exp)
        where T : class
    {
        var mockDbSet = GetMockedDbSet(data).Object;

        MockContext.Setup(c => c.Set<T>()).Returns(mockDbSet);
        MockContext.Setup(exp).Returns(mockDbSet);

        return this;
    }

    public TContext GetMockObject()
    {
        return MockContext.Object;
    }

    private Mock<DbSet<T>> GetMockedDbSet<T>(IEnumerable<T> sourceList) where T : class
    {

        var queryable = new TestAsyncEnumerable<T>(sourceList);
        var sourceListAsQueryable = queryable.AsQueryable();


        var mockSet = new Mock<DbSet<T>>();

        mockSet.As<IAsyncEnumerable<T>>()
               .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
               .Returns((CancellationToken ct) => queryable.GetAsyncEnumerator(ct));

        mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(sourceListAsQueryable.Provider);
        mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(sourceListAsQueryable.Expression);
        mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(sourceListAsQueryable.ElementType);
        mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => sourceListAsQueryable.GetEnumerator());

        return mockSet;
    }

    /*
    public static DbSet<TEntity> CreateMock<TEntity, TContext>(
        List<TEntity> entities, 
        out TContext context) 
        where TEntity : class where TContext : class
    {
        IQueryable<TEntity> data = entities.AsQueryable();

        DbSet<TEntity> mockSet = A.Fake<DbSet<TEntity>>(d => d.Implements<IQueryable<TEntity>>().Implements<IAsyncEnumerable<TEntity>>());
        A.CallTo(() => ((IAsyncEnumerable<TEntity>)mockSet).GetEnumerator())
            .Returns(new TestAsyncEnumerator<TEntity>(data.GetEnumerator()));
        A.CallTo(() => ((IQueryable<TEntity>)mockSet).Provider).Returns(new TestAsyncQueryProvider<TEntity>(data.Provider));
        A.CallTo(() => ((IQueryable<TEntity>)mockSet).Expression).Returns(data.Expression);
        A.CallTo(() => ((IQueryable<TEntity>)mockSet).ElementType).Returns(data.ElementType);
        A.CallTo(() => ((IQueryable<TEntity>)mockSet).GetEnumerator()).Returns(data.GetEnumerator());

        context = A.Fake<TContext>();
        return mockSet;

    }*/
    //FakeItEasy 
}

public class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;
    public TestAsyncEnumerator(IEnumerator<T> inner) => _inner = inner;
    public ValueTask DisposeAsync() => default;
    public ValueTask<bool> MoveNextAsync() => new ValueTask<bool>(_inner.MoveNext());
    public T Current => _inner.Current;
}

public class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    private IEnumerable<T> enumerable;
    private Expression expression;

    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable) 
    {
        this.enumerable = enumerable;
    }
    public TestAsyncEnumerable(Expression expression) : base(expression)
    {
        this.expression = expression;
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
}

public class TestAsyncQueryProvider<T> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner) => _inner = inner;

    public IQueryable CreateQuery(Expression expression) => new TestAsyncEnumerable<T>(expression);
    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => new TestAsyncEnumerable<TElement>(expression);

    public object Execute(Expression expression) => _inner.Execute(expression);
    public TResult Execute<TResult>(Expression expression) => _inner.Execute<TResult>(expression);
    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        // We assume TResult is Task<T>, so we wrap the sync result in Task.FromResult
        var result = _inner.Execute(expression);

        // Cast the result to TResult (which is expected to be Task<T>)
        return (TResult)typeof(Task)
            .GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(result!.GetType())
            .Invoke(null, new[] { result })!;
    }
}
