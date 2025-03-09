
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

            // Dependency Injections for services
            builder.Services.AddScoped<PersonServices>(); 
            builder.Services.AddScoped<GithubService>();
            builder.Services.AddScoped<WorkService>();
            builder.Services.AddScoped<EduService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            // Endpoints for Person and Github
            PersonEndpoints.CVEndpoints(app);
            GithubEndpoint.GetGithubRepo(app); 
            

            app.Run();
        }
    }
}
