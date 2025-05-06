using Microsoft.AspNetCore.Http;

namespace DGDiemRenLuyen.Common
{
    public class ApiRoute
    {
        public const string baseUrl = "https://localhost:44328/api/";

        public const string getProfessorList = "course/get-list";


        // student
        public const string getDetailStudent = "student/";
        public const string getListStudent = "student/get-list";
    }
}
