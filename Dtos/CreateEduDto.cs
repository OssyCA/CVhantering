using System.ComponentModel.DataAnnotations;

namespace CVhantering.Dtos
{
    public class CreateEduDto
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
        [Required]
        // Set as string to avoid date format issues
        public string StartDate { get; set; }

        public string EndDate { get; set; }
    }
}
