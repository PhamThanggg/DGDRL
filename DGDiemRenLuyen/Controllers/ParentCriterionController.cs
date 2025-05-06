using Business.APIBusinessServices.CountryServices;
using DGDiemRenLuyen.Common;
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
        private readonly ParentCriterionGetListDetailService _parentCriterionGetListDetailService;
        private readonly ParentCriterionGetListService _parentCriterionGetListService;
        private readonly ParentCriterionDeleteService _parentCriterionDeleteService;
        private readonly ParentCriterionExportPDFService _parentCriterionExportPDFService;

        public ParentCriterionController(
            ParentCriterionCreateService parentCriterionCreateService
            , ParentCriterionUpdateService parentCriterionUpdateService
            , ParentCriterionGetDetailService parentCriterionGetDetailService
            , ParentCriterionGetListDetailService parentCriterionGetListDetailService
            , ParentCriterionGetListService parentCriterionGetListService
            , ParentCriterionDeleteService parentCriterionDeleteService
            , ParentCriterionExportPDFService parentCriterionExportPDFService)
        {
            _parentCriterionCreateService = parentCriterionCreateService;
            _parentCriterionUpdateService = parentCriterionUpdateService;
            _parentCriterionGetDetailService = parentCriterionGetDetailService;
            _parentCriterionGetListDetailService = parentCriterionGetListDetailService;
            _parentCriterionGetListService = parentCriterionGetListService;
            _parentCriterionDeleteService = parentCriterionDeleteService;
            _parentCriterionExportPDFService = parentCriterionExportPDFService;
            _parentCriterionExportPDFService = parentCriterionExportPDFService;
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("create")]
        public IActionResult Create([FromBody] ParentCriterionRequest parentCriterionRequest) {
            ApiResponse<ParentCriterionResponse> result = _parentCriterionCreateService.Process(parentCriterionRequest);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("update")]
        public IActionResult Update([FromBody] ParentCriterionRequest parentCriterionRequest)
        {
            ApiResponse<ParentCriterionResponse> result = _parentCriterionUpdateService.Process(parentCriterionRequest);
            return Ok(result);
        }

        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [HttpGet("{id}")]
        public IActionResult GetDetail(Guid? id)
        {
            ApiResponse<ParentCriterionResponse> result = _parentCriterionGetDetailService.Process(id);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("get-list")]
        public IActionResult GetList([FromBody] ParentCriterionGetListRequest parentCriterionGetListRequest)
        {
            var result = _parentCriterionGetListService.Process(parentCriterionGetListRequest);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("get-list-detail")]
        public IActionResult GetListDetail([FromBody] ParentCriterionGetListDetailRequest parentCriterionGetListDetailRequest)
        {
            var result = _parentCriterionGetListDetailService.Process(parentCriterionGetListDetailRequest);

            return Ok(result);
        }

        [HttpPost("export-pdf")]
        public async Task<IActionResult> ExportPDF([FromBody] ParentCriterionGetListDetailRequest request)
        {
            var response = _parentCriterionExportPDFService.Process(request); 
            var fileResult = await response.Data;

            return fileResult;
        }

        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid? id)
        {
            ApiResponse<string> result = _parentCriterionDeleteService.Process(id);

            return Ok(result);
        }


    }
}
