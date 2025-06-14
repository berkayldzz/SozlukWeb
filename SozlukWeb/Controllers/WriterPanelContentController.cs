using BusinessLayer.Concrete;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SozlukWeb.Controllers
{
    public class WriterPanelContentController : Controller
    {
      
            ContentManager cm = new ContentManager(new EfContentDal());
            Context c = new Context(); // Context'i bırakalım, WriterManager'ı da ekleyelim
            WriterManager wm = new WriterManager(new EfWriterDal()); // Yazar bilgilerini çekmek için ekledik

            // Bu metot, WriterPanelContentController'daki her Action çalışmadan önce çalışır.
            protected override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                string writerMail = (string)Session["WriterMail"];
                if (string.IsNullOrEmpty(writerMail))
                {
                    // Eğer session'da mail yoksa veya boşsa, login sayfasına yönlendir
                    filterContext.Result = RedirectToAction("WriterLogin", "Login"); // Login Controller'ınızın adını doğru yazdığınızdan emin olun
                    return;
                }

                // WriterManager kullanarak yazar bilgisini çekiyoruz
                // wm.GetList().FirstOrDefault() veya wm.GetByMail(writerMail) eğer böyle bir metodunuz varsa
                var writer = wm.GetList().FirstOrDefault(x => x.WriterMail == writerMail);

                if (writer != null)
                {
                    // Yazar bilgilerini ViewBag'e atıyoruz. Bu bilgiler hem layout'ta hem de View'lerde kullanılabilir.
                    ViewBag.WriterName = writer.WriterName;
                    ViewBag.WriterSurName = writer.WriterSurName;
                    ViewBag.WriterMail = writer.WriterMail;
                    ViewBag.WriterImage = writer.WriterImage;
                    ViewBag.WriterID = writer.WriterID;
                    ViewBag.WriterTitle = writer.WriterTitle;
                }
                base.OnActionExecuting(filterContext); // Varsayılan davranışı sürdür
            }


            // GET: WriterPanelContent
            public ActionResult MyContent(string p = null) // p parametresi opsiyonel hale getirildi
            {
                // OnActionExecuting metodu sayesinde ViewBag.WriterMail zaten dolu
                p = (string)Session["WriterMail"]; // Session'dan maili al

                // ViewBag'deki WriterID'yi kullanarak içeriği çekmek daha güvenli olabilir
                // int writeridinfo = (int)ViewBag.WriterID; // Eğer OnActionExecuting'den WriterID'yi alırsak
                // var contentvalues = cm.GetListByWriter(writeridinfo);

                // Veya mevcut kodunuzu koruyarak mail üzerinden çekmeye devam edebilirsiniz
                var writeridinfo = c.Writers.Where(x => x.WriterMail == p).Select(y => y.WriterID).FirstOrDefault();
                var contentvalues = cm.GetListByWriter(writeridinfo);

                return View(contentvalues);
            }

            [HttpGet]
            public ActionResult AddContent(int id)
            {
                ViewBag.d = id; // Bu id, muhtemelen eklenecek içeriğin ait olduğu başlığın ID'si
                return View();
            }

            [HttpPost]
            public ActionResult AddContent(Content p)
            {
                // OnActionExecuting metodu sayesinde ViewBag.WriterID zaten dolu
                int writerId = (int)ViewBag.WriterID; // ViewBag'den yazar ID'sini al

                p.ContentDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                p.WriterID = writerId; // ViewBag'den gelen ID'yi kullan
                p.ContentStatus = true;
                cm.ContentAdd(p);

                return RedirectToAction("MyContent");
            }

            public ActionResult ToDoList()
            {
                // Bu Action da OnActionExecuting'den faydalanacak
                return View();
            }
        }
    }
