using Business.APIBusinessServices.CountryServices;
using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Services.ParentCriterionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DGDiemRenLuyen.Controllers
{
    [Route("api/parent-criterion")]   
    public class ParentCriterionController : ControllerBase
    {
        private readonly ParentCriterionCreateService _parentCriterionCreateService;
        private readonly ParentCriterionUpdateService _parentCriterionUpdateService;
        private readonly ParentCriterionGetDetailService _parentCriterionGetDetailService;
        private readonly ParentCriterionGetListService _parentCriterionGetListService;
        private readonly ParentCriterionDeleteService _parentCriterionDeleteService;

        public ParentCriterionController(
            ParentCriterionCreateService parentCriterionCreateService
            , ParentCriterionUpdateService parentCriterionUpdateService
            , ParentCriterionGetDetailService parentCriterionGetDetailService
            , ParentCriterionGetListService parentCriterionGetListService
            , ParentCriterionDeleteService parentCriterionDeleteService)
        {
            _parentCriterionCreateService = parentCriterionCreateService;
            _parentCriterionUpdateService = parentCriterionUpdateService;
            _parentCriterionGetDetailService = parentCriterionGetDetailService;
            _parentCriterionGetListService = parentCriterionGetListService;
            _parentCriterionDeleteService = parentCriterionDeleteService;
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public IActionResult Create([FromBody] ParentCriterionRequest parentCriterionRequest) {
            ApiResponse<ParentCriterionResponse> result = _parentCriterionCreateService.Process(parentCriterionRequest);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        public IActionResult Update([FromBody] ParentCriterionRequest parentCriterionRequest)
        {
            ApiResponse<ParentCriterionResponse> result = _parentCriterionUpdateService.Process(parentCriterionRequest);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetDetail(Guid? id)
        {
            var user = HttpContext.User;

            // Lấy sub (ID user)
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            // Lấy email
            var email = user.FindFirst(ClaimTypes.Email)?.Value;

            // Lấy preferred_username
            var username = user.FindFirst("preferred_username")?.Value;

            // Lấy role từ Keycloak
            var role = user.FindFirst("roleDB")?.Value;

            ApiResponse<ParentCriterionResponse> result = _parentCriterionGetDetailService.Process(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        [Route("get-list")]
        public IActionResult GetList([FromBody] ParentCriterionGetListRequest parentCriterionGetListRequest)
        {
            var result = _parentCriterionGetListService.Process(parentCriterionGetListRequest);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid? id)
        {
            ApiResponse<string> result = _parentCriterionDeleteService.Process(id);

            return Ok(result);
        }


    }
}
