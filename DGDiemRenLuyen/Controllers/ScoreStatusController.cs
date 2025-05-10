using DGDiemRenLuyen.Common;
using DGDiemRenLuyen.DTOs.Requests;
using DGDiemRenLuyen.DTOs.requsets;
using DGDiemRenLuyen.DTOs.responses;
using DGDiemRenLuyen.DTOs.Responses;
using DGDiemRenLuyen.DTOs.Responses.Students;
using DGDiemRenLuyen.Services.ScoreStatusService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DGDiemRenLuyen.Controllers
{
    [Route("api/score-status")]   
    public class ScoreStatusController : ControllerBase
    {
        private readonly ScoreStatusUpdateService _scoreStatusUpdateService;
        private readonly ScoreStatusUpdateListService _scoreStatusUpdateListService;
        private readonly ScoreStatusGetListService _scoreStatusGetListService;
        private readonly ScoreStatusGetDetailService _scoreStatusGetDetailService;
        private readonly ScoreStatusDeleteService _scoreStatusDeleteService;
        private readonly ScoreStatusGetMyListService _scoreStatusGetMyListService;

        public ScoreStatusController(
            ScoreStatusUpdateService scoreStatusUpdateService
            , ScoreStatusGetListService scoreStatusGetListService
            , ScoreStatusGetDetailService scoreStatusGetDetailService
            , ScoreStatusDeleteService scoreStatusDeleteService
            ,ScoreStatusGetMyListService scoreStatusGetMyListService)
        {
            _scoreStatusUpdateService = scoreStatusUpdateService;
            _scoreStatusGetListService = scoreStatusGetListService;
            _scoreStatusGetDetailService = scoreStatusGetDetailService;
            _scoreStatusDeleteService = scoreStatusDeleteService;
            _scoreStatusGetMyListService = scoreStatusGetMyListService;
        }

        /*[HttpPost]
        [Authorize]
        [Route("create")]
        public IActionResult Create([FromBody] ScoreStatusRequest scoreStatusRequest) {
            ApiResponse<ScoreStatusResponse> result = _scoreStatusCreateService.Process(scoreStatusRequest);
            return Ok(result);
        }*/

        [HttpPut]
        [Authorize]
        [Route("update")]
        public IActionResult Update([FromBody] ScoreStatusRequest scoreStatusRequest)
        {
            ApiResponse<ScoreStatusResponse> result = _scoreStatusUpdateService.Process(scoreStatusRequest);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.HdtOrTkOrCvhtOrCbl)]
        [Route("update-list")]
        public IActionResult UpdateList([FromBody] StatusUpdateListRequest scoreStatusRequest)
        {
            ApiResponse<IActionResult> result = _scoreStatusUpdateListService.Process(scoreStatusRequest);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.SvOrCbl)]
        public IActionResult GetDetail([FromBody] ScoreStatusRequest scoreStatusRequest)
        {
            ApiResponse<StudentResponse> result = _scoreStatusGetDetailService.Process(scoreStatusRequest);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.HdtOrTkOrCvhtOrCbl)]
        [Route("get-list")]
        public IActionResult GetList([FromBody] ScoreStatusGetListRequest scoreStatusGetListRequest)
        {
            var result = _scoreStatusGetListService.Process(scoreStatusGetListRequest);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Roles = RoleConstants.SvOrCbl)]
        [Route("get-my-list")]
        public IActionResult GetMyList([FromBody] ScoreStatusGetListRequest scoreStatusGetListRequest)
        {
            var result = _scoreStatusGetMyListService.Process(scoreStatusGetListRequest);

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        [Authorize(Roles = RoleConstants.AdminOrHdt)]
        public IActionResult Delete(Guid? id)
        {
            ApiResponse<string> result = _scoreStatusDeleteService.Process(id);

            return Ok(result);
        }


    }
}
