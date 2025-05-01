using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Assigment2.Models
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>  
    {
        private readonly IConfiguration _configuration;

        public AppDbContext() : base()
        {

        }
        // Modified constructor
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Trainee> Trainees { get; set; }  // Use PascalCase
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<CourseResult> CourseResults { get; set; }
 


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = _configuration.GetConnectionString("DefaultConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

            

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add custom configurations
            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.Property(u => u.FullName).HasMaxLength(100);
                b.Property(u => u.Address).HasMaxLength(200);
                b.Property(u => u.ProfilePicture).HasMaxLength(500);
                b.Property(u => u.Gender).HasMaxLength(50);
                b.Property(u => u.DateOfBirth).HasColumnType("date");
            });
        }
    }
}
