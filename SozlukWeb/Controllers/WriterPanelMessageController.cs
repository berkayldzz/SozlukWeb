using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using FluentValidation.Results;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SozlukWeb.Controllers
{
    public class WriterPanelMessageController : Controller
    {
        MessageManager mm = new MessageManager(new EfMessageDal());
        MessageValidator messagevalidator = new MessageValidator();
        WriterManager wm = new WriterManager(new EfWriterDal()); // WriterManager'ı ekledik!

        // Bu metot, WriterPanelMessageController'daki her Action çalışmadan önce çalışır.
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string writerMail = (string)Session["WriterMail"];
            if (string.IsNullOrEmpty(writerMail))
            {
                // Eğer session'da mail yoksa veya boşsa, login sayfasına yönlendir
                filterContext.Result = RedirectToAction("WriterLogin", "Login");
                return;
            }

            // WriterManager kullanarak yazar bilgisini çekiyoruz
            var writer = wm.GetList().FirstOrDefault(x => x.WriterMail == writerMail);

            if (writer != null)
            {
                // Yazar bilgilerini ViewBag'e atıyoruz.
                ViewBag.WriterName = writer.WriterName;
                ViewBag.WriterSurName = writer.WriterSurName;
                ViewBag.WriterMail = writer.WriterMail;
                ViewBag.WriterImage = writer.WriterImage;
                ViewBag.WriterID = writer.WriterID;
                ViewBag.WriterTitle = writer.WriterTitle;
            }
            base.OnActionExecuting(filterContext);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Inbox()
        {
            string p = (string)Session["WriterMail"];
            var messagelist = mm.GetListInbox(p);
            return View(messagelist);
        }

        public ActionResult Sendbox()
        {
            string p = (string)Session["WriterMail"];
            var messagelist = mm.GetListSendbox(p);
            return View(messagelist);
        }

        public PartialViewResult MessageListMenu()
        {
            return PartialView();
        }

        public ActionResult GetInBoxMessageDetails(int id)
        {
            var values = mm.GetByID(id);
            return View(values);
        }

        public ActionResult GetSendBoxMessageDetails(int id)
        {
            var values = mm.GetByID(id);
            return View(values);
        }

        [HttpGet]
        public ActionResult NewMessage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewMessage(Message p)
        {
            string sender = (string)Session["WriterMail"];
            ValidationResult result = messagevalidator.Validate(p);
            if (result.IsValid)
            {
                p.SenderMail = sender;
                p.MessageDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                mm.MessageAdd(p);
                return RedirectToAction("SendBox");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }
            return View(p);
        }
    }
}