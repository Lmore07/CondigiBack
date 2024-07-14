using CondigiBack.Contexts;
using CondigiBack.Libs.Interfaces;
using CondigiBack.Libs.Responses;
using CondigiBack.Models;
using Microsoft.EntityFrameworkCore;
using static CondigiBack.Modules.Geography.DTOs.GeographyDTO;

namespace CondigiBack.Modules.Geography.Services
{
    public class GeographyService
    {
        private readonly AppDBContext _context;

        public GeographyService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<GeneralResponse<List<ProvinceResponseDTO>>> GetProvinces()
        {
            var provinces = await _context.Provinces.ToListAsync();

            if (provinces.Count==0)
            {
                return new ErrorResponse<List<ProvinceResponseDTO>>("No se encontraron provincias","No hay datos",404);
            }
            var provincesResponse = provinces.Select(p => new ProvinceResponseDTO
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
            return new StandardResponse<List<ProvinceResponseDTO>>(provincesResponse, "Provincias obtenidas", 200);
        }

        public async Task<GeneralResponse<List<CantonResponseDTO>>> GetCantons(int provinceId)
        {
            var province = await _context.Provinces.FindAsync(provinceId);
            if (province == null)
            {
                return new ErrorResponse<List<CantonResponseDTO>>("No se encontró la provincia","No hay datos",404);
            }
            var cantons = await _context.Cantons.Where(c => c.ProvinceId == provinceId).ToListAsync();
            if (cantons.Count==0)
            {
                return new ErrorResponse<List<CantonResponseDTO>>("No se encontraron cantones","No hay datos",404);
            }
            var cantonsResponse = cantons.Select(c => new CantonResponseDTO
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
            return new StandardResponse<List<CantonResponseDTO>>(cantonsResponse, "Cantones obtenidos", 200);
        }

        public async Task<GeneralResponse<List<ParishResponseDTO>>> GetParishes(int cantonId)
        {
            var canton = await _context.Cantons.FindAsync(cantonId);
            if (canton == null)
            {
                return new ErrorResponse<List<ParishResponseDTO>>("No se encontró el cantón","No hay datos",404);
            }
            var parishes = await _context.Parishes.Where(p => p.CantonId == cantonId).ToListAsync();
            if (parishes.Count==0)
            {
                return new ErrorResponse<List<ParishResponseDTO>>("No se encontraron parroquias","No hay datos",404);
            }
            var parishesResponse = parishes.Select(p => new ParishResponseDTO
            {
                Id = p.IdParish,
                Name = p.NameParish
            }).ToList();
            return new StandardResponse<List<ParishResponseDTO>>(parishesResponse, "Parroquias obtenidas", 200);
        }
    }
}
