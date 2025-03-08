using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ChildCriterionRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid ParentCriteriaId { get; set; }

    [Required]
    public string? CriteriaName { get; set; }

    [Required]
    public int? MaxScore { get; set; }

    [Required]
    public int? OrderIndex { get; set; }

    [Required]
    public string? Note { get; set; }

    [Required]
    public int? IsActive { get; set; }

    [Required]
    public int? CriteriaType { get; set; }
}
