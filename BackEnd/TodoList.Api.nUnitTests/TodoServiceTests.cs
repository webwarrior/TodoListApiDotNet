using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using TodoList.Api.DataServices;
using TodoList.Api.DBContexts;
using TodoList.Api.Helpers;
using TodoList.Api.Models.dao;
using TodoList.Api.Services;

namespace TodoList.Api.nUnitTests
{
    [TestFixture]
    public class TodoServiceTests
    {
        private TodoDBContext _dbContext;
        private TodoService _service;
        private TodoDataService _dataService;
        private TodoHelper _helper;

        [SetUp]
        public void Setup()
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

        [TearDown]
        public void Teardown()
        {
            _dbContext.Database.EnsureDeleted();
        }

        [Test]
        public void PostTodoItem_ShouldAddNewItem()
        {
            // arrange
            var item = new TodoItem
            {
                Description = "Test Item"
            };

            // act
            _service.PostTodoItem(item);
            var storedItem = _service.GetTodoItem(item.Id);

            // assert
            Assert.IsNotNull(storedItem);
            Assert.IsFalse(storedItem.Result.IsCompleted);
            Assert.That(storedItem.Result.Description, Is.EqualTo("Test Item"));
        }

        [Test]
        public void GetAllItems_ShouldReturnAllItems()
        {
            _ = _service.PostTodoItem(new TodoItem { Description = "Test Item 1" });
            _ = _service.PostTodoItem(new TodoItem { Description = "Test Item 2" });

            var items = _service.GetTodoItems();

            Assert.That(new List<TodoItem>(items.Result).Count, Is.EqualTo(2));
        }        

        [Test]
        public void ostTodoItem_ShouldNotAddDupicateItem()
        {
            _ = _service.PostTodoItem(new TodoItem { Description = "Test Item 1" });

            var newItem = _service.PostTodoItem(new TodoItem { Description = "Test Item 1" }).Result;
            bool failed = newItem.Success;
            string failedMessage = newItem.Description;
            var items = _service.GetTodoItems();

            Assert.That(new List<TodoItem>(items.Result).Count, Is.EqualTo(1));
            Assert.IsFalse(failed);
            Assert.That(newItem.Description, Is.EqualTo("Description already exists"));
        }

        [Test]
        public void ostTodoItem_ShouldNotAddBadItem()
        {
            var newItem = _service.PostTodoItem(new TodoItem { Description = "" }).Result;
            bool failed = newItem.Success;
            string failedMessage = newItem.Description;
            var items = _service.GetTodoItems();

            Assert.That(new List<TodoItem>(items.Result).Count, Is.EqualTo(0));
            Assert.IsFalse(failed);
            Assert.That(newItem.Description, Is.EqualTo("Description is required"));
        }

        [Test]
        public void PutTodoItem_ShouldUpdateItem()
        {
            var item = new TodoItem
            {
                Description = "Item to update"
            };

            var newItem = _service.PostTodoItem(item).Result.todoItem;
            newItem.IsCompleted = true;
            newItem.Description = "Item to update again";

            bool sucess = _service.PutTodoItem(newItem.Id, newItem).Result.Success;
            var storedItem = _service.GetTodoItem(newItem.Id);

            Assert.IsTrue(sucess);
            Assert.IsTrue(storedItem.Result.IsCompleted);
            Assert.That(storedItem.Result.Description, Is.EqualTo("Item to update again"));
        }

        [Test]
        public void ostTodoItem_ShouldNotUpdateBadItem()
        {
            var item = new TodoItem
            {
                Description = "Item to update"
            };

            var newItem = _service.PostTodoItem(item).Result.todoItem;
            newItem.IsCompleted = true;
            newItem.Description = "";

            var failedItem = _service.PutTodoItem(newItem.Id, newItem).Result;
            bool failed = failedItem.Success;
            string failedMessage = failedItem.Description;

            Assert.IsFalse(failed);
            Assert.That(failedMessage, Is.EqualTo("Description is required"));
        }
    }
}