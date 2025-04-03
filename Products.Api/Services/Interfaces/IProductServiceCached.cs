using Products.Api.Entities;

namespace Products.Api.Services.Interfaces
{
    public interface IProductServiceCached
    {
        Task<Entities.Products> GetProductbyIdAsync(Guid id, CancellationToken token);

        Task<IEnumerable<Entities.Products>> GetProductsAsync(CancellationToken ct,
            int page = 1,
            int pageSize = 10);


    }
}
