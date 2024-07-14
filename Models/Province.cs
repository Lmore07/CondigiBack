using System;
using System.ComponentModel.DataAnnotations;

namespace CondigiBack.Models
{
    public class Province
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Canton> Cantons { get; set; }
    }
}