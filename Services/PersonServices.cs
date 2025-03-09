using CVhantering.Data;
using CVhantering.Dtos;
using CVhantering.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace CVhantering.Services
{
    public class PersonServices
    {
        private readonly HandleCvDbContext context;
        public PersonServices(HandleCvDbContext _context) //dependency injection of the database context
        {
            context = _context; //assigning the injected context to the local context
        }
        public async Task<List<PersonDto>> GetAllPerson()
        {
            return await GetPersons();
        }
        public async Task<List<PersonDto>> GetPerson(int id)
        {
            return await GetPersons(id);
        }
        public async Task<List<PersonDto>> GetPersons(int? id = null) // can be called with or without an id, null is default
        {
            IQueryable<Person> query = context.Persons //querying the database for the person
                .Include(e => e.Educations)
                .Include(w => w.WorkExperiences)
                .AsSplitQuery(); //splitting the query into multiple queries to avoid loading unnecessary data

            if (id != null)
            {
                query = query.Where(p => p.Id == id); //if the id is not null, filter the query by the id
            }

            return await query.Select(p => new PersonDto
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                Educations = p.Educations.Select(e => new EducationDto
                {
                    School = e.School,
                    Degree = e.Degree,
                    StartDate = e.StartDate.ToShortDateString(),
                    EndDate = e.EndDate.HasValue ? e.EndDate.Value.ToShortDateString() : null // if the end date is null, return null
                }).ToList(),
                WorkExperiences = p.WorkExperiences.Select(w => new WorkExperienceDto
                {
                    Company = w.Company,
                    WorkTitle = w.WorkTitle,
                    StartDate = w.StartDate.ToShortDateString(),
                    EndDate = w.EndDate.ToString()
                }).ToList()
            }).ToListAsync();
        }
        public async Task<WorkExperienceDto> AddWorkExperience(int id, CreateWorkDto cwDto)
        {
            var person = await context.Persons
                .Include(p => p.WorkExperiences)
    .           FirstOrDefaultAsync(p => p.Id == id);

            if (person == null)
            {
                throw new KeyNotFoundException();
            }

            var validContext = new ValidationContext(cwDto, null, null);
            var validResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(cwDto, validContext, validResult, true);

            if (!isValid)
            {
                throw new ArgumentException(); // if the object is not valid, throw an exception
            }

            var newWork = new WorkExperience
            {
                Company = cwDto.Company,
                WorkTitle = cwDto.WorkTitle,
                StartDate = cwDto.StartDate,
                EndDate = cwDto.EndDate,
                Person = person
            };

            person.WorkExperiences.Add(newWork);

            await context.SaveChangesAsync();

            return new WorkExperienceDto
            {
                Company = newWork.Company,
                WorkTitle = newWork.WorkTitle,
                StartDate = newWork.StartDate.ToShortDateString(),
                EndDate = newWork.EndDate.ToString()
            };
        }
        public async Task<EducationDto> AddEducation(int id, CreateEducationDto eDto)
        {
            var person = await context.Persons
                .Include(e => e.Educations)
                .FirstOrDefaultAsync(pe => pe.Id == id);

            if (person == null)
            {
                throw new KeyNotFoundException("Person not found");
            }
            var validContext = new ValidationContext(eDto, null, null);
            var validResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(eDto, validContext, validResult, true);

            if (!isValid)
            {
                throw new ArgumentException();
            }
            var newEdu = new Education
            {
                School = eDto.School,
                Degree = eDto.Degree,
                Grade = eDto.Grade,
                StartDate = eDto.StartDate,
                EndDate = eDto.EndDate,
                Person = person
            };

            person.Educations.Add(newEdu);
            await context.SaveChangesAsync();

            return new EducationDto
            {
                School = eDto.School,
                Degree = eDto.Degree,
                StartDate = eDto.StartDate.ToShortDateString(),
                EndDate = eDto.EndDate.ToString()
            };
        }
        public async Task<WorkExperienceDto> UpdateWorkExperience(int id, WorkExperienceDto wDto)
        {
            var work = await context.WorkExperiences.FindAsync(id);

            if (work == null)
            {
                throw new KeyNotFoundException();
            }

            var validContext = new ValidationContext(wDto, null, null);
            var validResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(wDto, validContext, validResult, true);

            if (!isValid)
            {
                throw new ArgumentException(); // if the object is not valid, throw an exception
            }
            work.Company = 
                !string.IsNullOrEmpty(wDto.Company) ? wDto.Company : work.Company;
            work.WorkTitle =
                !string.IsNullOrEmpty(wDto.WorkTitle) ? wDto.WorkTitle : work.WorkTitle;
            work.StartDate =
                !string.IsNullOrEmpty(wDto.StartDate) ? DateTime.Parse(wDto.StartDate) : work.StartDate;
            work.EndDate =
                !string.IsNullOrEmpty(wDto.EndDate) ? DateTime.Parse(wDto.EndDate) : work.EndDate;

            await context.SaveChangesAsync();

            return wDto;

        }
        public async Task<EducationDto> UpdateEducation(int id, EducationDto eDto)
        {
            var education = await context.Educations.FindAsync(id);

            if (education == null)
            {
                throw new KeyNotFoundException();
            }

            // validate the object
            var validContext = new ValidationContext(eDto, null, null);
            var validResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(eDto, validContext, validResult, true);

            if (!isValid)
            {
                throw new ArgumentException(); // if the object is not valid, throw an exception
            }

            // update the object and Check if the property is not null or empty, if it is not, update the property
            education.School =
                !string.IsNullOrEmpty(eDto.School) ? eDto.School : education.School;
            education.Degree =
                !string.IsNullOrEmpty(eDto.Degree) ? eDto.Degree : education.Degree;
            education.StartDate =
                !string.IsNullOrEmpty(eDto.StartDate) ? DateTime.Parse(eDto.StartDate) : education.StartDate;
            education.EndDate =
                !string.IsNullOrEmpty(eDto.EndDate) ? DateTime.Parse(eDto.EndDate) : education.EndDate;

            await context.SaveChangesAsync();

            return eDto;
        }
    }
}
