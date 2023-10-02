using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TodoList.Api.DBContexts;

namespace TodoList.Api.Helpers
{
    public class TodoHelper
    {
        public TodoDBContext _context { get; }
        public TodoHelper(TodoDBContext dbContext)
        {
            _context = dbContext;
        }

        public bool TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }

        public bool TodoItemDescriptionExists(string description)
        {
            return _context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }
    }
}
