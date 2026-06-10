using CertificateSolver.Core.Interfaces;
using CertificateSolver.Models;

namespace CertificateSolver.Core.Rules
{
    public class RequestStatusTransition : IRequestStatusTransition
    {
        private static readonly Dictionary<RequestStatus, HashSet<RequestStatus>> _allowedTransitions = new()
        {
            [RequestStatus.Draft] = new() { RequestStatus.Submitted },
            [RequestStatus.Submitted] = new() { RequestStatus.InProgress, RequestStatus.Rejected },
            [RequestStatus.InProgress] = new() { RequestStatus.Completed, RequestStatus.Rejected },
            [RequestStatus.Completed] = new(),
            [RequestStatus.Rejected] = new()
        };
        private static readonly Dictionary<(RequestStatus, string), bool> _roleRules = new()
        {
            [(RequestStatus.Draft, "employee")] = true,
            [(RequestStatus.Submitted, "accountant")] = true,
            [(RequestStatus.InProgress, "accountant")] = true
        };
        public bool CanTransition(RequestStatus from, RequestStatus to)
        {
            return _allowedTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);
        }

        public bool TryTransition(CertificateRequest request, RequestStatus newStatus, string actorRole, out string? error)
        {
            error = null;

            if(!CanTransition(request.Status, newStatus))
            {
                error = $"Невозможно перейти из {request.Status} в {newStatus}";
                return false;
            }

            var key = (request.Status, actorRole);
            if (!_roleRules.ContainsKey(key))
            {
                error = $"Роль {actorRole} не может выполнить этот переход";
                return false;
            }

            request.Status = newStatus;
            request.LastUpdatedAt = DateTime.UtcNow;
            return true;
        }
    }
}
