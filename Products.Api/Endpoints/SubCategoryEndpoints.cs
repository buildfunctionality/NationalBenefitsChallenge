using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Products.Api.Contracts;
using Products.Api.Database;
using Products.Api.Entities;
using Products.Api.Services;
using Products.Api.Services.Interfaces;
using System;

namespace Products.Api.Endpoints;

public static class SubCategoryEndpoints
{
    
    public static void MapSubCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("subcategory", async (
            [FromServices] ISubCategoryService subCategoryService,
            CreateSubCategoryRequest request,
            ApplicationDbContext context,
            CancellationToken ct) =>
        {
            if (request.Id != Guid.Empty || request.Id != null)
            {
                var productCreated = subCategoryService.SaveSubCategoryAsync(request, ct);
                return await Task.FromResult(Results.Ok("Subcategory created success"));
            }
          
            else
            {
                return Results.Problem("Not created review your request");
            };
        });

        app.MapPost("/subcategories/bulk", async (List<CreateBulkSubCategoryRequest> subcategories,
            [FromServices] ISubCategoryService subCategoryService,
            CancellationToken ct) =>
        {
          
            var productCreated = subCategoryService.SaveSubCategoryBulkAsync(subcategories, ct);

            return Results.Created($"/subcategories/bulk", subcategories.Count);
         
        });


        app.MapGet("subcategory", async (
       [FromServices] ISubCategoryServiceCached subcategoryService,
          [FromServices] ApplicationDbContext context,
          CancellationToken ct,
       int page = 1,
       int pageSize = 10,
       string code = ""
        ) =>
        {

          
            var products = await subcategoryService.GetSubCategoriesAsync(ct, page, pageSize,code);
            if (products == null)
            {
                return Results.Problem($"Subcategory not found page {page} and pageSize {pageSize} on the database verify your connection or your database");
            }
            else return Results.Ok(products);


        });

        app.MapGet("subcategories/{id}", async (

          [FromServices] ISubCategoryServiceCached cacheSubCategoryService,
          Guid id,
            ApplicationDbContext context,
            IDistributedCache cache,
        CancellationToken ct) =>
        {

            var product = await cacheSubCategoryService.GetSubCategorybyIdAsync(id, ct);
            if (product == null)
            {
                return Results.NotFound($"SubCategory Id {id} is not found Id {id} on the database verify your connection or your database");
            }
            else return Results.Ok(product);
          
        });

        app.MapPut("subcategories/{id}", async (
            [FromServices] ISubCategoryService subCategoryService,
            Guid id,
            CreateSubCategoryRequest request,
            ApplicationDbContext context,
            IDistributedCache cache,
            CancellationToken ct) =>
        {

            if (id != Guid.Empty || id != null)
            {
                var productCreated = subCategoryService.SaveSubCategoryAsync(request, ct);
                return await Task.FromResult(Results.Ok("Subcategory created success"));
            }
          
            else {
                return Results.Problem("Not created review your request"); 
            };
          
        });

        app.MapPut("subcategory/update/{id}", async (
          [FromServices] ISubCategoryService subCategoryService,
          [FromRoute]  Guid id,
           [FromBody] UpdateSubCategoryRequest request,
            ApplicationDbContext context,
            IDistributedCache cache,
            CancellationToken ct) =>
        {

            if (id != Guid.Empty || id != null)
            {
                var productUpdated = await subCategoryService.UpdateSubCategoryAsync(request, ct);
                return await Task.FromResult(Results.Ok("Product updated success"));
            }
            else
            {
                return Results.NotFound($"Not found id {id} ,review your Id ");
            }


        });

        app.MapDelete("subcategories/{id}", async (
            [FromServices] ISubCategoryService subCategoryService,
            Guid id,
            ApplicationDbContext context,
            IDistributedCache cache,
            CancellationToken ct) =>
        {

            if (id != Guid.Empty || id != null)
            {
                var productDeleted = await subCategoryService.DeleteSubCategoryAsync(id, ct);
                return await Task.FromResult(Results.Ok("SubCategory deleted success"));
            }
            else
            {
                return Results.NotFound($"Not found id {id} ,review your Id ");
            }
          
        });
    }
}
