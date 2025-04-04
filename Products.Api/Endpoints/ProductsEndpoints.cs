using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Products.Api.Contracts;
using Products.Api.Database;
using Products.Api.Entities;
using Products.Api.Services.Interfaces;
using System;

namespace Products.Api.Endpoints;

public static class ProductsEndpoints
{
    
    public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("products", async (
            [FromServices] IProductService productService,
            CreateProductRequest request,
            ApplicationDbContext context,
            CancellationToken ct) =>
        {
            if (request.Id != Guid.Empty || request.Id != null)
            {
                var productCreated = productService.SaveProductAsync(request, ct);
                return await Task.FromResult(Results.Ok("Product created success"));
            }
         
            else
            {
                return Results.Problem("Not created review your request");
            };

        });

        app.MapGet("products", async (
       [FromServices] IProductServiceCached productService,
          [FromServices] ApplicationDbContext context,
          CancellationToken ct,
          string name = "",
          int page = 1,
          int pageSize = 10
        ) =>
        {
          
            var products = await productService.GetProductsAsync(ct, page, pageSize,name);
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
            [FromServices] IProductService productService,
            Guid id,
            ApplicationDbContext context,
            IDistributedCache cache,
            CancellationToken ct) =>
        {

            if (id != Guid.Empty || id != null)
            {
                var productDeleted = await productService.DeleteProductAsync(id, ct);
                return await Task.FromResult(Results.Ok("Product deleted success"));
            }
            else
            {
                return Results.NotFound($"Not found id {id} ,review your Id ");
            }


            //var product = await context.Products
            //    .FirstOrDefaultAsync(p => p.Id == id, ct);

            //if (product is null)
            //{
            //    return Results.NotFound();
            //}

            //context.Remove(product);

            //await context.SaveChangesAsync(ct);

            //await cache.RemoveAsync($"products-{id}", ct);

            //return await Task.FromResult(Results.Ok("Delete object success "));
        });
    }
}
