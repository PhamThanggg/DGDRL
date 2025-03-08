using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.responses;

public class ScoreStatusResponse
{
    public Guid Id { get; set; }

    public Guid studentId { get; set; }

    public Guid timeId { get; set; }

    public int? status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}