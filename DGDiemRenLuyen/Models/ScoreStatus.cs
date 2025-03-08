using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGDiemRenLuyen.Models;

[Table("score")]
public partial class ScoreStatus : BaseEntity
{
    [Key]
    [Column("scoreId")]
    public Guid ScoreId { get; set; }

    [Column("studentId")]
    [StringLength(255)]
    public string? StudentId { get; set; }

    [Column("timeId")]
    public Guid TimeId { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [InverseProperty("Score")]
    public virtual ICollection<CriteriaDetail> CriteriaDetails { get; set; } = new List<CriteriaDetail>();

    [ForeignKey("StudentId")]
    [InverseProperty("Scores")]
    public virtual Student? Student { get; set; }

    [ForeignKey("TimeId")]
    [InverseProperty("Scores")]
    public virtual Time? Time { get; set; }
}
