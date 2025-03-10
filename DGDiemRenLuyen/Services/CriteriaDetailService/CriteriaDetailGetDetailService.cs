using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.CriteriaDetailService
{
    public class CriteriaDetailGetDetailService : BaseService<Guid?, CriteriaDetailResponse>
    {
        private readonly ICriteriaDetailRepository _criteriaDetailRepository;
        private CriteriaDetail? _criteriaDetail;

        public CriteriaDetailGetDetailService(
            ICriteriaDetailRepository criteriaDetailRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _criteriaDetailRepository = criteriaDetailRepository;
        }

        public override void P1GenerateObjects()
        {
            if (!_dataRequest.HasValue)
            {
                throw new BaseException { Messages = "Giá trị truyền vào không hợp lệ." };
            }
        }

        public override void P2PostValidation()
        {
            _criteriaDetail = _criteriaDetailRepository.GetById(_dataRequest.Value);
            if (_criteriaDetail == null)
            {
                throw new BaseException
                {
                    StatusCode = StatusCodes.Status404NotFound.ToString(),
                    Messages = "Id không tồn tại!"
                };
            }
        }

        public override void P3AccessDatabase()
        {
            
        }

        public override void P4GenerateResponseData()
        {
            if(_criteriaDetail != null)
            {
                _dataResponse = new CriteriaDetailResponse
                {
                    Id = _criteriaDetail.Id,
                    ChildCriteriaId = _criteriaDetail.ChildCriteriaId,
                    ScoreId = _criteriaDetail.ScoreId,
                    Note = _criteriaDetail.Note,
                    Proof = _criteriaDetail.Proof,
                    StudentScore = _criteriaDetail.StudentScore,
                    MoniterScore = _criteriaDetail.MoniterScore,
                    TeacherScore = _criteriaDetail.TeacherScore,
                    CreatedAt = _criteriaDetail.CreatedAt,
                    UpdatedAt = _criteriaDetail.UpdatedAt,
                };
            }
        }
    }
}
