using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using TodoList.Api.DataServices;
using TodoList.Api.DBContexts;
using TodoList.Api.Helpers;
using TodoList.Api.Models.dao;
using TodoList.Api.Services;
using Xunit;

namespace TodoList.Api.UnitTests
{
    public class TodoItemServiceTests
    {
        private TodoDBContext _dbContext;
        private TodoService _service;
        private TodoDataService _dataService;
        private TodoHelper _helper;

        public TodoItemServiceTests()
        {
            // Setup an in-memory database. Database is fresh for every test.
            var options = new DbContextOptionsBuilder<TodoDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique name for each test run
                .Options;

            _dbContext = new TodoDBContext(options);
            _dataService = new TodoDataService(_dbContext);
            _helper = new TodoHelper(_dbContext);
            _service = new TodoService(_dataService, _helper);
        }

        [Fact]
        public void AddItem_ShouldAddNewItem()
        {
            // arrange
            var item = new TodoItem
            {
                Description = "Test Item"
            };

            // act
            _ = _service.PostTodoItem(item);

            // assert
            var storedItem = _service.GetTodoItem(item.Id);
            Assert.NotNull(storedItem);
            Assert.Equal("Test Item", storedItem.Result.Description);
        }

        [Fact]
        public void GetAllItems_ShouldReturnAllItems()
        {
            _ = _service.PostTodoItem(new TodoItem { Description = "Test Item 1" });
            _ = _service.PostTodoItem(new TodoItem { Description = "Test Item 2" });

            var items = _service.GetTodoItems();

            Assert.Equal(2, items.Result.Count());
        }

        // Add more tests for Bad inserts and Updates... 
    }
}
