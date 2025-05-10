using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGDiemRenLuyen.Models;

[Table("activeTokens")]
public partial class ActiveToken
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("objectId")]
    [StringLength(20)]
    public string ObjectId { get; set; }

    [Column("expiresAt", TypeName = "datetime")]
    public DateTime ExpiresAt { get; set; }
}
