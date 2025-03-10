using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.Requests.CriteriaDetail;

public class CriteriaDetailUpdateRequest
{
    [Required]
    public Guid Id { get; set; }

    public string? Note { get; set; }

    public int? StudentScore { get; set; }

    public int? TeacherScore { get; set; }

    public int? MoniterScore { get; set; }
}
