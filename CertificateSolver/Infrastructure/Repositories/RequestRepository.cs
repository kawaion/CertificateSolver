using CertificateSolver.Core.Interfaces;
using CertificateSolver.Models;
using System.Collections.Concurrent;

namespace CertificateSolver.Infrastructure.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ConcurrentDictionary<Guid, CertificateRequest> _storage = new();


        public Task<IEnumerable<CertificateRequest>> GetAllForAccountantAsync()
        {
            var result = _storage.Values.Where(r => r.Status != RequestStatus.Draft).OrderByDescending(r => r.CreatedAt).ToList().AsEnumerable();
            return Task.FromResult(result);
        }

        public Task<IEnumerable<CertificateRequest>> GetByEmployeeAsync(string employeeId)
        {
            var result = _storage.Values.Where(r => r.EmployeeId == employeeId).OrderBy(r => r.CreatedAt).ToList().AsEnumerable();
            return Task.FromResult(result);
        }

        public Task<CertificateRequest?> GetByIdAsync(Guid Id)
        {
            _storage.TryGetValue(Id, out var result);
            return Task.FromResult(result);
        }

        public Task<IEnumerable<CertificateRequest>> GetByStatusAsync(RequestStatus status)
        {
            var result = _storage.Values.Where(r => r.Status == status).OrderBy(r => r.CreatedAt).ToList().AsEnumerable();
            return Task.FromResult(result);
        }
        public Task AddAsync(CertificateRequest request)
        {
            _storage.TryAdd(request.Id, request);
            return Task.CompletedTask;
        }
        public Task UpdateAsync(CertificateRequest request)
        {
            _storage[request.Id] = request;
            return Task.CompletedTask;
        }
    }
}
