using System.Web;
using System.Web.Mvc;

namespace fuzzy_octo_tribble
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}