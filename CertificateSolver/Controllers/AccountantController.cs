using CertificateSolver.Core.Interfaces;
using CertificateSolver.DTOs;
using CertificateSolver.Models;
using Microsoft.AspNetCore.Mvc;

namespace CertificateSolver.Controllers;

[ApiController]
[Route("api/accountant")]
public class AccountantController : ControllerBase
{
    private readonly IRequestService _requestService;

    public AccountantController(
        IRequestService requestService)
    {
        _requestService = requestService;
    }

    /// <summary>
    /// Получить очередь заявок
    /// </summary>
    [HttpGet("requests")]
    public async Task<IActionResult> GetQueue()
    {
        try
        {
            var requests = await _requestService.GetAllRequestsForAccountantAsync();

            var response = requests.Select(r => new
            {
                r.Id,
                r.EmployeeId,
                Type = r.Type.ToString(),
                r.Copies,
                r.Reason,
                Status = r.Status.ToString(),
                r.CreatedAt,
                r.LastUpdatedAt,
                CanProcess = r.Status != RequestStatus.Completed && r.Status != RequestStatus.Rejected
            });

            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
        }
    }

    /// <summary>
    /// Изменить статус заявки
    /// </summary>
    [HttpPatch("requests/{id}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusDto dto)
    {
        // 1. Валидация
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
            // 2. Вызываем сервис (роль жёстко задана)
            var updated = await _requestService.UpdateStatusAsync(
                id,
                dto.Status,
                "accountant",  // ← роль зашита, как в вашем коде
                dto.RejectionReason);

            return Ok(new
            {
                updated.Id,
                Status = updated.Status.ToString(),
                updated.LastUpdatedAt,
                updated.RejectionReason
            });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
        }
    }
}
