using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.Services.TimeService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGDiemRenLuyen.Controllers
{
    [Route("api/time")]   
    public class TimeController : ControllerBase
    {
        private readonly TimeCreateService _timeCreateService;
        private readonly TimeUpdateService _timeUpdateService;
        private readonly TimeGetDetailService _timeGetDetailService;
        private readonly TimeGetListService _timeGetListService;
        private readonly TimeDeleteService _timeDeleteService;

        public TimeController(
            TimeCreateService timeCreateService
            , TimeUpdateService timeUpdateService
            , TimeGetDetailService timeGetDetailService
            , TimeGetListService timeGetListService
            , TimeDeleteService timeDeleteService)
        {
            _timeCreateService = timeCreateService;
            _timeUpdateService = timeUpdateService;
            _timeGetDetailService = timeGetDetailService;
            _timeGetListService = timeGetListService;
            _timeDeleteService = timeDeleteService;
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("create")]
        public IActionResult Create([FromBody] TimeRequest timeRequest) {
            ApiResponse<TimeResponse> result = _timeCreateService.Process(timeRequest);
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [Route("update")]
        public IActionResult Update([FromBody] TimeRequest timeRequest)
        {
            ApiResponse<TimeResponse> result = _timeUpdateService.Process(timeRequest);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetDetail(Guid? id)
        {
            ApiResponse<TimeResponse> result = _timeGetDetailService.Process(id);
            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("get-list")]
        public IActionResult GetList([FromBody] TimeGetListRequest timeGetListRequest)
        {
            var result = _timeGetListService.Process(timeGetListRequest);

            return Ok(result);
        }

        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid? id)
        {
            ApiResponse<string> result = _timeDeleteService.Process(id);

            return Ok(result);
        }


    }
}
