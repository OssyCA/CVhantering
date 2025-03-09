using CVhantering.Dtos;
using CVhantering.Services;
using CVhantering.ValidateFolder;

namespace CVhantering.EndpointFolder
{
    public class EducationEndpoints
    {
        public static void EduEndpoint(WebApplication app)
        {
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
