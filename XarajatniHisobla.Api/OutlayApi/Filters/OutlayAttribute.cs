using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using OutlayApi.Data;
using OutlayApi.Entities;
using System.Security.Claims;

namespace OutlayApi.Filters;

public class OutlayAttribute : ActionFilterAttribute
{
    private readonly AppDbContext _context;
    public string? Position;
    public OutlayAttribute(AppDbContext context, string position)
    {
        _context = context;
        Position = position;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        int? categoryId = default;
        Outlay? outlay = default;

        if (!(context.ActionArguments.ContainsKey("outlayId")))
        {
            await next();
            return;
        }
        else if (context.ActionArguments.ContainsKey("outlayId"))
        {
            var outlayId = (int?)context.ActionArguments["outlayId"];

            outlay = await _context.Outlays.FirstOrDefaultAsync(pro => pro.Id == outlayId);

            if (outlay is null)
            {
                context!.Result = new NotFoundResult();
                return;
            }

            categoryId = outlay.CategoryId;
        }

        var user = context.HttpContext.User;
        var userId = Convert.ToInt32(user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        var organizationUsers = await _context.UserCategories.FirstOrDefaultAsync(x => x.UserId == userId && x.CategoryId == categoryId);

        if (organizationUsers is null)
        {
            context.Result = new NotFoundResult();
            return;
        }

        if (Position == "Owner")
        {
            if (organizationUsers.IsAdmin != true)
            {
                if ((outlay?.UserId == userId) != true)
                {
                    context.Result = new NotFoundResult();
                    return;
                }

            }
        }

        await next();
    }
}