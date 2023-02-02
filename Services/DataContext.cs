using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VidFluentAI.Models;

namespace VidFluentAI.Services
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Job> Jobs { get; set; }
        public DbSet<DemoJob> DemoJobs { get; set; }
    }
}
