using DGDiemRenLuyen.DTOs.Requests;
using System.ComponentModel.DataAnnotations;

namespace DGDiemRenLuyen.DTOs.requsets;

public class ScoreStatusGetListRequest : BaseListRequest
{
    public string? DepartmentID { get; set; }
    public string? CourseID { get; set; }
    public string? ClassStudentID { get; set; }

    [MaxLength(255)]
    public string? StudentID { get; set; }

    public Guid? TimeId { get; set; }
}
