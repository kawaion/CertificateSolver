using CertificateSolver.Models;
using System.ComponentModel.DataAnnotations;

namespace CertificateSolver.DTOs
{
    public class SubmitRequestDto
    {
        [Required]
        public string EmployeeId { get; set; } = string.Empty;

        [Required]
        public CertificateType Type { get; set; }

        [Range(1, 10)]
        public int Copies { get; set; } = 1;

        [Required]
        [MaxLength(500)]
        public string Reason { get; set; } = string.Empty;
    }
}
