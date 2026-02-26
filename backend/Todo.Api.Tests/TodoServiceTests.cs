using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using AutoMapper;
using Todo.Api.Services;
using Todo.Api.Repositories;
using Todo.Api.MappingProfiles;
using Todo.Api.Models;
using Todo.Api.Dtos;
using NUnit.Framework;

namespace Todo.Api.Tests
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

        private TodoService CreateService(TodoContext ctx)
        {
            return new TodoService(ctx, CreateMapper(), new NullLogger<TodoService>());
        }

        [Test]
        public async Task CreateAsync_PersistsItem()
        {
            using var ctx = CreateContext();
            var svc = CreateService(ctx);

            var dto = new TodoItemCreateDto { Title = "create-test", IsCompleted = false };
            var created = await svc.CreateAsync(dto, CancellationToken.None);

            Assert.IsNotNull(created);
            Assert.AreEqual("create-test", created.Title);
            var saved = await ctx.TodoItems.FindAsync(new object[] { created.Id }, CancellationToken.None);
            Assert.IsNotNull(saved);
        }

        [Test]
        public async Task GetAllAsync_ReturnsItems()
        {
            using var ctx = CreateContext();
            var svc = CreateService(ctx);

            await svc.CreateAsync(new TodoItemCreateDto { Title = "one" }, CancellationToken.None);
            await svc.CreateAsync(new TodoItemCreateDto { Title = "two" }, CancellationToken.None);

            var all = await svc.GetAllAsync(CancellationToken.None);

            Assert.IsNotNull(all);
            var list = all.ToList();
            Assert.GreaterOrEqual(list.Count, 2);
            CollectionAssert.IsSupersetOf(list.Select(x => x.Title).ToList(), new[] { "one", "two" });
        }

        [Test]
        public async Task GetByIdAsync_ReturnsItem()
        {
            using var ctx = CreateContext();
            var svc = CreateService(ctx);

            var created = await svc.CreateAsync(new TodoItemCreateDto { Title = "byid" }, CancellationToken.None);
            var fetched = await svc.GetByIdAsync(created.Id, CancellationToken.None);

            Assert.IsNotNull(fetched);
            Assert.AreEqual(created.Id, fetched!.Id);
            Assert.AreEqual("byid", fetched.Title);
        }

        [Test]
        public async Task UpdateAsync_UpdatesItem()
        {
            using var ctx = CreateContext();
            var svc = CreateService(ctx);

            var created = await svc.CreateAsync(new TodoItemCreateDto { Title = "before" }, CancellationToken.None);

            var updateDto = new TodoItemUpdateDto
            {
                Id = created.Id,
                Title = "after",
                IsCompleted = true
            };

            var updated = await svc.UpdateAsync(updateDto, CancellationToken.None);

            Assert.IsNotNull(updated);
            Assert.AreEqual("after", updated!.Title);
            Assert.IsTrue(updated.IsCompleted);

            var persisted = await ctx.TodoItems.FindAsync(new object[] { created.Id }, CancellationToken.None);
            Assert.IsNotNull(persisted);
            Assert.AreEqual("after", persisted!.Title);
        }

        [Test]
        public async Task UpdateAsync_NonExisting_ReturnsNull()
        {
            using var ctx = CreateContext();
            var svc = CreateService(ctx);

            var updateDto = new TodoItemUpdateDto
            {
                Id = 9999,
                Title = "nope",
                IsCompleted = false
            };

            var result = await svc.UpdateAsync(updateDto, CancellationToken.None);
            Assert.IsNull(result);
        }

        [Test]
        public async Task DeleteAsync_RemovesItem()
        {
            using var ctx = CreateContext();
            var svc = CreateService(ctx);

            var created = await svc.CreateAsync(new TodoItemCreateDto { Title = "todelete" }, CancellationToken.None);

            var deleted = await svc.DeleteAsync(created.Id, CancellationToken.None);
            Assert.IsTrue(deleted);

            var fetched = await svc.GetByIdAsync(created.Id, CancellationToken.None);
            Assert.IsNull(fetched);
        }

        [Test]
        public async Task DeleteAsync_NonExisting_ReturnsFalse()
        {
            using var ctx = CreateContext();
            var svc = CreateService(ctx);

            var deleted = await svc.DeleteAsync(9999, CancellationToken.None);
            Assert.IsFalse(deleted);
        }
    }
}