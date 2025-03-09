using System.ComponentModel.DataAnnotations;

namespace CVhantering.Models
{
    public class Person
    {
        [Key]
        public int Id { get; set; }
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
        public DateTime Birthday { get; set; }
        // Navigation properties
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<WorkExperience> WorkExperiences { get; set; }
    }
}
