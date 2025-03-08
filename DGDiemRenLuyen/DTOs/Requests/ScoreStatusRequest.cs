using System;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ScoreStatusRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid timeId { get; set; }

    [Required]
    public int? status { get; set; }
}
