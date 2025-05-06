using DGDiemRenLuyen.DTOs.Requests;
using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.RoleAssignmentService
{
    public class RoleAssignmentCreateService : BaseService<RoleAssignmentRequest, RoleAssignmentResponse>
    {
        private readonly IRoleAssignmentRepository _roleAssignmentRepository;
        private RoleAssignment? newRoleAssignment;

        public RoleAssignmentCreateService(
            IRoleAssignmentRepository roleAssignmentRepository,
            IHttpContextAccessor httpContextAccessor,
            string successMessageDefault = ValidationKeyWords.CREATE) : base(httpContextAccessor, successMessageDefault)
        {
            _roleAssignmentRepository = roleAssignmentRepository;
        }

        public override void P1GenerateObjects()
        {
            newRoleAssignment = new RoleAssignment
            {
                ObjectId = _dataRequest.ObjectId,
                ObjectType = _dataRequest.ObjectType,
                Role = _dataRequest.Role,
            };
        }

        public override void P2PostValidation()
        {
            if (_roleAssignmentRepository.ExistsBy(pc => pc.ObjectId == _dataRequest.ObjectId))
            {
                throw new BaseException { Messages = "Id đã tồn tại." };
            }
        }

        public override void P3AccessDatabase()
        {
            _roleAssignmentRepository.Add(newRoleAssignment);
            _roleAssignmentRepository.Save();
        }

        public override void P4GenerateResponseData()
        {
            if(newRoleAssignment != null)
            {
                _dataResponse = new RoleAssignmentResponse
                {
                    ObjectId = newRoleAssignment.ObjectId,
                    ObjectType = newRoleAssignment.ObjectType,
                    Role = newRoleAssignment.Role,
                    CreatedAt = newRoleAssignment.CreatedAt,
                    UpdatedAt = newRoleAssignment.UpdatedAt,
                };
            }
        }
    }
}
