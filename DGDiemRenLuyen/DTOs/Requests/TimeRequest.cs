using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public partial class TimeRequest
{
    public Guid Id { get; set; }

    [Required]
    public int? Semester { get; set; }

    [Required]
    public int? StartYear { get; set; }

    [Required]
    public DateTime? StartDate { get; set; }

    [Required]
    public DateTime? EndDate { get; set; }
}
