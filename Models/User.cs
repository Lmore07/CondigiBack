using CondigiBack.Libs.Enums;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace CondigiBack.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        
        public Guid PersonId { get; set; }
        
        [Required]        
        public string Username { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public UserTypeEnum UserType { get; set; }
        
        public bool Status { get; set; }
        
        public DateTime? CreatedAt { get; set; }
                
        public DateTime? UpdatedAt { get; set; }
        
        public Person Person { get; set; }

        public ICollection<ContractParticipant> ContractParticipants { get; set; }
        public ICollection<UserCompanies> UserCompanies { get; set; }

    }
}