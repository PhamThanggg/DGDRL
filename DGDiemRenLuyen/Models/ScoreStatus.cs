using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGDiemRenLuyen.Models;

[Table("scoreStatus")]
public partial class ScoreStatus : BaseEntity
{
    [Key]
    [Column("scoreId")]
    public Guid Id { get; set; }

    [Column("studentId")]
    [StringLength(255)]
    public string? StudentId { get; set; }

    [Column("departmentId")]
    [StringLength(30)]
    public string? DepartmentId { get; set; }

    [Column("classStudentId")]
    [StringLength(30)]
    public string? ClassStudentId { get; set; }

    [Column("timeId")]
    public Guid TimeId { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("seductedPoint")]
    public int? SeductedPoint { get; set; }

    [Column("plusPoint")]
    public int? PlusPoint { get; set; }

    [Column("note")]
    public string? Note { get; set; }

    [InverseProperty("ScoreStatus")]
    public virtual ICollection<CriteriaDetail> CriteriaDetails { get; set; } = new List<CriteriaDetail>();

    [ForeignKey("TimeId")]
    [InverseProperty("ScoreStatus")]
    public virtual Time? Time { get; set; }
}
