
using System.ComponentModel.DataAnnotations.Schema;

namespace DGDiemRenLuyen.DTOs.responses;

public class ScoreStatusResponse
{
    public Guid ScoreStatusId { get; set; }

    public string StudentID { get; set; }

    public Guid TimeId { get; set; }

    public int? Status { get; set; }

    public int? SeductedPoint { get; set; }

    public int? PlusPoint { get; set; }

    public string? Note { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}