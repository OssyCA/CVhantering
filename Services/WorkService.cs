using CVhantering.Data;
using CVhantering.Dtos;
using CVhantering.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using CVhantering.ValidateFolder;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.IdentityModel.Tokens;

namespace CVhantering.Services
{
    public class WorkService
    {
        private readonly HandleCvDbContext context; 
        public WorkService(HandleCvDbContext _context)
        {
            context = _context;  // Dependency injection
        } 
        public async Task<ResultHelper<WorkExperienceDto>> AddWorkExperience(int id, CreateWorkDto cwDto)
        {
            var person = await context.Persons
                .Include(p => p.WorkExperiences) // to add the work experience to the person
                .FirstOrDefaultAsync(p => p.Id == id);


            if (person == null)
            {
                return ResultHelper<WorkExperienceDto>.Failure(["person not found"]);
            }
            if (!DateTime.TryParse(cwDto.StartDate, out DateTime startDate)) //if the start date is not a valid date
            {
                return ResultHelper<WorkExperienceDto>.Failure(["Invalid startdate "]);
            }
            DateTime? endDate = null;

            if (!string.IsNullOrEmpty(cwDto.EndDate))
            {
                if (DateTime.TryParse(cwDto.EndDate, out DateTime parsedEndDate))
                {
                    endDate = parsedEndDate;
                }
                else
                {
                    return ResultHelper<WorkExperienceDto>.Failure(["Invalid enddate"]); //if the end date is not a valid date
                }
            }

            var newWork = new WorkExperience 
            {
                Company = cwDto.Company,
                WorkTitle = cwDto.WorkTitle,
                StartDate = startDate,
                EndDate = endDate
            };

            person.WorkExperiences.Add(newWork);
            await context.SaveChangesAsync();

            return ResultHelper<WorkExperienceDto>.Success(MapWorkExperienceDto(newWork));
        }
        public async Task<ResultHelper<WorkExperienceDto>> UpdateWorkExperience(int id, WorkExperienceDto wDto)
        {
            var work = await context.WorkExperiences.FindAsync(id); 

            if (work == null)
            {
                return ResultHelper<WorkExperienceDto>.Failure(["Person not found"]);
            }

            work.Company = !string.IsNullOrEmpty(wDto.Company) ? wDto.Company : work.Company;
            work.WorkTitle = !string.IsNullOrEmpty(wDto.WorkTitle) ? wDto.WorkTitle : work.Company;


            if (!string.IsNullOrEmpty(wDto.StartDate))
            {
                if (DateTime.TryParse(wDto.StartDate, out DateTime startDate))
                {
                    work.StartDate = startDate;
                }
                else
                {
                    return ResultHelper<WorkExperienceDto>.Failure(new List<string> { "Invalid StartDate" });
                }
            }

            if (!string.IsNullOrEmpty(wDto.EndDate))
            {
                if (DateTime.TryParse(wDto.EndDate, out DateTime endDate))
                {
                    if (endDate < work.StartDate)
                    {
                        return ResultHelper<WorkExperienceDto>.Failure(new List<string> { "EndDate can't be earlier than StartDate" });
                    }
                    work.EndDate = endDate;
                }
                else
                {
                    return ResultHelper<WorkExperienceDto>.Failure(new List<string> { "Invalid EndDate" });
                }
            }

            await context.SaveChangesAsync();

            return ResultHelper<WorkExperienceDto>.Success(MapWorkExperienceDto(work));

        }
        public async Task<bool> DeleteWork(int id)
        {
            var work = await context.WorkExperiences.FirstOrDefaultAsync(p => p.Id == id);

            if (work == null)
            {
                throw new KeyNotFoundException();
            }
            try
            {
                context.WorkExperiences.Remove(work);
                await context.SaveChangesAsync();
                return true;

            }
            catch (DbUpdateException)
            {
                throw new InvalidOperationException("Failed to delete.");
            }
        }
        private WorkExperienceDto MapWorkExperienceDto(WorkExperience work)
        {
            return new WorkExperienceDto
            {
                Company = work.Company,
                WorkTitle = work.WorkTitle,
                StartDate = work.StartDate.ToShortDateString(),
                EndDate = work.EndDate.ToString()
            };
        }
    }
}
