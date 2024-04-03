
using Microsoft.EntityFrameworkCore;
using mla.projectSchool.Models;

namespace mla.projectSchool.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>()
                .HasKey(s => s.DNI);

            modelBuilder.Entity<Course>()
                .HasKey(c => c.Title);

            modelBuilder.Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentDNI, sc.CourseTitle });
        }
        
        public DbSet<Student> Student { get; set; } = default!;
        public DbSet<Course> Course { get; set; } = default!;
        public DbSet<StudentCourse> StudentCourse { get; set; } = default!;
    }
}