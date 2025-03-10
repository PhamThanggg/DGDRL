using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace DGDiemRenLuyen.Models;

[Table("childCriteria")]
public partial class ChildCriterion : BaseEntity
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("parentCriteriaId")]
    public Guid ParentCriteriaId { get; set; }

    [Column("criteriaName")]
    public string? CriteriaName { get; set; }

    [Column("maxScore")]
    public int? MaxScore { get; set; }

    [Column("orderIndex")]
    public int? OrderIndex { get; set; }

    [Column("criteriaType")]
    public int? CriteriaType { get; set; }

    [Column("isActive")]
    public int? IsActive { get; set; }

    [Column("note")]
    public string? Note { get; set; }

    [InverseProperty("ChildCriteria")]
    public virtual ICollection<CriteriaDetail> CriteriaDetails { get; set; } = new List<CriteriaDetail>();

    [JsonIgnore]
    [ForeignKey("ParentCriteriaId")]
    [InverseProperty("ChildCriteria")]
    public virtual ParentCriterion? ParentCriteria { get; set; }
}
