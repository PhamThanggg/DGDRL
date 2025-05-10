using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.DTOs.Responses.Students;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using DGDiemRenLuyen.Services.AuthService;

namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusGetListService : BaseService<ScoreStatusGetListRequest, PageResponse<List<StudentResponse>>>
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private readonly ITimeRepository _timeRepository;
        private readonly ApiClientService _apiClientService;
        private readonly AuthService.AuthService _authService;
        private List<StudentResponse> _studentListResponses;
        private Time _time = null;
        private int _totalRecords;

        public ScoreStatusGetListService(
            IScoreStatusRepository scoreStatusRepository,
            IHttpContextAccessor httpContextAccessor,
            ApiClientService apiClientService,
            AuthService.AuthService authService) : base(httpContextAccessor)
        {
            _scoreStatusRepository = scoreStatusRepository;
            _apiClientService = apiClientService;
            _authService = authService;
        }

        public override void P1GenerateObjects()
        {
            _dataRequest.PageIndex = _dataRequest.PageIndex ?? 1;
            _dataRequest.PageSize = _dataRequest.PageSize ?? 20;
        }

        public override void P2PostValidation()
        {

            // begin authorize
            string RoleTK = Role;
            // CBL chi sua lop phu trach
            if (RoleTK == RoleConstants.CBL && ClassStudentID != _dataRequest.ClassStudentID)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }

            // GV chi sua lop phu trach
            if (RoleTK == RoleConstants.GV)
            {
                _time = _timeRepository.GetById(_dataRequest.TimeId);
                if(_time == null)
                {
                    throw new BaseException { Messages = "Thời gian đánh giá điểm không tồn tại!" };
                }
                var listClass = _authService.GetClasses(
                    UserID, _time.TermID
                    , _time.StartYear + "-" + _time.EndYear);

                if (!listClass.Contains(_dataRequest.ClassStudentID))
                {
                    throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
                }
            }


            // TK chi sua khoa phu trach
            if (RoleTK == RoleConstants.TK && DepartmentID != _dataRequest.DepartmentID)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
            // end authorize
        }

        public override void P3AccessDatabase()
        {
            var requestData = new
            {
                StudentID = _dataRequest.StudentID,
                DepartmentID = _dataRequest.DepartmentID,
                CourseID = _dataRequest.CourseID,
                ClassStudentID = _dataRequest.ClassStudentID,
                PageSize = _dataRequest.PageSize,
                PageIndex = _dataRequest.PageIndex
            };

            string url = ApiRoute.baseUrl + ApiRoute.getListStudent;

            var apiTask = _apiClientService.GetDataFromApi<PageResponse<List<StudentResponse>>>(url, requestData);
            Task.WhenAll(apiTask);
            var apiResponse = apiTask.Result;

            // list msv tu api hnue
            var studentIDs = apiResponse.Data.Data.Select(s => s.StudentID).ToList();

            // truy van DB lay sv
            var studentsFromDb = _scoreStatusRepository.FindInStudentId(studentIDs, _dataRequest.TimeId);

            // map du lieu
            var studentsFromDbDictionary = studentsFromDb.ToDictionary(s => s.StudentId);

            var result = apiResponse.Data.Data.Select(apiStudent =>
            {
                // Kiểm tra nếu sinh viên có trong DB
                if (studentsFromDbDictionary.TryGetValue(apiStudent.StudentID, out var dbStudent))
                {
                    // Map các dữ liệu từ DB vào API_HNUE
                    apiStudent.ScoreStatusId = dbStudent.Id;
                    apiStudent.StudentID = dbStudent.StudentId;
                    apiStudent.TimeId = dbStudent.TimeId;
                    apiStudent.Status = dbStudent.Status;
                    apiStudent.SeductedPoint = dbStudent.SeductedPoint;
                    apiStudent.PlusPoint = dbStudent.PlusPoint;
                    apiStudent.Note = dbStudent.Note;
                    apiStudent.CreatedAt = dbStudent.CreatedAt;
                    apiStudent.UpdatedAt = dbStudent.UpdatedAt;
                }
                

                return apiStudent;
            }).ToList();

            _studentListResponses = result;
            _totalRecords = apiResponse.Data.TotalRecords;

        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = new PageResponse<List<StudentResponse>>
            {
                Data = _studentListResponses,
                PageIndex = _dataRequest.PageIndex ?? 1,
                PageSize = _dataRequest.PageSize ?? 20,
                TotalRecords = _totalRecords,
            };
        }
    }
}