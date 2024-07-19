using CondigiBack.Libs.Enums;
using System.ComponentModel.DataAnnotations;

namespace CondigiBack.Modules.Auth.DTOs
{
    public class AuthDTO
    {
        public class PersonCreationDto
        {
            [Required(ErrorMessage ="El nombre es requerido")]
            [MinLength(3, ErrorMessage = "El nombre debe tener mínimo 3 letras")]
            [MaxLength(50, ErrorMessage = "El nombre debe tener máximo 50 letras")]
            public string FirstName { get; set; }


            [Required(ErrorMessage = "El apellido es requerido")]
            [MinLength(3, ErrorMessage = "El apellido debe tener mínimo 3 letras")]
            [MaxLength(50, ErrorMessage = "El apellido debe tener máximo 50 letras")]
            public string LastName { get; set; }


            [Required(ErrorMessage = "La identificación es requerida")]
            [MinLength(10, ErrorMessage = "La identificación debe tener mínimo 10 caracteres")]
            [MaxLength(13, ErrorMessage = "La identificación debe tener máximo 13 caracteres")]
            [RegularExpression(@"^[0-9]*$", ErrorMessage = "La identificación debe ser numérica")]
            public string Identification { get; set; }


            [MinLength(10, ErrorMessage = "El teléfono debe tener mínimo 10 caracteres")]
            [MaxLength(10, ErrorMessage = "El teléfono debe tener máximo 10 caracteres")]
            [RegularExpression(@"^[0-9]*$", ErrorMessage = "El teléfono debe ser numérico")]
            public string? Phone { get; set; }


            [RegularExpression(@"^[0-9]*$", ErrorMessage = "La parroquia debe ser numérica")]
            public int? ParishId { get; set; }


            public string? Address { get; set; }

        }

        public class UserCreationDto
        {
            [Required(ErrorMessage = "El nombre de usuario es requerido")]
            [MinLength(3, ErrorMessage = "El nombre de usuario debe tener mínimo 3 letras")]
            [MaxLength(50, ErrorMessage = "El nombre de usuario debe tener máximo 50 letras")]
            public string Username { get; set; }

            [Required(ErrorMessage = "La contraseña es requerida")]
            [MinLength(8, ErrorMessage = "La contraseña debe tener mínimo 8 caracteres")]
            [MaxLength(50, ErrorMessage = "La contraseña debe tener máximo 50 caracteres")]
            [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,50}$", ErrorMessage = "La contraseña debe tener al menos una letra mayúscula, una letra minúscula, un número y un caracter especial")]
            public string Password { get; set; }

            [Required(ErrorMessage = "El correo electrónico es requerido")]
            [EmailAddress(ErrorMessage = "El correo electrónico no es válido")]
            public string Email { get; set; }

            [Required(ErrorMessage = "El tipo de usuario es requerido")]
            [EnumDataType(typeof(UserTypeEnum), ErrorMessage = "El tipo de usuario no es válido")]
            public UserTypeEnum UserType { get; set; }

        }

        public class UserRegistrationDto
        {
            public PersonCreationDto Person { get; set; }
            public UserCreationDto User { get; set; }
        }

        public class UserLoginDto
        {
            [Required(ErrorMessage = "El nombre de usuario es requerido")]
            public string Username { get; set; }

            [Required(ErrorMessage = "La contraseña es requerida")]
            public string Password { get; set; }
        }

        public class UserLoginResponseDto
        {
            public string Token { get; set; }
            public UserResponseDTO User { get; set; }
        }

        public class UserResponseDTO
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public string Role { get; set; }
        }
    }
}
