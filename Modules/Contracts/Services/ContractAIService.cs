using CondigiBack.Contexts;
using CondigiBack.Libs.Enums;
using CondigiBack.Libs.Interfaces;
using CondigiBack.Libs.Responses;
using CondigiBack.Libs.Services;
using CondigiBack.Libs.Utils;
using CondigiBack.Models;
using CondigiBack.Modules.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using static CondigiBack.Modules.Contracts.DTOs.ContractAIDTO.CreateReceiverCompany;

namespace CondigiBack.Modules.Contracts.Services
{
    public class ContractAIService
    {
        private readonly AppDBContext _context;
        private readonly GeminiService _geminiService;

        public ContractAIService(AppDBContext context, GeminiService geminiService)
        {
            _context = context;
            _geminiService = geminiService;
        }

        public async Task<GeneralResponse<List<ContractAIDTO.GetCompaniesDTO>>> GetCompanies(Guid userId)
        {
            var userCompanies = await _context.UserCompanies
                .Where(uc => uc.UserId == userId && uc.RoleInCompany== UserTypeEnum.OWNER)
                .Select(uc => uc.CompanyId)
                .ToListAsync();

            var companies = await _context.Companies
                .Where(c => !userCompanies.Contains(c.Id))
                .Select(c => new ContractAIDTO.GetCompaniesDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Email = c.Email,
                    Address = c.Address,
                    Phone = c.Phone,
                    ParishId = c.ParishId,
                    RUC = c.RUC
                }).ToListAsync();

            if (companies.Count == 0)
            {
                return new ErrorResponse<List<ContractAIDTO.GetCompaniesDTO>>("No se encontraron empresas", "Empresas no encontradas",
                    StatusCodes.Status404NotFound);
            }

            return new StandardResponse<List<ContractAIDTO.GetCompaniesDTO>>(companies, "Empresas encontradas", StatusCodes.Status200OK);
        }

        public async Task<GeneralResponse<List<ContractAIDTO.GetPersonsDTO>>> GetPersons(Guid userId)
        {
            var currentUserIdentification = await _context.Persons
                .Where(p => p.User.Id == userId)
                .Select(p => p.Identification)
                .FirstOrDefaultAsync();

            var persons = await _context.Persons
                .Where(p => p.Identification != currentUserIdentification)
                .Select(p => new ContractAIDTO.GetPersonsDTO
                {
                    Id = p.Id,
                    Identification = p.Identification,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.User.Email,
                    Phone = p.Phone,
                    ParishId = p.ParishId??0,
                    Address = p.Address
                }).ToListAsync();

            if (persons.Count == 0)
            {
                return new ErrorResponse<List<ContractAIDTO.GetPersonsDTO>>("No se encontraron personas", "Personas no encontradas",
                    StatusCodes.Status404NotFound);
            }

            return new StandardResponse<List<ContractAIDTO.GetPersonsDTO>>(persons, "Personas encontradas", StatusCodes.Status200OK);
        }

        public async Task<GeneralResponse<bool>> CreateContractAICompanyToCompany(ContractAIDTO.CreateContractAICompanyToCompanyDTO contractDto, Guid userId)
        {

            var senderCompany = await _context.Companies.FindAsync(contractDto.SenderCompanyId);
            if(senderCompany == null)
            {
                return new ErrorResponse<bool>("La empresa remitente no existe", "Empresa no encontrada",
                    StatusCodes.Status404NotFound);
            }

            var receiverCompany = await _context.Companies.FindAsync(contractDto.ReceiverCompanyId);

            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (receiverCompany == null)
                {
                    // Create receiver company if it doesn't exist
                    receiverCompany = new Company
                    {
                        Name = contractDto.ReceiverCompany.Name,
                        Address = contractDto.ReceiverCompany.Address,
                        Email = contractDto.ReceiverCompany.Email,
                        Phone = contractDto.ReceiverCompany.Phone,
                        ParishId = contractDto.ReceiverCompany.ParishId,
                        RUC = contractDto.ReceiverCompany.RUC,
                        Status = true,
                        CreatedBy = userId,
                        CreatedAt = DateTime.UtcNow,
                    };

                    _context.Companies.Add(receiverCompany);
                    await _context.SaveChangesAsync();
                }

                var contractType = await _context.ContractTypes.FindAsync(contractDto.ContractTypeId);
                if (contractType == null)
                {
                    return new ErrorResponse<bool>("El tipo de contrato no existe", "Tipo de contrato no encontrado",
                        StatusCodes.Status404NotFound);
                }

                var prompt = new GeneratePrompt().CompanyToCompany(receiverCompany, senderCompany, contractType,contractDto);

                var key = Guid.NewGuid().ToString();
                var contract = new Contract
                {
                    Id= new Guid(),
                    ContractTypeId = contractDto.ContractTypeId,
                    StartDate = contractDto.StartDate,
                    EndDate = contractDto.EndDate,
                    NumClauses = contractDto.NumClauses,
                    PaymentAmount = contractDto.PaymentAmount,
                    PaymentFrequency = contractDto.PaymentFrequency,
                    Status = contractDto.Status,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = userId,
                    EncryptionKey = Encrypt.GenerateHash(key),
                    Content = await _geminiService.GenerateContentAsync(prompt),
                };

                _context.Contracts.Add(contract);
                await _context.SaveChangesAsync();

                var aiRequests = new List<AIRequest>
                {
                    new AIRequest
                    {
                        Content = contractDto.ContractDetails,
                        type = 0,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId,
                        ContractId = contract.Id
                    },
                    new AIRequest
                    {
                        Content = contractDto.ContractObjects,
                        type = 1,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId,
                        ContractId = contract.Id
                    },
                    new AIRequest
                    {
                        Content = contractDto.ContractConfidentiality,
                        type = 2,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = userId,
                        ContractId = contract.Id
                    }
                };

                _context.AIRequests.AddRange(aiRequests);
                await _context.SaveChangesAsync();

                var contractParticipants = new List<ContractParticipant>
                {
                    new ContractParticipant
                    {
                        ContractId = contract.Id,
                        Role = RoleParticipantEnum.Sender,
                        CompanyId = contractDto.SenderCompanyId,
                        Status = true,
                        Signed = true
                    },
                    new ContractParticipant
                    {
                        ContractId = contract.Id,
                        Role = RoleParticipantEnum.Receiver,
                        CompanyId = contractDto.ReceiverCompanyId??receiverCompany.Id,
                        Status = true,
                        Signed = false
                    }
                };

                _context.ContractParticipants.AddRange(contractParticipants);

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new ErrorResponse<bool>("Error al crear el contrato", "Error",
                    StatusCodes.Status500InternalServerError);
            }

            return new StandardResponse<bool>(true, "Contrato creado", StatusCodes.Status201Created);
        }

    }
}
