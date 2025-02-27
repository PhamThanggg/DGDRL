using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Models;

[Table("criteriaDetail")]
public partial class CriteriaDetail
{
    [Key]
    [Column("id")]
    [StringLength(255)]
    public string Id { get; set; } = null!;

    [Column("childCriteriaId")]
    [StringLength(255)]
    public string? ChildCriteriaId { get; set; }

    [Column("scoreId")]
    [StringLength(255)]
    public string? ScoreId { get; set; }

    [Column("proof")]
    [StringLength(255)]
    public string? Proof { get; set; }

    [Column("note")]
    [StringLength(255)]
    public string? Note { get; set; }

    [Column("status")]
    public int? Status { get; set; }

    [Column("studentScore")]
    public int? StudentScore { get; set; }

    [Column("moniterScore")]
    public int? MoniterScore { get; set; }

    [Column("teacherScore")]
    public int? TeacherScore { get; set; }

    [Column("createdAt", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updatedAt", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("ChildCriteriaId")]
    [InverseProperty("CriteriaDetails")]
    public virtual ChildCriterion? ChildCriteria { get; set; }

    [ForeignKey("ScoreId")]
    [InverseProperty("CriteriaDetails")]
    public virtual Score? Score { get; set; }
}
