using System.ComponentModel.DataAnnotations;

namespace CVhantering.Dtos
{
    public class EducationDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string School { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Degree { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 1)]
        public string Grade { get; set; }
        public string StartDate { get; set; } // Set as string to avoid date format issues
        public string? EndDate { get; set; } 

    }
}
