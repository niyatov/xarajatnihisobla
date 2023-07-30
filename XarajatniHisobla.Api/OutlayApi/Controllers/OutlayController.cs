using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutlayApi.Dtoes;
using OutlayApi.Filters;
using OutlayApi.Services;

namespace OutlayApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class OutlayController : ControllerBase
{
    private readonly IOutlayService _outlayService;

    public OutlayController(
        IOutlayService outlayService)
    {
        _outlayService = outlayService;
    }

    [HttpGet("GetOutlays")]
    public async Task<IActionResult> GetOutlays(int categoryId)
    {
        var entity = await _outlayService.GetAllAsync(categoryId, User);

        return Ok(entity?.Select(x => x.Adapt<Outlays>()).ToList());
    }

    [HttpPost]
    public async Task<IActionResult> CreateOutlay(CreateOutlay createOutlay)
    {
        await _outlayService.CreateAsync(createOutlay.Adapt<Models.CreateOutlay>(), User);

        return Ok();
    }


    [OutlayFilter("")]
    [HttpGet("GetOutlay/{outlayId}")]
    public async Task<IActionResult> GetOutlay(int outlayId)
    {
        var entity = await _outlayService.GetByIdAsync(outlayId, User);

        return Ok(entity?.Adapt<Outlay>());
    }


    [OutlayFilter("Owner")]
    [HttpPut("UpdateOutlay/{outlayId}")]
    public async Task<IActionResult> UpdateOutlay(int outlayId, UpdateOutlay outlay)
    {
        await _outlayService.UpdateAsync(outlayId, outlay.Adapt<Models.UpdateOutlay>(), User);
        return Ok();
    }


    [OutlayFilter("Owner")]
    [HttpDelete("DeleteOutlay/{outlayId}")]
    public async Task<IActionResult> DeleteOutlay(int outlayId)
    {
        await _outlayService.RemoveByIdAsync(outlayId, User);
        return Ok();
    }
}