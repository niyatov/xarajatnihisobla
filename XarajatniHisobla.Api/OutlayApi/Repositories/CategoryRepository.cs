using OutlayApi.Data;
using OutlayApi.Entities;

namespace OutlayApi.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}