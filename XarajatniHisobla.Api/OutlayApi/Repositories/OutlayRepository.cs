using OutlayApi.Data;
using OutlayApi.Entities;

namespace OutlayApi.Repositories;

public class OutlayRepository : GenericRepository<Outlay>, IOutlayRepository
{
    public OutlayRepository(AppDbContext context) : base(context)
    {
    }
}