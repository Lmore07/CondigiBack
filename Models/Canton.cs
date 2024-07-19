using System.ComponentModel.DataAnnotations;

namespace CondigiBack.Models
{
    public class Canton
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int ProvinceId { get; set; }

        public Province Province { get; set; }

        public ICollection<Parish> Parishes { get; set; }
    }
}
