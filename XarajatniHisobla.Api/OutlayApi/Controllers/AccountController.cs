using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OutlayApi.Dtoes;
using OutlayApi.Exceptions;
using OutlayApi.Repositories;
using System.Security.Claims;

namespace OutlayApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Entities.User> _userManager;
    private readonly SignInManager<Entities.User> _signInManager;
    public AccountController(UserManager<Entities.User> userManager,
        SignInManager<Entities.User> signInManager,
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _signInManager = signInManager;
    }


    [HttpPost("SignIn")]
    public async Task<IActionResult> SignIn(SignIn signIn, string? ReturnUrl)
    {
        Entities.User user;

        if (signIn.UsernameOrEmail!.Contains("@"))
            user = await _userManager.FindByEmailAsync(signIn.UsernameOrEmail);
        else
            user = await _userManager.FindByNameAsync(signIn.UsernameOrEmail);

        if (user == null)
            throw new BadRequestException("foydalanuvchi nomi yoki emaili noto'g'ri", 400);

        var result = await _signInManager.PasswordSignInAsync(user, signIn.Password, true, false);

        if (!result.Succeeded)
            throw new BadRequestException("parol noto'g'ri", 400);

        if (ReturnUrl != null) return Ok(ReturnUrl);

        return Ok("/");
    }


    [HttpPost("Register")]
    public async Task<IActionResult> Register(Register register)
    {
        if (!ModelState.IsValid) return BadRequest();

        var userExists = await _userManager.FindByNameAsync(register.Username) != null;
        var emailExists = await _userManager.FindByEmailAsync(register.Email) != null;

        if (userExists)
            return BadRequest("Bunday nomli foydalanuvchi mavjud");

        if (emailExists)
            throw new BadRequestException("bunday nomli email mavjud", 400);


        Entities.User user = new Entities.User
        {
            Email = register.Email,
            UserName = register.Username
        };

        IdentityResult result = await _userManager.CreateAsync(user, register.Password);

        if (!result.Succeeded)
            throw new BadRequestException("Xatolik sodir bo'ldi", 400);

        return Ok("SignIn");
    }

    [Authorize]
    [HttpGet("Get")]
    public async Task<IActionResult> Get()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _unitOfWork.Users.GetByIdAsync(Convert.ToInt32(userId));

        var userVM = new UserAvatar();

        userVM.Name = user.NormalizedUserName;
        userVM.Bytes = user.Bytes;

        return Ok(userVM);
    }


    [Authorize]
    [HttpPut("UpdateAccount")]
    public async Task<IActionResult> UpdateAccount(UpdateAccount updateAccountDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _unitOfWork.Users.GetByIdAsync(Convert.ToInt32(userId));

        var userExists = await _userManager.FindByNameAsync(updateAccountDto.Username) != null;

        if (userExists && user.UserName != updateAccountDto.Username)
        {
            throw new BadRequestException("Bunday nomli foydalanuvchi mavjud", 400);
        }

        user.UserName = updateAccountDto.Username;

        var newPasswordHash = _userManager.PasswordHasher.HashPassword(user, updateAccountDto.Password);
        user.PasswordHash = newPasswordHash;

        await _unitOfWork.Users.Update(user);
        await _userManager.UpdateAsync(user);

        return Ok();
    }

    [Authorize]
    [HttpDelete("LogOut")]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }


    [Authorize]
    [HttpPost("CreateOrUpdateAvatar")]
    public async Task<IActionResult> CreateOrUpdateAvatar(Avatar avatarDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var user = await _unitOfWork.Users.GetByIdAsync(Convert.ToInt32(userId));
        user.Bytes = avatarDto.Bytes;

        await _unitOfWork.Users.Update(user);

        return Ok();
    }

    [Authorize]
    [HttpDelete("DeleteAvatar")]
    public async Task<IActionResult> DeleteAvatar()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _unitOfWork.Users.GetByIdAsync(Convert.ToInt32(userId));

        user.Bytes = null;

        await _unitOfWork.Users.Update(user);

        return Ok();
    }
}