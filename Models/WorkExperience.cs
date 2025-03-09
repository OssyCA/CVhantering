using System.ComponentModel.DataAnnotations;

namespace CVhantering.Models
{
    public class WorkExperience
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Company { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string WorkTitle { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}

