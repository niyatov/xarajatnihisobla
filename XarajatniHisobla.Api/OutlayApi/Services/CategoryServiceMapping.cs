using OutlayApi.Models;

namespace OutlayApi.Services;

public partial class CategoryService
{
    private static Entities.Category ToEntityCreateCategory(CreateCategory category)
    {
        return new Entities.Category()
        {
            Name = category.Name,
            Description = category.Description,
            Key = category.Key,
            IsPrivate = category.Key == null,
            CreateAt = DateTime.Now,
            UserCategories = ToEntityUserCategory(category.UserCategories)
        };
    }

    private static List<Categories> ToModelCategories(List<Entities.Category> categories, int userId)
    {
        int index = 0;
        List<Categories> categoriesM = new List<Categories>();

        foreach (var category in categories)
        {
            var categoryM = new Categories()
            {
                Index = ++index,
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                NumberOfOutlays = category.Outlays == null ? 0 : category.Outlays.Count(),
                IsAdmin = category.UserCategories.First(x => x.UserId == userId).IsAdmin,
                Type = category.IsPrivate == true ? "Private" : "Public"
            };
            categoriesM.Add(categoryM);
        }

        return categoriesM;
    }

    private static Category ToModelCategory(Entities.Category category, int userId)
    {

        Category categoryM = new Category();

        categoryM.Id = category.Id;
        categoryM.Name = category.Name;
        categoryM.Description = category.Description;
        categoryM.CreateAt = category.CreateAt;
        categoryM.LastUpdateAt = category.LastUpdateAt;
        categoryM.IsAdmin = category.UserCategories.First(x => x.UserId == userId).IsAdmin;
        categoryM.Type = category.IsPrivate == true ? "Private" : "Public";
        categoryM.Key = category.Key;
        return categoryM;
    }


    private static List<Entities.UserCategory> ToEntityUserCategory(List<UserCategory> categories)
    {
        var userCategories = new List<Entities.UserCategory>();

        foreach (var category in categories)
        {
            userCategories.Add(
                new Entities.UserCategory()
                {
                    UserId = category.UserId,
                    IsAdmin = category.IsAdmin,
                });
        }
        return userCategories;
    }
}
