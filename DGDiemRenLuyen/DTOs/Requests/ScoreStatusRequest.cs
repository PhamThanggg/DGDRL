using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ScoreStatusRequest
{
    [Required]
    public Guid Id { get; set; }

    
    public Guid TimeId { get; set; }

    [MaxLength(255)]
    public string? StudentId { get; set; }

    [Required]
    public int? status { get; set; }
}
