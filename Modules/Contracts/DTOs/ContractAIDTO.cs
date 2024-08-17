using CondigiBack.Libs.Enums;
using CondigiBack.Modules.Users.DTOs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CondigiBack.Modules.Contracts.DTOs
{
    public class ContractAIDTO
    {

        public class GetCompaniesDTO
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public int ParishId { get; set; }
            public string RUC { get; set; }
        }

        public class GetPersonsDTO
        {
            public Guid Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Phone { get; set; }
            public int ParishId { get; set; }
            public string Identification { get; set; }
            public string Address { get; set; }
        }

        public class CreateContractAIDTO
        {
            public Guid ContractTypeId { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public int? NumClauses { get; set; }
            public decimal? PaymentAmount { get; set; }
            public PaymentFrequencyEnum? PaymentFrequency { get; set; }
            public StatusContractEnum Status { get; set; }
            public string ContractDetails { get; set; }
            public string ContractObjects { get; set; }
            public string ContractConfidentiality { get; set; }
        }

        public class CreateReceiverCompany
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string Phone { get; set; }
            public int ParishId { get; set; }
            public string RUC { get; set; }
        }

        public class CreateContractAICompanyToCompanyDTO : CreateContractAIDTO
        {
            public Guid SenderCompanyId { get; set; }
            public Guid? ReceiverCompanyId { get; set; }
            public CreateReceiverCompany? ReceiverCompany { get; set; }
        }

        public class CreateContractAICompanyToPersonDTO : CreateContractAIDTO
        {
            public Guid SenderCompanyId { get; set; }
            public Guid? ReceiverPersonId { get; set; }
        }

        public class CreateContractAIPersonToCompanyDTO : CreateContractAIDTO
        {
            public Guid? SenderPersonId { get; set; }
            public Guid? ReceiverCompanyId { get; set; }
        }

        public class CreateContractAIPersonToPersonDTO : CreateContractAIDTO
        {
            public Guid? SenderPersonId { get; set; }
            public Guid? ReceiverPersonId { get; set; }
        }

        public class ContractAIResponseCompanyToCompany
        {
            public Guid Id { get; set; }
            
            public Guid ContractTypeId { get; set; }

            public DateTime StartDate { get; set; }

            public DateTime EndDate { get; set; }

            public int? NumClauses { get; set; }

            public decimal? PaymentAmount { get; set; }

            public PaymentFrequencyEnum? PaymentFrequency { get; set; }

            public string? Content { get; set; }

            public StatusContractEnum Status { get; set; }

        }

    }
}
