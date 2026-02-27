using System.Net;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Todo.Api.Tests
{
    public class TodoApiIntegrationTests
    {
        private WebApplicationFactory<Program> _factory = null!;
        private JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [TearDown]
        public void TearDown()
        {
            _factory.Dispose();
        }

        [Test]
        public async Task GetAll_ReturnsOk()
        {
            var client = _factory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var res = await client.GetAsync("/api/todo");
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
        }

        private class TodoDto
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public bool IsComplete { get; set; }
        }

        [Test]
        public async Task Create_Then_GetById_ReturnsCreatedAndFetchedItem()
        {
            var client = _factory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var createPayload = new { title = "integration create", isComplete = false };
            var content = new StringContent(JsonSerializer.Serialize(createPayload, _jsonOptions), Encoding.UTF8, "application/json");

            var postRes = await client.PostAsync("/api/todo", content);
            Assert.AreEqual(HttpStatusCode.Created, postRes.StatusCode);

            var created = await postRes.Content.ReadFromJsonAsync<TodoDto>(_jsonOptions);
            Assert.IsNotNull(created);

            var getRes = await client.GetAsync($"/api/todo/{created!.Id}");
            Assert.AreEqual(HttpStatusCode.OK, getRes.StatusCode);

            var dto = await getRes.Content.ReadFromJsonAsync<TodoDto>(_jsonOptions);
            Assert.IsNotNull(dto);
            Assert.AreEqual("integration create", dto!.Title);
        }

        [Test]
        public async Task Update_ReturnsUpdatedItem()
        {
            var client = _factory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var createPayload = new { title = "to update", isComplete = false };
            var createContent = new StringContent(JsonSerializer.Serialize(createPayload, _jsonOptions), Encoding.UTF8, "application/json");
            var postRes = await client.PostAsync("/api/todo", createContent);
            Assert.AreEqual(HttpStatusCode.Created, postRes.StatusCode);

            var created = await postRes.Content.ReadFromJsonAsync<TodoDto>(_jsonOptions);
            Assert.IsNotNull(created);

            var updatePayload = new { id = created!.Id, title = "updated title", isComplete = true };
            var updateContent = new StringContent(JsonSerializer.Serialize(updatePayload, _jsonOptions), Encoding.UTF8, "application/json");

            var putRes = await client.PutAsync($"/api/todo/{created.Id}", updateContent);
            Assert.AreEqual(HttpStatusCode.OK, putRes.StatusCode);

            var updated = await putRes.Content.ReadFromJsonAsync<TodoDto>(_jsonOptions);
            Assert.IsNotNull(updated);
            Assert.AreEqual("updated title", updated!.Title);
        }

        [Test]
        public async Task Delete_RemovesItem_Then_GetByIdReturnsNotFound()
        {
            var client = _factory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(10);

            var createPayload = new { title = "to delete", isComplete = false };
            var createContent = new StringContent(JsonSerializer.Serialize(createPayload, _jsonOptions), Encoding.UTF8, "application/json");
            var postRes = await client.PostAsync("/api/todo", createContent);
            Assert.AreEqual(HttpStatusCode.Created, postRes.StatusCode);

            var created = await postRes.Content.ReadFromJsonAsync<TodoDto>(_jsonOptions);
            Assert.IsNotNull(created);

            var delRes = await client.DeleteAsync($"/api/todo/{created!.Id}");
            Assert.AreEqual(HttpStatusCode.NoContent, delRes.StatusCode);

            var getRes = await client.GetAsync($"/api/todo/{created.Id}");
            Assert.AreEqual(HttpStatusCode.NotFound, getRes.StatusCode);
        }
    }
}