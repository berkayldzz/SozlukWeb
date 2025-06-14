using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System.Collections.Generic;
using System.Linq;
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
            var headinglist = hm.GetList().Where(x => x.HeadingStatus == true).ToList(); // Aktif başlıkları çek

            // Sayfa yüklendiğinde ilk başlığın içeriğini göstermek için
            var initialHeading = headinglist.FirstOrDefault();
            if (initialHeading != null)
            {
                ViewBag.InitialHeadingId = initialHeading.HeadingID;
                ViewBag.CurrentHeadingId = initialHeading.HeadingID; // Yeni entry için başlık ID'si
            }
            else
            {
                ViewBag.InitialHeadingId = 0;
                ViewBag.CurrentHeadingId = 0;
            }

            return View(headinglist); // Başlık listesini Headings.cshtml'ye gönderiyoruz
        }

        public PartialViewResult Index(int id = 0) // id parametresi seçilen başlığın ID'si olacak
        {
            List<Content> contentlist;

            if (id == 0) // Eğer ID gelmezse (sayfa ilk yüklendiğinde veya hata durumunda)
            {
                // Headings Action'ından gelen ilk başlığın ID'sini kullanabiliriz.
                // Veya burada da ilk başlığın ID'sini çekebiliriz.
                var firstHeading = hm.GetList().FirstOrDefault(x => x.HeadingStatus == true);
                if (firstHeading != null)
                {
                    id = firstHeading.HeadingID;
                }
            }

            // İçerikleri çekiyoruz
            contentlist = cm.GetListByHeadingID(id).Where(x => x.ContentStatus == true).ToList();

            ViewBag.CurrentHeadingId = id; // AddContent butonu için current heading ID'yi taşıyoruz

            return PartialView(contentlist);
        }
    }
}