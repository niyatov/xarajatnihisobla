using Microsoft.AspNetCore.Mvc;

namespace OutlayApi.Filters;

public class CategoryFilterAttribute : TypeFilterAttribute
{
    public CategoryFilterAttribute(string position) : base(typeof(CategoryAttribute))
    {
        Arguments = new object[] { position };
    }
}