using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Products.Api.Contracts;
using Products.Api.Database;
using Products.Api.Entities;
using System.Reflection.Metadata.Ecma335;

namespace Products.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
      
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<bool> DeleteProductAsync(Guid id, CancellationToken token)
        {
            var product = await _productRepository.DeleteProduct(id, token);

            return await Task.FromResult(product);


        }

        public async Task<Entities.Products> GetProductbyIdAsync(Guid id, CancellationToken token)
        {
            var product = await _productRepository.GetProductbyIdAsync(id, token);

            return await Task.FromResult(product);
        }

        public async Task<IEnumerable<Entities.Products>> GetProductsAsync(CancellationToken ct, int page = 1, int pageSize = 10)
        {
            var products = await _productRepository.GetProductsAsync(ct, page, pageSize);

            return products;
        }

        public async Task<bool> SaveProductAsync(CreateProductRequest request, CancellationToken token)
        {
            var product = await _productRepository.SaveProduct(request, token);

            return await Task.FromResult(product);
        }

        public async Task<bool> UpdateProductAsync(UpdateProductRequest request, CancellationToken token)
        {
            var product = await _productRepository.UpdateProduct(request, token);

            return await Task.FromResult(product);
        }
    }
}
