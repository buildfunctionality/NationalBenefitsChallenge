using Products.Api.Contracts;
using Products.Api.Entities;

namespace Products.Api.Services
{
    public interface IProductService
    {
        Task<Entities.Products> GetProductbyIdAsync(Guid id, CancellationToken token);

        Task<IEnumerable<Entities.Products>> GetProductsAsync(CancellationToken ct,
            int page = 1,
            int pageSize = 10);

        Task<bool> SaveProductAsync(CreateProductRequest request, CancellationToken token);

        Task<bool> DeleteProductAsync(Guid id, CancellationToken token);

        Task<bool> UpdateProductAsync(UpdateProductRequest request, CancellationToken token);
    }
}
