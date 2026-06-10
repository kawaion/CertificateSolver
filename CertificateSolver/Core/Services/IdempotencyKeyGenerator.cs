using CertificateSolver.Core.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CertificateSolver.Core.Services
{
    public class IdempotencyKeyGenerator : IIdempotencyKeyGenerator
    {
        public string GenerateKey(string employeeId, string type, int copies, string reason)
        {
            {
                var raw = $"{employeeId}|{type}|{copies}|{reason}";
                var hash = SHA256.HashData(Encoding.UTF8.GetBytes(raw));
                return Convert.ToHexString(hash).ToLower();
            }
        }
    }
}
