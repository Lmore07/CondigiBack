using CondigiBack.Contexts;
using CondigiBack.Libs.Interfaces;
using CondigiBack.Libs.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static CondigiBack.Modules.Companies.DTOs.CompanyDTO;

namespace CondigiBack.Modules.Companies.Services
{
    public class CompanyService
    {
        private readonly AppDBContext _dbContext;

        public CompanyService(AppDBContext appDBContext)
        {
            _dbContext = appDBContext;
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
                Address = c.Address,
                Email = c.Email,
                ParishId = c.ParishId,
                Phone = c.Phone,
                RUC = c.RUC,
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
