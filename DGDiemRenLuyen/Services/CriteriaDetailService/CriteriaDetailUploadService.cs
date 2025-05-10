using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.Requests.CriteriaDetail;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using DGDiemRenLuyen.Services.UploadFileSrevice;

namespace DGDiemRenLuyen.Services.CriteriaDetailService
{
    public class CriteriaDetailUploadService : BaseService<CriteriaDetailUploadRequest, CriteriaDetailResponse>
    {
        private readonly ICriteriaDetailRepository _criteriaDetailRepository;
        private readonly IChildCriteriaRepository _childCriteriaRepository;
        private readonly ITimeRepository _timeRepository;
        private readonly FileService _fileService;
     
        private CriteriaDetail? updateCriteriaDetail;


        public CriteriaDetailUploadService(
            ICriteriaDetailRepository criteriaDetailRepository,
            IChildCriteriaRepository childCriteriaRepository,
            ITimeRepository timeRepository,
            FileService fileService,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _criteriaDetailRepository = criteriaDetailRepository;
            _childCriteriaRepository = childCriteriaRepository;
            _timeRepository = timeRepository;
            _fileService = fileService;
        }

        public override void P1GenerateObjects()
        {
            updateCriteriaDetail = _criteriaDetailRepository.FindById(_dataRequest.Id);
            if(updateCriteriaDetail == null)
            {
                throw new BaseException { Messages = "Chi tiết diểm rèn luyên không tồn tại. ID: " + _dataRequest.Id };
            }

            // begin authorize
            if (Role == RoleConstants.SV)
            {
                if (UserID != updateCriteriaDetail.ScoreStatus.StudentId)
                {
                    throw new BaseException { Messages = ValidationKeyWords.ACCESS_DENIED };
                }
            }
            // end

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

            if (_dataRequest.File != null && _dataRequest.File.Length > 0)
            {
                var oldFilePath = updateCriteriaDetail.Proof;

                var filePath = _fileService.UploadPdfAsync(_dataRequest.File, UserID).GetAwaiter().GetResult();
                updateCriteriaDetail.Proof = filePath;

                if (!string.IsNullOrEmpty(oldFilePath))
                {
                    var deleteSuccess = _fileService.DeleteFileAsync(oldFilePath).GetAwaiter().GetResult();
                    if (!deleteSuccess)
                    {
                        throw new BaseException { Messages = "Không thể xóa tệp cũ: " + oldFilePath };
                    }
                }
            }
        }

        public override void P2PostValidation()
        {   
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
