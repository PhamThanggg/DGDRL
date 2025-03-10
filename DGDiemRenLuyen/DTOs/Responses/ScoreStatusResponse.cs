using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.responses;

public class ScoreStatusResponse
{
    public Guid Id { get; set; }

    public string StudentId { get; set; }

    public Guid TimeId { get; set; }

    public int? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}