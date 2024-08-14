using System.ComponentModel.DataAnnotations;
using CondigiBack.Libs.Enums;

namespace CondigiBack.Modules.Contracts.DTOs;

public class ContractDto
{
    public class ContractResponseDTO
    {
        public Guid ContractId { get; set; }
        public string? Content { get; set; }
        public Guid ContractTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? NumClauses { get; set; }
        public decimal? PaymentAmount { get; set; }
        public PaymentFrequencyEnum? PaymentFrequency { get; set; }
        public StatusContractEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public List<ContractParticipantDTO.ContractParticipantResponseDTO> ContractParticipants { get; set; }
    }   
    
    public class CreateContractDTO
    {
        public Guid ContractTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? NumClauses { get; set; }
        public decimal? PaymentAmount { get; set; }
        public PaymentFrequencyEnum? PaymentFrequency { get; set; }
        public StatusContractEnum Status { get; set; }
        
        public Guid? CompanyId { get; set; }
    }
    
    public class UpdateContractDTO
    {
        public Guid? ContractTypeId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? NumClauses { get; set; }
        public decimal? PaymentAmount { get; set; }
        public PaymentFrequencyEnum? PaymentFrequency { get; set; }
        public StatusContractEnum? Status { get; set; }
        
        public Guid? CompanyId { get; set; }
        
        public string? Content { get; set; }
    }
    
    public class CreateContractResponseDTO
    {
        public Guid ContractId { get; set; }
        public Guid ContractTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? NumClauses { get; set; }
        public decimal? PaymentAmount { get; set; }
        public PaymentFrequencyEnum? PaymentFrequency { get; set; }
        public StatusContractEnum Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
    
    public class UpdateStatusContractDTO
    {
        [Required]
        public StatusContractEnum Status { get; set; }
    }


}