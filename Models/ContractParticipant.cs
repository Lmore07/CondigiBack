using CondigiBack.Libs.Enums;
using System.ComponentModel.DataAnnotations;

namespace CondigiBack.Models
{
    public class ContractParticipant
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ContracId { get; set; }
        public Guid UserId { get; set; }
        public Guid? CompanyId { get; set; }
        public RoleParticipantEnum Role { get; set; }
        public string Status { get; set; }

        public User User { get; set; }
        public Contract Contract { get; set; }
        public Company? Company { get; set; }

    }
}
