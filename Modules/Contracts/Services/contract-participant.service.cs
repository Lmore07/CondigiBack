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

        var user = await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);
        if (user == null)
        {
            return new ErrorResponse<bool>
            {
                StatusCode = StatusCodes.Status404NotFound,
                Message = "Usuario no encontrado"
            };
        }

        var contractParticipantExists = await appDbContext.ContractParticipants
            .AnyAsync(cp => cp.ContractId == payload.ContractId && cp.UserId == user.Id);

        if (contractParticipantExists)
        {
            await appDbContext.ContractParticipants
                .Where(cp => cp.ContractId == payload.ContractId && cp.UserId == user.Id)
                .ForEachAsync(cp => cp.Status = true);

            await appDbContext.SaveChangesAsync();
            return new StandardResponse<bool>(true, "Usuario agregado al contrato", StatusCodes.Status200OK);
        }

        var contractParticipant = new ContractParticipant
        {
            ContractId = payload.ContractId,
            UserId = user.Id,
            Role = RoleParticipantEnum.Receiver,
            Status = true,
            CompanyId = payload.CompanyId,
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