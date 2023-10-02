using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Models.dao;
using TodoList.Api.Models.dto;

namespace TodoList.Api.DataServices.Interfaces
{
    public interface ITodoDataService
    {
        public Task<List<TodoItem>> GetTodoItems();
        public Task<TodoItem> GetTodoItem(Guid id);
        public Task<TodoResponse> PutTodoItem(Guid id, TodoItem todoItem);
        public Task<TodoResponse> PostTodoItem(TodoItem todoItem);
        public Task<TodoResponse> DeleteTodoItem(Guid id);
    }
}
