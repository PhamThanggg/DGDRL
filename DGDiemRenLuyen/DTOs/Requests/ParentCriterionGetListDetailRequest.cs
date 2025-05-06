using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ParentCriterionGetListDetailRequest
{
    [Required]
    public Guid TimeId { get; set; }

    [Required]
    public Guid scoreId { get; set; }

    /*[Required]
    [MaxLength(255)]
    public string? UserId { get; set; }*/
}
