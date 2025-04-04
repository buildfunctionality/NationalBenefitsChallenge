using Products.Api.Entities;

namespace Products.Api.Services.Interfaces
{
    public interface ISubCategoryServiceCached
    {
        Task<SubCategory> GetSubCategorybyIdAsync(Guid id, CancellationToken token);

        Task<IEnumerable<SubCategory>> GetSubCategoriesAsync(CancellationToken ct,
            int page = 1,
            int pageSize = 10,
            string code = "");


    }
}
