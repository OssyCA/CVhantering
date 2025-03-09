using CVhantering.Dtos;
using CVhantering.Services;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace CVhantering.EndpointFolder
{
    public class GithubEndpoint()
    {
        public static void GetGithubRepo(WebApplication app)
        {
            app.MapGet("/Github/{user}", async (GithubService gh, string? user) =>
            {
                try
                {
                    var results = await gh.GetGithubData(user);

                    if (results == null || results.Count == 0)
                    {
                        return Results.NotFound("No repos found for user.");
                    }
                    return Results.Ok(results);
                }
                catch (HttpRequestException ex)
                {
                    return Results.BadRequest($"Invalid request: {ex.Message}");
                }
            });
        }
    }
}
