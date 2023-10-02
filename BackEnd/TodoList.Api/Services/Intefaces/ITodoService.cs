using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Models.dao;
using TodoList.Api.Models.dto;

namespace TodoList.Api.Services.Intefaces
{
    public interface ITodoService
    {
        public Task<List<TodoItem>> GetTodoItems();
        public Task<TodoResponse> PostTodoItem(TodoItem todoItem);
        public Task<TodoItem> GetTodoItem(Guid id);
        public Task<TodoResponse> PutTodoItem(Guid id, TodoItem todoItem);
        public Task<TodoResponse> RemoveItem(Guid id);
    }
}
