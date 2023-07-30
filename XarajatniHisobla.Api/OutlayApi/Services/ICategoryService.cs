using OutlayApi.Models;
using System.Security.Claims;

namespace OutlayApi.Services;

public interface ICategoryService
{
    ValueTask<Result<List<Categories>>> GetAllAsync(ClaimsPrincipal claims);
    ValueTask<Result<Category>> GetByIdAsync(int id, ClaimsPrincipal claims);
    ValueTask<Result<Category>> RemoveByIdAsync(int id, ClaimsPrincipal claims);
    ValueTask<Result<int>> CreateAsync(CreateCategory category, ClaimsPrincipal claims);
    ValueTask<Result<Category>> UpdateAsync(int id, UpdateCategory category, ClaimsPrincipal claims);
    ValueTask<Result<int>> JoinAsync(string name, string key, ClaimsPrincipal claims);
    ValueTask<Result<Statistics>> ShowStatisticsAsync(int id, ClaimsPrincipal claims);
    ValueTask<Result<Users>> ShowUsersAsync(int id, ClaimsPrincipal claims);
    ValueTask<Result<string>> DeleteUserAsync(int id, int userId, ClaimsPrincipal claims);
    ValueTask<Result> CleanAsync(int id, ClaimsPrincipal claims);

}
