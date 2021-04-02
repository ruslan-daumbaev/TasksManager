using Microsoft.EntityFrameworkCore;
using System;
using TasksManager.Data.Entities;

namespace TasksManager.Data.DataContext
{
    public class TasksDbContext : DbContext
    {
        public TasksDbContext(DbContextOptions<TasksDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoTask>().ToTable("tasks").HasKey(x => x.Id);
            modelBuilder.Entity<TodoTask>().Property(t => t.ChangeDate).HasDefaultValue(DateTime.Now);
        }
    }
}