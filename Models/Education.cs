using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVhantering.Models
{
    public class Education
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string School { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Degree { get; set; }
        [Required]
        [StringLength(5, MinimumLength = 1)]
        public string Grade { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Required]
        [ForeignKey("Person")]
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
