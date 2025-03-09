
using CVhantering.Data;
using CVhantering.EndpointFolder;
using CVhantering.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CVhantering
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient();

            builder.Services.AddDbContext<HandleCvDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<PersonServices>(); //dependency injection of the PersonServices Create a new instance of the PersonServices class for each request
            builder.Services.AddScoped<GithubService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            PersonEndpoints.PersonGetEndpoint(app);
            GithubEndpoint.GetGithubRepo(app); 
            

            app.Run();
        }
    }
}
