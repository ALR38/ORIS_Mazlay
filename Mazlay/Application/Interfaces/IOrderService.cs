using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IOrderService
{
    Task<int> CreateAsync(Guid userId, IReadOnlyList<CartLineDto> lines);
}