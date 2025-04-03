using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Products.Api.Contracts;
using Products.Api.Database;
using Products.Api.Entities;
using Products.Api.Services;
using System;

namespace Products.Api.Endpoints;

public static class ProductsEndpoints
{
    
    public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("products", async (
            CreateProductRequest request,
            ApplicationDbContext context,
            CancellationToken ct) =>
        {
            var product = new Products.Api.Entities.Products
            {
                Name = request.Name,
                Description = request.Description,
                //Createdat = request.CreatedAt,
                //Updatedat = request.Updatedat,
                Id = request.Id,
                Ski = request.Ski,
                Subcategory = request.SubCategoryId,
              
            };

            context.Add(product);

            await context.SaveChangesAsync(ct);

            return Results.Ok(product);
        });

        app.MapGet("products", async (
       [FromServices] IProductServiceCached productService,
          [FromServices] ApplicationDbContext context,
          CancellationToken ct,
       int page = 1,
       int pageSize = 10
        ) =>
        {

          
            var products = await productService.GetProductsAsync(ct, page, pageSize);
            if (products == null)
            {
                return Results.Problem($"Product not found page {page} and pageSize {pageSize} on the database verify your connection or your database");
            }
            else return Results.Ok(products);


        });

        app.MapGet("products/{id}", async (

          IProductService cacheProductService,
          Guid id,
            ApplicationDbContext context,
            IDistributedCache cache,
        CancellationToken ct) =>
        {

            var product = await cacheProductService.GetProductbyIdAsync(id, ct);
            if (product == null)
            {
                return Results.NotFound($"Product  Id {id} is not found Id {id} on the database verify your connection or your database");
            }
            else return Results.Ok(product);
            //var product = await cache.GetAsync($"products-{id}",
            //    async token =>
            //    {
            //        var product = await context.Products
            //            .AsNoTracking()
            //            .FirstOrDefaultAsync(p => p.Id == id, token);

            //        return product;
            //    },
            //    CacheOptions.DefaultExpiration,
            //    ct);

            //return product is null ? Results.NotFound() : Results.Ok(product);
        });

        app.MapPut("products/{id}", async (
            [FromServices] IProductService productService,
            Guid id,
            CreateProductRequest request,
            ApplicationDbContext context,
            IDistributedCache cache,
            CancellationToken ct) =>
        {

            if (id != Guid.Empty || id != null)
            {
                var productCreated = productService.SaveProductAsync(request, ct);
                return await Task.FromResult(Results.Ok("Product created success"));
            }
            //else { 
            //    var product = await productService.GetProductbyIdAsync(id, ct);
            //    var productCreated = productService.UpdateProductAsync(request, ct);
            //    return Results.Ok(product);
            //}
            else {
                return Results.Problem("Not created review your request"); 
            };
          
        });

        app.MapPut("product/update/{id}", async (
            [FromServices] IProductService productService,
          [FromRoute]  Guid id,
           [FromBody] UpdateProductRequest request,
            ApplicationDbContext context,
            IDistributedCache cache,
            CancellationToken ct) =>
        {

            if (id != Guid.Empty || id != null)
            {
                var productUpdated = await productService.UpdateProductAsync(request, ct);
                return await Task.FromResult(Results.Ok("Product updated success"));
            }
            else
            {
                return Results.NotFound($"Not found id {id} ,review your Id ");
            }


        });

        app.MapDelete("products/{id}", async (
            Guid id,
            ApplicationDbContext context,
            IDistributedCache cache,
            CancellationToken ct) =>
        {
            var product = await context.Products
                .FirstOrDefaultAsync(p => p.Id == id, ct);

            if (product is null)
            {
                return Results.NotFound();
            }

            context.Remove(product);

            await context.SaveChangesAsync(ct);

            await cache.RemoveAsync($"products-{id}", ct);

            return await Task.FromResult(Results.Ok("Delete object success "));
        });
    }
}
