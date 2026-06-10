namespace CertificateSolver.Models
{
    public class CertificateRequest
    {
        public Guid Id { get; set; }
        public string EmployeeId { get; set; } = string.Empty;
        public CertificateType Type { get; set; }
        public int Copies { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }
    }
}
