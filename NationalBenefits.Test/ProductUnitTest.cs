using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using Xunit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Products.Api.Services.Interfaces;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;
using Products.Api.Contracts;




public class ProductsEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<IProductService> _mockProductService;

    public ProductsEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _mockProductService = new Mock<IProductService>();
    }

    [Fact]
    public async Task GetProducts_ReturnsOk()
    {
        // Arrange: Mock the Product Service
        var mockProducts = new List<Products.Api.Entities.Products>
        {
            new Products.Api.Entities.Products
            {
                Name="Play 4",
                Description="Playstation 4 pro",
                Ski="ski99928",
                Subcategory = Guid.NewGuid(),
                Id=Guid.NewGuid()
            },
            new Products.Api.Entities.Products
            {
                Name="Play 5",
                Description="Playstation 5 pro",
                Ski="ski22222",
                Subcategory = Guid.NewGuid(),
                Id=Guid.NewGuid()
            }
        };
        _mockProductService.Setup(service => service.GetProductsAsync(new CancellationToken(),1,10)).ReturnsAsync(mockProducts);

        // Act: Call the API
        var response = await _client.GetAsync("/products");

        // Assert: Check if response is OK (200)
        Assert.Equals(HttpStatusCode.OK, response.StatusCode);

        // Assert: Check if the response contains the expected data
        var products = await response.Content.ReadFromJsonAsync<List<Products.Api.Entities.Products>>();
        Assert.IsNotNull(products);
        Assert.Equals(2, products.Count);
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreated()
    {
        // Arrange: Create a new product
        var newProduct = new CreateProductRequest
        {
            Name = "Apple M3 tablet",
            Description = "New IPAD Apple Pro M3",
            Ski = "ski50001234",
            SubCategoryId = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };

        // Act: Call the API
        var response = await _client.PostAsJsonAsync("/products", newProduct);

        // Assert: Check if response is Created (201)
        Assert.Equals(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_InvalidData_ReturnsBadRequest()
    {
        // Arrange: Create an invalid product (missing name)
        var invalidProduct = new Products.Api.Entities.Products
        {
            Name = "SAMSUNG tablet A1",
            Description = "New SAMSUNG tablet ",
            Ski = "ski500023344",
            Subcategory = Guid.NewGuid(),
            Id = Guid.NewGuid()
        };

        // Act: Call the API
        var response = await _client.PostAsJsonAsync("/products", invalidProduct);

        // Assert: Check if response is BadRequest (400)
        Assert.Equals(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
