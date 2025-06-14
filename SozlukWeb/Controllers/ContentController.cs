using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace SozlukWeb.Controllers
{
    public class ContentController : Controller
    {
        // GET: Content

        ContentManager cm = new ContentManager(new EfContentDal());
        public ActionResult Index()
        {
            return View();
        }

        Context c = new Context();
        public ActionResult GetAllContent(string p = "", int page = 1)
        {
            int pageSize = 10;

            // cm.GetList(p) metodu artık kendi içinde hem filtrelemeyi hem de
            // Heading ve Writer bilgilerini Include etmeyi hallediyor olmalı.
            // Bu şekilde, Controller katmanı daha sade kalır.
            var contentList = cm.GetList(p);

            // İçerikleri belirli bir kritere göre sırala (bu hala Controller'da yapılabilir)
            contentList = contentList.OrderByDescending(x => x.ContentID).ToList();

            // Sayfalama bilgileri ViewBag'e aktarılıyor.
            ViewBag.TotalItems = contentList.Count();
            ViewBag.PageCount = (int)System.Math.Ceiling((double)contentList.Count / pageSize);
            ViewBag.CurrentPage = page;
            ViewBag.CurrentFilter = p;

            // Geçerli sayfadaki içerikleri al ve View'e gönder.
            var pagedContent = contentList.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return View(pagedContent);
        }
    


    public ActionResult ContentByHeading(int id) // Başlık kısmındaki yazılar butonu için.
        {
            var contentvalues = cm.GetListByHeadingID(id);
            return View(contentvalues);
        }

        [HttpGet] // Düzenleme formunu göstermek için
        public ActionResult EditContent(int id)
        {
            var contentValue = cm.GetByID(id); 
            if (contentValue == null)
            {
            
                return RedirectToAction("GetAllContent");
            }
            return View(contentValue); 
        }

        public ActionResult ContentDelete(int id)
        {
            // Silinecek içeriği ID'sine göre bul
            var contentValue = cm.GetByID(id);

            // İçeriği sil
            cm.ContentDelete(contentValue);

            // İçerik silindikten sonra tüm içeriklerin listelendiği sayfaya yönlendir.
            // Eğer yazar paneli için farklı bir silme action'ınız varsa
            // ve oradan yönlendirme yapılıyorsa, o controller/action'a yönlendirin.
            return RedirectToAction("GetAllContent");
        }

        [HttpPost] 
     
        public ActionResult EditContent(Content p) 
        {
       
            if (string.IsNullOrWhiteSpace(p.ContentValue))
            {
                ModelState.AddModelError("ContentValue", "İçerik boş bırakılamaz.");
                return View(p); 
            }

     
            var existingContent = cm.GetByID(p.ContentID);

            if (existingContent != null)
            {
               
                existingContent.ContentValue = p.ContentValue;

           
                cm.ContentUpdate(existingContent);

          
                return RedirectToAction("GetAllContent");
            }
            else
            {
              
                ModelState.AddModelError("", "Güncellenecek içerik bulunamadı.");
                return View(p); 
            }
        }
    }
}
