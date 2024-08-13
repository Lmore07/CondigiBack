using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondigiBack.Models;

[Table("ai_requests")]
public class AIRequest
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Content { get; set; }
    
    [Required]
    public int type { get; set; }
    
    public DateTime CreatedAt { get; set; }
        
    public Guid? CreatedBy { get; set; }
        
    public DateTime UpdatedAt { get; set; }
        
    public Guid? UpdatedBy { get; set; }
    
    public Guid ContractId { get; set; }
    
    public Contract Contract { get; set; }
}