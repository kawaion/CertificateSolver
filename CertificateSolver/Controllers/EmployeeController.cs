using CertificateSolver.Core.Interfaces;
using CertificateSolver.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CertificateSolver.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly IRequestService _requestService;

        public EmployeeController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        [HttpPost("requests")]
        public async Task<IActionResult> SubmitRequest([FromBody] SubmitRequestDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    error = "Validation failed",
                    details = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                var requestId = await _requestService.SubmitRequestAsync(
                    dto.EmployeeId,
                    dto.Type,
                    dto.Copies,
                    dto.Reason,
                    "employee"
                );
                return Ok(new { RequestId = requestId, Status = "submitted" });
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Дубликат"))
            {
                return Conflict(new { error = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }

        [HttpGet("{employeeId}/requests")]
        public async Task<IActionResult> GetMyRequests(string employeeId)
        {
            var requests = await _requestService.GetEmployeeRequestsAsync(employeeId);
            return Ok(requests.Select(r => new
            {
                r.Id,
                r.Type,
                r.Copies,
                r.Reason,
                r.Status,
                r.CreatedAt,
                r.RejectionReason
            }));
        }
    }
}
