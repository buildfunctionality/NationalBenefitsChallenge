using Products.Api.Entities;

namespace Products.Api.Services
{
    public interface ISubCategoryServiceCached
    {
        Task<Entities.Products> GetProductbyIdAsync(Guid id, CancellationToken token);

        Task<IEnumerable<Entities.Products>> GetProductsAsync(CancellationToken ct,
            int page = 1,
            int pageSize = 10);


    }
}
