using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using OutlayApi.Data;
using System.Security.Claims;

namespace OutlayApi.Filters;

public class CategoryAttribute : ActionFilterAttribute
{
    private readonly AppDbContext _context;
    public string? Position { get; set; }
    public CategoryAttribute(AppDbContext context, string? position)
    {
        _context = context;
        Position = position;
    }

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!(context.ActionArguments.ContainsKey("categoryId")))
        {
            await next();
            return;
        }

        var categoryId = (int?)context.ActionArguments["categoryId"];
        var user = context.HttpContext.User;
        var userId = Convert.ToInt32(user.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)?.Value);

        var categoryUsers = await _context.UserCategories.FirstOrDefaultAsync(x => x.UserId == userId && x.CategoryId == categoryId);

        if (categoryUsers is null)
        {
            context.Result = new NotFoundResult();
            return;
        }

        if (Position is "Admin")
        {
            if (categoryUsers.IsAdmin != true)
            {
                context.Result = new NotFoundResult();
                return;
            }
        }

        await next();
    }
}