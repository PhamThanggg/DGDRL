using DGDiemRenLuyen.DTOs.Requests;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories;
using DGDiemRenLuyen.Repositories.Interfaces;
using System.Linq.Expressions;

namespace DGDiemRenLuyen.Services.RoleAssignmentService
{
    public class RoleAssignmentGetListService : BaseService<RoleAssignmentGetListRequest, PageResponse<List<RoleAssignmentResponse>>>
    {
        private readonly IRoleAssignmentRepository _roleAssignmentRepository;
        private List<RoleAssignmentResponse> _listRoleAssignment;
        private int _totalRecords;

        public RoleAssignmentGetListService(
            IRoleAssignmentRepository roleAssignmentRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _roleAssignmentRepository = roleAssignmentRepository;
        }

        public override void P1GenerateObjects()
        {
            _dataRequest.PageIndex = _dataRequest.PageIndex ?? 1;
            _dataRequest.PageSize = _dataRequest.PageSize ?? 20;
        }

        public override void P2PostValidation()
        {
            
        }

        public override void P3AccessDatabase()
        {
            int skipRows = _dataRequest.PageSize.Value * (_dataRequest.PageIndex.Value - 1);

            Expression<Func<RoleAssignment, bool>> searchCondition = s =>
                (string.IsNullOrEmpty(_dataRequest.ObjectId) || (s.ObjectId ?? "").Contains(_dataRequest.ObjectId)) &&
                (string.IsNullOrEmpty(_dataRequest.ObjectType) || (s.ObjectType ?? "").Contains(_dataRequest.ObjectType)) &&
                (string.IsNullOrEmpty(_dataRequest.Role) || (s.Role ?? "").Contains(_dataRequest.Role));

            _totalRecords = _roleAssignmentRepository.GetBy(searchCondition).Count();

            _listRoleAssignment = _roleAssignmentRepository
                 .GetBy(searchCondition)
                 .Select(s => new RoleAssignmentResponse
                 {
                     ObjectId = s.ObjectId,
                     ObjectType = s.ObjectType,
                     Role = s.Role,
                     CreatedAt = s.CreatedAt,
                     UpdatedAt = s.UpdatedAt
                 })
                 .Skip(skipRows)
                 .Take(_dataRequest.PageSize.Value)
                 .ToList();
        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = new PageResponse<List<RoleAssignmentResponse>>
            {
                Data = _listRoleAssignment,
                PageIndex = _dataRequest.PageIndex ?? 1,
                PageSize = _dataRequest.PageSize ?? 20,
                TotalRecords = _totalRecords
            };
        }
    }
}
