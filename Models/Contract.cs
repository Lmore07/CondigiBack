using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CondigiBack.Libs.Enums;

namespace CondigiBack.Models
{
    [Table("contracts")]
    public class Contract
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }
        
        [Required]
        [Column("contract_type_id")]
        public Guid ContractTypeId { get; set; }
        
        [Column("start_date")]
        public DateTime StartDate { get; set; }
        
        [Column("end_date")]
        public DateTime EndDate { get; set; }
        
        [Column("num_clauses")]
        public int? NumClauses { get; set; }
        
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? PaymentAmount { get; set; }
        
        [Column("payment_frequency")]
        public PaymentFrequencyEnum? PaymentFrequency { get; set; }
        
        [Column("content")]
        public string? Content { get; set; }
        
        [Column("status")]
        public StatusContractEnum Status { get; set; }

        [Column("encryption_key")]
        public string EncryptionKey { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [Column("created_by")]
        public Guid? CreatedBy { get; set; }
        
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        
        [Column("updated_by")]
        public Guid? UpdatedBy { get; set; }


        public ContractType ContractType { get; set; }
        
        public ICollection<AIRequest> AiRequests { get; set; }

        public ICollection<ContractParticipant> ContractParticipants { get; set; }

    }
}
