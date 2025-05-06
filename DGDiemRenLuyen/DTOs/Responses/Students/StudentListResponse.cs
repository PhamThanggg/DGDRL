using DGDiemRenLuyen.DTOs.responses;

namespace DGDiemRenLuyen.DTOs.Responses.Students
{
    public class StudentListResponse
    {
        public string? StudentID { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Birthday { get; set; }
        public string? DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public string? CourseID { get; set; }
        public string? ClassStudentID { get; set; }

        List<ScoreStatusResponse> ScoreStatus { get; set; } = new List<ScoreStatusResponse>();
    }
}
