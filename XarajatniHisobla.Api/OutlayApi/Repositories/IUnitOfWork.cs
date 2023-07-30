namespace OutlayApi.Repositories;

public interface IUnitOfWork : IDisposable
{
    ICategoryRepository Categories { get; }
    IOutlayRepository Outlays { get; }
    IDebtRepository Debts { get; }
    IUserRepository Users { get; }
}