using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Api.Tests
{
    public class TodoApiIntegrationTests
    {
        private WebApplicationFactory<Program> _factory = null!;

        [SetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        [Test]
        public async Task GetAll_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var res = await client.GetAsync("/api/todo");
            Assert.AreEqual(HttpStatusCode.OK, res.StatusCode);
        }
    }
}