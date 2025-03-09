using System.ComponentModel.DataAnnotations;

namespace CVhantering.Dtos
{
    public class UpdatePersonDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Phone]
        public string Phone { get; set; }
        public string Birthday { get; set; }
    }
}
