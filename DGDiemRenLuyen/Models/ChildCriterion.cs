using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Models;

[Table("childCriteria")]
public partial class ChildCriterion
{
    [Key]
    [Column("id")]
    [StringLength(255)]
    public string Id { get; set; } = null!;

    [Column("parentCriteriaId")]
    [StringLength(255)]
    public string? ParentCriteriaId { get; set; }

    [Column("criteriaName", TypeName = "text")]
    public string? CriteriaName { get; set; }

    [Column("maxScore")]
    public int? MaxScore { get; set; }

    [Column("orderIndex")]
    public int? OrderIndex { get; set; }

    [Column("criteriaType")]
    public int? CriteriaType { get; set; }

    [Column("isActive")]
    public int? IsActive { get; set; }

    [Column("createdAt", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updatedAt", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("ChildCriteria")]
    public virtual ICollection<CriteriaDetail> CriteriaDetails { get; set; } = new List<CriteriaDetail>();

    [ForeignKey("ParentCriteriaId")]
    [InverseProperty("ChildCriteria")]
    public virtual ParentCriterion? ParentCriteria { get; set; }
}
