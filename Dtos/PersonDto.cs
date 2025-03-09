namespace CVhantering.Dtos
{
    public class PersonDto
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<EducationDto>  Educations { get; set; }
        public List<WorkExperienceDto> WorkExperiences { get; set; }
    }
}
