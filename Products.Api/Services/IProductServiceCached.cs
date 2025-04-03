using Products.Api.Entities;

namespace Products.Api.Services
{
    public interface IProductServiceCached
    {
        Task<Entities.Products> GetProductbyIdAsync(Guid id, CancellationToken token);

        Task<IEnumerable<Entities.Products>> GetProductsAsync(CancellationToken ct,
            int page = 1,
            int pageSize = 10);


    }
}
