using CertificateSolver.Models;

namespace CertificateSolver.Infrastructure.Storage
{
    public interface IIdempotencyStore
    {
        string GenerateKey(string employeeId, string type, int copies, string reason);
        bool IsDublicate(string key);
        void MarkProcessed(string key);
    }
}
