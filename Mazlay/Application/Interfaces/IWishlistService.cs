using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces;

public interface IWishlistService
{
    Task<IList<int>> GetAsync(string userId);
    Task ToggleAsync(string userId, int productId);
}