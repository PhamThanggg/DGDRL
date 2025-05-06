using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ScoreStatusRequest
{
    [Required]
    public Guid Id { get; set; }

    public Guid TimeId { get; set; }

    [MaxLength(255)]
    public string? StudentId { get; set; }

    public int? status { get; set; }

    public int? SeductedPoint { get; set; }

    public int? PlusPoint { get; set; }

    public string? Note { get; set; }
}
