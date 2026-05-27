using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Data;
using VitrineSemiJoias.Enums;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Repository.Interfaces;

namespace VitrineSemiJoias.Repository;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<ProductModel> AddProductAsync(ProductModel product)
    {
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return product;
    }

    public async Task DeleteProductAsync(int id)
    {
       await context.Products
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();
    }

    public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
    {
      return await context.Products
            .AsNoTracking() 
            .ToListAsync();
    }

    public async Task<IEnumerable<ProductModel>> GetProductByCategoryAsync(CategoryEnum category)
    {
       return await context.Products
            .AsNoTracking()
            .Where(p => p.CategoryEnum == category)
            .ToListAsync();
    }

    public async Task<ProductModel> GetProductByIdAsync(int id)
    {
        return await context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task UpdateProductAsync(ProductModel product)
    {
       context.Products.Update(product);
        await context.SaveChangesAsync();
    }
}

