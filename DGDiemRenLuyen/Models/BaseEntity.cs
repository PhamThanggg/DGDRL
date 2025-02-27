using System.ComponentModel.DataAnnotations.Schema;

namespace DGDiemRenLuyen.Models
{
    public class BaseEntity
    {
        [Column("createdAt", TypeName = "datetime")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updatedAt", TypeName = "datetime")]
        public DateTime? UpdatedAt { get; set; }
    }
}
