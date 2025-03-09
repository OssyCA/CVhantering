using CVhantering.Data;
using CVhantering.Dtos;
using CVhantering.Models;
using CVhantering.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace CVhantering.EndpointFolder
{
    public class PersonEndpoints
    {
        public static void PersonGetEndpoint(WebApplication app)
        {
            app.MapGet("/GetAllData", async (PersonServices ps) =>
            {
                try
                {
                    var result = await ps.GetAllPerson();
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    // Log the exception (ex) here if needed
                    return Results.NotFound($"An error occurred while retrieving data.{ex.Message}");
                }
            });
            app.MapGet("GetPerson/{id}", async (int id, PersonServices ps) =>
            {
                var result = await ps.GetPerson(id);
                
                if (result != null && result.Count != 0) //if the result is not null and has any elements
                {
                    return Results.Ok(result);
                }
                else
                {
                    return Results.NotFound("Person not found");
                }

            });
            app.MapPost("/person/{id}/workexperience", async (int id, CreateWorkDto dto, PersonServices ps) =>
            {
                try
                {
                    var result = await ps.AddWorkExperience(id, dto);

                    return Results.Ok(result);
                }
                catch (KeyNotFoundException)
                {

                    return Results.NotFound();
                }
                catch (ArgumentException)
                {
                    return Results.BadRequest();
                }
                
        
            });
            app.MapPost("/person/{id}/Education", async (int id, CreateEducationDto dto, PersonServices ps) =>
            {
                try
                {
                    var result = await ps.AddEducation(id, dto);
                    return Results.Ok(result);
                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound("Person not found");
                }
                catch (ArgumentException)
                {
                    return Results.BadRequest("Wrong format or invalid data");
                }


            });
            app.MapPut("workexperience/{id}", async (int id, WorkExperienceDto dto, PersonServices ps) =>
            {
                try
                {
                    var result = await ps.UpdateWorkExperience(id, dto);
                    return Results.Ok(result);
                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound("Person not found");
                }
                catch (ArgumentException)
                {
                    return Results.BadRequest("Wrong format or invalid data");
                }
            });
            app.MapPut("education/{id}", async (int id, EducationDto dto, PersonServices ps) =>
            {
                try
                {
                    var result = await ps.UpdateEducation(id, dto);
                    return Results.Ok(result);
                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound("Person not found");
                }
                catch (ArgumentException)
                {
                    return Results.BadRequest("Wrong format or invalid data");
                }
            });
        }

    }
}
