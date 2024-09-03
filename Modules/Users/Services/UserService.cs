using CondigiBack.Contexts;
using CondigiBack.Libs.Enums;
using CondigiBack.Libs.Interfaces;
using CondigiBack.Libs.Responses;
using CondigiBack.Libs.Utils;
using CondigiBack.Models;
using CondigiBack.Modules.Auth.DTOs;
using CondigiBack.Modules.Companies.DTOs;
using CondigiBack.Modules.Users.DTOs;
using Microsoft.EntityFrameworkCore;
using static CondigiBack.Modules.Geography.DTOs.GeographyDTO;
using static CondigiBack.Modules.Users.DTOs.UserDTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
namespace CondigiBack.Modules.Users.Services
{
    public class UserService
    {
        private readonly AppDBContext _dbContext;

        public UserService(AppDBContext appDBContext)
        {
            _dbContext = appDBContext;
        }

        public async Task<GeneralResponse<bool>> AddCompanyByUser(Guid userId, CompanyRegistrationDTO companyRegistration)
        {
            if (userId == Guid.Empty)
            {
                return new ErrorResponse<bool>("El id de usuario no es válido", "Usuario no encontrado", 404);
            }
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var company = new Company
                    {
                        Name = companyRegistration.Name,
                        Status = true,
                        CreatedAt = DateTime.UtcNow,
                        Email = companyRegistration.Email,
                        Phone = companyRegistration.Phone,
                        Address = companyRegistration.Address,
                        ParishId = companyRegistration.ParishId,
                        RUC = companyRegistration.RUC,
                        CreatedBy = userId
                    };
                    _dbContext.Companies.Add(company);
                    await _dbContext.SaveChangesAsync();

                    var userCompany = new UserCompanies
                    {
                        UserId = userId,
                        CompanyId = company.Id,
                        RoleInCompany = UserTypeEnum.OWNER
                    };
                    _dbContext.UserCompanies.Add(userCompany);
                    await _dbContext.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return new StandardResponse<bool>(true, "Compañía creada correctamente", 200);

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ErrorResponse<bool>("Ocurrio un error al crear la compañia", "Error al crear la compañía", 500);
                }
            }

        }

        public async Task<GeneralResponse<bool>> AddUser(RegistrationUserToCompanyDTO userRegistration)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var person = new Person
                    {
                        FirstName = userRegistration.UserRegistrationDto.Person.FirstName,
                        LastName = userRegistration.UserRegistrationDto.Person.LastName,
                        Identification = userRegistration.UserRegistrationDto.Person.Identification,
                        Phone = userRegistration.UserRegistrationDto.Person.Phone,
                        ParishId = userRegistration.UserRegistrationDto.Person.ParishId,
                        Address = userRegistration.UserRegistrationDto.Person.Address,
                        Status = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    _dbContext.Persons.Add(person);
                    await _dbContext.SaveChangesAsync();

                    var user = new User
                    {
                        Username = userRegistration.UserRegistrationDto.User.Username,
                        Password = Encrypt.GenerateHash(userRegistration.UserRegistrationDto.User.Password),
                        Email = userRegistration.UserRegistrationDto.User.Email,
                        UserType = userRegistration.UserRegistrationDto.User.UserType,
                        PersonId = person.Id,
                        Status = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    _dbContext.Users.Add(user);
                    await _dbContext.SaveChangesAsync();

                    var userCompany = new UserCompanies
                    {
                        UserId = user.Id,
                        CompanyId = userRegistration.CompanyId,
                        RoleInCompany = UserTypeEnum.SENDER
                    };
                    _dbContext.UserCompanies.Add(userCompany);
                    await _dbContext.SaveChangesAsync();


                    await transaction.CommitAsync();

                    return new StandardResponse<bool>(true, "Usuario creado correctamente", 200);

                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return new ErrorResponse<bool>("Ocurrio un error al crear el usuario", "Error al crear el usuario", 500);
                }
            }
        }

        public async Task<GeneralResponse<List<CompanyDTO.CompaniesByUserResponseDTO>>> GetCompaniesByUser(Guid userId)
        {
            var userCompanies = await _dbContext.UserCompanies
                .Where(uc => uc.UserId == userId && uc.RoleInCompany == UserTypeEnum.OWNER)
                .ToListAsync();
            if (userCompanies.Count == 0)
            {
                return new ErrorResponse<List<CompanyDTO.CompaniesByUserResponseDTO>>("No se encontraron compañías", "No se encontraron compañías", 404);
            }
            var companyIds = userCompanies.Select(uc => uc.CompanyId).ToList();

            var companies = await _dbContext.Companies
                .Where(c => companyIds.Contains(c.Id))
                .ToListAsync();
            var companiesResponse = companies.Select(p => new CompanyDTO.CompaniesByUserResponseDTO
            {
                CompanyId = p.Id,
                CompanyName = p.Name,
                Status = p.Status
            }).ToList();
            return new StandardResponse<List<CompanyDTO.CompaniesByUserResponseDTO>>(companiesResponse, "Compañías encontradas", 200);
        }
        
        public async Task<GeneralResponse<bool>> UpdateUser(Guid userId, UpdateUserDto updateUserDto)
        {
            var user = await _dbContext.Users.Include(user => user.Person).FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new ErrorResponse<bool>("Usuario no encontrado", "Usuario no encontrado", 404);
            }
            if (updateUserDto.Phone != null)
            {
                user.Person.Phone = updateUserDto.Phone;
            }
            if (updateUserDto.ParishId != null)
            {
                user.Person.ParishId = updateUserDto.ParishId.Value;
            }
            if (updateUserDto.Address != null)
            {
                user.Person.Address = updateUserDto.Address;
            }
            if (updateUserDto.FirstName != null)
            {
                user.Person.FirstName = updateUserDto.FirstName;
            }
            if (updateUserDto.LastName != null)
            {
                user.Person.LastName = updateUserDto.LastName;
            }
            if (updateUserDto.email != null)
            {
                user.Email = updateUserDto.email;
            }
            await _dbContext.SaveChangesAsync();
            return new StandardResponse<bool>(true, "Usuario actualizado correctamente", 200);
        }

        public async Task<GeneralResponse<GetUserDto>> GetUser(Guid userId)
        {
            var user = await _dbContext.Users.Include(user => user.Person).ThenInclude(person => person.Parish)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new ErrorResponse<GetUserDto>("Usuario no encontrado", "Usuario no encontrado", 404);
            }

            var userResponse = new GetUserDto
            {
                Id = user.Id,
                Address = user.Person.Address,
                FirstName = user.Person.FirstName,
                LastName = user.Person.LastName,
                Email = user.Email,
                Parish = new ParishResponseDTO()
                {
                    Id = user.Person.Parish.IdParish,
                    Name = user.Person.Parish.NameParish
                },
                Phone = user.Person.Phone
            };
            return new StandardResponse<GetUserDto>(userResponse, "Usuario encontrado", 200);
        }
    }
}
