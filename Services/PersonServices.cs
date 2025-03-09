using CVhantering.Data;
using CVhantering.Dtos;
using CVhantering.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;
using CVhantering.ValidateFolder;
using Microsoft.AspNetCore.Identity;
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
            var person = context.Persons
                .Include(e => e.Educations)
                .Include(w => w.WorkExperiences)
                .AsSplitQuery(); //splitting the query into multiple queries to avoid loading unnecessary data

            if (id != null)
            {
                person = person.Where(p => p.Id == id); //if the id is not null, filter the query by the id
            }

            return await person.Select(p => new PersonDto
            {
                FirstName = p.FirstName,
                LastName = p.LastName,
                Educations = p.Educations.Select(e => new EducationDto
                {
                    School = e.School,
                    Degree = e.Degree,
                    Grade = e.Grade,
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
        public async Task<ResultHelper<PersonDto>> CreatePerson(CreatePersonDto cP)
        {

            if(!DateTime.TryParse(cP.Birthday, out DateTime birthday))
            {
                return ResultHelper<PersonDto>.Failure(["Invalid birthday format"]);
            }

            var newPerson = new Person
            {
                FirstName = cP.FirstName,
                LastName = cP.LastName,
                Email = cP.Email,
                Phone = cP.Phone,
                Birthday = birthday
            };

            var personDto = new PersonDto
            {
                FirstName = cP.FirstName,
                LastName = cP.LastName
            };
            context.Persons.Add(newPerson);
            await context.SaveChangesAsync();
            return ResultHelper<PersonDto>.Success(personDto);
            
        }
        public async Task<ResultHelper<PersonDto>> UpdatePerson(int id, UpdatePersonDto dto)
        {
            var person = await context.Persons.FindAsync(id);
            if (person == null)
                return ResultHelper<PersonDto>.Failure(["Person not found"]);

            person.FirstName = !string.IsNullOrEmpty(dto.FirstName) ? dto.FirstName : person.FirstName;
            person.LastName = !string.IsNullOrEmpty(dto.LastName) ? dto.LastName : person.LastName;
            person.Email = !string.IsNullOrEmpty(dto.Email) ? dto.Email : person.Email;
            person.Phone = !string.IsNullOrEmpty(dto.Phone) ? dto.Phone : person.Phone;

            if (!string.IsNullOrWhiteSpace(dto.Birthday))
            {
                if (DateTime.TryParse(dto.Birthday, out DateTime birthday))
                    person.Birthday = birthday;
                else
                    return ResultHelper<PersonDto>.Failure(["Invalid birthday format"]);
            }

            await context.SaveChangesAsync();

            return ResultHelper<PersonDto>.Success(new PersonDto
            {
                FirstName = person.FirstName,
                LastName = person.LastName
            });
        }


    }
}
