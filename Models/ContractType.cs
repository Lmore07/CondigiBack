﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CondigiBack.Models
{
    [Table("contract_types")]
    public class ContractType
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public bool Status { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        public Guid? CreatedBy { get; set; }
        
        public DateTime UpdatedAt { get; set; }
        
        public Guid? UpdatedBy { get; set; }

        public ICollection<Contract> Contracts { get; set; }
    }
}
