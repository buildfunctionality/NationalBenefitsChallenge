using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Products.Api.Contracts;
using Products.Api.Database;
using Products.Api.Entities;
using Products.Api.Services.Interfaces;
using System.Reflection.Metadata.Ecma335;

namespace Products.Api.Services
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
      
        public SubCategoryService(ISubCategoryRepository subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
        }

        public async Task<bool> DeleteSubCategoryAsync(Guid id, CancellationToken token)
        {
            var product = await _subCategoryRepository.DeleteSubCategory(id, token);

            return await Task.FromResult(product);


        }

        public async Task<SubCategory> GetSubCategorybyIdAsync(Guid id, CancellationToken token)
        {
            var product = await _subCategoryRepository.GetSubCategorybyIdAsync(id, token);

            return await Task.FromResult(product);
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoryAsync(CancellationToken ct, int page = 1, int pageSize = 10)
        {
            var products = await _subCategoryRepository.GetSubCategoryAsync(ct, page, pageSize);

            return products;
        }

        public async Task<bool> SaveSubCategoryAsync(CreateSubCategoryRequest request, CancellationToken token)
        {
            var product = await _subCategoryRepository.SaveSubCategory(request, token);

            return await Task.FromResult(product);
        }

        public async Task<bool> SaveSubCategoryBulkAsync(List<CreateSubCategoryRequest> request, CancellationToken token)
        {
            var product = await _subCategoryRepository.SaveSubCategoryBulk(request, token);

            return await Task.FromResult(product);
        }

        public async Task<bool> UpdateSubCategoryAsync(UpdateSubCategoryRequest request, CancellationToken token)
        {
            var product = await _subCategoryRepository.UpdateSubCategory(request, token);

            return await Task.FromResult(product);
        }
    }
}
