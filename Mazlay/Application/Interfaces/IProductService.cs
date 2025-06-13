using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

/// <summary>
/// Контракт всех операций с товарами.
/// </summary>
public interface IProductService
{
    Task<IReadOnlyList<Product>> GetLatestAsync(int count = 8);
    Task<Product?>               GetByIdAsync(int id);
    Task<(IReadOnlyList<Product> Items, int Total)>
        SearchAsync(int page, int pageSize, int? categoryId, string? query);
    
    /// <summary>Id товара с меньшим Id (предыдущий) или null.</summary>
    Task<int?> GetPrevIdAsync(int currentId);

    /// <summary>Id товара с большим Id (следующий) или null.</summary>
    Task<int?> GetNextIdAsync(int currentId);
}