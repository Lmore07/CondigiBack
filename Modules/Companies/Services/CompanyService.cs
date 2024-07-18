using CondigiBack.Contexts;
using CondigiBack.Libs.Interfaces;
using CondigiBack.Libs.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static CondigiBack.Modules.Companies.DTOs.CompanyDTO;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CondigiBack.Modules.Companies.Services
{
    public class CompanyService
    {
        private readonly AppDBContext _dbContext;

        public CompanyService(AppDBContext appDBContext)
        {
            _dbContext = appDBContext;
        }

        public async Task<GeneralResponse<List<UsersByCompanyResponseDTO>>> GetUsersByCompany(Guid companyId)
        {
            var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            if (company == null)
            {
                return new ErrorResponse<List<UsersByCompanyResponseDTO>>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "No se encontró la compañia indicada"
                };
            }

            var userCompanies = await _dbContext.UserCompanies.Where(c=>c.CompanyId==companyId).ToListAsync();

            var users = await _dbContext.Users.Where(u => userCompanies.Select(uc => uc.UserId).Contains(u.Id)).ToListAsync();

            var usersResponse = users.Select(u => new UsersByCompanyResponseDTO
            {
                UserId = u.Id,
                RoleInCompany = userCompanies.FirstOrDefault(uc => uc.UserId == u.Id).RoleInCompany,
                Email = u.Email,
                Status = u.Status,
                Username = u.Username
            }).ToList();

            return new StandardResponse<List<UsersByCompanyResponseDTO>>(usersResponse,"Usuarios encontrados",StatusCodes.Status200OK);
        }
    }
}
