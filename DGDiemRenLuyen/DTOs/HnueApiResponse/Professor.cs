namespace DGDiemRenLuyen.DTOs.HnueApiResponse
{
    public class Professor
    {
        public string? ProfessorID { get; set; }
        public string? lastName { get; set; }
        public string? firstName { get; set; }
        public string? birthday { get; set; }
        public string? departmentID { get; set; }

        public List<Class> classes { get; set; } = new List<Class>();
    }

    public class Class
    {
        public string? classStudentID { get; set; }
        public string? yearStudy { get; set; }
        public string? termID { get; set; }
    }
}
