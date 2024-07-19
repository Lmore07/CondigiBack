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

        public async Task<GeneralResponse<List<AllCompanies>>> GetCompanies(int currentPage, int pageSize)
        {
            var companies = await _dbContext.Companies
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _dbContext.Companies.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            var paginationInfo = new Pagination(currentPage, totalPages, pageSize, totalCount);
            var companiesResponse = companies.Select(c => new AllCompanies
            {
                CompanyId = c.Id,
                CompanyName = c.Name,
                Description = c.Description,
                Status = c.Status
            }).ToList();

            if(companiesResponse.Count == 0)
            {
                return new ErrorResponse<List<AllCompanies>>
                {
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "No se encontraron compañias"
                };
            }

            return new PaginatedResponse<List<AllCompanies>>(companiesResponse, StatusCodes.Status200OK, "Compañias encontradas", paginationInfo);
        }
    }
}
