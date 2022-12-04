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
        public DbSet<UserExam> UserExams { get; set; }
    
    }
}
