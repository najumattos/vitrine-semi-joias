using Microsoft.EntityFrameworkCore;
using VitrineSemiJoias.Common;
using VitrineSemiJoias.Data;
using VitrineSemiJoias.Models;
using VitrineSemiJoias.Repository.Interfaces;

namespace VitrineSemiJoias.Repository;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<Result<ProductModel>> AddProductAsync(ProductModel product)
    {
        try
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            return Result<ProductModel>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<ProductModel>.Failure($"Erro ao adicionar produto: {ex.Message}");
        }
    }

    public async Task<Result> DeleteProductAsync(int id)
    {
        try
        {
            int rowsAffected = await context.Products
             .Where(p => p.Id == id)
             .ExecuteDeleteAsync();

            if (rowsAffected == 0)
                return Result.Failure("Nenhum produto foi encontrado com este ID.");

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro ao excluir produto: {ex.Message}");
        }
    }

    public async Task<Result<IEnumerable<ProductModel>>> GetAllProductsAsync()
    {
        try
        {
            var products = await context.Products
                .AsNoTracking() 
                .ToListAsync();

            return Result<IEnumerable<ProductModel>>.Success(products);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProductModel>>.Failure($"Erro ao listar produtos: {ex.Message}");
        }
    }

    public async Task<Result<ProductModel>> GetProductByIdAsync(int id)
    {
        try
        {
            var product = await context.Products
             .AsNoTracking()
             .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
                return Result<ProductModel>.Failure("Produto não encontrado.");
            return Result<ProductModel>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<ProductModel>.Failure($"Erro ao buscar produto: {ex.Message}");
        }
    }

    public async Task<Result> UpdateProductAsync(ProductModel product)
    {
        try
        {
            context.Products.Update(product);
            await context.SaveChangesAsync();

            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Erro ao atualizar produto: {ex.Message}");
        }
    }
}

//Property(x => x.Preco).IsModified = true.