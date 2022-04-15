using InventoryManager.API.Data;
using InventoryManager.API.Entities;
using InventoryManager.API.ViewModel;
using InventoryManager.API.Wrappers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.API.Endpoints
{
    public static class ProductEndPoints
    {
        public static WebApplication MapProductEndPoints(this WebApplication app)
        {
            app.MapGet("/api/product", GetProducts());

            app.MapGet("/api/product/search/{ query}", GetProductSearch());

            app.MapGet("/api/product/{id:int}", GetProductById());

            app.MapPost("/api/product/", AddProduct());

            app.MapPut("/api/product/{id}", UpdateProduct());

            app.MapDelete("/product/{id}", DeleteProduct());

            return app;

        }

        private static Func<int, int, InventoryDb, Task<IResult>> GetProducts()
        {
            return async (int pageNumber, int pageSize, InventoryDb db) =>
            {
                var validFilter = new PaginationFilter(pageNumber, pageSize);
                var response = await db.Products.OrderByDescending(o => o.Id)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .AsNoTracking()
                .ToListAsync();
                return Results.Ok(new PagedResponse<IEnumerable<Product>>(response, validFilter.PageNumber, validFilter.PageSize, response.Count / validFilter.PageSize, response.Count));

            };
        }

        private static Func<string, InventoryDb, Task<IResult>> GetProductSearch()
        {
            return async (string query, InventoryDb db) =>
            {
                var response = await db.Products.OrderByDescending(o => o.Id)
                .Where(w => w.Name.ToLower().Contains(query.ToLower()))
                .AsNoTracking()
                .ToListAsync();
                return Results.Ok(new Response<IEnumerable<Product>>(response));


            };
        }

        private static Func<int, InventoryDb, Task<IResult>> GetProductById()
        {
            return async (int id, InventoryDb db) =>
                           await db.Products.AsNoTracking()
                           .FirstOrDefaultAsync(x => x.Id == id) is Product product
                           ? Results.Ok(new Response<Product>(product))
                           : Results.NotFound();
        }

        private static Func<ProductViewModel, InventoryDb, HttpResponse, Task> AddProduct()
        {
            return async (ProductViewModel productViewModel, InventoryDb db, HttpResponse response) =>
            {
                var product = productViewModel.Adapt<Product>();
                db.Products.Add(product);
                await db.SaveChangesAsync();
                response.StatusCode = 201;
                response.Headers.Location = $"/api/product/{product.Id}";
            };
        }

        private static Func<int, ProductViewModel, InventoryDb, Task<IResult>> UpdateProduct()
        {
            return async (int id, ProductViewModel productViewModel, InventoryDb db) =>
            {
                var product = productViewModel.Adapt<Product>();

                if (id != product.Id)
                {
                    return Results.BadRequest();
                }

                if (!await db.Products.AnyAsync(x => x.Id == id))
                {
                    return Results.NotFound();
                }

                db.Update(product);
                await db.SaveChangesAsync();

                return Results.Ok();
            };
        }

        private static Func<int, InventoryDb, Task<IResult>> DeleteProduct()
        {
            return async (int id, InventoryDb db) =>
            {
                var product = await db.Products.FindAsync(id);
                if (product is null)
                {
                    return Results.NotFound();
                }

                db.Products.Remove(product);
                await db.SaveChangesAsync();

                return Results.Ok();
            };
        }
    }
}
