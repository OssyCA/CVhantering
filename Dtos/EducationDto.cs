using System.ComponentModel.DataAnnotations;

namespace CVhantering.Dtos
{
    public class EducationDto
    {
        public string School { get; set; }
        public string Degree { get; set; }
        public string Grade { get; set; }
        public string StartDate { get; set; } // Set as string to avoid date format issues
        public string? EndDate { get; set; } 

    }
}
