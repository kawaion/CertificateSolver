
namespace CertificateSolver.Core.Interfaces
{
    public interface IIdempotencyKeyGenerator
    {
        string GenerateKey(string employeeId, string type, int copies, string reason);
    }
}
