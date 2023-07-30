using OutlayApi.Data;

namespace OutlayApi.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private ICategoryRepository? categoryRepository;
    public ICategoryRepository Categories
    {
        get
        {
            if (categoryRepository is null) categoryRepository = new CategoryRepository(_context);
            return categoryRepository;
        }
    }

    private IOutlayRepository? outlayRepository;
    public IOutlayRepository Outlays
    {
        get
        {
            if (outlayRepository is null) outlayRepository = new OutlayRepository(_context);
            return outlayRepository;
        }
    }

    private IDebtRepository? debtRepository;
    public IDebtRepository Debts
    {
        get
        {
            if (debtRepository is null) debtRepository = new DebtRepository(_context);
            return debtRepository;
        }
    }

    private IUserRepository? userRepository;
    public IUserRepository Users
    {
        get
        {
            if (userRepository is null) userRepository = new UserRepository(_context);
            return userRepository;
        }
    }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public async ValueTask SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}