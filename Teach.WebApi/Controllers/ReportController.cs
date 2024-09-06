using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teach.Domain.Dto.Report;
using Teach.Domain.Entity;
using Teach.Domain.Interfaces.Services;
using Teach.Domain.Result;

namespace Teach.WebApi.Controllers
{
    //[Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        public ReportController(IReportService _reportService)
        {
            this._reportService = _reportService;
        }

        [HttpGet("GetReport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDto>>> GetReport(long Id)
        {
            var response = await _reportService.GetReportByIdAsync(Id);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
                
            }
            return Ok(response);

        }

        /// <summary>
        /// Request for create report / Запрос для создания отчета
        /// </summary>
        /// <param name="reportDto"></param>
        /// <remarks>
        /// 
        ///     POST
        ///     
        ///     {
        ///     
        ///         "name": "Report 1",
        ///         "description": "Test Report",
        ///         "userId": 1
        ///         
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Если отчет создался</response>
        /// <response code="400">Если произошла какая то ошибка</response>
        [HttpPost("AddReport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDto>>> AddReport(CreateReportDto reportDto)
        {
            var response = await _reportService.CreateReportAsync(reportDto);
            if (!response.IsSuccess)
            {
                return BadRequest(response);

            }
            return Ok(response);
        }


        [HttpGet("GetReports")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CollectionResult<ReportDto>>> GetAllReportsAsync(long userId)
        {
            var response = await _reportService.GetAllReportsAsync(userId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);

            }
            return Ok(response);
        }

        [HttpPut("UpdateReport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDto>>> UpdateReportAsync(UpdateReportDto updateReportDto)
        {
            var response = await _reportService.UpdateReportAsync(updateReportDto);
            if (!response.IsSuccess)
            {
                return BadRequest(response);

            }
            return Ok(response);
        }

        [HttpDelete("DeleteReport")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BaseResult<ReportDto>>> DeleteReportAsync(long reportId)
        {
            var response = await _reportService.DeleteReportAsync(reportId);
            if (!response.IsSuccess)
            {
                return BadRequest(response);

            }
            return Ok(response);
        }



    }
}
