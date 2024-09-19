using System.Globalization;
using System.Text;
using CondigiBack.Contexts;
using CondigiBack.Libs.Enums;
using CondigiBack.Libs.Interfaces;
using CondigiBack.Libs.Responses;
using CondigiBack.Libs.Utils;
using CondigiBack.Models;
using CondigiBack.Modules.Contracts.DTOs;
using Microsoft.EntityFrameworkCore;
using CondigiBack.Libs.Services;

namespace CondigiBack.Modules.Contracts.Services;

public class ContractService
{
    private readonly PdfGeneratorClass pdfGenerator;
    private readonly CloudinaryUploader cloudinaryUploader;
    private readonly AppDBContext appDbContext;
    private readonly GeminiService _geminiService;
    private readonly EmailService _emailService;

    public ContractService(AppDBContext appDbContext, PdfGeneratorClass pdfGenerator, CloudinaryUploader cloudinaryUploader, GeminiService geminiService, EmailService emailService)
    {
        this.appDbContext = appDbContext;
        this.pdfGenerator = pdfGenerator;
        this.cloudinaryUploader = cloudinaryUploader;
        _geminiService = geminiService;
        _emailService = emailService;
    }

    public async Task<GeneralResponse<List<ContractDto.ContractResponseDTO>>> GetContractsByUser(int currentPage,
        int pageSize, Guid userId, StatusContractEnum? status)
    {
        var contracts = await appDbContext.Contracts
            .Where(c => (!status.HasValue || c.Status == status) &&
                        c.ContractParticipants.Any(cp =>
                            (cp.Company != null && cp.Company.UserCompanies.Any(uc => uc.UserId == userId)) ||
                            cp.UserId == userId))
            .Include(c => c.ContractType)
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
            ContractType = new ContractTypeDto.ContractTypeResponseDTO
            {
                ContractTypeId = c.ContractType.Id,
                Name = c.ContractType.Name,
                Description = c.ContractType.Description,
                CreatedAt = c.ContractType.CreatedAt,
                UpdatedAt = c.ContractType.UpdatedAt,
            },
            StartDate = c.StartDate.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES")),
            EndDate = c.EndDate.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES")),
            NumClauses = c.NumClauses,
            PaymentAmount = c.PaymentAmount,
            PaymentFrequency = c.PaymentFrequency,
            Status = c.Status,
            CreatedAt = c.CreatedAt.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES")),
            CreatedBy = c.CreatedBy,
            UpdatedAt = c.UpdatedAt.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES")),
            UpdatedBy = c.UpdatedBy,
            UrlPDF = c.UrlPDF,
        }).ToList();

        return new PaginatedResponse<List<ContractDto.ContractResponseDTO>>(contractsResponse, StatusCodes.Status200OK,
            "Contratos encontrados", paginationInfo);
    }

    public async Task<GeneralResponse<ContractDto.ContractResponseDTO>> GetContractById(Guid contractId)
    {
        var contract = await appDbContext.Contracts.Include(contract => contract.ContractParticipants)
            .ThenInclude(contractParticipant => contractParticipant.User).ThenInclude(user => user!.Person)
            .Include(contract => contract.ContractParticipants)
            .ThenInclude(contractParticipant => contractParticipant.Company)
            .Include(contract => contract.ContractType)
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
            ContractType = new ContractTypeDto.ContractTypeResponseDTO
            {
                ContractTypeId = contract.ContractType.Id,
                Name = contract.ContractType.Name,
                Description = contract.ContractType.Description,
                CreatedAt = contract.ContractType.CreatedAt,
                UpdatedAt = contract.ContractType.UpdatedAt,
            },
            Content = contract.Content != null
                ? Encoding.UTF8.GetString(Convert.FromBase64String(contract.Content))
                : null,
            StartDate = contract.StartDate.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES")),
            EndDate = contract.EndDate.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES")),
            NumClauses = contract.NumClauses,
            PaymentAmount = contract.PaymentAmount,
            PaymentFrequency = contract.PaymentFrequency,
            Status = contract.Status,
            CreatedAt = contract.CreatedAt.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES")),
            UpdatedAt = contract.UpdatedAt.ToString("dd 'de' MMMM 'del' yyyy", new CultureInfo("es-ES")),
            CreatedBy = contract.CreatedBy,
            UpdatedBy = contract.UpdatedBy,
            UrlPDF = contract.UrlPDF,
            ContractParticipants = contract.ContractParticipants.Select(cp =>
                new ContractParticipantDTO.ContractParticipantResponseDTO
                {
                    ContractParticipantId = cp.Id,
                    Signed = cp.Signed,
                    Role = cp.Role,
                    Status = cp.Status,
                    ContractId = cp.ContractId,
                    User = cp.UserId != null
                        ? new ContractParticipantDTO.UserDto
                        {
                            Id = cp.UserId.Value,
                            FirstName = cp.User!.Person!.FirstName,
                            LastName = cp.User!.Person!.LastName
                        }
                        : null,
                    Company = cp.CompanyId != null
                        ? new ContractParticipantDTO.CompanyDto
                        {
                            Id = cp.CompanyId.Value,
                            Name = cp.Company!.Name
                        }
                        : null
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
            StartDate = contractDto.StartDate.ToUniversalTime(),
            EndDate = contractDto.EndDate.ToUniversalTime(),
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

        if (contractDto.ContractTypeId != null)
        {
            var contractType = await appDbContext.ContractTypes.FirstOrDefaultAsync(ct =>
                ct.Id == contractDto.ContractTypeId);

            if (contractType == null)
            {
                return new ErrorResponse<bool>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "No se encontró el tipo de contrato indicado"
                };
            }

            contract.ContractTypeId = contractDto.ContractTypeId.Value;
        }

        contract.StartDate = contractDto.StartDate?.ToUniversalTime() ?? contract.StartDate;
        contract.EndDate = contractDto.EndDate?.ToUniversalTime() ?? contract.EndDate;
        contract.NumClauses = contractDto.NumClauses ?? contract.NumClauses;
        contract.PaymentAmount = contractDto.PaymentAmount ?? contract.PaymentAmount;
        contract.PaymentFrequency = contractDto.PaymentFrequency ?? contract.PaymentFrequency;
        contract.Status = contractDto.Status ?? contract.Status;
        contract.UpdatedAt = DateTime.UtcNow;
        contract.UpdatedBy = userId;
        contract.Content = contractDto.Content != null
            ? Convert.ToBase64String(Encoding.UTF8.GetBytes(contractDto.Content))
            : contract.Content;

        // Generar PDF a partir del contenido en HTML
        if (contractDto.Content != null)
        {
            var pdf = pdfGenerator.GeneratePdf(contractDto.Content, contract.EncryptionKey);
            var uploadResult = await cloudinaryUploader.UploadPdfToCloudinary(pdf);

            if (uploadResult != null)
            {
                contract.UrlPDF = uploadResult.SecureUrl.ToString();
            }
            else
            {
                contract.UrlPDF = null;
            }
        }

        //Obtener información a partir del contenido
        var response = await _geminiService.GenerateContentAsync(new GeneratePrompt().ExtractInformation(contractDto.Content));
        var senderName = _geminiService.ExtractInfo(response, "Emisor: ", "<");
        var senderEmail = _geminiService.ExtractInfo(response, "<", ">");
        var receiverName = _geminiService.ExtractInfo(response, "Remitente: ", "<", true);
        var receiverEmail = _geminiService.ExtractInfo(response, "<", ">", true);
        //Enviar email al receptor y al emisor con la información obtenida
        await _emailService.SendEmailAsync(senderEmail, senderName, receiverEmail, receiverName, contract.UrlPDF);

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