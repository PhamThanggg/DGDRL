using DGDiemRenLuyen.DTOs.responses;

namespace DGDiemRenLuyen.DTOs.Responses.Students
{
    public class StudentResponse : ScoreStatusResponse
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Birthday { get; set; }
        public string? DepartmentID { get; set; }
        public string? DepartmentName { get; set; }
        public string? CourseID { get; set; }
        public string? ClassStudentID { get; set; }

    }
}
