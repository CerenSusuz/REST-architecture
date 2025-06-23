using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net.Http.Json;
using System.Net;

namespace Test;

public class CategoriesIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public CategoriesIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetCategories_Should_Return_OK()
    {
        // Act
        var response = await _client.GetAsync("/api/categories");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostCategory_Should_Create_Category()
    {
        // Arrange
        var newCategory = new
        {
            name = "Integration Test Category",
            description = "Integration Test Description"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/categories", newCategory);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
