using CondigiBack.Libs.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondigiBack.Models
{
    [Table("contract_participants")]
    public class ContractParticipant
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        [Column("contract_id")]
        public Guid ContractId { get; set; }
        [Column("user_id")]
        public Guid? UserId { get; set; }
        [Column("company_id")]
        public Guid? CompanyId { get; set; }
        [Column("role")]
        public RoleParticipantEnum Role { get; set; }
        [Column("status")]
        public Boolean Status { get; set; }
        public User? User { get; set; }
        public Contract Contract { get; set; }
        public Company? Company { get; set; }
        [Column("signed")]
        public Boolean Signed { get; set; }
    }
}
