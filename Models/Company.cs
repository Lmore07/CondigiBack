using System.ComponentModel.DataAnnotations;

namespace CondigiBack.Models
{
    public class Company
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }

        public ICollection<UserCompanies> UserCompanies { get; set; }
        public ICollection<ContractParticipant> ContractParticipants { get; set; }

    }
}
