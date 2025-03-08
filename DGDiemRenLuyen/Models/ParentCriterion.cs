using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Models;

[Table("parentCriteria")]
public partial class ParentCriterion : BaseEntity
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("criteriaName")]
    public string? CriteriaName { get; set; }

    [Column("maxScore")]
    public int? MaxScore { get; set; }

    [Column("orderIndex")]
    public int? OrderIndex { get; set; }

    [Column("isActive")]
    public int? IsActive { get; set; }

    [Column("note")]
    public string? Note { get; set; }

    [InverseProperty("ParentCriteria")]
    public virtual ICollection<ChildCriterion> ChildCriteria { get; set; } = new List<ChildCriterion>();
}
