using Microsoft.AspNetCore.Http;

namespace DGDiemRenLuyen.Common
{
    public class ApiRoute
    {
        public const string baseUrl = "https://localhost:44328/api/";


        // professor
        public const string getProfessorList = "professor/get-list";
        public const string getDetailProfessor = "professor/";

        // student
        public const string getDetailStudent = "student/";
        public const string getListStudent = "student/get-list";
    }
}
