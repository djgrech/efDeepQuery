using System.ComponentModel.DataAnnotations;

namespace DataDomain.Order
{
    public class Product
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
