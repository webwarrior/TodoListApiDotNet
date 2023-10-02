using Microsoft.EntityFrameworkCore;
using TodoList.Api.Models.dao;

namespace TodoList.Api.DBContexts
{
    public class TodoDBContext : DbContext
    {
        public TodoDBContext(DbContextOptions<TodoDBContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}
