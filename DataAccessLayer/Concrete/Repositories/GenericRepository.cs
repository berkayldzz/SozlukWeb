﻿using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccessLayer.Concrete.Repositories
{
    public class GenericRepository<T> : IRepository<T> where T : class
    {

        Context c = new Context();

        DbSet<T> _object;

        // Ben burda T değerine karşılık olarak gelecek sınıfı constructor yardımıyla bulucam.

        public GenericRepository()  // Oluşturduğum sınıfın ismiyle aynı bir metot oluşturuyosak bunun türü constructordır. 
        {
            _object = c.Set<T>(); // Dışarıdan gönderdiğim T değeri(entity) _object in üstüne alıncak.
        }

        public void Delete(T p)
        {
            var deletedEntity = c.Entry(p);
            deletedEntity.State = EntityState.Deleted;
            //_object.Remove(p);
            c.SaveChanges();
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            return _object.SingleOrDefault(filter); // SingleOrDefault:Bir dizide veya listede sadece bir tane değer geriye döndürmek için kullanılan ef linq metodudur.
        }

        public void Insert(T p)
        {
            var addedEntity = c.Entry(p);
            addedEntity.State = EntityState.Added;
            //_object.Add(p);
            c.SaveChanges();
        }

        public List<T> List()
        {
            return _object.ToList();
        }

        public List<T> List(Expression<Func<T, bool>> filter)
        {
            return _object.Where(filter).ToList();
        }

        public void Update(T p)
        {
            var updatedEntity = c.Entry(p);
            updatedEntity.State = EntityState.Modified;
            c.SaveChanges();
        }
    }
}
