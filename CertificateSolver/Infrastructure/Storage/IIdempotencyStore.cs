using CertificateSolver.Models;

namespace CertificateSolver.Infrastructure.Storage
{
    public interface IIdempotencyStore
    {
        bool IsDublicate(string key);
        void MarkProcessed(string key);
    }
}
