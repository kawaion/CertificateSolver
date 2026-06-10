using CertificateSolver.Infrastructure.Storage;
using System.Collections.Concurrent;

namespace CertificateSolver.Core.Interfaces
{
    public class IdempotencyStore : IIdempotencyStore
    {
        private readonly ConcurrentDictionary<string, DateTime> _processedKeys = new();

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
