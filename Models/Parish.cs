using System.ComponentModel.DataAnnotations;

namespace CondigiBack.Models
{
    public class Parish
    {
        [Key]
        public int IdParish { get; set; }

        [Required]
        public string NameParish { get; set; }

        [Required]
        public int CantonId { get; set; }

        public Canton Canton { get; set; }

        public ICollection<Person> Persons { get; set; }

    }
}
