
namespace DGDiemRenLuyen.DTOs.responses;

public class CriteriaDetailResponse
{
    public Guid Id { get; set; }

    public Guid ChildCriteriaId { get; set; }

    public Guid ScoreId { get; set; }

    public string? Proof { get; set; }

    public string? Note { get; set; }

    public int? StudentScore { get; set; }

    public int? MoniterScore { get; set; }

    public int? TeacherScore { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}