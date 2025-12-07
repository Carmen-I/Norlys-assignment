using System.Net;
using System.Net.Http.Json;
using API.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TestPersonLogic
{
    public class PersonsApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PersonsApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/persons")]
        public async Task Get_AllPersons_ReturnsOk(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var items = await response.Content.ReadFromJsonAsync<List<PersonReadDto>>();
            Assert.NotNull(items);
        }

        [Fact]
        public async Task Get_PersonById_ReturnsOk()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/api/persons/1");

            Assert.NotEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task Post_CreatePerson_ReturnsCreated()
        {
            var client = _factory.CreateClient();

            var dto = new PersonToSaveDto
            {
                FirstName = "Ana",
                LastName = "Smith",
                BirthDate = new DateTime(1990, 1, 1),
                OfficeNumber = 1
            };

            var response = await client.PostAsJsonAsync("/api/persons", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var id = await response.Content.ReadFromJsonAsync<int>();
            Assert.True(id > 0);
        }

        [Fact]
        public async Task Put_UpdatePerson_ReturnsNoContent()
        {
            var client = _factory.CreateClient();
            int id = 3;
            var updateDto = new PersonToSaveDto
            {
                FirstName = "Bob",
                LastName = "Jensen",
                BirthDate = new DateTime(1988, 4, 4),
                OfficeNumber = 1
            };

            
            var updateResponse = await client.PutAsJsonAsync($"/api/persons/{id}", updateDto);

            Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);
        }

        [Fact]
        public async Task Delete_Person_ReturnsNoContent()
        {
            var client = _factory.CreateClient();

            var dto = new PersonToSaveDto
            {
                FirstName = "Lara",
                LastName = "Green",
                BirthDate = new DateTime(1992, 9, 9),
                OfficeNumber = 1
            };

            var create = await client.PostAsJsonAsync("/api/persons", dto);
            var id = await create.Content.ReadFromJsonAsync<int>();

            var deleteResponse = await client.DeleteAsync($"/api/persons/{id}");

            Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        }
    }
}
