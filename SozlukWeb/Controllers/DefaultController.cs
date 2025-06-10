using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using System.Web.Mvc;

namespace SozlukWeb.Controllers
{
    [AllowAnonymous]
    public class DefaultController : Controller
    {
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        ContentManager cm = new ContentManager(new EfContentDal());
        public ActionResult Headings()
        {
            var headinglist = hm.GetList();
            return View(headinglist);
        }
        public PartialViewResult Index(int id = 0)
        {
            // Contentleri burada çekicez.

            var contentlist = cm.GetListByHeadingID(id);
            return PartialView(contentlist);
        }
    }
}