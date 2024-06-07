using System.Web.Mvc;

namespace api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Contact", "api", new {id = 1});
        }
    }
}
