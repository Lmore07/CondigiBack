namespace CondigiBack.Modules.Companies.DTOs
{
    public class CompanyDTO
    {
        public class CompaniesByUserResponseDTO
        {
            public Guid CompanyId { get; set; }
            public string CompanyName { get; set; }
            public string Description { get; set; }
            public bool Status { get; set; }
        }

        public class AllCompanies
        {
            public Guid CompanyId { get; set; }
            public string CompanyName { get; set; }
            public string Description { get; set; }
            public bool Status { get; set; }
        }

        public class  UsersByCompanyResponseDTO
        {
            public Guid UserId { get; set; }
            public string RoleInCompany { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }

            public bool Status { get; set; }
        }
    }
}
