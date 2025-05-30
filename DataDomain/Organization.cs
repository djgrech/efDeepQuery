
using System.ComponentModel.DataAnnotations;

namespace DataDomain
{
    public class Organization
    {
        [StringLength(100)]
        public string Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Brand> Brands { get; set; }
    }
}
