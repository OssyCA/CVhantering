using System.ComponentModel.DataAnnotations;

namespace CVhantering.Dtos
{
    public class WorkExperienceDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Company { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string WorkTitle { get; set; }
        public string StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
