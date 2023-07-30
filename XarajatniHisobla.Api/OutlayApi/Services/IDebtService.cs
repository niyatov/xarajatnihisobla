using OutlayApi.Models;
using System.Security.Claims;

namespace OutlayApi.Services;

public interface IDebtService
{
    ValueTask<int> CreateAsync(CreateDebt debt, ClaimsPrincipal claims);
    ValueTask<List<Debts>> GetAllAsync(ClaimsPrincipal claims);
    ValueTask UpdateAsync(int id, UpdateDebt debt, ClaimsPrincipal claims);
    ValueTask UpdateDebtForDeleteAsync(int id, ClaimsPrincipal claims);
    ValueTask UpdateDebtByReveiverAsync(int id, UpdateDebtByReveiver debt, ClaimsPrincipal claims);
    ValueTask RemoveByIdAsync(int id, ClaimsPrincipal claims);
    ValueTask<List<DebtStatistics>> ShowStatisticsAsync(ClaimsPrincipal claims);
}
