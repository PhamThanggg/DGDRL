using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;


namespace DGDiemRenLuyen.Services.ScoreStatusService
{
    public class ScoreStatusUpdateService : BaseService<ScoreStatusRequest, ScoreStatusResponse>
    {
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private readonly AuthService.AuthService  _authService;
        private ScoreStatus? updateScoreStatus;
        private bool checkUpdate = false;

        public ScoreStatusUpdateService(
            IScoreStatusRepository scoreStatusRepository,
            IHttpContextAccessor httpContextAccessor,
            AuthService.AuthService authService,
            string successMessageDefault = ValidationKeyWords.UPDATE) : base(httpContextAccessor, successMessageDefault)
        {
            _scoreStatusRepository = scoreStatusRepository;
            _authService = authService;
        }

        public override void P1GenerateObjects()
        {
            updateScoreStatus = _scoreStatusRepository.FindById(_dataRequest.Id);
            if(updateScoreStatus == null)
            {
                throw new BaseException { Messages = "Điểm rèn luyện không tồn tại." };
            }

            updateScoreStatus.Status = _dataRequest.status ?? updateScoreStatus.Status;

            if(_dataRequest.SeductedPoint != null 
                || _dataRequest.PlusPoint != null 
                || !string.IsNullOrEmpty(_dataRequest.Note))
            {
                checkUpdate = true;
            }

            updateScoreStatus.SeductedPoint = _dataRequest.SeductedPoint ?? updateScoreStatus.SeductedPoint;
            updateScoreStatus.PlusPoint = _dataRequest.PlusPoint ?? updateScoreStatus.PlusPoint;
            updateScoreStatus.Note = _dataRequest.Note ?? updateScoreStatus.Note;
        }

        public override void P2PostValidation()
        {
            // begin phan quyen
            string RoleTK = Role;
            string userID = UserID;
            // SV chi dc sua ban ghi cua minh
            if (RoleTK == RoleConstants.SV && updateScoreStatus.StudentId != userID)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }

            // khong the chinh sua khi diem dã duyệt
            CheckUpdate(RoleTK);

            // Cập nhật trạng thái điểm theo vai trò
            CheckUpdateStatus(RoleTK);

            // CBL chi sua lop phu trach
            if(RoleTK ==  RoleConstants.CBL && ClassStudentID != updateScoreStatus.ClassStudentId) 
            {
                throw new BaseException { Messages = ValidationKeyWords.SOCRE_STATUS };
            }

            // GV chi sua lop phu trach
            if(RoleTK == RoleConstants.GV)
            {
                var listClass = _authService.GetClasses(
                    userID, updateScoreStatus.Time.TermID
                    , updateScoreStatus.Time.StartYear + "-" + updateScoreStatus.Time.EndYear);

                if (!listClass.Contains(updateScoreStatus.ClassStudentId))
                {
                    throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
                }
            }

            // TK chi sua khoa phu trach
            if (RoleTK == RoleConstants.TK && DepartmentID != updateScoreStatus.DepartmentId)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }

            // end phan quyen
        }

        public override void P3AccessDatabase()
        {
            if(updateScoreStatus != null)
            {
                _scoreStatusRepository.Update(updateScoreStatus);
                _scoreStatusRepository.Save();
            }
        }

        public override void P4GenerateResponseData()
        {
            if(updateScoreStatus != null)
            {
                _dataResponse = new ScoreStatusResponse
                {
                    ScoreStatusId = updateScoreStatus.Id,
                    StudentID = updateScoreStatus.StudentId,
                    DepartmentID = updateScoreStatus.DepartmentId,
                    ClassStudentID = updateScoreStatus.ClassStudentId,
                    TimeId = updateScoreStatus.TimeId,
                    Status = updateScoreStatus.Status,
                    SeductedPoint = updateScoreStatus.SeductedPoint,
                    PlusPoint = updateScoreStatus.PlusPoint,
                    Note = updateScoreStatus.Note,
                    CreatedAt = updateScoreStatus.CreatedAt,
                    UpdatedAt = updateScoreStatus.UpdatedAt,
                };
            }
        }

        
        private void CheckUpdate(string RoleTK)
        {
            // SV khong duoc sua SeductedPoint, PlusPoint, Note
            if (RoleTK == RoleConstants.SV && checkUpdate)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
            else if (_dataRequest.status == ScoreStatusConstants.GV && RoleTK == RoleConstants.GV && checkUpdate)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
            else if (_dataRequest.status == ScoreStatusConstants.CBL && RoleTK == RoleConstants.CBL && checkUpdate)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
            else if (_dataRequest.status == ScoreStatusConstants.TK && RoleTK == RoleConstants.TK && checkUpdate)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
        }

        private void CheckUpdateStatus(string RoleTK)
        {
            // Trang thai diem
            if (_dataRequest.status < updateScoreStatus.Status - 1 || _dataRequest.status > ScoreStatusConstants.TK)
            {
                throw new BaseException { Messages = ValidationKeyWords.SOCRE_STATUS };
            }

            if (_dataRequest.status > ScoreStatusConstants.SV && RoleTK == RoleConstants.SV)
            {
                throw new BaseException { Messages = ValidationKeyWords.SOCRE_STATUS };
            }
            else if (_dataRequest.status > ScoreStatusConstants.GV && RoleTK == RoleConstants.GV)
            {
                throw new BaseException { Messages = ValidationKeyWords.SOCRE_STATUS };
            }
            else if (_dataRequest.status > ScoreStatusConstants.CBL && RoleTK == RoleConstants.CBL)
            {
                throw new BaseException { Messages = ValidationKeyWords.SOCRE_STATUS };
            }
            else if (_dataRequest.status > ScoreStatusConstants.TK && RoleTK == RoleConstants.TK)
            {
                throw new BaseException { Messages = ValidationKeyWords.SOCRE_STATUS };
            }
        }


    }
}
