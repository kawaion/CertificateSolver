using CertificateSolver.Infrastructure.Storage;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace CertificateSolver.Core.Interfaces
{
    public class IdempotencyStore : IIdempotencyStore
    {
        private readonly ConcurrentDictionary<string, DateTime> _processedKeys = new();
        public string GenerateKey(string employeeId, string type, int copies, string reason)
        {
            var raw = $"{employeeId}|{type}|{copies}|{reason}";
            var hash = SHA256.HashData(Encoding.UTF8.GetBytes(employeeId));
            return Convert.ToHexString(hash);
        }

        public bool IsDublicate(string key)
        {
            return _processedKeys.ContainsKey(key);
        }

        public void MarkProcessed(string key)
        {
            _processedKeys.TryAdd(key, DateTime.Now);
        }
    }
}
