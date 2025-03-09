using CVhantering.Dtos;
using CVhantering.Services;
using CVhantering.ValidateFolder;

namespace CVhantering.EndpointFolder
{
    public class WorkEndpoints
    {
        public static void WorkEndpoint(WebApplication app)
        {

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

        }
    }
}
