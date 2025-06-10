using System.Web.Mvc;

namespace SozlukWeb.Controllers
{
    public class ErrorPageController : Controller
    {
        // GET: ErrorPage

        // Hata mesajı aldığı zaman kullanıcıyı bir sayfaya yönlendircez
        public ActionResult Page403()
        {
            Response.StatusCode = 403; // 403 alırsan
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
        public ActionResult Page404()
        {
            Response.StatusCode = 404; // 404 alırsan
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}