using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;

namespace DGDiemRenLuyen.Services.RoleAssignmentService
{
    public class RoleAssignmentGetDetailService : BaseService<string, RoleAssignmentResponse>
    {
        private readonly IRoleAssignmentRepository _roleAssignmentRepository;
        private RoleAssignment? roleAssignment;

        public RoleAssignmentGetDetailService(
            IRoleAssignmentRepository roleAssignmentRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _roleAssignmentRepository = roleAssignmentRepository;
        }

        public override void P1GenerateObjects()
        {
            if (string.IsNullOrEmpty(_dataRequest))
            {
                throw new BaseException { Messages = "Giá trị truyền vào không hợp lệ." };
            }
        }

        public override void P2PostValidation()
        {
            
        }

        public override void P3AccessDatabase()
        {
            roleAssignment = _roleAssignmentRepository.GetByObjectId(_dataRequest);
            if (roleAssignment == null)
            {
                throw new BaseException
                {
                    StatusCode = StatusCodes.Status404NotFound.ToString(),
                    Messages = "Id không tồn tại!"
                };
            }
        }

        public override void P4GenerateResponseData()
        {
            if(roleAssignment != null)
            {
                _dataResponse = new RoleAssignmentResponse
                {
                    ObjectId = roleAssignment.ObjectId,
                    ObjectType = roleAssignment.ObjectType,
                    Role = roleAssignment.Role,
                    CreatedAt = roleAssignment.CreatedAt,
                    UpdatedAt = roleAssignment.UpdatedAt,
                };
            }
        }
    }
}
