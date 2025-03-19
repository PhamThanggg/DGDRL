using DGDiemRenLuyen.DTOs.Requests.CriteriaDetail;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.CriteriaDetailService
{
    public class CriteriaDetailCreateService : BaseService<CriteriaDetailRequest, CriteriaDetailResponse>
    {
        private readonly ICriteriaDetailRepository _criteriaDetailRepository;
        private readonly IScoreStatusRepository _scoreStatusRepository;
        private readonly IChildCriteriaRepository _childCriteriaRepository;
        private readonly ITimeRepository _timeRepository;
        private CriteriaDetail? newCriteriaDetail;
        private Guid newID = Guid.NewGuid();

        public CriteriaDetailCreateService(
            ICriteriaDetailRepository criteriaDetailRepository,
            IScoreStatusRepository scoreStatustatusRepository,
            IChildCriteriaRepository childCriteriaRepository,
            ITimeRepository timeRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _criteriaDetailRepository = criteriaDetailRepository;
            _scoreStatusRepository = scoreStatustatusRepository;
            _childCriteriaRepository = childCriteriaRepository;
            _timeRepository = timeRepository;
        }

        public override void P1GenerateObjects()
        {
            newCriteriaDetail = new CriteriaDetail
            {
                Id = newID,
                ChildCriteriaId = _dataRequest.ChildCriteriaId,
                ScoreId = _dataRequest.ScoreId,
                Proof = _dataRequest.Proof,
                StudentScore = _dataRequest.StudentScore,
                MoniterScore = _dataRequest.StudentScore,
                TeacherScore = _dataRequest.StudentScore
             };
        }

        public override void P2PostValidation()
        {
            if (string.IsNullOrEmpty(UserID))
            {
                throw new BaseException { Messages = "Bạn không có quyền truy cập" };
            }

            // kiểm tra thời gian xét có tồn tại k?
            Time? timeData = _timeRepository.GetCurrentTimeRecords();

            if (timeData == null)
            {
                throw new BaseException { Messages = "Đã hết thời đánh giá điểm rèn luyện!" };
            }

            ChildCriterion? childCriterion = _childCriteriaRepository.GetChildCriteriaByIdAndStatus(_dataRequest.ChildCriteriaId, 1);
            if (childCriterion == null)
            {
                throw new BaseException { Messages = "Không tồn tại tiêu chí với ID : " + _dataRequest.ChildCriteriaId };
            }

            ScoreStatus scoreStatus = _scoreStatusRepository.GetById(_dataRequest.ScoreId);
            if (scoreStatus == null)
            {
                throw new BaseException { Messages = "Không tồn tại trạng thái điểm với ID : " + _dataRequest.ScoreId };
            }

            // check tồn tại {1 tài khoản trong 1 đợt xét chỉ tạo 1 lần}
            CriteriaDetail? criteriaDetail = _criteriaDetailRepository.FindByChildCriterieIdAndScoreId(childCriterion.Id, scoreStatus.Id);
            if(criteriaDetail != null)
            {
                throw new BaseException { Messages = "Vui lòng nhập ID để có thể cập nhật"};
            }

            // check giới hạn điểm tiêu chí con
            if(_dataRequest.StudentScore < 0 || _dataRequest.StudentScore > childCriterion.MaxScore)
            {
                throw new BaseException { Messages = "Vui lòng nhập điểm số hợp lệ" };
            }

            // check giới hạn điểm tiêu chí cha
            int totalScore = _dataRequest.StudentScore ?? 0;
            List<CriteriaDetail>? criteriaList = _criteriaDetailRepository.FindByScoreIdAndChildCriteriaParentCriterieId(_dataRequest.ScoreId, childCriterion.ParentCriteriaId);
            if (criteriaList != null)
            {
                foreach(CriteriaDetail list in criteriaList)
                {
                    totalScore += list.StudentScore ?? 0;
                }
            }

            if(totalScore > childCriterion.ParentCriteria.MaxScore)
            {
                throw new BaseException { Messages = "Tổng điểm của bạn đã vượt quá số điểm của phần - " + childCriterion.ParentCriteria.CriteriaName };
            }

        }

        public override void P3AccessDatabase()
        {
            _criteriaDetailRepository.Add(newCriteriaDetail);
            _criteriaDetailRepository.Save();
        }

        public override void P4GenerateResponseData()
        {
            if(newCriteriaDetail != null)
            {
                _dataResponse = new CriteriaDetailResponse
                {
                    Id = newCriteriaDetail.Id,
                    ChildCriteriaId = newCriteriaDetail.ChildCriteriaId,
                    ScoreId = newCriteriaDetail.ScoreId,
                    Proof = newCriteriaDetail.Proof,
                    StudentScore = newCriteriaDetail.StudentScore,
                    MoniterScore = newCriteriaDetail.MoniterScore,
                    TeacherScore = newCriteriaDetail.TeacherScore,
                    CreatedAt = newCriteriaDetail.CreatedAt,
                    UpdatedAt = newCriteriaDetail.UpdatedAt,
                };
            }
        }
    }
}
