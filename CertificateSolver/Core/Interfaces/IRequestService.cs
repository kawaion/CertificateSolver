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

        Task<IEnumerable<CertificateRequest>> GetEmploypeeRequestsAsync(string employeeId);

        Task<CertificateRequest> UpdateStatusAsync(
            Guid requestId,
            RequestStatus newStatus,
            string role,
            string rejectionReason = null);
    }
}
