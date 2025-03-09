using CVhantering.Data;
using CVhantering.Dtos;
using CVhantering.Models;
using CVhantering.Services;
using CVhantering.ValidateFolder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace CVhantering.EndpointFolder
{
    public class PersonEndpoints
    {
        public static void CVEndpoints(WebApplication app)
        {
            //Person
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
            app.MapGet("person/{id}", async (int id, PersonServices ps) =>
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
            app.MapPost("person", async (PersonServices ps, CreatePersonDto cp) =>
            {
                var validErrors = ValidateObjekts.ValidateObject(cp);

                if (validErrors.Any())
                {
                    string errors = string.Join("||", validErrors);
                    return Results.BadRequest(errors);
                }
                var result = await ps.CreatePerson(cp);

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Data);
                }
                else
                {
                    return Results.BadRequest(result.Errors);
                }


            });
            app.MapPut("person/{id}", async (PersonServices ps, UpdatePersonDto upDto, int id) =>
            {
                var validErrors = ValidateObjekts.ValidateObject(upDto);
                if (validErrors.Any())
                {
                    string errors = string.Join("||", validErrors);
                    return Results.BadRequest(errors);
                }

                var result = await ps.UpdatePerson(id, upDto);
                if (result.IsSuccess)
                {
                    return Results.Ok(result.Data);
                }
                else
                {
                    return Results.BadRequest(result.Errors);
                }
            });



            /// WORK
            app.MapPost("/workexperience/{id}", async (int id, CreateWorkDto cwDto, WorkService ws) =>
            {
                var validationErrors = ValidateObjekts.ValidateObject(cwDto);
                if (validationErrors.Any())
                {
                    string errors = string.Join("||", validationErrors);
                    return Results.BadRequest(errors);
                }
                var result = await ws.AddWorkExperience(id, cwDto);

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Data);
                }
                else
                {
                    if (result.Errors.Any(e => e.Contains("not found")))
                    {
                        return Results.NotFound(new { result.Errors });
                    }
                    return Results.BadRequest(new { result.Errors });
                }

            });
            app.MapPut("workexperience/{id}", async (int id, WorkExperienceDto dto, WorkService ws) =>
            {
                var result = await ws.UpdateWorkExperience(id, dto);
                try
                {
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
            app.MapDelete("workexperience/{id}", async (int id, WorkService ws) =>
            {
                try
                {
                    bool deleted = await ws.DeleteWork(id);
                    return deleted ? Results.Ok($"Deleted work with id {id}") : Results.NotFound();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Problem(ex.Message);
                }

            });


            // EDUCATION
            app.MapPost("/education/{id}", async (int id, CreateEduDto ceDto, EduService eduService) =>
            {

                var validationErrors = ValidateObjekts.ValidateObject(ceDto); // validate the object
                if (validationErrors.Any())
                {
                    string errors = string.Join("||", validationErrors);
                    return Results.BadRequest(errors);
                }

                var result = await eduService.AddEducation(id, ceDto); // add the education

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Data);
                }
                else
                {
                    if (result.Errors.Any(e => e.Contains("not found")))
                    {
                        return Results.NotFound(new { result.Errors });
                    }
                    return Results.BadRequest(new { result.Errors });
                }


            });
            app.MapPut("education/{id}", async (int id, EducationDto edto, EduService es) =>
            {
                var validationErrors = ValidateObjekts.ValidateObject(edto);
                if (validationErrors.Any())
                {
                    string errors = string.Join("||", validationErrors);
                    return Results.BadRequest(errors);
                }

                var result = await es.UpdateEducation(id, edto);

                if (result.IsSuccess)
                {
                    return Results.Ok(result.Data);
                }
                else
                {
                    if (result.Errors.Any(e => e.Contains("not found")))
                    {
                        return Results.NotFound(new { result.Errors });
                    }
                    return Results.BadRequest(new { result.Errors });
                }
            });
            app.MapDelete("education/{id}", async (int id, EduService es) =>
            {
                try
                {
                    bool deleted = await es.DeleteEducation(id);
                    return deleted ? Results.Ok($"Deleted work with id {id}") : Results.NotFound();
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Problem(ex.Message);
                }
            });
        }

    }
}
