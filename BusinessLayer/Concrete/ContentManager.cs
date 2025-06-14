﻿using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class ContentManager : IContentService
    {
        IContentDal _contentDal;

        public ContentManager(IContentDal contentDal)
        {
            _contentDal = contentDal;
        }

        public void ContentAdd(Content content)
        {
            _contentDal.Insert(content);
        }

        public void ContentDelete(Content content)
        {
            _contentDal.Delete(content);
        }

       

        public Content GetByID(int id)
        {
            return _contentDal.Get(x => x.ContentID == id);
        }

        public List<Content> GetList(string p)
        {
            return _contentDal.List(x => x.ContentValue.Contains(p));
        }

        public List<Content> GetAll()
        {
            return _contentDal.List(); 
        }

        public List<Content> GetListByHeadingID(int id)
        {
            return _contentDal.List(x => x.HeadingID == id); // Dışarıdan girdiğim id ile yani yazılar butonuna tıkladığımda ilgili başlığa ait içeriği getiricek.

        }

        public List<Content> GetListByWriter(int id)
        {
            return _contentDal.List(x => x.WriterID == id);
        }
        

        public void ContentUpdate(Content content)
        {
            _contentDal.Update(content);
        }

    }
}
