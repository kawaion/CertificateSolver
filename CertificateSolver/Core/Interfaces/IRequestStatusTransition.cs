using CertificateSolver.Models;

namespace CertificateSolver.Core.Interfaces
{
    public interface IRequestStatusTransition
    {
        bool CanTransition(RequestStatus from, RequestStatus to);
        bool TryTransition(CertificateRequest request, RequestStatus newStatus, string actorRole, out string? error);
    }
}
