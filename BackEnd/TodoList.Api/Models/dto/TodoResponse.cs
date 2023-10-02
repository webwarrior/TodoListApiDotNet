using TodoList.Api.Models.dao;

namespace TodoList.Api.Models.dto
{
    public class TodoResponse
    {
        public bool Success { get; set; }
        public string Description { get; set; }
        public TodoItem todoItem { get; set; }
    }
}
