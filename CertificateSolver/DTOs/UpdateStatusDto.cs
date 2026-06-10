using CertificateSolver.Models;
using System.ComponentModel.DataAnnotations;

namespace CertificateSolver.DTOs
{
    public class UpdateStatusDto
    {
        [Required]
        public RequestStatus Status { get; set; }

        public string? RejectionReason { get; set; }
    }
}
