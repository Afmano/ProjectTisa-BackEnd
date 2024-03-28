using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProjectTisa.Models.BusinessLogic
{
    /// <summary>
    /// Model for category. Primary use at <see cref="Product"/>.
    /// </summary>
    public class Category()
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required string Name { get; set; }
        public required string PhotoPath { get; set; }
        public virtual Category? SubCategory { get; set; }
        [JsonIgnore]
        public virtual List<Product> Products { get; set; } = [];
        public required virtual EditInfo EditInfo { get; set; }
    }
}
