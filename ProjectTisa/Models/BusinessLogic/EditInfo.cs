using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ProjectTisa.Models.BusinessLogic
{
    /// <summary>
    /// Complex type for storing changes in other models.
    /// </summary>
    [ComplexType]
    public class EditInfo
    {
        public EditInfo() { }
        [SetsRequiredMembers]
        public EditInfo(string Author)
        {
            CreationTime = DateTime.UtcNow;
            CreatedBy = Author;
            ModificationTime = CreationTime;
            ModifiedBy = CreatedBy;
        }
        public required DateTime CreationTime { get; init; }
        public required string CreatedBy { get; init; }
        public required DateTime ModificationTime { get; set; }
        public required string ModifiedBy { get; set; }
        public void Modify(string Auhor)
        {
            ModificationTime = DateTime.UtcNow;
            ModifiedBy = Auhor;
        }
    }
}
