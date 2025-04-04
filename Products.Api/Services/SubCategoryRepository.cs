
using Microsoft.EntityFrameworkCore;
using Products.Api.Contracts;
using Products.Api.Database;
using Products.Api.Entities;
using Products.Api.Services.Interfaces;
using System.Xml.Linq;

namespace Products.Api.Services
{
    public class SubCategoryRepository : ISubCategoryRepository
    {
        private readonly ApplicationDbContext context;
        public SubCategoryRepository(ApplicationDbContext datacontext)
        {
           context = datacontext;
        }

        public async Task<bool> DeleteSubCategory(Guid id, CancellationToken token)
        {
            var subCategory = await context.SubCategories
               .FirstOrDefaultAsync(p => p.Id == id, token);

            if (subCategory == null) { 
                return false;
            }

            context.SubCategories.Remove(subCategory);

            await context.SaveChangesAsync(token);

            return await Task.FromResult(true);
        }

        public async Task<SubCategory> GetSubCategorybyIdAsync(Guid id, CancellationToken token)
        {
            var subCategory = await context.SubCategories
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p => p.Id == id, token);

            return await Task.FromResult(subCategory);
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategoryAsync(CancellationToken ct, int page = 1, int pageSize = 10,string code="")
        {
            var subCategories = await context.SubCategories
            .AsNoTracking()
            .Where(c => c.Code.StartsWith(code))
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

            return await Task.FromResult(subCategories);
        }

        public async Task<bool> SaveSubCategory(CreateSubCategoryRequest request, CancellationToken token)
        {
          
            if (request is not null)
            {
               SubCategory newSubCategory = new SubCategory()
                {
                   Id = request.Id,
                   Code = request.Code,
                   Description = request.Description,
                   Updated_at = DateTime.UtcNow,
                   CategoryId = request.CategoryId,
                   Created_at = DateTime.UtcNow,
                };

                await context.SubCategories.AddAsync(newSubCategory);
                await context.SaveChangesAsync(token);
            }

           
           
            return await Task.FromResult(true); 
        }

        public async Task<bool> UpdateSubCategory(UpdateSubCategoryRequest request, CancellationToken token)
        {
            var product = await context.SubCategories
            .FirstOrDefaultAsync(p => p.Id == request.Id, token);

            if (product is null)
            {
                return false;
            }

            product.Code = request.Code;
            product.Description = request.Description;
            product.Updated_at = DateTime.Now;
            product.CategoryId = request.CategoryId;
            product.Created_at = DateTime.Now;

            await context.SaveChangesAsync(token);
            return await Task.FromResult(true);
        }
    }
}
