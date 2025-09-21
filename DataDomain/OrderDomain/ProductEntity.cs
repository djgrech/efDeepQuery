using System.ComponentModel.DataAnnotations;

namespace DataDomain.Interfaces.Domain;

public class ProductEntity : IEntity
{
    public int Id { get; set; }
    [StringLength(50)]
    public string Name { get; set; }
    public virtual ICollection<OrderEntity> Orders { get; set; }
}
