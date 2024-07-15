using CondigiBack.Contexts;
using CondigiBack.Libs.Interfaces;
using CondigiBack.Libs.Responses;
using CondigiBack.Libs.Utils;
using CondigiBack.Models;
using Microsoft.EntityFrameworkCore;
using static CondigiBack.Modules.Auth.DTOs.AuthDTO;


namespace CondigiBack.Modules.Auth.Services
{
    public class AuthService
    {
        private readonly AppDBContext _context;
        private readonly JWT _jwt;

        public AuthService(AppDBContext context, JWT jwt)
        {
            _context = context;
            _jwt = jwt;
        }

        public async Task<GeneralResponse<bool>> CreateUser(UserRegistrationDto userRequest)
        {
            var userExists = await _context.Users.AnyAsync(u => u.Username == userRequest.User.Username || u.Email == userRequest.User.Email);
            if (userExists)
            {
                return new ErrorResponse<bool>("El usuario ya existe", "Usuario duplicado", 404);
            }

            var personExists = await _context.Persons.AnyAsync(p => p.Identification == userRequest.Person.Identification);
            if (personExists)
            {
                return new ErrorResponse<bool>("La identificación ya existe", "Identificación duplicada", 404);
            }

            var emailExists = await _context.Users.AnyAsync(u => u.Email == userRequest.User.Email);
            if (emailExists)
            {
                return new ErrorResponse<bool>("El correo ya existe", "Correo duplicado", 404);
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var person = new Person
                    {
                        FirstName = userRequest.Person.FirstName,
                        LastName = userRequest.Person.LastName,
                        Identification = userRequest.Person.Identification,
                        Phone = userRequest.Person.Phone,
                        ParishId = userRequest.Person.ParishId,
                        Address = userRequest.Person.Address,
                        Status = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Persons.Add(person);
                    await _context.SaveChangesAsync();

                    var user = new User
                    {
                        Username = userRequest.User.Username,
                        Password = Encrypt.GenerateHash(userRequest.User.Password),
                        Email = userRequest.User.Email,
                        UserType = userRequest.User.UserType,
                        PersonId = person.Id,
                        Status = true,
                        CreatedAt = DateTime.UtcNow
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();

                    return new StandardResponse<bool>(true, "Usuario creado correctamente", 201);
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    return new ErrorResponse<bool>("Error al crear el usuario", "Error", 500);
                }
            }
        }

        public async Task<GeneralResponse<UserLoginResponseDto>> Login(UserLoginDto userRequest)
        {
            var user = await _context.Users.Include(u => u.Person).FirstOrDefaultAsync(u => u.Username == userRequest.Username);
            if (user == null)
            {
                return new ErrorResponse<UserLoginResponseDto>("Usuario no encontrado", "Usuario no encontrado", 404);
            }

            if (!Encrypt.VerifyHash(userRequest.Password, user.Password))
            {
                return new ErrorResponse<UserLoginResponseDto>("Contraseña incorrecta", "Contraseña incorrecta", 400);
            }

            return new StandardResponse<UserLoginResponseDto>(new UserLoginResponseDto
            {
                Token = _jwt.generateToken(user),
                User = new UserResponseDTO
                {
                    Name = $"{user.Person.FirstName} {user.Person.LastName}",
                    Email = user.Email,
                    Role = user.UserType.ToString()
                }
            }, "Usuario autenticado correctamente", 200);
        }
    }
}
