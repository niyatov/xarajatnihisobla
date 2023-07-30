using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OutlayApi.Models;
using OutlayApi.Repositories;
using System.Security.Claims;

namespace OutlayApi.Services;

public partial class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMemoryCache _memoryCache;
    private readonly string error = "Xatolik sodir bo'ldi, Iltimos keyinroq urinib ko'ring";

    public CategoryService(IUnitOfWork unitOfWork, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _memoryCache = memoryCache;
    }

    public async ValueTask<Result<List<Categories>>> GetAllAsync(ClaimsPrincipal claims)
    {
        try
        {
            var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = _unitOfWork.Categories.GetAll().Where(x => x.UserCategories.Any(x => x.UserId == userId)).ToList();
            var result = ToModelCategories(entity, userId);

            return new(true) { Data = result };
        }
        catch (DbUpdateException e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriyalar olib kelina olmadi. Xato databasega aloqador. {e}" };
        }
        catch (Exception e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriyalar olib kelina olmadi. {e}" };
        }
    }


    public async ValueTask<Result<Category>> GetByIdAsync(int id, ClaimsPrincipal claims)
    {
        try
        {
            var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = await _unitOfWork.Categories.GetByIdAsync(id);

            var result = ToModelCategory(entity, userId);

            return new(true) { Data = result };
        }
        catch (DbUpdateException e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriya olib kelina olmadi. Xato databasega aloqador. {e}" };
        }
        catch (Exception e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriya olib kelina olmadi. {e}" };
        }
    }


    public async ValueTask<Result<int>> CreateAsync(CreateCategory category, ClaimsPrincipal claims)
    {
        try
        {
            var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (_unitOfWork.Categories.GetAll().Any(x => x.Name.ToLower() == category.Name.ToLower()))
                return new(false) { ErrorMessage = "bunday nomli kategoriya mavjud" };

            category.UserCategories = new List<UserCategory> { new UserCategory() { UserId = userId, IsAdmin = true } };

            var entity = ToEntityCreateCategory(category);
            await _unitOfWork.Categories.AddAsync(entity);

            return new(true) { Data = entity.Id };
        }
        catch (DbUpdateException e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriya yaratilmadi. Xato databasega aloqador. {e}" };
        }
        catch (Exception e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriya yaratilmadi. {e}" };
        }
    }


    public async ValueTask<Result<Category>> UpdateAsync(int id, UpdateCategory category, ClaimsPrincipal claims)
    {
        try
        {
            var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            var entity = await _unitOfWork.Categories.GetByIdAsync(id);

            if (_unitOfWork.Categories.GetAll().Any(x => x.Name.ToLower() == category.Name.ToLower()) && entity.Name != category.Name)
                return new(false) { ErrorMessage = "bunday nomli kategoriya mavjud" };

            if (entity.IsPrivate == false)
            {
                entity.Key = category.Key;
            }

            entity.Name = category.Name;
            entity.Description = category.Description;
            entity.LastUpdateAt = DateTime.Today;

            await _unitOfWork.Categories.Update(entity);
            return new(true) { Data = ToModelCategory(entity, userId) };
        }
        catch (DbUpdateException e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriya yaratilmadi.Xato databasega aloqador. {e}" };
        }
        catch (Exception e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriya yaratilmadi. {e}" };
        }
    }


    public async ValueTask<Result<Category>> RemoveByIdAsync(int id, ClaimsPrincipal claims)
    {
        try
        {
            var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            var entity = await _unitOfWork.Categories.GetByIdAsync(id);

            var entry = await _unitOfWork.Categories.Remove(entity);
            return new(true) { Data = ToModelCategory(entry, userId) };
        }
        catch (DbUpdateException e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriya o'chirilmadi.Xato databasega aloqador. {e}" };
        }
        catch (Exception e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriya o'chirilmadi. {e}" };
        }
    }



    public async ValueTask<Result> CleanAsync(int id, ClaimsPrincipal claims)
    {
        try
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            var outlays = category.Outlays?.ToList();

            if (outlays is not null && outlays.Count() > 0)
            {

                category.Outlays = null;
                await _unitOfWork.Categories.Update(category);
                return new(true);
            }

            return new(true);
        }
        catch (DbUpdateException e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriya tozalanmadi. Xato databasega aloqador. {e}" };
        }
        catch (Exception e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriya tozalanmadi.{e}" };
        }
    }


    public async ValueTask<Result<int>> JoinAsync(string name, string key, ClaimsPrincipal claims)
    {
        try
        {
            var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = _unitOfWork.Categories.GetAll().Where(x => x.Name == name && x.Key == key).FirstOrDefault();

            if (entity is null)
                return new(false) { ErrorMessage = "kategoriya topilmadi" };

            if (entity.UserCategories.Any(x => x.UserId == userId))
                return new(false) { ErrorMessage = "siz bu kategoriyaga allaqachon qo'shilgansiz" };

            entity.UserCategories.Add(new Entities.UserCategory() { UserId = userId, CategoryId = entity.Id });

            await _unitOfWork.Categories.Update(entity);
            return new(true) { Data = entity.Id };
        }
        catch (DbUpdateException e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriyaga qo'shilmadi.Xato databasega aloqador. {e}" };
        }
        catch (Exception e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"kategoriyaga qo'shilmadi. {e}" };
        }
    }


    public async ValueTask<Result<Statistics>> ShowStatisticsAsync(int id, ClaimsPrincipal claims)
    {
        try
        {
            var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            var entity = _memoryCache.Get<Statistics>($"userId={userId},categoryId={id}");
            if (entity == null)
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);

                var averageSum = (category.Outlays.Select(x => x.Cost).Sum() ?? 0) / category.UserCategories.Count();

                var statistics = new Statistics();
                var usersResult = new List<UsersResult>();
                var dict = new Dictionary<string, int>();
                string username;
                int money;

                foreach (var user in category.UserCategories)
                {
                    var userMoney = category.Outlays.Where(x => x.UserId == user.UserId).Select(x => x.Cost).Sum() ?? 0;

                    username = user.User.UserName;
                    money = userMoney - averageSum;
                    dict.Add(username, money);
                }
                dict = dict.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                foreach (var d in dict)
                {
                    usersResult.Add(

                        new UsersResult()
                        {
                            UserName = d.Key,
                            ResultMoney = d.Value,
                        }
                    );

                }
                string userName = claims.FindFirst(ClaimTypes.Name).Value;
                statistics.UsersResults = usersResult;
                statistics.CategoryId = id;
                statistics.IsPrivate = category.IsPrivate;
                statistics.NumberOfPeople = category.UserCategories.Count();
                statistics.NumberOfOutlays = category.Outlays.Count();
                statistics.NumberOfYourOutlays = category.Outlays.Where(x => x.UserId == userId).Count();
                statistics.TotalSum = category.Outlays.Select(x => x.Cost).Sum() ?? 0;
                statistics.StartedAt = category?.Outlays?.OrderBy(x => x.CreateAt).FirstOrDefault()?.CreateAt;
                statistics.FinishedAt = category?.Outlays?.OrderByDescending(x => x.CreateAt).FirstOrDefault()?.CreateAt;
                statistics.SpentMoney = category.Outlays.Where(x => x.UserId == userId).Select(x => x.Cost).Sum() ?? 0;
                statistics.ResultMoney = usersResult.First(x => x.UserName == userName).ResultMoney;
                statistics.IsAdmin = category.UserCategories.First(x => x.UserId == userId).IsAdmin;

                entity = statistics;

                _memoryCache.Set($"userId={userId},categoryId={id}", entity, TimeSpan.FromSeconds(5));
            }

            return new(true) { Data = entity };
        }
        catch (DbUpdateException e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"statistika ko'rsatilmadi. Xato databasega aloqador. {e}" };
        }
        catch (Exception e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"statistika ko'rsatilmadi. {e}" };
        }
    }


    public async ValueTask<Result<Users>> ShowUsersAsync(int id, ClaimsPrincipal claims)
    {
        try
        {
            var userId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            var users = category.UserCategories.Select(x => x.User).ToList();

            var outlays = category.Outlays;

            Users usersM = new Users();

            usersM.UsersList = new List<User>();


            var averageSum = (outlays.Select(x => x.Cost).Sum() ?? 0) / (users.Count() == 0 ? 1 : users.Count());
            int userMoney;
            foreach (var user in users)
            {
                User userM = new User();
                userM.UserName = user.UserName;

                userMoney = outlays.Where(x => x.UserId == user.Id).Select(x => x.Cost).Sum() ?? 0;

                userM.ResultMoney = userMoney - averageSum;
                userM.NumbersOfBuyingThings = outlays.Where(x => x.UserId == user.Id).Count();
                userM.Id = user.Id;
                usersM.UsersList.Add(userM);
            }
            usersM.UsersList = usersM.UsersList.OrderByDescending(x => x.ResultMoney).ToList();
            usersM.CategoryId = id;
            usersM.IsAdmin = category.UserCategories.First(x => x.CategoryId == id && x.UserId == userId).IsAdmin;

            return new(true) { Data = usersM };
        }
        catch (DbUpdateException e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"Foydalanuvchilar ko'rsatilmadi. Xato databasega aloqador. {e}" };
        }
        catch (Exception e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"Foydalanuvchilar ko'rsatilmadi. {e}" };
        }

    }


    public async ValueTask<Result<string>> DeleteUserAsync(int id, int userId, ClaimsPrincipal claims)
    {
        try
        {
            var mainUserId = Convert.ToInt32(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            var category = await _unitOfWork.Categories.GetByIdAsync(id);

            if (!category.UserCategories.Any(x => x.UserId == userId))
                return new(false) { ErrorMessage = $"Foydalanuvchi topilmadi" };

            if (userId == mainUserId)
            {
                try
                {
                    await _unitOfWork.Categories.Remove(category);
                    return new(true) { Data = "GotoGetCategories" };
                }
                catch (DbUpdateException e)
                {
                    return new(false) { ErrorMessage = error, ErrorDetails = $"Foydalanuvchi o'chirilmadi.Xato databasega aloqador. {e}" };
                }
                catch (Exception e)
                {
                    return new(false) { ErrorMessage = error, ErrorDetails = $"Foydalanuvchi o'chirilmadi.{e}" };
                }
            }

            var outlays = category.Outlays.Where(x => x.UserId != userId).ToList();
            category.Outlays = outlays;

            List<Entities.UserCategory> userCategories = category.UserCategories.Where(x => x.UserId != userId).ToList();
            category.UserCategories = userCategories;


            await _unitOfWork.Categories.Update(category);
            return new(true) { Data = "GotoShowUsers" };
        }
        catch (DbUpdateException e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"Foydalanuvchi o'chirilmadi.Xato databasega aloqador. {e}" };
        }
        catch (Exception e)
        {
            return new(false) { ErrorMessage = error, ErrorDetails = $"Foydalanuvchi o'chirilmadi. {e}" };
        }
    }
}
