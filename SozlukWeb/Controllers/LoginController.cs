﻿using BusinessLayer.Concrete;
using BusinessLayer.ValidationRules;
using DataAccessLayer.Concrete;
using DataAccessLayer.EntityFramework;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FluentValidation.Results;


namespace SozlukWeb.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        // GET: Login

        // Admin Login İşlemleri

        WriterLoginManager wm = new WriterLoginManager(new EfWriterDal());
       
        WriterManager wm2 = new WriterManager(new EfWriterDal());

        WriterValidator writervalidator = new WriterValidator();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Admin p)
        {
            Context c = new Context();   // Parametreden gelen kullanıcı adı vt den gelenle aynıysa ve şifre de aynıysa
            var adminuserinfo = c.Admins.FirstOrDefault(x => x.AdminUserName == p.AdminUserName && x.AdminPassword == p.AdminPassword);   // 1 tane admin üzerinden giriş yapıcaz o yüzden firsordefault

            if (adminuserinfo != null)
            {
                FormsAuthentication.SetAuthCookie(adminuserinfo.AdminUserName, false); // Form içinde bir yetkili tanımlıyoruz.Sisteme giriş yapıcak kişinin bilgilerini hazırlıyoruz.
                Session["AdminUserName"] = adminuserinfo.AdminUserName;  // Oturum yönetimi oluşturuluyor.Sessionun içine sisteme giriş yapıcak kişinin mail adresi.Sisteme girecek kişinin mail adresi burdan gelecek.
                return RedirectToAction("Index", "AdminCategory");
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        // Yazar için login oluşturacağız

        [HttpGet]
        public ActionResult WriterLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult WriterLogin(Writer p)
        {
            //Context c = new Context();   // Parametreden gelen kullanıcı adı vt den gelenle aynıysa ve şifre de aynıysa
            //var writeruserinfo = c.Writers.FirstOrDefault(x => x.WriterMail == p.WriterMail && x.WriterPassword == p.WriterPassword);   // 1 tane admin üzerinden giriş yapıcaz o yüzden firsordefault

            var writeruserinfo = wm.GetWriter(p.WriterMail, p.WriterPassword);


            if (writeruserinfo != null)
            {
                FormsAuthentication.SetAuthCookie(writeruserinfo.WriterMail, false); // Form içinde bir yetkili tanımlıyoruz.Sisteme giriş yapıcak kişinin bilgilerini hazırlıyoruz.
                Session["WriterMail"] = writeruserinfo.WriterMail;  // Oturum yönetimi oluşturuluyor.Sessionun içine sisteme giriş yapıcak kişinin mail adresi.Sisteme girecek kişinin mail adresi burdan gelecek.
                return RedirectToAction("MyContent", "WriterPanelContent");
            }
            else
            {
                return RedirectToAction("WriterLogin");
            }


        }

        [HttpGet]
        public ActionResult AddWriter()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddWriter(Writer p)
        {

            ValidationResult result = writervalidator.Validate(p);
            if (result.IsValid)
            {
                wm2.WriterAdd(p);
                return RedirectToAction("WriterLogin");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.PropertyName, item.ErrorMessage);
                }
            }

            return View();
        }


        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Headings", "Default");
        }

    }
}