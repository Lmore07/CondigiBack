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
using System.Text;
using static CondigiBack.Modules.Contracts.DTOs.ContractAIDTO;
using static CondigiBack.Modules.Contracts.DTOs.ContractAIDTO.CreateReceiverCompany;

namespace CondigiBack.Modules.Contracts.Services
{
    public class ContractAIService
    {
        private readonly AppDBContext _context;
        private readonly GeminiService _geminiService;
        private readonly EmailService _emailService;

        public ContractAIService(AppDBContext context, GeminiService geminiService, EmailService emailService)
        {
            _context = context;
            _geminiService = geminiService;
            _emailService = emailService;
        }

        public async Task<GeneralResponse<List<ContractAIDTO.GetCompaniesDTO>>> GetCompanies(Guid userId)
        {
            var userCompanies = await _context.UserCompanies
                .Where(uc => uc.UserId == userId && uc.RoleInCompany == UserTypeEnum.OWNER)
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
                return new ErrorResponse<List<ContractAIDTO.GetCompaniesDTO>>("No se encontraron empresas",
                    "Empresas no encontradas",
                    StatusCodes.Status404NotFound);
            }

            return new StandardResponse<List<ContractAIDTO.GetCompaniesDTO>>(companies, "Empresas encontradas",
                StatusCodes.Status200OK);
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
                    Id = p.User.Id,
                    Identification = p.Identification,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Email = p.User.Email,
                    Phone = p.Phone,
                    ParishId = p.ParishId ?? 0,
                    Address = p.Address
                }).ToListAsync();

            if (persons.Count == 0)
            {
                return new ErrorResponse<List<ContractAIDTO.GetPersonsDTO>>("No se encontraron personas",
                    "Personas no encontradas",
                    StatusCodes.Status404NotFound);
            }

            return new StandardResponse<List<ContractAIDTO.GetPersonsDTO>>(persons, "Personas encontradas",
                StatusCodes.Status200OK);
        }


        public async Task<GeneralResponse<Guid>> CreateContractAi(ContractAIDTO.ContractAIGeneralDto payload,
            Guid userId)
        {
            var user = await _context.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == userId);
            Company receiverCompany = null;
            User receiverPerson = null;
            Company senderCompany = null;
            User senderUser = null;
            if (payload.SenderType == ParticipantEnum.Company)
            {
                senderCompany = await _context.Companies.FindAsync(payload.SenderId);
                if (senderCompany == null)
                    return new ErrorResponse<Guid>("La empresa remitente no existe", "Empresa no encontrada",
                        StatusCodes.Status404NotFound);
            }
            else
            {
                senderUser = await _context.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Id == userId);
                if (senderUser == null)
                    return new ErrorResponse<Guid>("La persona remitente no existe", "Persona no encontrada",
                        StatusCodes.Status404NotFound);
            }

            if (payload.ReceiverId != null)
            {
                if (payload.ReceiverType == ParticipantEnum.Company)
                {
                    receiverCompany = await _context.Companies.FindAsync(payload.ReceiverId);
                    if (receiverCompany == null)
                        return new ErrorResponse<Guid>("La empresa receptora no existe", "Empresa no encontrada",
                            StatusCodes.Status404NotFound);
                }
                else
                {
                    receiverPerson = await _context.Users.FirstOrDefaultAsync(u => u.Id == payload.ReceiverId);
                    if (receiverPerson == null)
                    {
                        return new ErrorResponse<Guid>("La persona receptora no existe", "Persona no encontrada",
                            StatusCodes.Status404NotFound);
                    }
                }
            }

            var contractType = await _context.ContractTypes.FindAsync(payload.ContractTypeId);
            if (contractType == null)
                return new ErrorResponse<Guid>("El tipo de contrato no existe", "Tipo de contrato no encontrado",
                    StatusCodes.Status404NotFound);

            var prompt = await GenerateContractContent(payload, user!, senderCompany!, contractType);

            var content = await _geminiService.GenerateContentAsync(prompt);


            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var contract = new Contract
                {
                    ContractTypeId = payload.ContractTypeId,
                    StartDate = payload.StartDate.ToUniversalTime(),
                    EndDate = payload.EndDate.ToUniversalTime(),
                    NumClauses = payload.NumClauses,
                    PaymentAmount = payload.PaymentAmount,
                    PaymentFrequency = payload.PaymentFrequency,
                    Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(content)),
                    Status = StatusContractEnum.Pending,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = userId,
                    EncryptionKey = "1234567890123456"
                };

                _context.Contracts.Add(contract);
                await _context.SaveChangesAsync();

                var aiRequest = new AIRequest
                {
                    Content = content,
                    type = 0,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = userId,
                    ContractId = contract.Id
                };

                _context.AIRequests.Add(aiRequest);
                await _context.SaveChangesAsync();

                var participants = new List<ContractParticipant>
                {
                    new ContractParticipant
                    {
                        ContractId = contract.Id,
                        Role = RoleParticipantEnum.Sender,
                        CompanyId = payload.SenderType == ParticipantEnum.Company ? payload.SenderId : (Guid?)null,
                        Status = true,
                        Signed = true,
                        UserId = userId
                    }
                };

                if (payload.ReceiverId.HasValue)
                {
                    participants.Add(new ContractParticipant
                    {
                        ContractId = contract.Id,
                        Role = RoleParticipantEnum.Receiver,
                        CompanyId =
                            payload.ReceiverType == ParticipantEnum.Company ? payload.ReceiverId.Value : (Guid?)null,
                        UserId =
                            payload.ReceiverType == ParticipantEnum.Person ? payload.ReceiverId.Value : (Guid?)null,
                        Status = true,
                        Signed = false
                    });
                }

                _context.ContractParticipants.AddRange(participants);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return new StandardResponse<Guid>(contract.Id, "Contrato creado", StatusCodes.Status201Created);
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new ErrorResponse<Guid>(e.Message, "Error al crear el contrato",
                    StatusCodes.Status400BadRequest);
            }
        }

        private async Task<string> GenerateContractContent(ContractAIDTO.ContractAIGeneralDto payload, User user,
            Company senderCompany, ContractType contractType)
        {
            if (payload.ReceiverType == ParticipantEnum.Company)
            {
                Company? receiverCompany = null;
                if (payload.ReceiverId != null)
                {
                    receiverCompany = await _context.Companies.FindAsync(payload.ReceiverId);
                    if (receiverCompany == null)
                        throw new Exception("La empresa receptora no existe");
                }
                else
                {
                    receiverCompany = new Company
                    {
                        Name = payload.ReceiverCompany!.Name,
                        Email = payload.ReceiverCompany.Email,
                        Address = payload.ReceiverCompany.Address,
                        Phone = payload.ReceiverCompany.Phone,
                        ParishId = payload.ReceiverCompany.ParishId,
                        RUC = payload.ReceiverCompany.RUC
                    };
                }

                return payload.SenderType == ParticipantEnum.Company
                    ? new GeneratePrompt().GenerateContent(receiverCompany, senderCompany, null, null, contractType,
                        payload)
                    : new GeneratePrompt().GenerateContent(receiverCompany, null, null, user?.Person, contractType,
                        payload);
            }
            else
            {
                User? receiverPerson = null;
                if (payload.ReceiverId != null)
                {
                    receiverPerson = await _context.Users
                        .Include(u => u.Person)
                        .FirstOrDefaultAsync(u => u.Id == payload.ReceiverId);

                    if (receiverPerson == null)
                        throw new Exception("La persona receptora no existe");
                }
                else
                {
                    receiverPerson = new User
                    {
                        Email = payload.ReceiverPerson!.Email,
                        Person = new Person
                        {
                            Identification = payload.ReceiverPerson.Identification,
                            FirstName = payload.ReceiverPerson.FirstName,
                            LastName = payload.ReceiverPerson.LastName,
                            Phone = payload.ReceiverPerson.Phone,
                            Address = payload.ReceiverPerson.Address,
                        }
                    };
                }

                return payload.SenderType == ParticipantEnum.Company
                    ? new GeneratePrompt().GenerateContent(null, senderCompany, receiverPerson.Person, null,
                        contractType,
                        payload)
                    : new GeneratePrompt().GenerateContent(null, null, receiverPerson.Person, user?.Person,
                        contractType,
                        payload);
            }
        }
    }
}