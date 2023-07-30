using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OutlayApi.Dtoes;
using OutlayApi.Filters;
using OutlayApi.Services;
using System.Data;

namespace OutlayApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;
    private readonly string error = "Xatolik sodir bo'ldi, Iltimos keyinroq urinib ko'ring";
    public CategoryController(
        ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("GetCategories")]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var entity = await _categoryService.GetAllAsync(User);

            if (!entity.IsSuccess)
                return BadRequest(new string?[] { entity.ErrorMessage, entity.ErrorDetails });
            return Ok(entity?.Data?.Select(x => x.Adapt<Categories>()).ToList());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new string?[] { error, e.ToString() });
        }
    }


    [CategoryFilter("")]
    [HttpGet("GetCategory/{categoryId}")]
    public async Task<IActionResult> GetCategory(int categoryId)
    {
        try
        {
            var entity = await _categoryService.GetByIdAsync(categoryId, User);
            if (!entity.IsSuccess)
            {
                return BadRequest(new string?[] { entity.ErrorMessage, entity.ErrorDetails });
            }

            return Ok(entity?.Data?.Adapt<Category>());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new string?[] { error, e.ToString() });
        }
    }


    [HttpPost("CreateCategory")]
    public async Task<IActionResult> CreateCategory(CreateCategory createCategoryDto)
    {
        try
        {
            var errors = new Dictionary<string, string[]>();

            var entity = await _categoryService.CreateAsync(createCategoryDto.Adapt<Models.CreateCategory>(), User);
            if (!entity.IsSuccess)
                return BadRequest(new string?[] { entity.ErrorMessage, entity.ErrorDetails });
            return Ok(entity?.Data);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new string?[] { error, e.ToString() });
        }
    }


    [CategoryFilter("Admin")]
    [HttpPut("UpdateCategory/{categoryId}")]
    public async Task<IActionResult> UpdateCategory(int categoryId, UpdateCategory updateCategoryDto)
    {
        try
        {
            var entity = await _categoryService.UpdateAsync(categoryId, updateCategoryDto.Adapt<Models.UpdateCategory>(), User);
            if (!entity.IsSuccess)
                return BadRequest(new string?[] { entity.ErrorMessage, entity.ErrorDetails });
            return Ok(entity?.Data?.Adapt<Category>());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new string?[] { error, e.ToString() });
        }

    }


    [CategoryFilter("Admin")]
    [HttpDelete("DeleteCategory/{categoryId}")]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        try
        {
            var entity = await _categoryService.RemoveByIdAsync(categoryId, User);
            if (!entity.IsSuccess)
                return BadRequest(new string?[] { entity.ErrorMessage, entity.ErrorDetails });
            return Ok(entity?.Data?.Adapt<Category>());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new string?[] { error, e.ToString() });
        }
    }


    [CategoryFilter("Admin")]
    [HttpDelete("CleanCategory/{categoryId}")]
    public async Task<IActionResult> CleanCategory(int categoryId)
    {
        try
        {
            var entity = await _categoryService.CleanAsync(categoryId, User);
            if (!entity.IsSuccess)
                return BadRequest(new string?[] { entity.ErrorMessage, entity.ErrorDetails });
            return Ok();
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new string?[] { error, e.ToString() });
        }

    }



    [HttpPost("JoinCategory")]
    public async Task<IActionResult> JoinCategory(JoinCategory joinCategory)
    {
        try
        {
            var entity = await _categoryService.JoinAsync(joinCategory.Name, joinCategory.Key, User);
            if (!entity.IsSuccess)
                return BadRequest(new string?[] { entity.ErrorMessage, entity.ErrorDetails });
            return Ok(entity?.Data);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new string?[] { error, e.ToString() });
        }

    }


    [CategoryFilter("")]
    [HttpGet("ShowUsers/{categoryId}")]
    public async Task<IActionResult> ShowUsers(int categoryId)
    {
        try
        {
            var entity = await _categoryService.ShowUsersAsync(categoryId, User);
            if (!entity.IsSuccess)
                return BadRequest(new string?[] { entity.ErrorMessage, entity.ErrorDetails });
            return Ok(entity?.Data?.Adapt<Users>());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new string?[] { error, e.ToString() });
        }
    }

    [CategoryFilter("")]
    [HttpGet("ShowStatistics/{categoryId}")]
    public async Task<IActionResult> ShowStatistics(int categoryId)
    {
        try
        {
            var entity = await _categoryService.ShowStatisticsAsync(categoryId, User);
            if (!entity.IsSuccess)
                return BadRequest(new string?[] { entity.ErrorMessage, entity.ErrorDetails });
            return Ok(entity?.Data?.Adapt<Statistics>());
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new string?[] { error, e.ToString() });
        }
    }


    [CategoryFilter("Admin")]
    [HttpDelete("DeleteUser/{categoryId}/{userId}")]
    public async Task<IActionResult> DeleteUser(int categoryId, int userId)
    {
        try
        {
            var entity = await _categoryService.DeleteUserAsync(categoryId, userId, User);
            if (!entity.IsSuccess)
                return BadRequest(new string?[] { entity.ErrorMessage, entity.ErrorDetails });
            return Ok(entity?.Data);
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new string?[] { error, e.ToString() });
        }

    }
}