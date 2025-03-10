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
        private readonly ITimeRepository _timeRepository;
     
        private CriteriaDetail? updateCriteriaDetail;
        private Guid newID = Guid.NewGuid();

        public CriteriaDetailUpdateService(
            ICriteriaDetailRepository criteriaDetailRepository,
            ITimeRepository timeRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _criteriaDetailRepository = criteriaDetailRepository;
            _timeRepository = timeRepository;
        }

        public override void P1GenerateObjects()
        {
            updateCriteriaDetail = _criteriaDetailRepository.GetById<Guid?>(_dataRequest.Id);
            if(updateCriteriaDetail == null)
            {
                throw new BaseException { Messages = "Chi tiết diểm rèn luyên không tồn tại. ID: " + _dataRequest.Id };
            }

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
    }
}
