using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ParentCriterionGetListDetailRequest
{
    [Required]
    public Guid TimeId { get; set; }

    [Required]
    public string? UserId { get; set; }
}
