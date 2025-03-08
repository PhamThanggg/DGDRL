using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Models;

[Table("criteriaDetail")]
public partial class CriteriaDetail : BaseEntity
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("childCriteriaId")]
    public Guid ChildCriteriaId { get; set; }

    [Column("scoreId")]
    public Guid ScoreId { get; set; }

    [Column("proof")]
    [StringLength(255)]
    public string? Proof { get; set; }

    [Column("note")]
    [StringLength(255)]
    public string? Note { get; set; }

    [Column("studentScore")]
    public int? StudentScore { get; set; }

    [Column("moniterScore")]
    public int? MoniterScore { get; set; }

    [Column("teacherScore")]
    public int? TeacherScore { get; set; }

    [ForeignKey("ChildCriteriaId")]
    [InverseProperty("CriteriaDetails")]
    public virtual ChildCriterion? ChildCriteria { get; set; }

    [ForeignKey("ScoreId")]
    [InverseProperty("CriteriaDetails")]
    public virtual ScoreStatus? Score { get; set; }
}
