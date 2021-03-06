using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Todos.IntegrationTest
{
    public class TodosTests : IClassFixture<TestFixture>
    {
        private const string ApiUrl = "/api/todos";
        private readonly HttpClient _client;

        public TodosTests(TestFixture fixture)
        {
            _client = fixture.Client;
        }

        [Fact]
        public async Task Create_Todo_Returns_NoContent()
        {
            // Arrange
            var todo = new Todo
            {
                Name = "Todo 1",
                IsComplete = false
            };
            var content = new StringContent(JsonSerializer.Serialize(todo), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync(ApiUrl, content);

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task Get_Todo_Returns_Ok()
        {
            // Arrange
            await Create_Todo_Returns_NoContent();

            // Act
            var response = await _client.GetAsync($"{ApiUrl}/1");
            var body = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Get_All_Todos_Returns_Ok()
        {
            // Arrange
            await Create_Todo_Returns_NoContent();
            await Create_Todo_Returns_NoContent();

            // Act
            var response = await _client.GetAsync(ApiUrl);
            var body = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Delete_Todo_Returns_NoContent()
        {
            // Arrange
            await Create_Todo_Returns_NoContent();

            // Act
            var response = await _client.DeleteAsync($"{ApiUrl}/1");

            // Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}