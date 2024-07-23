using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CondigiBack.Libs.Enums;

namespace CondigiBack.Models
{
    public class Contract
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public Guid ContractTypeId { get; set; }
        
        public DateTime StartDate { get; set; }
        
        public DateTime EndDate { get; set; }
        
        public int? NumClauses { get; set; }
        
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? PaymentAmount { get; set; }
        
        public PaymentFrequencyEnum? PaymentFrequency { get; set; }

        [Required]
        public string Content { get; set; }
        
        public StatusContractEnum Status { get; set; }

        public string EncryptionKey { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public Guid? CreatedBy { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public Guid? UpdatedBy { get; set; }


        public ContractType ContractType { get; set; }

        public ICollection<ContractParticipant> ContractParticipants { get; set; }

    }
}
