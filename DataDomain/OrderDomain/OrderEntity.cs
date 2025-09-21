namespace DataDomain.Interfaces.Domain;

public class OrderEntity : IEntity
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }

    public int ProductId { get; set; }
    public virtual ProductEntity Product { get; set; }

    public int CustomerId { get; set; }
    public virtual CustomerEntity Customer { get; set; }
}
