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





        }

    }
}
