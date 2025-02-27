using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Models;

[Table("score")]
public partial class Score
{
    [Key]
    [Column("scoreId")]
    [StringLength(255)]
    public string ScoreId { get; set; } = null!;

    [Column("studentId")]
    [StringLength(255)]
    public string? StudentId { get; set; }

    [Column("timeId")]
    [StringLength(255)]
    public string? TimeId { get; set; }

    [Column("studentScore")]
    public int? StudentScore { get; set; }

    [Column("moniterScore")]
    public int? MoniterScore { get; set; }

    [Column("teacherScore")]
    public int? TeacherScore { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("createdAt", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updatedAt", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Score")]
    public virtual ICollection<CriteriaDetail> CriteriaDetails { get; set; } = new List<CriteriaDetail>();

    [ForeignKey("StudentId")]
    [InverseProperty("Scores")]
    public virtual Student? Student { get; set; }

    [ForeignKey("TimeId")]
    [InverseProperty("Scores")]
    public virtual Time? Time { get; set; }
}
