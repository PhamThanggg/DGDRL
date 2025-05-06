using Business.APIBusinessServices.CountryServices;
using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.Requests;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Services.RoleAssignmentService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGDiemRenLuyen.Controllers
{
    [Route("api/role-assignment")]   
    public class RoleAssignmentController : ControllerBase
    {
        private readonly RoleAssignmentCreateService _roleAssignmentCreateService;
        private readonly RoleAssignmentUpdateService _roleAssignmentUpdateService;
        private readonly RoleAssignmentGetDetailService _roleAssignmentGetDetailService;
        private readonly RoleAssignmentGetListService _roleAssignmentGetListService;
        private readonly RoleAssignmentDeleteService _roleAssignmentDeleteService;

        public RoleAssignmentController(
            RoleAssignmentCreateService roleAssignmentCreateService
            , RoleAssignmentUpdateService roleAssignmentUpdateService
            , RoleAssignmentGetDetailService roleAssignmentGetDetailService
            , RoleAssignmentGetListService roleAssignmentGetListService
            , RoleAssignmentDeleteService roleAssignmentDeleteService)
        {
            _roleAssignmentCreateService = roleAssignmentCreateService;
            _roleAssignmentUpdateService = roleAssignmentUpdateService;
            _roleAssignmentGetDetailService = roleAssignmentGetDetailService;
            _roleAssignmentGetListService = roleAssignmentGetListService;
            _roleAssignmentDeleteService = roleAssignmentDeleteService;
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("create")]
        public IActionResult Create([FromBody] RoleAssignmentRequest roleAssignmentRequest) {
            ApiResponse<RoleAssignmentResponse> result = _roleAssignmentCreateService.Process(roleAssignmentRequest);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("update")]
        public IActionResult Update([FromBody] RoleAssignmentRequest roleAssignmentRequest)
        {
            ApiResponse<RoleAssignmentResponse> result = _roleAssignmentUpdateService.Process(roleAssignmentRequest);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [HttpGet("{id}")]
        public IActionResult GetDetail(string id)
        {
            ApiResponse<RoleAssignmentResponse> result = _roleAssignmentGetDetailService.Process(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("get-list")]
        public IActionResult GetList([FromBody] RoleAssignmentGetListRequest roleAssignmentGetListRequest)
        {
            var result = _roleAssignmentGetListService.Process(roleAssignmentGetListRequest);

            return Ok(result);
        }

        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            ApiResponse<string> result = _roleAssignmentDeleteService.Process(id);

            return Ok(result);
        }
    }
}
