using Business.APIBusinessServices.CountryServices;
using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.HnueApiResponse;
using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Services.ChildCriterionService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace DGDiemRenLuyen.Controllers
{
    [Route("api/child-criterion")]   
    public class ChildCriterionController : ControllerBase
    {
        private readonly ChildCriterionCreateService _childCriterionCreateService;
        private readonly ChildCriterionUpdateService _childCriterionUpdateService;
        private readonly ChildCriterionGetDetailService _childCriterionGetDetailService;
        private readonly ChildCriterionGetListService _childCriterionGetListService;
        private readonly ChildCriterionDeleteService _childCriterionDeleteService;

        public ChildCriterionController(
            ChildCriterionCreateService childCriterionCreateService
            , ChildCriterionUpdateService childCriterionUpdateService
            , ChildCriterionGetDetailService childCriterionGetDetailService
            , ChildCriterionGetListService childCriterionGetListService
            , ChildCriterionDeleteService childCriterionDeleteService)
        {
            _childCriterionCreateService = childCriterionCreateService;
            _childCriterionUpdateService = childCriterionUpdateService;
            _childCriterionGetDetailService = childCriterionGetDetailService;
            _childCriterionGetListService = childCriterionGetListService;
            _childCriterionDeleteService = childCriterionDeleteService;
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("create")]
        public IActionResult Create([FromBody] ChildCriterionRequest childCriterionRequest) {
            ApiResponse<ChildCriterionResponse> result = _childCriterionCreateService.Process(childCriterionRequest);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("update")]
        public IActionResult Update([FromBody] ChildCriterionRequest childCriterionRequest)
        {
            ApiResponse<ChildCriterionResponse> result = _childCriterionUpdateService.Process(childCriterionRequest);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetail(Guid? id)
        {
            ApiResponse<ChildCriterionResponse> result =  _childCriterionGetDetailService.Process(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("get-list")]
        public IActionResult GetList([FromBody] ChildCriterionGetListRequest childCriterionGetListRequest)
        {
            var result = _childCriterionGetListService.Process(childCriterionGetListRequest);

            return Ok(result);
        }

        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid? id)
        {
            ApiResponse<string> result = _childCriterionDeleteService.Process(id);

            return Ok(result);
        }


    }
}
