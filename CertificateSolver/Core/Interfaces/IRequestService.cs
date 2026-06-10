using CertificateSolver.Models;

namespace CertificateSolver.Core.Interfaces
{
    public interface IRequestService
    {
        Task<Guid> SubmitRequestAsync(
            string employeeId,
            CertificateType type,
            int copies,
            string reason,
            string role);

        Task<IEnumerable<CertificateRequest>> GetEmployeeRequestsAsync(string employeeId);

        Task<IEnumerable<CertificateRequest>> GetAllRequestsForAccountantAsync();

        Task<CertificateRequest> UpdateStatusAsync(
            Guid requestId,
            RequestStatus newStatus,
            string role,
            string rejectionReason = null);
    }
}
