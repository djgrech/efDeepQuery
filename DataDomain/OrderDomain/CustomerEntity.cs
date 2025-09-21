using System.ComponentModel.DataAnnotations;

namespace DataDomain.Interfaces.Domain;

public class CustomerEntity : IEntity
{
    public int Id { get; set; }
    [StringLength(50)]
    public string FirstName { get; set; }
    [StringLength(50)]
    public string LastName { get; set; }

    public virtual ICollection<OrderEntity> Orders { get; set; }
}
