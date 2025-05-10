using DGDiemRenLuyen.DTOs.Requests;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.RoleAssignmentService
{
    public class RoleAssignmentUpdateService : BaseService<RoleAssignmentRequest, RoleAssignmentResponse>
    {
        private readonly IRoleAssignmentRepository _roleAssignmentRepository;
        private RoleAssignment? updateRoleAssignment;

        public RoleAssignmentUpdateService(
            IRoleAssignmentRepository roleAssignmentRepository,
            IHttpContextAccessor httpContextAccessor,
            string successMessageDefault = ValidationKeyWords.UPDATE) : base(httpContextAccessor, successMessageDefault)
        {
            _roleAssignmentRepository = roleAssignmentRepository;
        }

        public override void P1GenerateObjects()
        {
            updateRoleAssignment = _roleAssignmentRepository.GetByObjectId(_dataRequest.ObjectId);
            if(updateRoleAssignment == null)
            {
                throw new BaseException { Messages = "ID không tồn tại." };
            }

            updateRoleAssignment.ObjectType = _dataRequest.ObjectType ?? updateRoleAssignment.ObjectType;
            updateRoleAssignment.Role = _dataRequest.Role ?? updateRoleAssignment.Role;
        }

        public override void P2PostValidation()
        {
            
        }

        public override void P3AccessDatabase()
        {
            if(updateRoleAssignment != null)
            {
                _roleAssignmentRepository.Update(updateRoleAssignment);
                _roleAssignmentRepository.Save();
            }
        }

        public override void P4GenerateResponseData()
        {
            if(updateRoleAssignment != null)
            {
                _dataResponse = new RoleAssignmentResponse
                {
                    ObjectId = updateRoleAssignment.ObjectId,
                    ObjectType = updateRoleAssignment.ObjectType,
                    Role = updateRoleAssignment.Role,
                    CreatedAt = updateRoleAssignment.CreatedAt,
                    UpdatedAt = updateRoleAssignment.UpdatedAt,
                };
            }
        }
    }
}
