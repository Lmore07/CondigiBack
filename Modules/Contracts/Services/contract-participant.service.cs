using CondigiBack.Contexts;
using CondigiBack.Libs.Enums;
using CondigiBack.Libs.Interfaces;
using CondigiBack.Libs.Responses;
using CondigiBack.Models;
using CondigiBack.Modules.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CondigiBack.Modules.Contracts.Services;

public class ContractParticipantService(AppDBContext appDbContext)
{
    public async Task<GeneralResponse<bool>> AddCompanyToContract(ContractParticipantDTO.AddCompanyToContractDTO payload)
    {
        var contract = await appDbContext.Contracts.Include(contract => contract.ContractParticipants)
            .FirstOrDefaultAsync(c => c.Id == payload.ContractId);
        if (contract == null)
        {
            return new ErrorResponse<bool>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Contrato no encontrado"
            };
        }
        
        var companyExists = await appDbContext.Companies.AnyAsync(c => c.Id == payload.CompanyId);
        
        if (!companyExists)
        {
            return new ErrorResponse<bool>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Compañia no encontrada"
            };
        }


        var contractParticipantExists = await appDbContext.ContractParticipants
            .FirstOrDefaultAsync(cp => cp.ContractId == payload.ContractId && cp.CompanyId == payload.CompanyId);

        if (contractParticipantExists != null)
        {
            await appDbContext.ContractParticipants
                .Where(cp => cp.Id == contractParticipantExists.Id)
                .ForEachAsync(cp => cp.Status = true);

            await appDbContext.SaveChangesAsync();
            return new StandardResponse<bool>(true, "Usuario agregado al contrato", StatusCodes.Status200OK);
        }

        var contractParticipant = new ContractParticipant
        {
            ContractId = payload.ContractId,
            Role = RoleParticipantEnum.Receiver,
            Status = true,
            CompanyId = payload.CompanyId,
            Signed = false
        };

        await appDbContext.ContractParticipants.AddAsync(contractParticipant);
        await appDbContext.SaveChangesAsync();

        return new StandardResponse<bool>(true, "Usuario agregado al contrato", StatusCodes.Status200OK);
    }

    public async Task<GeneralResponse<bool>> AddUserToContract(ContractParticipantDTO.AddUserToContractDTO payload)
    {
        var contract = await appDbContext.Contracts.Include(contract => contract.ContractParticipants)
            .FirstOrDefaultAsync(c => c.Id == payload.ContractId);
        if (contract == null)
        {
            return new ErrorResponse<bool>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Contrato no encontrado"
            };
        }

        var userExists = await appDbContext.Users.AnyAsync(u => u.Id == payload.UserId);
        if (!userExists)
        {
            return new ErrorResponse<bool>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Usuario no encontrado"
            };
        }

        var contractParticipantExists = await appDbContext.ContractParticipants
            .Where(cp => cp.ContractId == payload.ContractId && cp.UserId == payload.UserId).FirstOrDefaultAsync();

        if (contractParticipantExists != null)
        {
            await appDbContext.ContractParticipants
                .Where(cp => cp.Id == contractParticipantExists.Id)
                .ForEachAsync(cp => cp.Status = true);

            await appDbContext.SaveChangesAsync();
            return new StandardResponse<bool>(true, "Usuario agregado al contrato", StatusCodes.Status200OK);
        }

        var contractParticipant = new ContractParticipant
        {
            ContractId = payload.ContractId,
            Role = RoleParticipantEnum.Receiver,
            Status = true,
            UserId = payload.UserId,
            Signed = false
        };

        await appDbContext.ContractParticipants.AddAsync(contractParticipant);
        await appDbContext.SaveChangesAsync();

        return new StandardResponse<bool>(true, "Usuario agregado al contrato", StatusCodes.Status200OK);
    }

    public async Task<GeneralResponse<bool>> UpdateStaus(Guid contractParticipantId)
    {
        var contractParticipant = await appDbContext.ContractParticipants
            .FirstOrDefaultAsync(cp => cp.Id == contractParticipantId);
        if (contractParticipant == null)
        {
            return new ErrorResponse<bool>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Participante no encontrado"
            };
        }

        contractParticipant.Status = !contractParticipant.Status;
        appDbContext.ContractParticipants.Update(contractParticipant);
        await appDbContext.SaveChangesAsync();

        return new StandardResponse<bool>(true, "Estado actualizado", StatusCodes.Status200OK);
    }

    public async Task<GeneralResponse<bool>> UpdateSigned(Guid userId, Guid contractId)
    {
        var contractParticipant = await appDbContext.ContractParticipants
            .FirstOrDefaultAsync(cp => cp.UserId == userId && cp.ContractId == contractId);
        if (contractParticipant == null)
        {
            return new ErrorResponse<bool>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Participante no encontrado"
            };
        }

        contractParticipant.Signed = !contractParticipant.Signed;
        await appDbContext.SaveChangesAsync();

        return new StandardResponse<bool>(true, "Firma actualizada", StatusCodes.Status200OK);
    }
}