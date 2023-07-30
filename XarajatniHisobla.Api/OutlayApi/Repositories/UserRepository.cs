using OutlayApi.Data;
using OutlayApi.Entities;

namespace OutlayApi.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }
}