namespace CondigiBack.Modules.Contracts.DTOs;

public class ContractTypeDto
{ 
    public class ContractTypeResponseDTO
    {
        public Guid ContractTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
    
    public class CreateContractTypeDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
    
    public class CreateContractTypeResponseDTO
    {
        public Guid ContractTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
    
    public class UpdateContractTypeDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}