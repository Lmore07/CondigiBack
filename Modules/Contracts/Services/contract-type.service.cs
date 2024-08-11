using CondigiBack.Contexts;
using CondigiBack.Libs.Interfaces;
using CondigiBack.Libs.Responses;
using CondigiBack.Models;
using CondigiBack.Modules.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CondigiBack.Modules.Contracts.Services;

public class ContractTypeService(AppDBContext appDbContext)
{
    public async Task<GeneralResponse<List<ContractTypeDto.ContractTypeResponseDTO>>> GetContractTypes(int currentPage, int pageSize)
    {
        var contractTypes = await appDbContext.ContractTypes
            .Skip((currentPage - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        if (contractTypes.Count == 0)
        {
            return new ErrorResponse<List<ContractTypeDto.ContractTypeResponseDTO>>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "No se encontraron tipos de contrato"
            };
        }

        var totalCount = await appDbContext.ContractTypes.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var paginationInfo = new Pagination(currentPage, totalPages, pageSize, totalCount);
        var contractTypesResponse = contractTypes.Select(ct => new ContractTypeDto.ContractTypeResponseDTO
        {
            ContractTypeId = ct.Id,
            Name = ct.Name,
            Description = ct.Description,
            CreatedAt = ct.CreatedAt,
            CreatedBy = ct.CreatedBy,
            UpdatedAt = ct.UpdatedAt,
            UpdatedBy = ct.UpdatedBy
        }).ToList();

        return new PaginatedResponse<List<ContractTypeDto.ContractTypeResponseDTO>>(contractTypesResponse, StatusCodes.Status200OK, "Tipos de contrato encontrados", paginationInfo);
    }
    
    public async Task<GeneralResponse<ContractTypeDto.CreateContractTypeDTO>> CreateContractType(ContractTypeDto.CreateContractTypeDTO contractTypeDto, Guid userId)
    {
        var contractType = new ContractType
        {
            Name = contractTypeDto.Name,
            Description = contractTypeDto.Description,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = userId,
            UpdatedAt = DateTime.UtcNow,
            UpdatedBy = userId
        };

        await appDbContext.ContractTypes.AddAsync(contractType);
        await appDbContext.SaveChangesAsync();

        return new StandardResponse<ContractTypeDto.CreateContractTypeDTO>(contractTypeDto, "Tipo de contrato creado", StatusCodes.Status201Created);
    }
}