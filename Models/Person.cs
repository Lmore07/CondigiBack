using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace CondigiBack.Models
{
    public class Person
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        [Required]
        public string LastName { get; set; }
        
        [Required]
        public string Identification { get; set; }
        
        public string Phone { get; set; }
        
        public int ParishId { get; set; }
        
        public string Address { get; set; }
        
        public bool Status { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public Guid? CreatedBy { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public Guid? UpdatedBy { get; set; }
        
        public Parish Parish { get; set; }
        
        public User User { get; set; }
        
    }
}
