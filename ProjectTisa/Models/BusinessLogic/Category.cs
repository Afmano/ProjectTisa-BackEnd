using Microsoft.AspNetCore.Identity;
using ProjectTisa.Controllers.GeneralData.Requests.CreationReq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace ProjectTisa.Models.BusinessLogic
{
    /// <summary>
    /// Model for category. Primary use at <see cref="Product"/>.
    /// </summary>
    public class Category
    {
        public Category() { }
        [SetsRequiredMembers]
        public Category(CategoryCreationReq request, EditInfo editInfo, Category? parentCategory, int id = 0)
        {
            Id = id;
            Name = request.Name;
            PhotoPath = request.PhotoPath;
            ParentCategory = parentCategory;
            EditInfo = editInfo;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private init; }
        public required string Name { get; set; }
        public required string PhotoPath { get; set; }
        public virtual Category? ParentCategory { get; set; }
        public virtual List<Category> SubCategories { get; set; } = [];
        [JsonIgnore]
        public virtual List<Product> Products { get; set; } = [];
        public required virtual EditInfo EditInfo { get; set; }
    }
}
