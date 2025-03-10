﻿using DGDiemRenLuyen.DTOs.Requests.CriteriaDetail;
using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Services.CriteriaDetailService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGDiemRenLuyen.Controllers
{
    [Route("api/criteria-detail")]   
    public class CriteriaDetailController : ControllerBase
    {
        private readonly CriteriaDetailCreateService _criteriaDetailCreateService;
        private readonly CriteriaDetailUpdateService _criteriaDetailUpdateService;
        private readonly CriteriaDetailGetDetailService _criteriaDetailGetDetailService;
        private readonly CriteriaDetailDeleteService _criteriaDetailDeleteService;

        public CriteriaDetailController(
            CriteriaDetailCreateService criteriaDetailCreateService
            , CriteriaDetailUpdateService criteriaDetailUpdateService
            , CriteriaDetailGetDetailService criteriaDetailGetDetailService
            , CriteriaDetailDeleteService criteriaDetailDeleteService)
        {
            _criteriaDetailCreateService = criteriaDetailCreateService;
            _criteriaDetailUpdateService = criteriaDetailUpdateService;
            _criteriaDetailGetDetailService = criteriaDetailGetDetailService;
            _criteriaDetailDeleteService = criteriaDetailDeleteService;
        }

        [HttpPost]
        [Authorize]
        [Route("create")]
        public IActionResult Create([FromBody] CriteriaDetailRequest criteriaDetailRequest) {
            ApiResponse<CriteriaDetailResponse> result = _criteriaDetailCreateService.Process(criteriaDetailRequest);
            return Ok(result);
        }

        [HttpPut]
        [Authorize]
        [Route("update")]
        public IActionResult Update([FromBody] CriteriaDetailUpdateRequest criteriaDetailUpdateRequest)
        {
            ApiResponse<CriteriaDetailResponse> result = _criteriaDetailUpdateService.Process(criteriaDetailUpdateRequest);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetDetail(Guid? id)
        {
            ApiResponse<CriteriaDetailResponse> result = _criteriaDetailGetDetailService.Process(id);
            return Ok(result);
        }

        /*[HttpPost]
        [Authorize]
        [Route("get-list")]
        public IActionResult GetList([FromBody] CriteriaDetailGetListRequest criteriaDetailGetListRequest)
        {
            var result = _criteriaDetailGetListService.Process(criteriaDetailGetListRequest);

            return Ok(result);
        }*/

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid? id)
        {
            ApiResponse<string> result = _criteriaDetailDeleteService.Process(id);

            return Ok(result);
        }


    }
}
