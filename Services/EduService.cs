using CVhantering.Data;
using CVhantering.Dtos;
using CVhantering.Models;
using CVhantering.ValidateFolder;
using Microsoft.EntityFrameworkCore;

namespace CVhantering.Services
{
    public class EduService
    {
        private readonly HandleCvDbContext context;
        public EduService(HandleCvDbContext _context) //dependency injection of the database context
        {
            context = _context; //assigning the injected context to the local context
        }
        public async Task<ResultHelper<EducationDto>> AddEducation(int id, CreateEduDto eDto)
        {
            var person = await context.Persons // get the person with the id
                .Include(e => e.Educations)
                .FirstOrDefaultAsync(pe => pe.Id == id);

            if (person == null)
            {
                return ResultHelper<EducationDto>.Failure(["Person not found"]); //if the person is not found, return an error
            }

            if (!DateTime.TryParse(eDto.StartDate, out DateTime startDate)) //if the start date is not a valid date
            {
                return ResultHelper<EducationDto>.Failure(["Invalid start date format"]); 
            }
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(eDto.EndDate))
            {
                if (DateTime.TryParse(eDto.EndDate, out DateTime parsedEndDate))
                {
                    endDate = parsedEndDate;
                }
                else
                {
                    return ResultHelper<EducationDto>.Failure(["Invalid end date format"]); //if the end date is not a valid date
                }
            }

            var newEdu = new Education //create a new education object
            {
                School = eDto.School,
                Degree = eDto.Degree,
                Grade = eDto.Grade,
                StartDate = startDate,
                EndDate = endDate,
                Person = person
            };

            person.Educations.Add(newEdu);
            await context.SaveChangesAsync();

            return ResultHelper<EducationDto>.Success(MapEducationToDto(newEdu)); //return the new education object
        }
        public async Task<ResultHelper<EducationDto>> UpdateEducation(int id, EducationDto eDto)
        {
            var education = await context.Educations.FindAsync(id);
            if (education == null)
            {
                return ResultHelper<EducationDto>.Failure(new List<string> { "Education not found" });
            }

            education.School = !string.IsNullOrEmpty(eDto.School) ? eDto.School : education.School;
            education.Degree = !string.IsNullOrEmpty(eDto.Degree) ? eDto.Degree : education.Degree;
            education.Grade = !string.IsNullOrEmpty(eDto.Grade) ? eDto.Grade : education.Grade;

            if (!string.IsNullOrEmpty(eDto.StartDate))
            {
                if (DateTime.TryParse(eDto.StartDate, out DateTime startDate))
                {
                    education.StartDate = startDate;
                }
                else
                {
                    return ResultHelper<EducationDto>.Failure(new List<string> { "Invalid StartDate" });
                }
            }

            if (!string.IsNullOrEmpty(eDto.EndDate))
            {
                if (DateTime.TryParse(eDto.EndDate, out DateTime endDate))
                {
                    if (endDate < education.StartDate)
                    {
                        return ResultHelper<EducationDto>.Failure(new List<string> { "EndDate can't be earlier than StartDate" });
                    }
                    education.EndDate = endDate;
                }
                else
                {
                    return ResultHelper<EducationDto>.Failure(new List<string> { "Invalid EndDate" });
                }
            }

            await context.SaveChangesAsync();
            return ResultHelper<EducationDto>.Success(MapEducationToDto(education));
        }

        // bool because we only need to know if it was successful or not
        public async Task<bool> DeleteEducation(int id)
        {
            var edu = await context.Educations.FirstOrDefaultAsync(p => p.Id == id);

            if (edu == null)
            {
                throw new KeyNotFoundException();
            }
            try
            {
                context.Educations.Remove(edu);
                await context.SaveChangesAsync();
                return true;

            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Failed to delete.");
            }
        }
        // Map Education to EducationDto
        private EducationDto MapEducationToDto(Education edu)
        {
            return new EducationDto
            {
                School = edu.School,
                Degree = edu.Degree,
                Grade = edu.Grade,
                StartDate = edu.StartDate.ToShortDateString(),
                EndDate = edu.EndDate.ToString()
            };
        }
    }
}
