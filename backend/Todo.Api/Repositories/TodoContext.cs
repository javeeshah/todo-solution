using Microsoft.EntityFrameworkCore;
using Todo.Api.Models;

namespace Todo.Api.Repositories
{
    public class TodoContext:DbContext
    {
        /// <summary>
        /// TodoContext constructor to initialize the DbContext with options
        /// </summary>
        /// <param name="options"></param>
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        /// <summary>
        /// TodoItems DbSet to represent the collection of TodoItem entities in the database
        /// </summary>
        public DbSet<TodoItem> TodoItems { get; set; } = null!;

        /// <summary>
        /// Override the OnModelCreating method to configure the model and its relationships using Fluent API
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the TodoItem entity
            modelBuilder.Entity<TodoItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.IsCompleted).IsRequired();
            });
        }
    }
}
