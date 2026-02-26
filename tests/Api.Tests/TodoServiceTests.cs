using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using AutoMapper;
using Todo.Api.Services;
using Todo.Api.Repositories;
using Todo.Api.MappingProfiles;
using Todo.Api.Models;
using Todo.Api.Dtos;
using NUnit.Framework;

namespace Api.Tests
{
    public class TodoServiceTests
    {
        private TodoContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new TodoContext(options);
        }

        private IMapper CreateMapper()
        {
            var cfg = new MapperConfiguration(cfg => cfg.AddProfile<TodoMappingProfile>());
            return cfg.CreateMapper();
        }

        [Test]
        public async Task CreateAsync_PersistsItem()
        {
            using var ctx = CreateContext();
            var mapper = CreateMapper();
            var svc = new TodoService(ctx, mapper, new NullLogger<TodoService>());

            var dto = new TodoItemCreateDto { Title = "test", IsComplete = false };
            var created = await svc.CreateAsync(dto, CancellationToken.None);

            Assert.IsNotNull(created);
            Assert.AreEqual("test", created.Title);
            var saved = await ctx.TodoItems.FindAsync(created.Id);
            Assert.IsNotNull(saved);
        }
    }
}