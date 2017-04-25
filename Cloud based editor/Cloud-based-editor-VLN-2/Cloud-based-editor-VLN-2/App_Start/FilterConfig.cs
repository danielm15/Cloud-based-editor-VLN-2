using System.Web;
using System.Web.Mvc;

namespace Cloud_based_editor_VLN_2
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
