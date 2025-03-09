using Microsoft.EntityFrameworkCore;
using CVhantering.Models;

namespace CVhantering.Data
{
    public class HandleCvDbContext: DbContext
    {
        public HandleCvDbContext(DbContextOptions<HandleCvDbContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<WorkExperience> WorkExperiences { get; set; }
    }
}
