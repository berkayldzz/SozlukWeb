using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System.Linq;
using System.Web.Mvc;


namespace SozlukWeb.Controllers
{
    public class WriterCategoryController : Controller
    {
        // GET: WriterCategory
        CategoryManager cm = new CategoryManager(new EfCategoryDal());
        WriterManager wm = new WriterManager(new EfWriterDal()); // Yazar bilgilerini çekmek için ekledik

        //[Authorize(Roles ="B")]
        public ActionResult Index()
        {
            var categoryvalues = cm.GetList();
            return View(categoryvalues);
        }

        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(Category p)
        {
            // Geçerlilik sorgulaması yapmak için categoryvalidatoru çağırıyoruz.
            CategoryValidator categoryvalidator = new CategoryValidator();
            ValidationResult result = categoryvalidator.Validate(p);
            if (result.IsValid)
            {
                cm.CategoryAdd(p);
                return RedirectToAction("Index");
            }
            else
            {
                // Hataları foreach ile yazdırıyoruz
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View();
        }

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


    }
}