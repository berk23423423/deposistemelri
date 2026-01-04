using System;
using DepoEnvanterApp.Models;

namespace DepoEnvanterApp.Repositories
{
    /// <summary>
    /// Unit of Work Pattern Interface
    /// Transaction yönetimi ve repository erişimi sağlar
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        // Repository'ler
        IRepository<Urun> Urunler { get; }
        IRepository<Kullanici> Kullanicilar { get; }

        // Transaction İşlemleri
        void BeginTransaction();
        void Commit();
        void Rollback();

        // Kaydetme İşlemi
        int SaveChanges();
    }
}

