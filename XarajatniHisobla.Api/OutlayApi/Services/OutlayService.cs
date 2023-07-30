using OutlayApi.Models;
using OutlayApi.Repositories;
using System.Security.Claims;

namespace OutlayApi.Services;

public partial class OutlayService : IOutlayService
{
    private readonly IUnitOfWork _unitOfWork;

    public OutlayService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async ValueTask CreateAsync(CreateOutlay outlay, ClaimsPrincipal claims)
    {
        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
        var userName = claims.FindFirst(ClaimTypes.Name).Value;

        var entity = ToEntityCreateOutlay(outlay, userId);
        await _unitOfWork.Outlays.AddAsync(entity);
    }

    public async ValueTask<List<Outlays>> GetAllAsync(int categoryId, ClaimsPrincipal claims)
    {
        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

        var entity = _unitOfWork.Outlays.GetAll().Where(x => x.CategoryId == categoryId).ToList();

        var result = ToModelOutlays(entity);

        return result;
    }

    public async ValueTask<Outlay?> GetByIdAsync(int id, ClaimsPrincipal claims)
    {
        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
        var userName = claims.FindFirst(ClaimTypes.Name).Value;

        var entity = await _unitOfWork.Outlays.GetByIdAsync(id);

        if (entity is null)
            return null;

        var result = ToModelOutlay(entity, userId, userName);

        return result;
    }

    public async ValueTask RemoveByIdAsync(int id, ClaimsPrincipal claims)
    {
        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
        var userName = claims.FindFirst(ClaimTypes.Name).Value;

        var entity = await _unitOfWork.Outlays.GetByIdAsync(id);

        var entry = await _unitOfWork.Outlays.Remove(entity);
    }

    public async ValueTask UpdateAsync(int id, UpdateOutlay outlay, ClaimsPrincipal claims)
    {
        var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
        var userName = claims.FindFirst(ClaimTypes.Name).Value;

        var entity = await _unitOfWork.Outlays.GetByIdAsync(id);

        entity.Name = outlay.Name;
        entity.Description = outlay.Description;
        entity.Cost = outlay.Cost;
        entity.LastUpdateAt = DateTime.Now;

        await _unitOfWork.Outlays.Update(entity);
    }

}
