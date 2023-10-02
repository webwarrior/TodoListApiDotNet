using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TodoList.Api.DataServices.Interfaces;
using TodoList.Api.DBContexts;
using TodoList.Api.Helpers;
using TodoList.Api.Models.dao;
using TodoList.Api.Models.dto;
using System.Linq;

namespace TodoList.Api.DataServices
{
    public class TodoDataService : ITodoDataService
    {
        internal TodoDBContext _context { get; }
        internal TodoHelper _todoHelper { get; }

        public TodoDataService(TodoDBContext dbContext)
        {
            _context = dbContext;
        }

        // Get List of items
        public async Task<List<TodoItem>> GetTodoItems()
        {
            List<TodoItem> results = await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
            return results;
        }

        //Get Item By Id
        public async Task<TodoItem> GetTodoItem(Guid id)
        {
            var result = await _context.TodoItems.FindAsync(id);
            return result;
        }

        // Update
        public async Task<TodoResponse> PutTodoItem(Guid id, TodoItem todoItem)
        {
            TodoResponse todoResponse = new TodoResponse();
            todoResponse.Success = false;

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                todoResponse.Success = true;
                todoResponse.todoItem = todoItem;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_todoHelper.TodoItemIdExists(id))
                {
                    todoResponse.Description = "Item does not exist";
                    todoResponse.todoItem = null;
                    return todoResponse;
                }
                else
                {
                    throw;
                }
            }
            return todoResponse;
        }

        // Add New
        public async Task<TodoResponse> PostTodoItem(TodoItem todoItem)
        {
            TodoResponse todoResponse = new TodoResponse();
            todoResponse.Success = false;

            todoItem.Created = DateTime.Now;
            _context.TodoItems.Add(todoItem);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (_todoHelper.TodoItemIdExists(todoItem.Id))
                {
                    todoResponse.Description = "Id already exists";
                    return todoResponse;
                }
                else
                {
                    throw;
                }
            }

            await _context.SaveChangesAsync();

            todoResponse.Success = true;
            todoResponse.todoItem = todoItem;

            return todoResponse;
        }

        // Delete
        public async Task<TodoResponse> DeleteTodoItem(Guid id)
        {
            TodoResponse todoResponse = new TodoResponse();
            todoResponse.Success = false; 
            
            var item = _context.TodoItems.FirstOrDefault(i => i.Id == id);
            if (item == null)
            {
                throw new Exception("Item not found");
            }

            _context.TodoItems.Remove(item);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                todoResponse.Description = "The Wheels came off - run!";
            }

            _context.SaveChanges();
            todoResponse.Success = true;
            todoResponse.todoItem = null;

            return todoResponse;
        }
    }
}
