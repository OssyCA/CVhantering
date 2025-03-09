using CVhantering.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVhantering.Dtos
{
    public class CreateEducationDto
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
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
