﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.DBContexts;
using TodoList.Api.Models.dao;
using TodoList.Api.Models.dto;
using TodoList.Api.Services;
using TodoList.Api.Services.Intefaces;

namespace TodoList.Api.Controllers
{  
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoDBContext _context;
        private readonly ILogger<TodoItemsController> _logger;
        private readonly ITodoService _todoService;

        public TodoItemsController(TodoDBContext context, ILogger<TodoItemsController> logger, TodoService todoService)
        {
            _context = context;
            _logger = logger;
            _todoService = todoService;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var results = await _todoService.GetTodoItems();
            return Ok(results);
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            if (id.ToString() == "")
            {
                return BadRequest("Id is required"); ;
            }

            var result = await _todoService.GetTodoItem(id);

            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // PUT: api/TodoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            if (todoItem == null)
            {
                return BadRequest("Todo Object is required"); ;
            }

            if (id.ToString() == "")
            {
                return BadRequest("Id is required"); ;
            }

            TodoResponse response = await _todoService.PostTodoItem(todoItem);

            if (!response.Success)
            {
                return BadRequest(response.Description);
            }
            else
            {
                return Ok(response.todoItem);
            }
        } 

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem(TodoItem todoItem)
        {
            if (todoItem == null)
            {
                return BadRequest("Todo Object is required");
            }

            TodoResponse response = await _todoService.PostTodoItem(todoItem);

            if (!response.Success) {
                return BadRequest(response.Description);
            } else
            {
                return CreatedAtAction(nameof(GetTodoItems), response.todoItem.Id, response.todoItem);
            }
        }

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostNewTodoItem(string description)
        {
            if (string.IsNullOrEmpty(description))
            {
                return BadRequest("Description is required");
            }

            TodoItem todoItem = new TodoItem();
            todoItem.Description = description;

            TodoResponse response = await _todoService.PostTodoItem(todoItem);

            if (!response.Success)
            {
                return BadRequest(response.Description);
            }
            else
            {
                return CreatedAtAction(nameof(GetTodoItems), response.todoItem.Id, response.todoItem);
            }
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            var todoItem = _todoService.GetTodoItem(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            TodoResponse response = await _todoService.RemoveItem(id);

            if (!response.Success)
            {
                return BadRequest(response.Description);
            }
            else
            {
                return NoContent();
            }
        }
    }
}
