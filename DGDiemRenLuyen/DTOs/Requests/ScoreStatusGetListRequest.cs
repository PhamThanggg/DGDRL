using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ScoreStatusGetListRequest
{
    public string? StudentId { get; set; }

    public Guid? TimeId { get; set; }

    [Required]
    public int? PageSize { get; set; }

    [Required]
    public int? PageIndex { get; set; }
}
