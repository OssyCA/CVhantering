using System.ComponentModel.DataAnnotations;

namespace CVhantering.Dtos
{
    public class CreatePersonDto
    {
        [Required]
        [StringLength(35, MinimumLength = 3)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(35, MinimumLength = 3)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string Birthday { get; set; } 
    }
}
