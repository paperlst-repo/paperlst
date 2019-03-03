using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Security;

namespace Nop.Web.Controllers
{
    public partial class HomeController : BasePublicController
    {
        [HttpsRequirement(SslRequirement.No)]
        public virtual IActionResult Index()
        {
            return View();
        }

        public virtual IActionResult HomepageProducts(int items, int page)
        {
            return ViewComponent("HomepageProducts", new { items = items, page = page });
        }
    }
}