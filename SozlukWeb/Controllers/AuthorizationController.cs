﻿using BusinessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System.Web.Mvc;

namespace SozlukWeb.Controllers
{

    public class AuthorizationController : Controller
    {
        // GET: Authorization

        AdminManager adminmanager = new AdminManager(new EfAdminDal());
        public ActionResult Index()
        {
            var adminvalues = adminmanager.GetList();

            return View(adminvalues);
        }

        [HttpGet]
        public ActionResult AddAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAdmin(Admin p)
        {
            adminmanager.AdminAdd(p);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult EditAdmin(int id)
        {
            var adminvalues = adminmanager.GetByID(id);
            return View(adminvalues);

        }

        [HttpPost]
        public ActionResult EditAdmin(Admin p)
        {
            adminmanager.AdminUpdate(p);
            return RedirectToAction("Index");
        }
    }
}
