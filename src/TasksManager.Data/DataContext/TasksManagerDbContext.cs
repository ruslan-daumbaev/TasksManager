using System;
using Microsoft.EntityFrameworkCore;
using TasksManager.Data.Entities;

namespace TasksManager.Data.DataContext
{
    public class TasksManagerDbContext : DbContext
    {
        public TasksManagerDbContext(DbContextOptions<TasksManagerDbContext> options) : base(options)
        {
        }

        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>().Property(t => t.ChangeDate).HasDefaultValue(DateTime.Now);
        }
    }
}