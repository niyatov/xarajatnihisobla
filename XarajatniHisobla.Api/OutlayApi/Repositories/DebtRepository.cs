using OutlayApi.Data;
using OutlayApi.Entities;

namespace OutlayApi.Repositories;

public class DebtRepository : GenericRepository<Debt>, IDebtRepository
{
    public DebtRepository(AppDbContext context) : base(context)
    {
    }
}