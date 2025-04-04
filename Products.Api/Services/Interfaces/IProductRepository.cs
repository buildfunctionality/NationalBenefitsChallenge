﻿using Products.Api.Contracts;

namespace Products.Api.Services.Interfaces
{
    public interface IProductRepository
    {
        Task<Entities.Products> GetProductbyIdAsync(Guid id, CancellationToken token);

        Task<IEnumerable<Entities.Products>> GetProductsAsync(CancellationToken ct,
            int page = 1,
            int pageSize = 10, string name = "");

        Task<bool> SaveProduct(CreateProductRequest request, CancellationToken token);

        Task<bool> DeleteProduct(Guid id, CancellationToken token);

        Task<bool> UpdateProduct(UpdateProductRequest request, CancellationToken token);
    }
}
