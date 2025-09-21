using DataDomain.Interfaces.Domain;
using System.ComponentModel.DataAnnotations;

namespace DataDomain.Interfaces.Domain;

public class Customer : IEntity
{
    public int Id { get; set; }
    [StringLength(50)]
    public string FirstName { get; set; }
    [StringLength(50)]
    public string LastName { get; set; }

    public virtual ICollection<Order> Orders { get; set; }
}
