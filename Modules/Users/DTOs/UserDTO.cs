

using CondigiBack.Modules.Auth.DTOs;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using CondigiBack.Modules.Geography.DTOs;

namespace CondigiBack.Modules.Users.DTOs
{
    public class UserDTO
    {
        public class CompanyRegistrationDTO
        {
            [Required(ErrorMessage = "El nombre de la empresa es requerido")]
            public string Name { get; set; }

            [Required(ErrorMessage = "El RUC de la empresa es requerido")]
            [MinLength(13, ErrorMessage = "El RUC debe tener mínimo 13 caracteres")]
            [MaxLength(13, ErrorMessage = "El RUC debe tener máximo 13 caracteres")]
            [RegularExpression(@"^[0-9]*$", ErrorMessage = "El RUC debe ser numérico")]
            public string RUC { get; set; }

            [Required(ErrorMessage = "La dirección de la empresa es requerida")]
            public string Address { get; set; }

            [Required(ErrorMessage = "El teléfono de la empresa es requerido")]
            [MinLength(10, ErrorMessage = "El teléfono debe tener mínimo 10 caracteres")]
            [MaxLength(10, ErrorMessage = "El teléfono debe tener máximo 10 caracteres")]
            [RegularExpression(@"^[0-9]*$", ErrorMessage = "El teléfono debe ser numérico")]
            public string Phone { get; set; }

            [Required(ErrorMessage = "El correo electrónico de la empresa es requerido")]
            [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
            public string Email { get; set; }

            [Required(ErrorMessage = "La parroquia de la empresa es requerida")]
            [RegularExpression(@"^[0-9]*$", ErrorMessage = "La parroquia debe ser numérica")]
            public int ParishId { get; set; }
        }

        public class RegistrationUserToCompanyDTO
        {
            public Guid CompanyId { get; set; }
            public AuthDTO.UserRegistrationDto UserRegistrationDto { get; set; }
        }

        public class UpdateUserDto
        {

            [MinLength(10, ErrorMessage = "El teléfono debe tener mínimo 10 caracteres")]
            [MaxLength(10, ErrorMessage = "El teléfono debe tener máximo 10 caracteres")]
            [RegularExpression(@"^[0-9]*$", ErrorMessage = "El teléfono debe ser numérico")]
            public string? Phone { get; set; }

            [RegularExpression(@"^[0-9]*$", ErrorMessage = "La parroquia debe ser numérica")]
            public int? ParishId { get; set; }

            public string? Address { get; set; }
            
            public string? FirstName { get; set; }
            
            public string? LastName { get; set; }
            
            public string? email { get; set; }
        }
        
        public class GetUserDto
        {
            public Guid Id { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Email { get; set; }
            public string? Phone { get; set; }
            public string? Address { get; set; }
            public GeographyDTO.ParishResponseDTO? Parish { get; set; }
        }
        
    }
}
