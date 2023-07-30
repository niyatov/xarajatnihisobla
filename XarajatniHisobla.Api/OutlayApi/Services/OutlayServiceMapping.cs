using OutlayApi.Models;

namespace OutlayApi.Services;

public partial class OutlayService
{
    public static Entities.Outlay ToEntityCreateOutlay(CreateOutlay outlay, int userId)
    {
        return new Entities.Outlay()
        {
            Name = outlay.Name,
            Description = outlay.Description,
            Cost = outlay.Cost,
            UserId = userId,
            CategoryId = outlay.CategoryId,
            CreateAt = DateTime.Now,
        };
    }

    public static Outlay ToModelOutlay(Entities.Outlay outlay, int userId, string userName)
    {

        Outlay categoryM = new Outlay();

        categoryM.Id = outlay.Id;
        categoryM.CategoryId = outlay.CategoryId;
        categoryM.Name = outlay.Name;
        categoryM.Description = outlay.Description;
        categoryM.CreateAt = outlay.CreateAt;
        categoryM.LastUpdateAt = outlay.LastUpdateAt;
        categoryM.Cost = outlay.Cost ?? 0;
        categoryM.Username = outlay.User.UserName;
        categoryM.IsAdmin = outlay.Category!.UserCategories!.Any(x => x.UserId == userId && x.IsAdmin == true);
        categoryM.IsOwner = outlay.User.UserName == userName;

        return categoryM;
    }


    public static List<Outlays> ToModelOutlays(List<Entities.Outlay> outlays)
    {

        List<Outlays>? outlaysM = new List<Outlays>();
        int index = 0;

        if ((outlays?.Count() > 0) == true)
        {
            foreach (var outlay in outlays)
            {
                var outlayM = new Outlays()
                {
                    Index = ++index,
                    Id = outlay.Id,
                    Name = outlay.Name,
                    Description = outlay.Description,
                    Cost = outlay.Cost ?? 0,
                    Username = outlay.User.UserName
                };
                outlaysM.Add(outlayM);
            }
        }

        return outlaysM;
    }
}
