using CondigiBack.Contexts;
using CondigiBack.Libs.Enums;
using CondigiBack.Libs.Interfaces;
using CondigiBack.Libs.Responses;
using CondigiBack.Libs.Utils;
using CondigiBack.Models;
using CondigiBack.Modules.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CondigiBack.Modules.Contracts.Services;

public class ContractService(AppDBContext appDbContext)
{
    public async Task<GeneralResponse<List<ContractDto.ContractResponseDTO>>> GetContractsByUser(int currentPage,
        int pageSize, Guid userId, StatusContractEnum? status)
    {
        Console.WriteLine("status "+ status);
        var contracts = await appDbContext.Contracts
            .Where(c => (!status.HasValue || c.Status == status) && c.ContractParticipants.Any(cp => cp.UserId == userId))
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        if (contracts.Count == 0)
        {
            return new ErrorResponse<List<ContractDto.ContractResponseDTO>>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "No se encontraron contratos"
            };
        }

        var totalCount = await appDbContext.Contracts.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var paginationInfo = new Pagination(currentPage, totalPages, pageSize, totalCount);
        var contractsResponse = contracts.Select(c => new ContractDto.ContractResponseDTO
        {
            ContractId = c.Id,
            ContractTypeId = c.ContractTypeId,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            NumClauses = c.NumClauses,
            PaymentAmount = c.PaymentAmount,
            PaymentFrequency = c.PaymentFrequency,
            Status = c.Status,
            CreatedAt = c.CreatedAt,
            CreatedBy = c.CreatedBy,
            UpdatedAt = c.UpdatedAt,
            UpdatedBy = c.UpdatedBy
        }).ToList();

        return new PaginatedResponse<List<ContractDto.ContractResponseDTO>>(contractsResponse, StatusCodes.Status200OK,
            "Contratos encontrados", paginationInfo);
    }

    public async Task<GeneralResponse<ContractDto.ContractResponseDTO>> GetContractById(Guid contractId)
    {
        var contract = await appDbContext.Contracts.Include(contract => contract.ContractParticipants)
            .FirstOrDefaultAsync(c => c.Id == contractId);
        if (contract == null)
        {
            return new ErrorResponse<ContractDto.ContractResponseDTO>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "No se encontró el contrato indicado"
            };
        }

        var contractResponse = new ContractDto.ContractResponseDTO
        {
            ContractId = contract.Id,
            ContractTypeId = contract.ContractTypeId,
            Content = contract.Content,
            StartDate = contract.StartDate,
            EndDate = contract.EndDate,
            NumClauses = contract.NumClauses,
            PaymentAmount = contract.PaymentAmount,
            PaymentFrequency = contract.PaymentFrequency,
            Status = contract.Status,
            CreatedAt = contract.CreatedAt,
            CreatedBy = contract.CreatedBy,
            UpdatedAt = contract.UpdatedAt,
            UpdatedBy = contract.UpdatedBy,
            ContractParticipants = contract.ContractParticipants.Select(cp =>
                new ContractParticipantDTO.ContractParticipantResponseDTO
                {
                    ContractParticipantId = cp.Id,
                    UserId = cp.UserId,
                    Signed = cp.Signed,
                    Role = cp.Role,
                    Status = cp.Status,
                    ContractId = cp.ContractId
                }).Where(cp => cp.Status).ToList()
        };

        return new StandardResponse<ContractDto.ContractResponseDTO>(contractResponse, "Contrato encontrado",
            StatusCodes.Status200OK);
    }

    public async Task<GeneralResponse<string>> CreateContract(ContractDto.CreateContractDTO contractDto, Guid userId)
    {
        var key = Guid.NewGuid().ToString();
        var contract = new Contract
        {
            Id = Guid.NewGuid(),
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
        };

        await using var transaction = await appDbContext.Database.BeginTransactionAsync();

        try
        {
            appDbContext.Contracts.Add(contract);

            var contractParticipant = new ContractParticipant
            {
                ContractId = contract.Id,
                UserId = userId,
                Role = RoleParticipantEnum.Sender,
                CompanyId = contractDto.CompanyId,
                Status = true,
                Signed = true
            };

            appDbContext.ContractParticipants.Add(contractParticipant);

            await appDbContext.SaveChangesAsync();

            await transaction.CommitAsync();
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return new ErrorResponse<string>("Error al crear el contrato", "Error",
                StatusCodes.Status500InternalServerError);
        }


        return new StandardResponse<string>(key, "Contrato creado", StatusCodes.Status201Created);
    }

    public async Task<GeneralResponse<bool>> UpdateContract(Guid contractId, ContractDto.UpdateContractDTO contractDto,
        Guid userId)
    {
        var contract = await appDbContext.Contracts.FirstOrDefaultAsync(c => c.Id == contractId);
        if (contract == null)
        {
            return new ErrorResponse<bool>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "No se encontró el contrato indicado"
            };
        }

        Console.WriteLine(contract);

        contract.ContractTypeId = contractDto.ContractTypeId ?? contract.ContractTypeId;
        contract.StartDate = contractDto.StartDate ?? contract.StartDate;
        contract.EndDate = contractDto.EndDate ?? contract.EndDate;
        contract.NumClauses = contractDto.NumClauses ?? contract.NumClauses;
        contract.PaymentAmount = contractDto.PaymentAmount ?? contract.PaymentAmount;
        contract.PaymentFrequency = contractDto.PaymentFrequency ?? contract.PaymentFrequency;
        contract.Status = contractDto.Status ?? contract.Status;
        contract.UpdatedAt = DateTime.UtcNow;
        contract.UpdatedBy = userId;
        contract.Content = contractDto.Content ?? contract.Content;

        appDbContext.Contracts.Update(contract);
        await appDbContext.SaveChangesAsync();

        return new StandardResponse<bool>(true, "Contrato actualizado", StatusCodes.Status200OK);
    }

    public async Task<GeneralResponse<bool>> UpdateStatusContract(Guid contractId, StatusContractEnum status)
    {
        var contract = await appDbContext.Contracts.FirstOrDefaultAsync(c => c.Id == contractId);
        if (contract == null)
        {
            return new ErrorResponse<bool>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "No se encontró el contrato indicado"
            };
        }

        contract.Status = status;
        contract.UpdatedAt = DateTime.UtcNow;

        await appDbContext.SaveChangesAsync();

        return new StandardResponse<bool>(true, "Estado del contrato actualizado", StatusCodes.Status200OK);
    }
}