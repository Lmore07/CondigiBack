

using CondigiBack.Modules.Auth.DTOs;

namespace CondigiBack.Modules.Users.DTOs
{
    public class UserDTO
    {
        public class CompanyRegistrationDTO
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        public class RegistrationUserToCompanyDTO
        {
            public Guid CompanyId { get; set; }
            public AuthDTO.UserRegistrationDto UserRegistrationDto { get; set; }
        }

        
    }
}
