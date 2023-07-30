using OutlayApi.Models;
using System.Security.Claims;

namespace OutlayApi.Services;

public interface IOutlayService
{
    ValueTask<List<Outlays>> GetAllAsync(int categoryId, ClaimsPrincipal claims);
    ValueTask<Outlay?> GetByIdAsync(int id, ClaimsPrincipal claims);
    ValueTask RemoveByIdAsync(int id, ClaimsPrincipal claims);
    ValueTask CreateAsync(CreateOutlay outlay, ClaimsPrincipal claims);
    ValueTask UpdateAsync(int id, UpdateOutlay outlay, ClaimsPrincipal claims);
}
