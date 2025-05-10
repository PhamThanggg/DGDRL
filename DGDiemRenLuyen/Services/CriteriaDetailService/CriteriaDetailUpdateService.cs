using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.Requests.CriteriaDetail;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using DGDiemRenLuyen.Services.AuthService;

namespace DGDiemRenLuyen.Services.CriteriaDetailService
{
    public class CriteriaDetailUpdateService : BaseService<CriteriaDetailUpdateRequest, CriteriaDetailResponse>
    {
        private readonly ICriteriaDetailRepository _criteriaDetailRepository;
        private readonly IChildCriteriaRepository _childCriteriaRepository;
        private readonly ITimeRepository _timeRepository;
        private readonly AuthService.AuthService _authService;
     
        private CriteriaDetail? updateCriteriaDetail;
        private Guid newID = Guid.NewGuid();
        private bool checkStudentScoreUpdate = false;
        private bool checkMoniterScoreUpdate = false;
        private bool checkTeacherScoreUpdate = false;


        public CriteriaDetailUpdateService(
            ICriteriaDetailRepository criteriaDetailRepository,
            IChildCriteriaRepository childCriteriaRepository,
            ITimeRepository timeRepository,
            AuthService.AuthService authService,
            IHttpContextAccessor httpContextAccessor,
            string successMessageDefault = ValidationKeyWords.UPDATE) : base(httpContextAccessor, successMessageDefault)
        {
            _criteriaDetailRepository = criteriaDetailRepository;
            _childCriteriaRepository = childCriteriaRepository;
            _timeRepository = timeRepository;
            _authService = authService;
        }

        public override void P1GenerateObjects()
        {
            updateCriteriaDetail = _criteriaDetailRepository.FindById(_dataRequest.Id);
            if(updateCriteriaDetail == null)
            {
                throw new BaseException { Messages = "Chi tiết diểm rèn luyên không tồn tại. ID: " + _dataRequest.Id };
            }

            checkStudentScoreUpdate = _dataRequest.StudentScore != null && _dataRequest.StudentScore != updateCriteriaDetail.StudentScore;
            checkMoniterScoreUpdate = _dataRequest.MoniterScore != null && _dataRequest.MoniterScore != updateCriteriaDetail.MoniterScore;
            checkTeacherScoreUpdate = _dataRequest.TeacherScore != null && _dataRequest.TeacherScore != updateCriteriaDetail.TeacherScore;
        }

        public override void P2PostValidation()
        {
            Time? timeData = _timeRepository.GetCurrentTimeRecords();
            if (timeData == null)
            {
                throw new BaseException { Messages = "Đã hết thời đánh giá điểm rèn luyện!" };
            }

            // begin authorize
            string RoleTK = Role;
            if (RoleTK == RoleConstants.SV && UserID != updateCriteriaDetail.ScoreStatus.StudentId)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }

            // Trang thai
            if (updateCriteriaDetail.ScoreStatus.Status >= ScoreStatusConstants.SV && RoleTK == RoleConstants.SV)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
            else if (updateCriteriaDetail.ScoreStatus.Status >= ScoreStatusConstants.GV && RoleTK == RoleConstants.GV)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
            else if (updateCriteriaDetail.ScoreStatus.Status >= ScoreStatusConstants.CBL && RoleTK == RoleConstants.CBL)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }
            else if (updateCriteriaDetail.ScoreStatus.Status >= ScoreStatusConstants.TK && RoleTK == RoleConstants.TK)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }

            if (RoleTK == RoleConstants.SV)
            {
                updateCriteriaDetail.StudentScore = _dataRequest.StudentScore ?? updateCriteriaDetail.StudentScore;
                updateCriteriaDetail.MoniterScore = _dataRequest.StudentScore ?? updateCriteriaDetail.StudentScore;
                updateCriteriaDetail.TeacherScore = _dataRequest.StudentScore ?? updateCriteriaDetail.StudentScore;
            }
            // CBL chi sua lop phu trach
            else if (RoleTK == RoleConstants.CBL)
            {
                if (ClassStudentID != updateCriteriaDetail.ScoreStatus.ClassStudentId)
                {
                    throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
                }
                if(RoleTK == updateCriteriaDetail.ScoreStatus.StudentId)
                {
                    updateCriteriaDetail.StudentScore = _dataRequest.StudentScore ?? updateCriteriaDetail.StudentScore;
                    updateCriteriaDetail.MoniterScore = _dataRequest.StudentScore ?? updateCriteriaDetail.StudentScore;
                    updateCriteriaDetail.TeacherScore = _dataRequest.StudentScore ?? updateCriteriaDetail.StudentScore;
                }
                else
                {
                    updateCriteriaDetail.MoniterScore = _dataRequest.MoniterScore ?? updateCriteriaDetail.MoniterScore;
                    updateCriteriaDetail.TeacherScore = _dataRequest.MoniterScore ?? updateCriteriaDetail.MoniterScore;
                }
            }
            // GV chi sua lop phu trach
            else if (RoleTK == RoleConstants.GV)
            {
                var listClass = _authService.GetClasses(
                    UserID, timeData.TermID
                    , timeData.StartYear + "-" + timeData.EndYear);

                if (!listClass.Contains(updateCriteriaDetail.ScoreStatus.ClassStudentId))
                {
                    throw new BaseException { Messages = "Không có quyền cập nhật lớp này." };
                }
                updateCriteriaDetail.TeacherScore = _dataRequest.TeacherScore ?? updateCriteriaDetail.TeacherScore;
            }

            // TK chi sua khoa phu trach
            if (RoleTK == RoleConstants.TK && DepartmentID != updateCriteriaDetail.ScoreStatus.DepartmentId)
            {
                throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
            }

            updateCriteriaDetail.Note = _dataRequest.Note ?? updateCriteriaDetail.Note;
            // end

            
            ChildCriterion? childCriterion = _childCriteriaRepository.GetChildCriteriaByIdAndStatus(updateCriteriaDetail.ChildCriteriaId, 1);
            if (childCriterion == null)
            {
                throw new BaseException { Messages = "Không tồn tại tiêu chí với ID : " + updateCriteriaDetail.ChildCriteriaId };
            }

            if (checkTeacherScoreUpdate)
            {
                CheckMaxScore(_dataRequest.TeacherScore, childCriterion);
            }

            if (checkMoniterScoreUpdate)
            {
                CheckMaxScore(_dataRequest.TeacherScore, childCriterion);
            }

            if (checkStudentScoreUpdate)
            {
                CheckMaxScore(_dataRequest.StudentScore, childCriterion);
            }
        }

        public override void P3AccessDatabase()
        {
            if(updateCriteriaDetail != null)
            {
                _criteriaDetailRepository.Update(updateCriteriaDetail);
                _criteriaDetailRepository.Save();
            }
        }

        public override void P4GenerateResponseData()
        {
            if(updateCriteriaDetail != null)
            {
                _dataResponse = new CriteriaDetailResponse
                {
                    Id = updateCriteriaDetail.Id,
                    ChildCriteriaId = updateCriteriaDetail.ChildCriteriaId,
                    ScoreId = updateCriteriaDetail.ScoreId,
                    Note = updateCriteriaDetail.Note,
                    Proof = updateCriteriaDetail.Proof,
                    StudentScore = updateCriteriaDetail.StudentScore,
                    MoniterScore = updateCriteriaDetail.MoniterScore,
                    TeacherScore = updateCriteriaDetail.TeacherScore,
                    CreatedAt = updateCriteriaDetail.CreatedAt,
                    UpdatedAt = updateCriteriaDetail.UpdatedAt,
                };
            }
        }

        public void CheckMaxScore(int? scoreRequest, ChildCriterion? childCriterion)
        {
            // check giới hạn điểm tiêu chí con
            if (scoreRequest < 0 || scoreRequest > childCriterion.MaxScore)
            {
                throw new BaseException { Messages = "Vui lòng nhập điểm số hợp lệ" };
            }

            // check giới hạn điểm tiêu chí cha
            int totalScore = scoreRequest ?? 0;
            List<CriteriaDetail>? criteriaList = _criteriaDetailRepository.FindByScoreIdAndChildCriteriaParentCriterieId(updateCriteriaDetail.ScoreId, childCriterion.ParentCriteriaId);
            if (criteriaList != null)
            {
                foreach (CriteriaDetail list in criteriaList)
                {
                    if(_dataRequest.TeacherScore != null)
                    {
                        totalScore += list.TeacherScore ?? 0;
                    }
                    else if(_dataRequest.MoniterScore != null)
                    {
                        totalScore += list.MoniterScore ?? 0;
                    }
                    else if(_dataRequest.StudentScore != null)
                    {
                        totalScore += list.StudentScore ?? 0;
                    }

                }
            }

            if (totalScore > childCriterion.ParentCriteria.MaxScore)
            {
                throw new BaseException { Messages = "Bạn đã nhập quá số điểm của phần - " + childCriterion.ParentCriteria.CriteriaName };
            }

        }
    }
}
