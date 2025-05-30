
using System.ComponentModel.DataAnnotations;

namespace DataDomain
{
    public class Brand
    {
        [StringLength(100)]
        public string Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }

        public string OrganizationId { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
