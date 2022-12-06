using ElectronicTestingSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace ElectronicTestingSystem.Data
{
    public class ElectronicTestingSystemDbContext : DbContext
    {
        public ElectronicTestingSystemDbContext(DbContextOptions<ElectronicTestingSystemDbContext> options) : base(options)
        {

        }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ExamRequest> ExamRequests { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ExamRequest>().HasMany(e => e.).WithOptional(s => s.Parent).WillCascadeOnDelete(true);
        //}

    }
}
