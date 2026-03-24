using Microsoft.EntityFrameworkCore;
using TodoApp.Data.Models;

namespace TodoApp.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=TodoAppDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }
}