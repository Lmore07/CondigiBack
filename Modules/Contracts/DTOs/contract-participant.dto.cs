using CondigiBack.Libs.Enums;

namespace CondigiBack.Modules.Contracts.DTOs;

public class ContractParticipantDTO
{
    public class ContractParticipantResponseDTO
    {
        public Guid ContractParticipantId { get; set; }
        public Guid ContractId { get; set; }
        public UserDto? User { get; set; }
        public CompanyDto? Company { get; set; }
        public bool Status { get; set; }
        public RoleParticipantEnum Role { get; set; }
        public bool Signed { get; set; }
    }
    
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    
    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
    
    public class CreateContractParticipantDTO
    {
        public Guid ContractId { get; set; }
        public Guid UserId { get; set; }
    }
    
    public class CreateContractParticipantResponseDTO
    {
        public Guid ContractParticipantId { get; set; }
        public Guid ContractId { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
    }
    
    public class UpdateContractParticipantDTO
    {
        public Guid ContractId { get; set; }
        public Guid UserId { get; set; }
        public Guid UpdatedBy { get; set; }
    }
    
    public class AddCompanyToContractDTO
    {
        public Guid ContractId { get; set; }
        public Guid CompanyId { get; set; }
    }

    public class AddUserToContractDTO
    {
        public Guid ContractId { get; set; }
        public Guid UserId { get; set; }
    }
}