using System.Web;
using System.Web.Mvc;
using Web.Infrastructure.Filters;

namespace Web.Services
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
           
        }
    }
}
