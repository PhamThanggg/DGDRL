using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ParentCriterionRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(500)]
    public string? CriteriaName { get; set; }

    [Required]
    public int? MaxScore { get; set; }

    [Required]
    public int? OrderIndex { get; set; }

    [Required]
    [MaxLength(255)]
    public string? Note { get; set; }

    [Required]
    public int? IsActive { get; set; }
}
