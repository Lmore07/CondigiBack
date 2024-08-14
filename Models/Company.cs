using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondigiBack.Models
{
    [Table("companies")]
    public class Company
    {
        [Key]
        public Guid Id { get; set; }
        public string RUC { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int ParishId { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }

        public ICollection<UserCompanies> UserCompanies { get; set; }
        public Parish Parish { get; set; }
        public ICollection<ContractParticipant> ContractParticipants { get; set; }

    }
}
