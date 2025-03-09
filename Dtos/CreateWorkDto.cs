using CVhantering.Models;
using System.ComponentModel.DataAnnotations;

namespace CVhantering.Dtos
{
    public class CreateWorkDto
    {
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Company { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string WorkTitle { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
