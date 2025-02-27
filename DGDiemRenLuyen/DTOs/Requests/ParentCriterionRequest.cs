using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ParentCriterionRequest
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string? CriteriaName { get; set; }

    [Required]
    public int? MaxScore { get; set; }

    [Required]
    public int? OrderIndex { get; set; }

    [Required]
    public int? IsActive { get; set; }
}
