using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.DBContexts;
using TodoList.Api.Models.dao;
using TodoList.Api.Models.dto;
using TodoList.Api.Services.Intefaces;
using TodoList.Api.Helpers;
using System;
using TodoList.Api.DataServices;

namespace TodoList.Api.Services
{
    public class TodoService : ITodoService
    {
        internal TodoHelper _todoHelper { get; }
        internal TodoDataService _todoDataService { get; }

        public TodoService(TodoDataService todoDataService, TodoHelper todoHelper)
        {
            _todoHelper = todoHelper;
            _todoDataService = todoDataService;
        }

        public async Task<List<TodoItem>> GetTodoItems()
        {
            List<TodoItem> results = await _todoDataService.GetTodoItems();
            return results;
        }

        public async Task<TodoItem> GetTodoItem(Guid id)
        {
            var result = await _todoDataService.GetTodoItem(id);
            return result;
        }

        public async Task<TodoResponse> PutTodoItem(Guid id, TodoItem todoItem)
        {
            TodoResponse todoResponse = new TodoResponse();
            if (id != todoItem.Id)
            {                
                todoResponse.Success = false;
                todoResponse.Description = "Ids do not match"; // Giive a valid user the reason why the bad request - or
                // Return non descriptive string if in order to avaoid hackers exploiting system
                todoResponse.Description = "Bad request";
                todoResponse.todoItem = null;
                return todoResponse;
            }else if (string.IsNullOrEmpty(todoItem?.Description))
            {
                todoResponse.Description = "Description is required";
                todoResponse.todoItem = null;
                return todoResponse;
            }

            todoResponse = await _todoDataService.PutTodoItem(id, todoItem);

            return todoResponse;
        }

        public async Task<TodoResponse> PostTodoItem(TodoItem todoItem)
        {
            TodoResponse todoResponse = new TodoResponse();
            todoResponse.Success = false;            

            if (string.IsNullOrEmpty(todoItem?.Description))
            {
                todoResponse.Description = "Description is required";
                todoResponse.todoItem = null;
                return todoResponse;
            } else if (_todoHelper.TodoItemDescriptionExists(todoItem.Description))
            {
                todoResponse.Description = "Description already exists";
                todoResponse.todoItem = null;
                return todoResponse;
            }
            
            todoItem.Created = DateTime.Now;
            todoItem.IsCompleted = false;

            todoResponse = await _todoDataService.PostTodoItem(todoItem);

            return todoResponse;
        }

        public async Task<TodoResponse> RemoveItem(Guid id)
        {
            TodoResponse todoResponse = new TodoResponse();
            todoResponse.Success = false;

            if (id == null)
            {
                todoResponse.Success = false;
                todoResponse.Description = "The ID is missing"; // Give a valid user the reason why the bad request - or
                // Return non descriptive string if in order to avaoid hackers exploiting system
                todoResponse.Description = "Bad request";
                todoResponse.todoItem = null;
                return todoResponse;
            }

            todoResponse = await _todoDataService.DeleteTodoItem(id);

            return todoResponse;
        }
    }
}
