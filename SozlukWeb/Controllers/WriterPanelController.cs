using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SozlukWeb.Controllers
{
    public class WriterPanelController : Controller
    {
        // GET: WriterPanel

        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        MessageManager mm = new MessageManager(new EfMessageDal());
        HeadingManager hm = new HeadingManager(new EfHeadingDal());
        WriterManager wm = new WriterManager(new EfWriterDal());
        ContentManager cmt = new ContentManager(new EfContentDal());
        Context c = new Context();

        [HttpGet]
        public ActionResult WriterPanel() // Profil düzenleme sayfası
        {
            // OnActionExecuting metodu zaten ViewBag'i doldurduğu için burada tekrar çekmeye gerek yok.
            // WriterPanel.cshtml'in Writer modelini beklediği için, bu metot da bir Writer nesnesi göndermeli.
            string writerMail = (string)Session["WriterMail"];
            var writerValue = c.Writers.FirstOrDefault(x => x.WriterMail == writerMail);

            if (writerValue == null)
            {
                // Yazar bulunamazsa (veri tabanında yoksa), login sayfasına yönlendir
                return RedirectToAction("WriterLogin", "Login");
            }

            return View(writerValue); // Writer modelini view'e gönderiyoruz
        }

        [HttpPost]
        public ActionResult WriterPanel(Writer p)
        {
            // Model validasyonları burada yapılabilir
            if (ModelState.IsValid)
            {
                wm.WriterUpdate(p); // WriterManager kullanarak yazarı güncelleyin
                return RedirectToAction("WriterPanel"); // Güncelleme sonrası aynı sayfaya yönlendir
            }
            // Hata varsa aynı view'i modelle birlikte geri döndür
            return View(p);
        }
        public ActionResult MyHeading()
        {
            int writerId = (int)ViewBag.WriterID; // ViewBag'den WriterID'yi al
            var headinglist = c.Headings.Where(x => x.WriterID == writerId).ToList();
            return View(headinglist);
        }
        [HttpGet]
        public ActionResult NewHeading()
        {
            List<SelectListItem> valuecategory = (from x in cm.GetList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryID.ToString()
                                                  }).ToList();
            ViewBag.vlc = valuecategory;

            return View();
        }
        [HttpPost]
        public ActionResult NewHeading(Heading p)
        {
            string writermailinfo = (string)Session["WriterMail"];
            var writeridinfo = c.Writers.Where(x => x.WriterMail == writermailinfo).Select(y => y.WriterID).FirstOrDefault();
            ViewBag.d = writeridinfo;
            p.HeadingDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            p.WriterID = writeridinfo;
            p.HeadingStatus = true;
            hm.HeadingAdd(p);
            return RedirectToAction("MyHeading");
        }
        [HttpGet]
        public ActionResult EditHeading(int id)
        {
            List<SelectListItem> valuecategory = (from x in cm.GetList()
                                                  select new SelectListItem
                                                  {
                                                      Text = x.CategoryName,
                                                      Value = x.CategoryID.ToString()
                                                  }).ToList();

            ViewBag.vlc = valuecategory;

            var HeadingValue = hm.GetByID(id);
            return View(HeadingValue);
        }
        [HttpPost]
        public ActionResult EditHeading(Heading p)
        {
            hm.HeadingUpdate(p);
            return RedirectToAction("MyHeading");
        }
        public ActionResult DeleteHeading(int id)
        {
            var HeadingValue = hm.GetByID(id);
            HeadingValue.HeadingStatus = false;
            hm.HeadingDelete(HeadingValue);
            return RedirectToAction("MyHeading");
        }
        public ActionResult AllHeading(int p = 1)
        {
            var headings = hm.GetList().ToPagedList(p, 4); // İlk parametre kaçıncı sayfadan başlayacağı ikincisi bir sayfada kaç tane var.
            return View(headings);
        }


        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string writerMail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(writerMail))
            {
                // Eğer session'da mail yoksa veya boşsa, login sayfasına yönlendir
                filterContext.Result = RedirectToAction("WriterLogin", "Login");
                return;
            }

            var writer = c.Writers.FirstOrDefault(x => x.WriterMail == writerMail);
            if (writer != null)
            {
                // Yazar bilgilerini ViewBag'e atıyoruz. Bu bilgiler hem layout'ta hem de View'lerde kullanılabilir.
                ViewBag.WriterName = writer.WriterName;
                ViewBag.WriterSurName = writer.WriterSurName;
                ViewBag.WriterMail = writer.WriterMail;
                ViewBag.WriterImage = writer.WriterImage;
                ViewBag.WriterID = writer.WriterID; // WriterID'ye de ihtiyacınız olabilir
                ViewBag.WriterTitle = writer.WriterTitle; // Title da eklendi
            }
            base.OnActionExecuting(filterContext); // Varsayılan davranışı sürdür
        }

        public ActionResult Inbox()
        {
            string p = (string)Session["WriterMail"]; // Yazarın mailini al
            // Bu mail adresine gelen mesajları getir (alıcı olan mesajlar)
            var messages = mm.GetListInbox(p);
            return View(messages);
        }

        // Gönderilen Kutusu (Opsiyonel, eğer varsa)
        public ActionResult Sendbox()
        {
            string p = (string)Session["WriterMail"];
            // Bu mail adresinden gönderilen mesajları getir (gönderici olan mesajlar)
            var messages = mm.GetListSendbox(p);
            return View(messages);
        }

        // Mesaj Detayı (Opsiyonel)
        public ActionResult GetMessageDetails(int id)
        {
            var messageValue = mm.GetByID(id);
            return View(messageValue);
        }

        // Yeni Mesaj Yazma (Opsiyonel)
        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewMessage(Message p)
        {
            string senderMail = (string)Session["WriterMail"];
            p.SenderMail = senderMail; // Gönderen mailini session'dan al
            p.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());

            mm.MessageAdd(p);
            return RedirectToAction("Sendbox"); // Mesaj gönderildikten sonra gönderilen kutusuna yönlendir
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
            cmt.ContentAdd(p);

            return RedirectToAction("MyContent");
        }

        public ActionResult MyContent(string p = "") // Arama özelliği için 'p' parametresi
        {
            // OnActionExecuting metodu sayesinde ViewBag.WriterID zaten dolu geliyor.
            int writerId = (int)ViewBag.WriterID; // Oturum açan yazarın ID'sini al

            // ContentManager'daki GetListByWriter metodunu kullanarak yazarın içeriklerini getir.
            // Bu metot zaten WriterID'ye göre filtreleme yapıyor.
            var contents = cmt.GetListByWriter(writerId);

            // Eğer arama terimi varsa (p boş değilse), içerikleri ContentValue'ye göre filtrele.
            if (!string.IsNullOrEmpty(p))
            {
                contents = contents.Where(x => x.ContentValue.Contains(p)).ToList();
            }

            // İçerikleri tarihe göre azalan sırada sırala (genellikle en yeni içerikler üstte gösterilir).
            contents = contents.OrderByDescending(x => x.ContentDate).ToList();

            // Arama filtresini View'e gönder (formda kalması için)
            ViewBag.CurrentFilter = p;

            // Filtrelenmiş ve sıralanmış içerik listesini View'e gönder.
            return View(contents);
        }

    }
}