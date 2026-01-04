using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace DepoEnvanterApp.Repositories
{
    /// <summary>
    /// Generic Repository Pattern Interface
    /// Tüm temel CRUD işlemlerini tanımlar
    /// </summary>
    /// <typeparam name="T">Entity tipi</typeparam>
    public interface IRepository<T> where T : class
    {
        // Okuma İşlemleri
        T? GetById(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        T? FirstOrDefault(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);

        // Yazma İşlemleri
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
    }
}

