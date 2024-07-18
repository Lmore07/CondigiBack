using System.ComponentModel.DataAnnotations;

namespace CondigiBack.Models
{
    public class UserCompanies
    {
        [Key]
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public string RoleInCompany { get; set; }

        public Company Company { get; set; }
        public User User { get; set; }
    }
}
