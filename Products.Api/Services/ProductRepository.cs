
using Microsoft.EntityFrameworkCore;
using Polly.CircuitBreaker;
using Products.Api.Contracts;
using Products.Api.Database;
using Products.Api.Entities;
using Products.Api.Services.Interfaces;
using Products.Api.Utils;

namespace Products.Api.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext context;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public ProductRepository(ApplicationDbContext datacontext)
        {
           context = datacontext;
           _circuitBreakerPolicy = DatabaseCircuitBreakerPolicy.CreatePolicy();
        }

        public async Task<bool> DeleteProduct(Guid id, CancellationToken token)
        {
            var product = await context.Products
               .FirstOrDefaultAsync(p => p.Id == id, token);

            if (product == null) { 
                return false;
            }

            context.Products.Remove(product);

            await context.SaveChangesAsync(token);

            return await Task.FromResult(true);
        }

        public async Task<Entities.Products> GetProductbyIdAsync(Guid id, CancellationToken token)
        {
            var product = await context.Products
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == id, token);

            return await Task.FromResult(product);
        }

        public async Task<IEnumerable<Entities.Products>> GetProductsAsync(CancellationToken ct, int page = 1, int pageSize = 10)
        {
            var products = await context.Products
               .AsNoTracking()
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync(ct);

            return await Task.FromResult(products);
        }

        public async Task<bool> SaveProduct(CreateProductRequest request, CancellationToken token)
        {
          

            if (request is not null)
            {
                Products.Api.Entities.Products newProduct = new Entities.Products()
                {
                    Id = request.Id,
                    Name = request.Name,
                    Description = request.Description,
                    Updatedat = DateTime.Now,
                    Ski = request.Ski,
                    Subcategory = request.SubCategoryId
                };

                await context.Products.AddAsync(newProduct);
                await context.SaveChangesAsync(token);
            }

           
           
            return await Task.FromResult(true); 
        }

        public async Task<bool> UpdateProduct(UpdateProductRequest request, CancellationToken token)
        {
            var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id == request.Id, token);

            if (product is null)
            {
                return false;
            }

            product.Name = request.Name;
            product.Description = request.Description;
            product.Updatedat = DateTime.Now;
            product.Ski = request.Ski;
            product.Subcategory = request.SubCategory;


            await context.SaveChangesAsync(token);
            return await Task.FromResult(true); ;
        }
    }
}
