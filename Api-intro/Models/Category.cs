using System.ComponentModel.DataAnnotations;

namespace Api_intro.Models
{
    public class Category : BaseEntity
    {
        [StringLength(10)]
        public string Name { get; set; }
    }
}
