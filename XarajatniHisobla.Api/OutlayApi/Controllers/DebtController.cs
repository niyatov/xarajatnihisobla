using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutlayApi.Dtoes;
using OutlayApi.Services;

namespace OutlayApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class DebtController : ControllerBase
{
    private readonly IDebtService _debtService;

    public DebtController(
        IDebtService debtService)
    {
        _debtService = debtService;
    }


    [HttpGet("GetDebts")]
    public async Task<IActionResult> GetDebts()
    {
        var entity = await _debtService.GetAllAsync(User);

        return Ok(entity?.Select(x => x.Adapt<Debts>()).ToList());
    }


    [HttpPost("CreateDebt")]
    public async Task<IActionResult> CreateDebt(CreateDebt createDebtDto)
    {
        var debtId = await _debtService.CreateAsync(createDebtDto.Adapt<Models.CreateDebt>(), User);

        return Ok(debtId);
    }


    // for createed and rejected to update , if it is rejected this cen change it and change its type to creted

    [HttpPut("UpdateDebt/{debtId}")]
    public async Task<IActionResult> UpdateDebt(int debtId, UpdateDebt updateDebtDto)
    {
        if (!ModelState.IsValid) return BadRequest();

        await _debtService.UpdateAsync(debtId, updateDebtDto.Adapt<Models.UpdateDebt>(), User);

        return Ok();
    }



    // if it is accepted, you can send it to delete or undelete , but also another person need to delete it from delete method

    [HttpPut("UpdateDebtForDelete/{debtId}")]
    public async Task<IActionResult> UpdateDebtForDelete(int debtId)
    {
        await _debtService.UpdateDebtForDeleteAsync(debtId, User);

        return Ok();
    }


    // createni by recever to accepte or rejecte debt

    [HttpPut("UpdateDebtByReveiver/{debtId}")]
    public async Task<IActionResult> UpdateDebtByReveiver(int debtId, UpdateDebtByReveiver updateDebtByReveiverDto)
    {
        await _debtService.UpdateDebtByReveiverAsync(debtId, updateDebtByReveiverDto.Adapt<Models.UpdateDebtByReveiver>(), User);

        return Ok();
    }


    // if accepted to delete, if  created or rejectedni to  delete

    [HttpDelete("DeleteDebt/{debtId}")]
    public async Task<IActionResult> DeleteDebt(int debtId)
    {
        await _debtService.RemoveByIdAsync(debtId, User);

        return Ok();
    }


    [HttpGet("ShowResults")]
    public async Task<IActionResult> ShowResults()
    {
        var entity = await _debtService.ShowStatisticsAsync(User);

        return Ok(entity?.Select(x => x.Adapt<DebtStatistics>()).ToList());
    }

}
