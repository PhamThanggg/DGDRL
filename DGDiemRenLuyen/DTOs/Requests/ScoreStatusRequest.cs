using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ScoreStatusRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid TimeId { get; set; }

    public int? status { get; set; }

    public int? SeductedPoint { get; set; }

    public int? PlusPoint { get; set; }

    public string? Note { get; set; }
}
