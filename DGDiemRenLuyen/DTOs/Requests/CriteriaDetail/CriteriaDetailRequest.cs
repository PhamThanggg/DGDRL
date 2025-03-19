using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.Requests.CriteriaDetail;

public class CriteriaDetailRequest
{
    public Guid Id { get; set; }

    [Required]
    public Guid ChildCriteriaId { get; set; }

    [Required]
    public Guid ScoreId { get; set; }

    [MaxLength(255)]
    public string? Proof { get; set; }

    [Required]
    public int? StudentScore { get; set; }
}
