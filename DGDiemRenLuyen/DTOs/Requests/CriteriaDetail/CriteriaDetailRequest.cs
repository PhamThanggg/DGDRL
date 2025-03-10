using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.Requests.CriteriaDetail;

public class CriteriaDetailRequest
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public Guid ChildCriteriaId { get; set; }

    [Required]
    public Guid ScoreId { get; set; }

    public string? Proof { get; set; }

    public int? StudentScore { get; set; }
}
