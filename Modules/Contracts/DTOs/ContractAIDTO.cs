using CondigiBack.Libs.Enums;
using CondigiBack.Modules.Users.DTOs;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

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

        public class CreateContractAIDTO : IValidatableObject
        {
            [Required]
            public Guid ContractTypeId { get; set; }
            [Required] public DateTime StartDate { get; set; }
            [Required] public DateTime EndDate { get; set; }
            public int? NumClauses { get; set; }
            public decimal? PaymentAmount { get; set; }
            public PaymentFrequencyEnum? PaymentFrequency { get; set; }
            [Required] public StatusContractEnum Status { get; set; }
            [Required] public string ContractDetails { get; set; }
            [Required] public string ContractObjects { get; set; }
            [Required] public string ContractConfidentiality { get; set; }
            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (StartDate > EndDate)
                {
                    yield return new ValidationResult(
                        "La fecha de inicio no puede ser mayor a la fecha de fin.",
                        new[] { nameof(StartDate), nameof(EndDate) }
                    );
                }
                
                if (PaymentAmount.HasValue && PaymentAmount.Value < 0)
                {
                    yield return new ValidationResult(
                        "El monto de pago no puede ser negativo.",
                        new[] { nameof(PaymentAmount) }
                    );
                }
                
                if (NumClauses.HasValue && NumClauses.Value < 0)
                {
                    yield return new ValidationResult(
                        "El número de cláusulas no puede ser negativo.",
                        new[] { nameof(NumClauses) }
                    );
                }
                
                if (ContractTypeId == Guid.Empty)
                {
                    yield return new ValidationResult(
                        "Debe especificar un tipo de contrato.",
                        new[] { nameof(ContractTypeId) }
                    );
                }
                
                if (string.IsNullOrWhiteSpace(ContractDetails))
                {
                    yield return new ValidationResult(
                        "Debe especificar los detalles del contrato.",
                        new[] { nameof(ContractDetails) }
                    );
                }
                
                if (string.IsNullOrWhiteSpace(ContractObjects))
                {
                    yield return new ValidationResult(
                        "Debe especificar los objetos del contrato.",
                        new[] { nameof(ContractObjects) }
                    );
                }
                
                if (string.IsNullOrWhiteSpace(ContractConfidentiality))
                {
                    yield return new ValidationResult(
                        "Debe especificar la confidencialidad del contrato.",
                        new[] { nameof(ContractConfidentiality) }
                    );
                }
            }
        }

        public class CreateReceiverCompany
        {
            [Required] public string Name { get; set; }
            [Required] public string Email { get; set; }
            [Required] public string Address { get; set; }
            [Required] public string Phone { get; set; }
            [Required] public int ParishId { get; set; }
            [Required] public string RUC { get; set; }
        }

        public class CreateContractAICompanyToCompanyDTO : CreateContractAIDTO
        {
            [Required] public Guid SenderCompanyId { get; set; }


            public Guid? ReceiverCompanyId { get; set; }
            public CreateReceiverCompany? ReceiverCompany { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (ReceiverCompanyId.HasValue && ReceiverCompany != null)
                {
                    yield return new ValidationResult(
                        "No se puede especificar tanto ReceiverCompanyId como ReceiverCompany.",
                        new[] { nameof(ReceiverCompanyId), nameof(ReceiverCompany) }
                    );
                }

                if (!ReceiverCompanyId.HasValue && ReceiverCompany == null)
                {
                    yield return new ValidationResult(
                        "Debe especificar al menos ReceiverCompanyId o ReceiverCompany.",
                        new[] { nameof(ReceiverCompanyId), nameof(ReceiverCompany) }
                    );
                }
            }
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

        public class CreateReceiverPerson
        {
            [Required] public string FirstName { get; set; }
            [Required] public string LastName { get; set; }
            [Required] public string Email { get; set; }
            [Required] public string Phone { get; set; }
            [Required] public string Identification { get; set; }
            [Required] public string Address { get; set; }
        }

        public class ContractAIGeneralDto : CreateContractAIDTO
        {
            public Guid? SenderId { get; set; }
            [Required]

            public ParticipantEnum SenderType { get; set; }

            [Required]
            public ParticipantEnum ReceiverType { get; set; }
            public Guid? ReceiverId { get; set; }
            public CreateReceiverCompany? ReceiverCompany { get; set; }
            public CreateReceiverPerson? ReceiverPerson { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (ReceiverType == ParticipantEnum.Company)
                {
                    if (ReceiverId.HasValue && ReceiverCompany != null)
                    {
                        yield return new ValidationResult(
                            "No se puede especificar tanto ReceiverCompanyId como ReceiverCompany.",
                            new[] { nameof(ReceiverId), nameof(ReceiverCompany) }
                        );
                    }

                    if (!ReceiverId.HasValue && ReceiverCompany == null)
                    {
                        yield return new ValidationResult(
                            "Debe especificar al menos ReceiverCompanyId o ReceiverCompany.",
                            new[] { nameof(ReceiverId), nameof(ReceiverCompany) }
                        );
                    }
                }

                if (ReceiverType == ParticipantEnum.Person)
                {
                    if (ReceiverId.HasValue && ReceiverPerson != null)
                    {
                        yield return new ValidationResult(
                            "No se puede especificar tanto ReceiverPersonId como ReceiverPerson.",
                            new[] { nameof(ReceiverId), nameof(ReceiverPerson) }
                        );
                    }

                    if (!ReceiverId.HasValue && ReceiverPerson == null)
                    {
                        yield return new ValidationResult(
                            "Debe especificar al menos ReceiverPersonId o ReceiverPerson.",
                            new[] { nameof(ReceiverId), nameof(ReceiverPerson) }
                        );
                    }
                }
                
                if(SenderType == ParticipantEnum.Company && SenderId == null)
                {
                    yield return new ValidationResult(
                        "Debe especificar el SenderId si el tipo de participante es una empresa.",
                        new[] { nameof(SenderId) }
                    );
                }
            }
        }
    }
}