using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public partial class TimeRequest
{
    public Guid Id { get; set; }

    [Required]
    [RegularExpression("^(HK01|HK02|HK03)$", ErrorMessage = "Chỉ chấp nhận: HK01, HK02, HK03")]
    public string? TermID { get; set; }

    [Required]
    public int? StartYear { get; set; }

    [Required]
    public DateTime? StartDate { get; set; }

    [Required]
    public DateTime? EndDate { get; set; }
}
