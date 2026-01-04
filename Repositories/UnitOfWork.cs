using System;
using DepoEnvanterApp.Data;
using DepoEnvanterApp.Models;
using Microsoft.EntityFrameworkCore.Storage;

namespace DepoEnvanterApp.Repositories
{
    /// <summary>
    /// Unit of Work Pattern Implementasyonu
    /// Transaction yönetimi ve tüm repository'leri tek bir yerden kontrol eder
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;

        // Repository'ler - lazy initialization
        private IRepository<Urun>? _urunler;
        private IRepository<Kullanici>? _kullanicilar;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        // Repository Property'leri
        public IRepository<Urun> Urunler
        {
            get
            {
                if (_urunler == null)
                    _urunler = new Repository<Urun>(_context);
                return _urunler;
            }
        }

        public IRepository<Kullanici> Kullanicilar
        {
            get
            {
                if (_kullanicilar == null)
                    _kullanicilar = new Repository<Kullanici>(_context);
                return _kullanicilar;
            }
        }

        // TRANSACTION YÖNETİMİ
        /// <summary>
        /// Transaction başlatır. Birden fazla işlemi tek bir transaction içinde yapmak için kullanılır.
        /// </summary>
        public void BeginTransaction()
        {
            if (_transaction != null)
            {
                throw new InvalidOperationException("Zaten aktif bir transaction var!");
            }

            _transaction = _context.Database.BeginTransaction();
        }

        /// <summary>
        /// Transaction'ı commit eder (değişiklikleri kalıcı yapar)
        /// </summary>
        public void Commit()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Aktif bir transaction yok!");
            }

            try
            {
                _context.SaveChanges();
                _transaction.Commit();
            }
            catch
            {
                Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        /// <summary>
        /// Transaction'ı geri alır (değişiklikleri iptal eder)
        /// </summary>
        public void Rollback()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("Aktif bir transaction yok!");
            }

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        // KAYDETME İŞLEMİ
        /// <summary>
        /// Değişiklikleri veritabanına kaydeder (transaction olmadan)
        /// </summary>
        /// <returns>Etkilenen kayıt sayısı</returns>
        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        // DISPOSE PATTERN
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

