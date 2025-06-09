using Products.Api.Contracts;
using Products.Api.Entities;

namespace Products.Api.Services.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<SubCategory> GetSubCategorybyIdAsync(Guid id, CancellationToken token);

        Task<IEnumerable<SubCategory>> GetSubCategoryAsync(CancellationToken ct,
            int page = 1,
            int pageSize = 10,
            string code="");

        Task<bool> SaveSubCategory(CreateSubCategoryRequest request, CancellationToken token);

        Task<bool> DeleteSubCategory(Guid id, CancellationToken token);

        Task<bool> UpdateSubCategory(UpdateSubCategoryRequest request, CancellationToken token);

        Task<bool> SaveSubCategoryBulk(List<CreateBulkSubCategoryRequest> request, CancellationToken token);
    }
}
