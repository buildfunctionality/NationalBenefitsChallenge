using Products.Api.Contracts;
using Products.Api.Entities;

namespace Products.Api.Services.Interfaces
{
    public interface ISubCategoryService
    {
        Task<SubCategory> GetSubCategorybyIdAsync(Guid id, CancellationToken token);

        Task<IEnumerable<SubCategory>> GetSubCategoryAsync(CancellationToken ct,
            int page = 1,
            int pageSize = 10);

        Task<bool> SaveSubCategoryAsync(CreateSubCategoryRequest request, CancellationToken token);

        Task<bool> DeleteSubCategoryAsync(Guid id, CancellationToken token);

        Task<bool> UpdateSubCategoryAsync(UpdateSubCategoryRequest request, CancellationToken token);
    }
}
