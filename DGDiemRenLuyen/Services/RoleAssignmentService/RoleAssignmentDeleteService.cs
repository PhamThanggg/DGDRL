using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Extentions;
using DGDiemRenLuyen.Models;
using DGDiemRenLuyen.Repositories.Interfaces;
using DGDiemRenLuyen.Services;
using System;

namespace Business.APIBusinessServices.CountryServices
{
    public class RoleAssignmentDeleteService : BaseService<string, string>
    {
        private readonly IRoleAssignmentRepository _roleAssignmentRepository;


        public RoleAssignmentDeleteService(
           IRoleAssignmentRepository roleAssignmentRepository,
           IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor
        )
        {
            _roleAssignmentRepository = roleAssignmentRepository;
        }

        public override void P1GenerateObjects()
        {
            
        }

        public override void P2PostValidation()
        {
            if (_dataRequest == null)
            {
                throw new BaseException { Messages = "Id không đúng!" };
            }
        }

        public override void P3AccessDatabase()
        {
            _roleAssignmentRepository.Delete<string?>(_dataRequest);
            _roleAssignmentRepository.Save();

        }

        public override void P4GenerateResponseData()
        {
            _dataResponse = ValidationKeyWords.DELETE.ToString();

        }
    }
}

