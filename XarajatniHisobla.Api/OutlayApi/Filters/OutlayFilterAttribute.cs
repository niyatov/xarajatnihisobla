using Microsoft.AspNetCore.Mvc;

namespace OutlayApi.Filters;

public class OutlayFilterAttribute : TypeFilterAttribute
{
    public OutlayFilterAttribute(string position) : base(typeof(OutlayAttribute))
    {
        Arguments = new object[] { position };
    }
}