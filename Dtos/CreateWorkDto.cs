using CVhantering.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string StartDate { get; set; }
        public string? EndDate { get; set; }
    }
}
