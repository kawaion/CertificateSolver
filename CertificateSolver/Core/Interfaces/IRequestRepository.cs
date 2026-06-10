using CertificateSolver.Models;
//using System.Security.Cryptography.X509Certificates;

namespace CertificateSolver.Core.Interfaces
{
    public interface IRequestRepository
    {
        Task<CertificateRequest?> GetByIdAsync(Guid Id);
        Task<IEnumerable<CertificateRequest>> GetByEmployeeAsync(string employeeId);
        Task<IEnumerable<CertificateRequest>> GetAllForAccountantAsync();
        Task<IEnumerable<CertificateRequest>> GetByStatusAsync(RequestStatus status);
        Task AddAsync(CertificateRequest request);
        Task UpdateAsync(CertificateRequest request);
    }
}
