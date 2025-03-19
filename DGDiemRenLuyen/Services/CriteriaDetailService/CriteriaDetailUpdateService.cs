using DGDiemRenLuyen.DTOs.Requests.CriteriaDetail;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.CriteriaDetailService
{
    public class CriteriaDetailUpdateService : BaseService<CriteriaDetailUpdateRequest, CriteriaDetailResponse>
    {
        private readonly ICriteriaDetailRepository _criteriaDetailRepository;
        private readonly IChildCriteriaRepository _childCriteriaRepository;
        private readonly ITimeRepository _timeRepository;
     
        private CriteriaDetail? updateCriteriaDetail;
        private Guid newID = Guid.NewGuid();
        private bool checkStudentScoreUpdate = false;
        private bool checkMoniterScoreUpdate = false;
        private bool checkTeacherScoreUpdate = false;


        public CriteriaDetailUpdateService(
            ICriteriaDetailRepository criteriaDetailRepository,
            IChildCriteriaRepository childCriteriaRepository,
            ITimeRepository timeRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _criteriaDetailRepository = criteriaDetailRepository;
            _childCriteriaRepository = childCriteriaRepository;
            _timeRepository = timeRepository;
        }

        public override void P1GenerateObjects()
        {
            updateCriteriaDetail = _criteriaDetailRepository.GetById<Guid?>(_dataRequest.Id);
            if(updateCriteriaDetail == null)
            {
                throw new BaseException { Messages = "Chi tiết diểm rèn luyên không tồn tại. ID: " + _dataRequest.Id };
            }

            checkStudentScoreUpdate = _dataRequest.StudentScore != null && _dataRequest.StudentScore != updateCriteriaDetail.StudentScore;
            checkMoniterScoreUpdate = _dataRequest.MoniterScore != null && _dataRequest.MoniterScore != updateCriteriaDetail.MoniterScore;
            checkTeacherScoreUpdate = _dataRequest.TeacherScore != null && _dataRequest.TeacherScore != updateCriteriaDetail.TeacherScore;

            // chưa phân quyền {chỉ đúng người mới sửa điểm...}
            updateCriteriaDetail.Note = _dataRequest.Note ?? updateCriteriaDetail.Note;
            updateCriteriaDetail.StudentScore = _dataRequest.StudentScore ?? updateCriteriaDetail.StudentScore;
            updateCriteriaDetail.MoniterScore = _dataRequest.MoniterScore ?? updateCriteriaDetail.MoniterScore;
            updateCriteriaDetail.TeacherScore = _dataRequest.TeacherScore ?? updateCriteriaDetail.TeacherScore;

        }

        public override void P2PostValidation()
        {
            if (string.IsNullOrEmpty(UserID))
            {
                throw new BaseException { Messages = "Bạn không có quyền truy cập" };
            }

            Time? timeData = _timeRepository.GetCurrentTimeRecords();

            if (timeData == null)
            {
                throw new BaseException { Messages = "Đã hết thời đánh giá điểm rèn luyện!" };
            }

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
