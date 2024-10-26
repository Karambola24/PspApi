using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using PspApi.Model;

namespace PspApi
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Student> Student { get; set; }
        public DbSet<Grade> Grade { get; set; }
        public DbSet<Discipline> Discipline { get; set; }
        // public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка модели Student
            modelBuilder.Entity<Student>()
                .HasKey(s => s.ID_Student); // Указываем первичный ключ

            modelBuilder.Entity<Discipline>()
                .HasKey(s => s.ID_Discipline);

            modelBuilder.Entity<Grade>()
                .HasKey(s => s.ID_Grade);
        }
    }
}
