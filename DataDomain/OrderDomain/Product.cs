using System.ComponentModel.DataAnnotations;

namespace DataDomain.Interfaces.Domain;

public class Product : IEntity
{
    public int Id { get; set; }
    [StringLength(50)]
    public string Name { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}
